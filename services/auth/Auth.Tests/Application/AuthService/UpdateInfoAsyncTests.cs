using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Moq;
using ErrorCode = Auth.Application.Common.ErrorCode;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service update user info method.
/// </summary>
public class UpdateInfoAsyncTests : BaseTests
{
    private readonly string _userId = "user-1";

    private readonly User _existingUser = new()
    {
        Id = "user-1",
        Name = "Old Name",
        Street = "Old St",
        City = "Old City",
        State = "Old State",
        Country = "Old Country",
        PostalCode = 1000
    };

    private readonly UpdateUserInfoRequest _request = new()
    {
        Name = "New Name",
        Street = "New Street",
        City = "New City",
        State = "New State",
        Country = "New Country",
        PostalCode = 1234
    };

    [Fact]
    public async Task Success_WhenUserUpdated()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ReturnsAsync(_existingUser);

        UserManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await AuthService.UpdateInfoAsync(_userId, _request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_userId), Times.Once);
        UserManagerMock.Verify(x => x.UpdateAsync(It.Is<User>(u =>
            u.Name == _request.Name &&
            u.Street == _request.Street &&
            u.City == _request.City &&
            u.State == _request.State &&
            u.Country == _request.Country &&
            u.PostalCode == _request.PostalCode
        )), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Once);

        response.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Fail_WhenUserIdIsNull()
    {
        // Act
        var response = await AuthService.UpdateInfoAsync(null, _request);

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorType.Should().Be(ErrorCode.UserIdRequired);

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
        var response = await AuthService.UpdateInfoAsync(_userId, _request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_userId), Times.Once);
        UserManagerMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        response.ErrorType.Should().Be(ErrorCode.UserNotFound);
    }

    [Fact]
    public async Task Fail_WhenUpdateFails()
    {
        // Arrange
        var error = new IdentityError { Code = "UpdateError", Description = "Update failed" };

        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ReturnsAsync(_existingUser);

        UserManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed(error));

        // Act
        var response = await AuthService.UpdateInfoAsync(_userId, _request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.FindByIdAsync(_userId), Times.Once);
        UserManagerMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);

        response.IsSuccess.Should().BeFalse();
        response.ErrorCode.Should().Be(Utils.IdentityErrorToCode(error.Code));
        response.ErrorType.Should().Be(Utils.IdentityErrorToType(error.Code));
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.FindByIdAsync(_userId))
            .ThrowsAsync(new Exception("unexpected"));

        // Act
        var response = await AuthService.UpdateInfoAsync(_userId, _request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        TransactionServiceMock.Verify(x => x.RollbackAsync(), Times.Once);

        response.IsSuccess.Should().BeFalse();
        response.ErrorType.Should().Be(ErrorType.ApiError);
    }
}