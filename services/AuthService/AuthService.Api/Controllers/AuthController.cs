using AuthService.Application.DTOs;
using AuthService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _userService.RegisterAsync(request);
            return HandleResponse(response, HttpContext.Request.Method);
        }
    }
}
