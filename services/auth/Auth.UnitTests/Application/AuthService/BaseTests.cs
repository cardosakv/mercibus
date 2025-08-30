using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Hangfire;
using Hangfire.MemoryStorage;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Auth.UnitTests.Application.AuthService;

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
    protected readonly Mock<IBlobStorageService> BlobStorageServiceMock;
    protected readonly Mock<IRefreshTokenRepository> RefreshTokenRepositoryMock;
    protected readonly Mock<IHttpContextAccessor> HttpContextAccessorMock;
    protected readonly Mock<LinkGenerator> LinkGeneratorMock;
    protected readonly Mock<IConfiguration> ConfigurationMock;
    protected readonly Mock<IMapper> MapperMock;

    protected BaseTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        UserManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            Array.Empty<IUserValidator<User>>(),
            Array.Empty<IPasswordValidator<User>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
        );

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
        BlobStorageServiceMock = new Mock<IBlobStorageService>();
        HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
        LinkGeneratorMock = new Mock<LinkGenerator>();
        ConfigurationMock = new Mock<IConfiguration>();
        MapperMock = new Mock<IMapper>();
        GlobalConfiguration.Configuration.UseMemoryStorage();

        AuthService = new Auth.Application.Services.AuthService(
            UserManagerMock.Object,
            TokenServiceMock.Object,
            EmailServiceMock.Object,
            TransactionServiceMock.Object,
            BlobStorageServiceMock.Object,
            RefreshTokenRepositoryMock.Object,
            HttpContextAccessorMock.Object,
            LinkGeneratorMock.Object,
            ConfigurationMock.Object,
            MapperMock.Object);
    }
}