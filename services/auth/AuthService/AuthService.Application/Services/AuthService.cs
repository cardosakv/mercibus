using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Services
{
    public class AuthService(UserManager<User> userManager) : IAuthService
    {
        public async Task<Response> RegisterAsync(RegisterRequest request)
        {
            var response = new Response();

            try
            {
                var user = new User { Email = request.Email };

                var createResult = await userManager.CreateAsync(user, request.Password);
                if (!createResult.Succeeded)
                {
                    var error = createResult.Errors.First();
                    response.IsSuccess = false;
                    response.Message = error.Description;
                    response.ErrorType = IdentityErrorMapper.MapToErrorType(error.Code);
                    return response;
                }

                var roleResult = await userManager.AddToRoleAsync(user, Roles.Customer);
                if (!roleResult.Succeeded)
                {
                    var error = roleResult.Errors.First();
                    response.IsSuccess = false;
                    response.Message = error.Description;
                    response.ErrorType = IdentityErrorMapper.MapToErrorType(error.Code);
                    return response;
                }

                response.IsSuccess = true;
                response.Message = "User registered successfully.";
                return response;
            }
            catch
            {
                response.IsSuccess = false;
                response.Message = "An error occurred while registering the user.";
                response.ErrorType = ErrorType.Internal;
                return response;
            }
        }
    }
}
