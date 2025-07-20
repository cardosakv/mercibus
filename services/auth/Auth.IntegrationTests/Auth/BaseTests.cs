using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Auth.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.IntegrationTests.Auth;

/// <summary>
/// Base class for integration tests.
/// </summary>
public abstract class BaseTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    protected const string RegisterUrl = "/api/auth/register";
    protected const string LoginUrl = "/api/auth/login";
    protected const string LogoutUrl = "/api/auth/logout";
    protected const string RefreshTokenUrl = "/api/auth/refresh-token";
    protected const string SendConfirmationEmailUrl = "/api/auth/send-confirmation-email";
    protected const string ConfirmEmailUrl = "/api/auth/confirm-email";
    protected const string ForgotPasswordUrl = "/api/auth/forgot-password";
    protected const string ResetPasswordUrl = "/api/auth/reset-password";
    protected const string ChangePasswordUrl = "/api/auth/change-password";
    protected const string GetUpdateInfoUrl = "/api/auth/info";
    protected const string UploadProfilePictureUrl = "/api/auth/upload-profile-picture";

    protected readonly IServiceScope ServiceScope = factory.Services.CreateScope();
    protected readonly IAuthService AuthService = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IAuthService>();
    protected readonly AppDbContext DbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    protected readonly UserManager<User> UserManager = factory.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
    protected readonly IConfiguration Configuration = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
    protected readonly HttpClient HttpClient = factory.CreateClient();
}