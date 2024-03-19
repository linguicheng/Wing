using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Wing.UI.Auth
{
    public class JwtAuth : IAuth
    {
        private readonly JwtSetting _setting;

        public JwtAuth(JwtSetting setting)
        {
            _setting = setting;
        }

        public string GetToken(string name)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
            };
            return GetToken(claims);
        }

        public string GetToken(params Claim[] claims)
        {
            var now = DateTime.UtcNow;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_setting.Secret));
            var jwt = new JwtSecurityToken(
                issuer: _setting.Iss,
                audience: _setting.Aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(_setting.Expire)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GetToken()
        {
            return GetToken(new Claim[] { });
        }
    }
}
