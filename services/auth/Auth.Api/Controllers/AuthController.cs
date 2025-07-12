using System.Security.Claims;
using Auth.Application.DTOs;
using Auth.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService, IConfiguration configuration) : BaseController
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await authService.RegisterAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var response = await authService.LogoutAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
    {
        var response = await authService.RefreshTokenAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("send-confirmation-email")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SendConfirmationEmail([FromBody] SendConfirmationEmailRequest request)
    {
        var response = await authService.SendConfirmationEmailAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpGet("confirm-email")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var response = await authService.ConfirmEmailAsync(userId, token);

        return response.IsSuccess
            ? Redirect(configuration["RedirectUrl:EmailConfirmSuccess"] ?? string.Empty)
            : Redirect(configuration["RedirectUrl:EmailConfirmFail"] ?? string.Empty);
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await authService.ForgotPasswordAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await authService.ResetPasswordAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("change-password")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var response = await authService.ChangePasswordAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpGet("info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(GetUserInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await authService.GetInfoAsync(userId);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInfo([FromBody] UpdateUserInfoRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await authService.UpdateInfoAsync(userId, request);
        return HandleResponse(response, HttpContext.Request.Method);
    }
}