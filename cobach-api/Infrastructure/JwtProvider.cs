using cobach_api.Features.Seguridad.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace cobach_api.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _expires;
        public JwtProvider(string key, string issuer, string audience, string expires)
        {
            _key = key;
            _issuer = issuer;
            _audience = audience;
            _expires = expires;
        }

        public string Generate((string userId, string userName) userDetails)
        {
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new("UserId", userDetails.userId),
                new(ClaimTypes.Name, userDetails.userName)
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey (
                    Encoding.UTF8.GetBytes(_key)
                ),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                null,
                DateTime.UtcNow.AddMinutes(Convert.ToDouble(_expires)),
                signingCredentials
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
