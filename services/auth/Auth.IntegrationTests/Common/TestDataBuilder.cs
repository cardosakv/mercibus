using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.IntegrationTests.Common;

/// <summary>
/// Builder for creating test data with fluent API.
/// Makes tests more readable and maintainable.
/// </summary>
public class TestDataBuilder
{
    private string _userName = "test_user";
    private string _email = "test@example.com";
    private string _password = "Test@123";
    private string _name = "Test User";
    private bool _emailConfirmed;

    public TestDataBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public TestDataBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public TestDataBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public TestDataBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public TestDataBuilder WithEmailConfirmed(bool confirmed = true)
    {
        _emailConfirmed = confirmed;
        return this;
    }

    public User Build()
    {
        return new User
        {
            UserName = _userName,
            Email = _email,
            Name = _name,
            EmailConfirmed = _emailConfirmed
        };
    }

    public async Task<User> BuildAndCreateAsync(UserManager<User> userManager, string? roleName = null)
    {
        var user = Build();
        var result = await userManager.CreateAsync(user, _password);

        if (!result.Succeeded)
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        if (!string.IsNullOrEmpty(roleName))
            await userManager.AddToRoleAsync(user, roleName);

        return user;
    }
}