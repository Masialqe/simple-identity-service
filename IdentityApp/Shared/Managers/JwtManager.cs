using IdentityApp.Shared.Managers.Interrfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using IdentityApp.Shared.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using IdentityApp.Configuration;
using System.Security.Claims;
using System.Text;

namespace IdentityApp.Shared.Managers
{
    /// <summary>
    /// A service for managing JWT tokens, including creation of access tokens for users.
    /// </summary>
    public sealed class JwtManager(
        IOptions<JwtOptions> options,
        IOptions<SecretsOptions> secrets) : IJwtManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtManager"/> class.
        /// </summary>
        /// <param name="options">The JWT options containing expiration, issuer, and audience details.</param>
        /// <param name="secrets">The secrets options containing the secret key for signing tokens.</param>
        public string CreateAccessToken(User user)
        {
            var secretKey = secrets.Value.SecretKey!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = CreateTokenDescriptor(user, credentials);

            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }

        /// <summary>
        /// Creates a JWT access token for the specified <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user for whom the access token is being created.</param>
        /// <returns>A JWT access token as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="user"/> is null.</exception>
        private SecurityTokenDescriptor CreateTokenDescriptor(User user, SigningCredentials credentials)
        {
            var jwtOptions = options.Value;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login)

                ]),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
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
