
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Revisio.Application.Common.Interfaces;
using Revisio.Domain.Entities;
using Revisio.Infrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Revisio.Infrastructure.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtSetting jwtSetting;
        public JwtGenerator(IOptions<JwtSetting> options)
        {
            jwtSetting = options.Value;
        }
        public string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(ClaimTypes.Role, user.Role.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                expires:DateTime.UtcNow.AddMinutes(jwtSetting.Expire),
                signingCredentials:cred
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
