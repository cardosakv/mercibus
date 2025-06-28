using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace Auth.Application.Services
{
    public class AuthService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IEmailService emailService,
        ITransactionService transactionService,
        IRefreshTokenRepository refreshTokenRepository,
        IHttpContextAccessor httpContextAccessor,
        LinkGenerator linkGenerator) : IAuthService
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
            catch (Exception)
            {
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

                var (accessToken, expiresIn) = tokenService.CreateAccessToken(user, role.First());
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
            catch (Exception)
            {
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
                if (persistedToken is null)
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

                var (newAccessToken, expiresIn) = tokenService.CreateAccessToken(user, role.First());
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
            catch (Exception)
            {
                await transactionService.RollbackAsync();
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UnexpectedError,
                    ErrorType = ErrorType.Internal
                };
            }
        }

        public async Task<Response> SendConfirmationEmail(SendConfirmationEmailRequest request)
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

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var baseUrl = httpContextAccessor.HttpContext.Request.Scheme + "://" +
                              httpContextAccessor.HttpContext.Request.Host;
                var confirmEndpoint =
                    linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, "ConfirmEmail", "Auth");
                var confirmLink = confirmEndpoint + "?userId=" + user.Id + "&token=" + token;

                var emailSent = await emailService.SendConfirmationLink(request.Email, confirmLink);
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
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UnexpectedError,
                    ErrorType = ErrorType.Internal
                };
            }
        }
    }
}