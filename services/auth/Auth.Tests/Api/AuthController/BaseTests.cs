using Auth.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Auth.Tests.Api.AuthController;

/// <summary>
/// Base test class for auth controller.
/// </summary>
public abstract class BaseTests
{
    protected readonly Auth.Api.Controllers.AuthController Controller;

    protected readonly Mock<IAuthService> AuthServiceMock;
    protected readonly Mock<IConfiguration> ConfigurationMock;

    protected BaseTests()
    {
        AuthServiceMock = new Mock<IAuthService>();
        ConfigurationMock = new Mock<IConfiguration>();

        Controller = new Auth.Api.Controllers.AuthController(AuthServiceMock.Object, ConfigurationMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }
}