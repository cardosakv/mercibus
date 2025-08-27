using System.Security.Cryptography;
using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Controllers;

[Route(".well-known")]
public class WellKnownController(IConfiguration configuration) : BaseController
{
    [HttpGet("openid-configuration")]
    public IActionResult GetOpenIdConfiguration()
    {
        return Ok(
            new
            {
                issuer = configuration["Jwt:Issuer"],
                jwks_uri = $"{Request.Scheme}://{Request.Host}/.well-known/jwks.json"
            });
    }

    [HttpGet("jwks.json")]
    public IActionResult GetJwks()
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(System.IO.File.ReadAllText(configuration["Jwt:PublicKeyPath"] ?? "jwt_pub_key.pem"));
        var parameters = rsa.ExportParameters(false);

        var key = new JsonWebKey
        {
            Kty = "RSA",
            Use = "sig",
            Kid = "mercibus-key-main",
            Alg = "RS256",
            N = Base64UrlEncoder.Encode(parameters.Modulus),
            E = Base64UrlEncoder.Encode(parameters.Exponent)
        };

        return Ok(
            new
            {
                keys = new[]
                {
                    key
                }
            });
    }
}