using System.Net;
using System.Net.Http.Json;
using Auth.IntegrationTests.Auth;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;

namespace Auth.IntegrationTests.WellKnown;

public class WellKnownControllerTests(TestWebAppFactory factory) : BaseTests(factory)
{
    [Fact]
    public async Task OpenIdConfiguration_ReturnsOk_WithIssuerAndJwksUri()
    {
        // Act
        var response = await HttpClient.GetAsync("/.well-known/openid-configuration");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        content.Should().NotBeNull();
        content!.Should().ContainKey("issuer");
        content.Should().ContainKey("jwks_uri");

        content!["jwks_uri"].Should().EndWith("/.well-known/jwks.json");
        content["issuer"].Should().Be(Configuration["Jwt:Issuer"]);
    }

    [Fact]
    public async Task Jwks_ReturnsOk_WithValidKey()
    {
        // Act
        var response = await HttpClient.GetAsync("/.well-known/jwks.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jwks = await response.Content.ReadFromJsonAsync<JsonWebKeySet>();
        jwks.Should().NotBeNull();
        jwks!.Keys.Should().NotBeEmpty();

        var key = jwks.Keys.First();
        key.Kty.Should().Be("RSA");
        key.Use.Should().Be("sig");
        key.Kid.Should().Be("mercibus-key-main");
        key.Alg.Should().Be("RS256");
        key.N.Should().NotBeNullOrEmpty();
        key.E.Should().NotBeNullOrEmpty();
    }
}