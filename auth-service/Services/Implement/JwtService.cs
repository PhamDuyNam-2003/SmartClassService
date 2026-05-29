using auth_service.Models;
using auth_service.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth_service.Services.Implement;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");

        var secretKey =
            jwtSection["Key"]
            ?? throw new Exception("Jwt:Key missing");

        var issuer =
            jwtSection["Issuer"]
            ?? throw new Exception("Jwt:Issuer missing");

        var audience =
            jwtSection["Audience"]
            ?? throw new Exception("Jwt:Audience missing");

        var expireDays =
            int.TryParse(jwtSection["ExpireDays"], out var days)
                ? days
                : 7;

        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()
            ),

            new(
                JwtRegisteredClaimNames.Email,
                user.Email ?? string.Empty
            ),

            new(
                JwtRegisteredClaimNames.UniqueName,
                user.UserName ?? string.Empty
            ),

            new(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()
            ),

            new(
                ClaimTypes.Name,
                user.UserName ?? string.Empty
            ),

            new(
                ClaimTypes.Role,
                user.Role.ToString()
            ),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()
            )
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expireDays),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

   
}