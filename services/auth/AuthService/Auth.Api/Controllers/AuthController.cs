using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService, IConfiguration configuration) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await authService.RegisterAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Login([FromBody] LogoutRequest request)
    {
        var response = await authService.LogoutAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
    {
        var response = await authService.RefreshTokenAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("send-confirmation-email")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> SendConfirmationEmail([FromBody] SendConfirmationEmailRequest request)
    {
        var response = await authService.SendConfirmationEmailAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var response = await authService.ConfirmEmailAsync(userId, token);

        return !response.IsSuccess
            ? Redirect(configuration["EmailConfirmRedirect:Fail"] ?? string.Empty)
            : Redirect(configuration["EmailConfirmRedirect:Success"] ?? string.Empty);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await authService.ForgotPasswordAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await authService.ResetPasswordAsync(request);
        return HandleResponse(response, HttpContext.Request.Method);
    }
}