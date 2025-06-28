using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Services
{
    public class AuthService(UserManager<User> userManager, ITokenService tokenService,  AppDbContext dbContext) : IAuthService
    {
        public async Task<Response> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var user = new User 
                { 
                    UserName = request.Username, 
                    Email = request.Email 
                };

                await using var transaction = await dbContext.Database.BeginTransactionAsync();
                    
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
                    await transaction.RollbackAsync();
                    
                    var error = roleResult.Errors.First();
                    return new Response
                    {
                        IsSuccess = false,
                        Message = error.Description,
                        ErrorType = IdentityErrorMapper.MapToErrorType(error.Code)
                    };
                }

                await transaction.CommitAsync();
                
                return new Response
                {
                    IsSuccess = true,
                    Message = Messages.UserRegistered
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

        public async Task<Response> LoginAsync(LoginRequest request)
        {
            try
            {
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
                
                var authToken = tokenService.CreateToken(user, role.First());

                return new Response
                {
                    IsSuccess = true,
                    Data = authToken
                };
            }
            catch (Exception ex)
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
