using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Auth.Tests.Application.AuthService;

/// <summary>
/// Base test class for auth service tests.
/// </summary>
public abstract class BaseTests
{
    protected readonly IAuthService AuthService;

    protected readonly Mock<UserManager<User>> UserManagerMock;
    protected readonly Mock<ITokenService> TokenServiceMock;
    protected readonly Mock<IEmailService> EmailServiceMock;
    protected readonly Mock<ITransactionService> TransactionServiceMock;
    protected readonly Mock<IRefreshTokenRepository> RefreshTokenRepositoryMock;
    protected readonly Mock<IHttpContextAccessor> HttpContextAccessorMock;
    protected readonly Mock<LinkGenerator> LinkGeneratorMock;
    protected readonly Mock<IConfiguration> ConfigurationMock;
    protected readonly Mock<ILogger<IAuthService>> LoggerMock;

    protected BaseTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        UserManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        TransactionServiceMock = new Mock<ITransactionService>();
        TransactionServiceMock
            .Setup(x => x.BeginAsync())
            .Returns(Task.CompletedTask);
        TransactionServiceMock
            .Setup(x => x.CommitAsync())
            .Returns(Task.CompletedTask);
        TransactionServiceMock
            .Setup(x => x.RollbackAsync())
            .Returns(Task.CompletedTask);

        TokenServiceMock = new Mock<ITokenService>();
        RefreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        EmailServiceMock = new Mock<IEmailService>();
        HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
        LinkGeneratorMock = new Mock<LinkGenerator>();
        ConfigurationMock = new Mock<IConfiguration>();
        LoggerMock = new Mock<ILogger<IAuthService>>();

        AuthService = new Auth.Application.Services.AuthService(
            UserManagerMock.Object,
            TokenServiceMock.Object,
            EmailServiceMock.Object,
            TransactionServiceMock.Object,
            RefreshTokenRepositoryMock.Object,
            HttpContextAccessorMock.Object,
            LinkGeneratorMock.Object,
            ConfigurationMock.Object,
            LoggerMock.Object);
    }
}