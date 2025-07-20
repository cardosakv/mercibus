using System.Text;
using Auth.Application.DTOs;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;

namespace Auth.IntegrationTests.Auth;

public class ConfirmEmailTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task RedirectsToSuccessUrl_WhenEmailIsConfirmed()
    {
        // Arrange
        var user = new User { UserName = "confirm_email_user", Email = "confirm@email.com" };
        await UserManager.CreateAsync(user, "Valid@123");
        await UserManager.AddToRoleAsync(user, Roles.Customer);

        var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var query = new ConfirmEmailQuery
        {
            UserId = user.Id,
            Token = encodedToken
        };

        // Act
        var result = await AuthService.ConfirmEmailAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RedirectsToFailUrl_WhenUserNotFound()
    {
        // Arrange
        var query = new ConfirmEmailQuery
        {
            UserId = "non_existent_user_id",
            Token = "invalid_token"
        };

        // Act
        var result = await AuthService.ConfirmEmailAsync(query);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}