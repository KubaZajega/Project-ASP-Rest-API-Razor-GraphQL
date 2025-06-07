using Moq;
using Microsoft.Extensions.Configuration;
using Jakub_Zajega_14987.Infrastructure.Identity;
using Jakub_Zajega_14987.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Jakub_Zajega_14987.Infrastructure.Tests
{
    public class JwtTokenGeneratorTests
    {
        [Fact]
        public void GenerateToken_ForValidUser_ShouldReturnValidToken()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            var mockJwtSettingsSection = new Mock<IConfigurationSection>();

            mockJwtSettingsSection.Setup(x => x["Key"]).Returns("THIS_IS_A_MUCH_LONGER_AND_MORE_SECURE_SECRET_KEY_FOR_JWT_GENERATION_JAKUB_ZAJEGA_MINIMUM_64_CHARS_LONG");
            mockJwtSettingsSection.Setup(x => x["Issuer"]).Returns("JZ_API");
            mockJwtSettingsSection.Setup(x => x["Audience"]).Returns("JZ_API_CLIENT");
            mockJwtSettingsSection.Setup(x => x["DurationInMinutes"]).Returns("60");

            mockConfiguration.Setup(x => x.GetSection("JwtSettings")).Returns(mockJwtSettingsSection.Object);

            var tokenGenerator = new JwtTokenGenerator(mockConfiguration.Object);

            var user = new User { Id = 1, Username = "test", Role = "User" };

            var token = tokenGenerator.GenerateToken(user);

            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "sub");

            Assert.Equal("1", userIdClaim?.Value);
        }
    }
}
