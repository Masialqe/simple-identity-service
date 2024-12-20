using Microsoft.IdentityModel.JsonWebTokens;
using IdentityApp.Managers.Interrfaces;
using Microsoft.IdentityModel.Tokens;
using IdentityApp.Users.Models;
using System.Security.Claims;
using System.Text;

namespace IdentityApp.Managers
{
    public sealed class JwtManager(
        IConfiguration configuration) : IJwtManager
    {
        public string CreateAccessToken(User user)
        {
            string secretKey = configuration["SECRET_KEY"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = CreateTokenDescriptor(user, credentials);

            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }

        private SecurityTokenDescriptor CreateTokenDescriptor(User user, SigningCredentials credentials)
        {
            var tokenDescriptor =  new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login)

                ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            foreach (var role in user.Roles)
            {
                var claim = new Claim(ClaimTypes.Role, role.Name);
                tokenDescriptor.Subject.AddClaim(claim);
            }

            return tokenDescriptor;
        }
    }
}
