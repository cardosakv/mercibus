using Auth.Application.DTOs;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Tests for auth service register method.
/// </summary>
public class RegisterAsyncTests : BaseTests
{
    private readonly RegisterRequest _request = new()
    {
        Username = "test",
        Password = "password",
        Email = "test@email.com"
    };

    [Fact]
    public async Task Success_WhenUserCreatedAndRoleAdded()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        UserManagerMock
            .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await AuthService.RegisterAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), _request.Password), Times.Once);
        UserManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), Roles.Customer), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Once);
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task Fail_WhenUserCreatedAndNoRoleAdded()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        UserManagerMock
            .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var response = await AuthService.RegisterAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), _request.Password), Times.Once);
        UserManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), Roles.Customer), Times.Once);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);
        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task Fail_WhenUserNotCreated()
    {
        // Arrange
        UserManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var response = await AuthService.RegisterAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        UserManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), _request.Password), Times.Once);
        UserManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), Roles.Customer), Times.Never);
        TransactionServiceMock.Verify(x => x.CommitAsync(), Times.Never);
        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task Fail_WhenExceptionOccurs()
    {
        // Arrange
        const string exceptionMessage = "Create user exception.";
        UserManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var response = await AuthService.RegisterAsync(_request);

        // Assert
        TransactionServiceMock.Verify(x => x.BeginAsync(), Times.Once);
        TransactionServiceMock.Verify(x => x.RollbackAsync(), Times.Once);
        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
        response.ErrorType.Should().Be(ErrorType.ApiError);
    }
}