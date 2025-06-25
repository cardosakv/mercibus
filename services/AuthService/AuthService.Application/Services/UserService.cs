using AuthService.Application.Common;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces.Services;

namespace AuthService.Application.Services
{
    public class UserService : IUserService
    {
        public Task<Result<bool>> RegisterAsync(RegisterRequest request)
        {
            var response = new Result<bool>
            {
                IsSuccess = true,
                Message = "User registered successfully (mock).",
                ErrorType = ErrorType.Conflict
            };

            return Task.FromResult(response);
        }
    }
}
