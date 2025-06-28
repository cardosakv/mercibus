using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : BaseController
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await authService.RegisterAsync(request);
            return HandleResponse(response, HttpContext.Request.Method);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await authService.LoginAsync(request);
            return HandleResponse(response, HttpContext.Request.Method);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
        {
            var response = await authService.RefreshTokenAsync(request);
            return HandleResponse(response, HttpContext.Request.Method);
        }

        [HttpPost]
        [Route("send-confirmation-email")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SendConfirmationEmail([FromBody] SendConfirmationEmailRequest request)
        {
            var response = await authService.SendConfirmationEmail(request);
            return HandleResponse(response, HttpContext.Request.Method);
        }

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var response = await authService.ConfirmEmail(userId, token);
            return HandleResponse(response, HttpContext.Request.Method);
        }
    }
}