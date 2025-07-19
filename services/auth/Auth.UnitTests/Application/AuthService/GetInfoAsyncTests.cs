using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.UnitTests.Application.AuthService;

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

    private readonly GetUserInfoResponse _userResponse = new()
    {
        Id = "user-1",
        Username = "test_user",
        Name = "Test User",
        Email = "test@email.com",
        IsEmailVerified = true,
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
        MapperMock
            .Setup(x => x.Map<GetUserInfoResponse>(_user))
            .Returns(_userResponse);

        // Act
        var response = await AuthService.GetInfoAsync(_userId);

        // Assert
        UserManagerMock.Verify(x => x.FindByIdAsync(_userId), Times.Once);

        response.IsSuccess.Should().BeTrue();
        var data = response.Data.Should().BeOfType<GetUserInfoResponse>().Subject;
        data.Name.Should().Be(_userResponse.Name);
        data.Email.Should().Be(_userResponse.Email);
        data.IsEmailVerified.Should().Be(_userResponse.IsEmailVerified);
        data.Street.Should().Be(_userResponse.Street);
        data.City.Should().Be(_userResponse.City);
        data.State.Should().Be(_userResponse.State);
        data.Country.Should().Be(_userResponse.Country);
        data.PostalCode.Should().Be(_userResponse.PostalCode);
    }

    [Fact]
    public async Task Fail_WhenUserIdIsNull()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act
        var response = await AuthService.GetInfoAsync(null);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.UserNotFound);
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
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorCode.Should().Be(ErrorCode.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ThrowsAsync(new Exception("An unexpected error occurred."));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            AuthService.GetInfoAsync(_userId));
    }
}