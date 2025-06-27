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
            try
            {
                var user = new User 
                { 
                    UserName = request.UserName, 
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
                    var error = roleResult.Errors.First();
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
                    Message = Messages.USER_REGISTERED
                };
            }
            catch
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Messages.UNEXPECTED_ERROR,
                    ErrorType = ErrorType.Internal
                };
            }
        }
    }
}
