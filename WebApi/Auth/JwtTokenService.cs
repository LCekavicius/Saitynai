using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Auth
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(string userId, IEnumerable<string> userRoles, int companyId);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly SymmetricSecurityKey _authSigningKey;
        private readonly string _issuer;
        private readonly string _audience;
        public JwtTokenService(IConfiguration configuration)
        {
            _audience = configuration["JWT:ValidAudience"];
            _issuer = configuration["JWT:ValidIssuer"];
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
        }

        public string CreateAccessToken(string userId, IEnumerable<string> userRoles, int companyId)
        {
            var authClaims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, userId),
                new ("companyId", companyId.ToString())
            };

            authClaims.AddRange(userRoles.Select(e => new Claim(ClaimTypes.Role, e)));

            var _accessSecurityToken = new JwtSecurityToken
                (
                    issuer: _issuer,
                    audience: _audience,
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(_accessSecurityToken);
        }
    }
}
