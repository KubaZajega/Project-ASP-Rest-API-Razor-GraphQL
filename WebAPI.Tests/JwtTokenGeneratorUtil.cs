using Jakub_Zajega_14987.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jakub_Zajega_14987.WebAPI.Tests;

public static class JwtTokenGeneratorUtil
{
    public static string GenerateTokenForUser(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS_IS_A_MUCH_LONGER_AND_MORE_SECURE_SECRET_KEY_FOR_JWT_GENERATION_JAKUB_ZAJEGA_MINIMUM_64_CHARS_LONG"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()), // Dodajemy dla spójności
            new(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = "JZ_API",
            Audience = "JZ_API_CLIENT",
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}