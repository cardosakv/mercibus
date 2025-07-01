using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service get user info method.
/// </summary>
public class GetInfoAsyncTests : BaseTests
{
    private readonly string _userId = "user-1";

    private readonly User _user = new()
    {
        Id = "user-1",
        Name = "Test User",
        Email = "test@email.com",
        EmailConfirmed = true,
        Street = "123 St",
        City = "Metro City",
        State = "Metro State",
        Country = "Ph",
        PostalCode = 1100
    };

    [Fact]
    public async Task Success_WhenUserFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ReturnsAsync(_user);

        // Act
        var response = await AuthService.GetInfoAsync(_userId);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(_userId), Times.Once);

        response.IsSuccess.Should().BeTrue();
        response.Data.Should().NotBeNull();

        var data = response.Data as GetUserInfoResponse;
        data!.Name.Should().Be(_user.Name);
        data.Email.Should().Be(_user.Email);
        data.IsEmailVerified.Should().BeTrue();
        data.Street.Should().Be(_user.Street);
        data.City.Should().Be(_user.City);
        data.State.Should().Be(_user.State);
        data.Country.Should().Be(_user.Country);
        data.PostalCode.Should().Be(_user.PostalCode);
    }

    [Fact]
    public async Task Fail_WhenUserIdIsNull()
    {
        // Act
        var response = await AuthService.GetInfoAsync(null);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.NotFound);
        response.Message.Should().Be(Messages.UserNotFound);

        UserManagerMock.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Fail_WhenUserNotFound()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.GetInfoAsync(_userId);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(_userId), Times.Once);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.NotFound);
        response.Message.Should().Be(Messages.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ThrowsAsync(new Exception("unexpected"));

        // Act
        var response = await AuthService.GetInfoAsync(_userId);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(_userId), Times.Once);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.Internal);
        response.Message.Should().Be(Messages.UnexpectedError);
    }
}