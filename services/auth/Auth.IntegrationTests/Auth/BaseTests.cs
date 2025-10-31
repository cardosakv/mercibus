using System.Net.Http.Json;
using Auth.Application.DTOs;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Auth.Infrastructure;
using Auth.IntegrationTests.Common;
using Mercibus.Common.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Auth.IntegrationTests.Auth;

/// <summary>
/// Base class for integration tests.
/// </summary>
[Collection("Integration Tests Collection")]
public abstract class BaseTests(TestWebAppFactory factory) : IAsyncLifetime
{
    private IServiceScope? _serviceScope;

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

    protected IAuthService AuthService => _serviceScope!.ServiceProvider.GetRequiredService<IAuthService>();
    protected AppDbContext DbContext => _serviceScope!.ServiceProvider.GetRequiredService<AppDbContext>();
    protected UserManager<User> UserManager => _serviceScope!.ServiceProvider.GetRequiredService<UserManager<User>>();
    protected IConfiguration Configuration => _serviceScope!.ServiceProvider.GetRequiredService<IConfiguration>();
    protected HttpClient HttpClient { get; private set; } = factory.CreateClient();

    public async Task InitializeAsync()
    {
        _serviceScope = factory.Services.CreateScope();
        // Clean database before each test class to prevent data pollution
        await CleanDatabaseAsync();
        await Task.CompletedTask;
    }

    private async Task CleanDatabaseAsync()
    {
        // Delete all data to ensure test isolation
        var dbContext = _serviceScope!.ServiceProvider.GetRequiredService<AppDbContext>();

        // Delete all refresh tokens first (due to foreign key)
        var refreshTokens = dbContext.RefreshTokens.ToList();
        foreach (var token in refreshTokens)
        {
            dbContext.RefreshTokens.Remove(token);
        }

        // Delete all users
        var users = dbContext.Users.ToList();
        foreach (var user in users)
        {
            dbContext.Users.Remove(user);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        _serviceScope?.Dispose();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Creates a user with the specified credentials using the fluent builder pattern.
    /// </summary>
    protected async Task<User> CreateUserAsync(string userName, string email, string password = "Test@123", string? roleName = null)
    {
        var user = await new TestDataBuilder()
            .WithUserName(userName)
            .WithEmail(email)
            .WithPassword(password)
            .BuildAndCreateAsync(UserManager, roleName);
        return user;
    }

    /// <summary>
    /// Logs in a user and returns the authentication token.
    /// </summary>
    protected async Task<AuthTokenResponse?> LoginAsync(string username, string password)
    {
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };
        var response = await HttpClient.PostAsJsonAsync(LoginUrl, request);
        response.EnsureSuccessStatusCode();

        var contentString = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiSuccessResponse>(contentString);

        if (apiResponse?.Data is null)
            throw new InvalidOperationException("Login response missing expected data");

        return JsonConvert.DeserializeObject<AuthTokenResponse>(apiResponse.Data.ToString()!);
    }

    /// <summary>
    /// Refreshes the user data from database to ensure fresh state.
    /// </summary>
    protected async Task RefreshEntityAsync<T>(T entity) where T : class
    {
        if (entity != null)
            await DbContext.Entry(entity).ReloadAsync();
    }

    /// <summary>
    /// Sets the authorization header with the provided token.
    /// </summary>
    protected void SetAuthorizationToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
            HttpClient.DefaultRequestHeaders.Remove("Authorization");
        else
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}