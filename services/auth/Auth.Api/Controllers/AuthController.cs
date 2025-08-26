using System.Security.Claims;
using Auth.Application.DTOs;
using Auth.Application.Interfaces.Services;
using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/auth")]
public class AuthController(IAuthService authService, IConfiguration configuration) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await authService.RegisterAsync(request);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request);
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var response = await authService.LogoutAsync(request);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
    {
        var response = await authService.RefreshTokenAsync(request);
        return Ok(response);
    }

    [HttpPost("send-confirmation-email")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> SendConfirmationEmail([FromBody] SendConfirmationEmailRequest request)
    {
        var response = await authService.SendConfirmationEmailAsync(request);
        return Ok(response);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
    {
        var response = await authService.ConfirmEmailAsync(query);

        return response.IsSuccess
            ? Redirect(configuration["RedirectUrl:EmailConfirmSuccess"] ?? string.Empty)
            : Redirect(configuration["RedirectUrl:EmailConfirmFail"] ?? string.Empty);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await authService.ForgotPasswordAsync(request);
        return Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await authService.ResetPasswordAsync(request);
        return Ok(response);
    }

    [HttpPost("change-password")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var response = await authService.ChangePasswordAsync(request);
        return Ok(response);
    }

    [HttpGet("info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await authService.GetInfoAsync(userId);
        return Ok(response);
    }

    [HttpPost("info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateInfo([FromBody] UpdateUserInfoRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await authService.UpdateInfoAsync(userId, request);
        return Ok(response);
    }

    [HttpPost("upload-profile-picture")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UploadProfilePicture(IFormFile image)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await authService.UploadProfilePictureAsync(userId, image);
        return Ok(response);
    }
}