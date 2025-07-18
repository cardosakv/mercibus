using System.Text;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Services;

public class AuthService(
    UserManager<User> userManager,
    ITokenService tokenService,
    IEmailService emailService,
    ITransactionService transactionService,
    IRefreshTokenRepository refreshTokenRepository,
    IHttpContextAccessor httpContextAccessor,
    LinkGenerator linkGenerator,
    IConfiguration configuration,
    ILogger<IAuthService> logger) : IAuthService
{
    public async Task<Response> RegisterAsync(RegisterRequest request)
    {
        try
        {
            await transactionService.BeginAsync();

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email
            };

            var createResult = await userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var error = createResult.Errors.First();
                return new Response
                {
                    IsSuccess = false,
                    Message = error.Description,
                    ErrorType = IdentityErrorMapper.MapToErrorType(error.Code)
                };
            }

            var roleResult = await userManager.AddToRoleAsync(user, Roles.Customer);
            if (!roleResult.Succeeded)
            {
                await transactionService.RollbackAsync();

                var error = roleResult.Errors.First();
                return new Response
                {
                    IsSuccess = false,
                    Message = error.Description,
                    ErrorType = IdentityErrorMapper.MapToErrorType(error.Code)
                };
            }

            await transactionService.CommitAsync();

            return new Response
            {
                IsSuccess = true,
                Message = Messages.UserRegistered
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            await transactionService.RollbackAsync();
            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> LoginAsync(LoginRequest request)
    {
        try
        {
            await transactionService.BeginAsync();

            var user = await userManager.FindByNameAsync(request.Username);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.PasswordIncorrect,
                    ErrorType = ErrorType.Validation
                };
            }

            var role = await userManager.GetRolesAsync(user);
            if (role.Count == 0)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserForbidden,
                    ErrorType = ErrorType.Forbidden
                };
            }

            var (accessToken, expiresIn) = tokenService.CreateAccessToken(user, role[0]);
            var refreshToken = await refreshTokenRepository.CreateTokenAsync(user.Id);

            await transactionService.CommitAsync();

            return new Response
            {
                IsSuccess = true,
                Data = new AuthToken
                {
                    AccessToken = accessToken,
                    ExpiresIn = expiresIn,
                    RefreshToken = refreshToken
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            await transactionService.RollbackAsync();
            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> LogoutAsync(LogoutRequest request)
    {
        try
        {
            await transactionService.BeginAsync();

            var persistedToken = await refreshTokenRepository.RetrieveTokenAsync(request.RefreshToken);
            if (persistedToken is null || persistedToken.IsRevoked)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserForbidden,
                    ErrorType = ErrorType.Forbidden
                };
            }

            var revoked = await refreshTokenRepository.RevokeTokenAsync(persistedToken);
            if (!revoked)
            {
                await transactionService.RollbackAsync();
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UnexpectedError,
                    ErrorType = ErrorType.Internal
                };
            }

            await transactionService.CommitAsync();

            return new Response
            {
                IsSuccess = true,
                Message = Messages.UserLoggedOut
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            await transactionService.RollbackAsync();
            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> RefreshTokenAsync(RefreshRequest request)
    {
        try
        {
            await transactionService.BeginAsync();

            var persistedToken = await refreshTokenRepository.RetrieveTokenAsync(request.RefreshToken);
            if (persistedToken is null || persistedToken.IsRevoked)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserForbidden,
                    ErrorType = ErrorType.Forbidden
                };
            }

            if (persistedToken.ExpiresAt < DateTime.UtcNow)
            {
                var revoked = await refreshTokenRepository.RevokeTokenAsync(persistedToken);
                if (!revoked)
                {
                    await transactionService.RollbackAsync();
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Messages.UnexpectedError,
                        ErrorType = ErrorType.Internal
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.RefreshTokenExpired,
                    ErrorType = ErrorType.Unauthorized
                };
            }

            var user = await userManager.FindByIdAsync(persistedToken.UserId);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            var role = await userManager.GetRolesAsync(user);
            if (role.Count == 0)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserForbidden,
                    ErrorType = ErrorType.Forbidden
                };
            }

            var (newAccessToken, expiresIn) = tokenService.CreateAccessToken(user, role[0]);
            var newRefreshToken = await refreshTokenRepository.RotateTokenAsync(persistedToken);

            await transactionService.CommitAsync();

            return new Response
            {
                IsSuccess = true,
                Data = new AuthToken
                {
                    AccessToken = newAccessToken,
                    ExpiresIn = expiresIn,
                    RefreshToken = newRefreshToken
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            await transactionService.RollbackAsync();
            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> SendConfirmationEmailAsync(SendConfirmationEmailRequest request)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            if (user.EmailConfirmed)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.EmailAlreadyVerified,
                    ErrorType = ErrorType.Conflict
                };
            }

            if (httpContextAccessor.HttpContext is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UnexpectedError,
                    ErrorType = ErrorType.Internal
                };
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmEndpoint =
                linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, "ConfirmEmail", "Auth");
            var confirmLink = confirmEndpoint + "?userId=" + user.Id + "&token=" + encodedToken;

            var emailSent = await emailService.SendEmailConfirmationLink(request.Email, confirmLink);
            if (!emailSent)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UnexpectedError,
                    ErrorType = ErrorType.Internal
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = Messages.EmailConfirmationSent
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> ConfirmEmailAsync(string userId, string token)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var verifyResult = await userManager.ConfirmEmailAsync(user, decodedToken);
            if (!verifyResult.Succeeded)
            {
                var error = verifyResult.Errors.First();
                return new Response
                {
                    IsSuccess = false,
                    Message = error.Description,
                    ErrorType = IdentityErrorMapper.MapToErrorType(error.Code)
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = Messages.EmailVerified
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            if (httpContextAccessor.HttpContext is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UnexpectedError,
                    ErrorType = ErrorType.Internal
                };
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var passwordResetRedirectUrl = configuration["RedirectUrl:PasswordReset"];
            var resetLink = passwordResetRedirectUrl + "?userId=" + user.Id + "&token=" + encodedToken;

            var emailSent = await emailService.SendPasswordResetLink(request.Email, resetLink);
            if (!emailSent)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UnexpectedError,
                    ErrorType = ErrorType.Internal
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = Messages.PasswordResetLinkSent
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            await transactionService.BeginAsync();

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            var resetResult = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!resetResult.Succeeded)
            {
                var error = resetResult.Errors.First();
                return new Response
                {
                    IsSuccess = false,
                    Message = error.Description,
                    ErrorType = IdentityErrorMapper.MapToErrorType(error.Code)
                };
            }

            await transactionService.CommitAsync();

            return new Response
            {
                IsSuccess = true,
                Message = Messages.PasswordResetSuccess
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            await transactionService.RollbackAsync();
            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            await transactionService.BeginAsync();

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            var changeResult =
                await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!changeResult.Succeeded)
            {
                var error = changeResult.Errors.First();
                return new Response
                {
                    IsSuccess = false,
                    Message = error.Description,
                    ErrorType = IdentityErrorMapper.MapToErrorType(error.Code)
                };
            }

            await transactionService.CommitAsync();

            return new Response
            {
                IsSuccess = true,
                Message = Messages.PasswordChanged
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            await transactionService.RollbackAsync();
            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> GetInfoAsync(string? userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            var userData = new GetUserInfoResponse
            {
                Name = user.Name,
                Email = user.Email ?? string.Empty,
                IsEmailVerified = user.EmailConfirmed,
                Street = user.Street,
                City = user.City,
                State = user.State,
                Country = user.Country,
                PostalCode = user.PostalCode is 0 ? null : user.PostalCode
            };

            return new Response
            {
                IsSuccess = true,
                Data = userData
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }

    public async Task<Response> UpdateInfoAsync(string? userId, UpdateUserInfoRequest request)
    {
        try
        {
            await transactionService.BeginAsync();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UserNotFound,
                    ErrorType = ErrorType.NotFound
                };
            }

            user.Name = request.Name;
            user.Street = request.Street;
            user.City = request.City;
            user.State = request.State;
            user.Country = request.Country;
            user.PostalCode = request.PostalCode;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var error = updateResult.Errors.First();
                return new Response
                {
                    IsSuccess = false,
                    Message = error.Description,
                    ErrorType = IdentityErrorMapper.MapToErrorType(error.Code)
                };
            }

            await transactionService.CommitAsync();

            return new Response
            {
                IsSuccess = true,
                Message = Messages.UserInfoUpdated
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Messages.ExceptionOccured);

            await transactionService.RollbackAsync();
            return new Response
            {
                IsSuccess = false,
                Message = Messages.UnexpectedError,
                ErrorType = ErrorType.Internal
            };
        }
    }
}