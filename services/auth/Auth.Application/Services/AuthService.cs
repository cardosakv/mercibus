using System.Text;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using MapsterMapper;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using ErrorCode = Auth.Application.Common.ErrorCode;

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
    IMapper mapper) : BaseService, IAuthService
{
    public async Task<ServiceResult> RegisterAsync(RegisterRequest request)
    {
        await transactionService.BeginAsync();
        var user = mapper.Map<User>(request);

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            await transactionService.RollbackAsync();
            var error = createResult.Errors.First();
            return Error(Utils.IdentityErrorToType(error.Code), Utils.IdentityErrorToCode(error.Code));
        }

        var roleResult = await userManager.AddToRoleAsync(user, Roles.Customer);
        if (!roleResult.Succeeded)
        {
            await transactionService.RollbackAsync();
            var error = roleResult.Errors.First();
            return Error(Utils.IdentityErrorToType(error.Code), Utils.IdentityErrorToCode(error.Code));
        }

        await transactionService.CommitAsync();
        var userResponse = mapper.Map<GetUserInfoResponse>(user);

        return Success(userResponse);
    }

    public async Task<ServiceResult> LoginAsync(LoginRequest request)
    {
        await transactionService.BeginAsync();

        var user = await userManager.FindByNameAsync(request.Username);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        var passwordMatch = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordMatch)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.PasswordMismatch);
        }

        var role = await userManager.GetRolesAsync(user);
        if (role.Count == 0)
        {
            return Error(ErrorType.PermissionError, ErrorCode.UserNoRoleAssigned);
        }

        user.LastLoginAt = DateTime.UtcNow;
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            await transactionService.RollbackAsync();
            var error = updateResult.Errors.First();
            return Error(Utils.IdentityErrorToType(error.Code), Utils.IdentityErrorToCode(error.Code));
        }

        var (accessToken, expiresIn) = tokenService.CreateAccessToken(user, role[0]);
        var refreshToken = await refreshTokenRepository.CreateTokenAsync(user.Id);

        await transactionService.CommitAsync();

        var authToken = new AuthTokenResponse
        {
            AccessToken = accessToken,
            ExpiresIn = expiresIn,
            RefreshToken = refreshToken
        };

        return Success(authToken);
    }

    public async Task<ServiceResult> LogoutAsync(LogoutRequest request)
    {
        await transactionService.BeginAsync();

        var persistedToken = await refreshTokenRepository.RetrieveTokenAsync(request.RefreshToken);
        if (persistedToken is null || persistedToken.IsRevoked)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.TokenInvalid);
        }

        var revoked = await refreshTokenRepository.RevokeTokenAsync(persistedToken);
        if (!revoked)
        {
            return Error(ErrorType.ApiError, ErrorCode.Internal);
        }

        await transactionService.CommitAsync();

        return Success();
    }

    public async Task<ServiceResult> RefreshTokenAsync(RefreshRequest request)
    {
        await transactionService.BeginAsync();

        var persistedToken = await refreshTokenRepository.RetrieveTokenAsync(request.RefreshToken);
        if (persistedToken is null || persistedToken.IsRevoked)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.TokenInvalid);
        }

        if (persistedToken.ExpiresAt < DateTime.UtcNow)
        {
            var revoked = await refreshTokenRepository.RevokeTokenAsync(persistedToken);
            if (!revoked)
            {
                return Error(ErrorType.ApiError, ErrorCode.Internal);
            }

            return Error(ErrorType.AuthenticationError, ErrorCode.RefreshTokenExpired);
        }

        var user = await userManager.FindByIdAsync(persistedToken.UserId);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        var role = await userManager.GetRolesAsync(user);
        if (role.Count == 0)
        {
            return Error(ErrorType.PermissionError, ErrorCode.UserNoRoleAssigned);
        }

        var (newAccessToken, expiresIn) = tokenService.CreateAccessToken(user, role[0]);
        var newRefreshToken = await refreshTokenRepository.RotateTokenAsync(persistedToken);

        await transactionService.CommitAsync();

        var authToken = new AuthTokenResponse
        {
            AccessToken = newAccessToken,
            ExpiresIn = expiresIn,
            RefreshToken = newRefreshToken
        };

        return Success(authToken);
    }

    public async Task<ServiceResult> SendConfirmationEmailAsync(SendConfirmationEmailRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        if (user.EmailConfirmed)
        {
            return Error(ErrorType.ConflictError, ErrorCode.EmailAlreadyVerified);
        }

        if (httpContextAccessor.HttpContext is null)
        {
            return Error(ErrorType.ApiError, ErrorCode.Internal);
        }

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var confirmEndpoint = linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, "ConfirmEmail", "Auth");
        var confirmLink = confirmEndpoint + "?userId=" + user.Id + "&token=" + encodedToken;

        var emailSent = await emailService.SendEmailConfirmationLink(request.Email, confirmLink);
        if (!emailSent)
        {
            return Error(ErrorType.ApiError, ErrorCode.Internal);
        }

        return Success();
    }

    public async Task<ServiceResult> ConfirmEmailAsync(ConfirmEmailQuery query)
    {
        var user = await userManager.FindByIdAsync(query.UserId);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(query.Token));
        var verifyResult = await userManager.ConfirmEmailAsync(user, decodedToken);
        if (!verifyResult.Succeeded)
        {
            var error = verifyResult.Errors.First();
            return Error(Utils.IdentityErrorToType(error.Code), Utils.IdentityErrorToCode(error.Code));
        }

        return Success();
    }

    public async Task<ServiceResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        if (httpContextAccessor.HttpContext is null)
        {
            return Error(ErrorType.ApiError, ErrorCode.Internal);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var passwordResetRedirectUrl = configuration["RedirectUrl:PasswordReset"];
        var resetLink = passwordResetRedirectUrl + "?userId=" + user.Id + "&token=" + encodedToken;

        var emailSent = await emailService.SendPasswordResetLink(request.Email, resetLink);
        if (!emailSent)
        {
            return Error(ErrorType.ApiError, ErrorCode.Internal);
        }

        return Success();
    }

    public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        await transactionService.BeginAsync();

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        var resetResult = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!resetResult.Succeeded)
        {
            var error = resetResult.Errors.First();
            return Error(Utils.IdentityErrorToType(error.Code), Utils.IdentityErrorToCode(error.Code));
        }

        await transactionService.CommitAsync();

        return Success();
    }

    public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        await transactionService.BeginAsync();

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        var changeResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!changeResult.Succeeded)
        {
            var error = changeResult.Errors.First();
            return Error(Utils.IdentityErrorToType(error.Code), Utils.IdentityErrorToCode(error.Code));
        }

        await transactionService.CommitAsync();

        return Success();
    }

    public async Task<ServiceResult> GetInfoAsync(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        var userResponse = mapper.Map<GetUserInfoResponse>(user);

        return Success(userResponse);
    }

    public async Task<ServiceResult> UpdateInfoAsync(string? userId, UpdateUserInfoRequest request)
    {
        await transactionService.BeginAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Error(ErrorType.InvalidRequestError, ErrorCode.UserNotFound);
        }

        mapper.Map(request, user);

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var error = updateResult.Errors.First();
            return Error(Utils.IdentityErrorToType(error.Code), Utils.IdentityErrorToCode(error.Code));
        }

        await transactionService.CommitAsync();

        var userResponse = mapper.Map<GetUserInfoResponse>(user);

        return Success(userResponse);
    }
}