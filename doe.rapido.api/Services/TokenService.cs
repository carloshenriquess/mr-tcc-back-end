using doe.rapido.api.Models;
using doe.rapido.business.DML;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace doe.rapido.api.Services
{
    public class TokenService
    {

        public class RefreshToken
        {
            public string token { get; set; } = String.Empty;
            public string refreshToken { get; set; } = String.Empty;
        }

        private static volatile ConcurrentDictionary<int, string> _refreshTokens = new();

        #region[métodos publicos]
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.Name.ToString()),
                    new Claim(ClaimTypes.Email,user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static string GenerateToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var TokenValidatorParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, TokenValidatorParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("Token inválido");
            return principal;
        }

        public static void SaveRefreshToken(int id, string refreshToken)
        {
            _refreshTokens.AddOrUpdate(id, refreshToken, (key, oldValue) => refreshToken);
        }
        public static string GetRefreshToken(int id)
        {
            return _refreshTokens.FirstOrDefault(x => x.Key == id).Value;
        }
        public static Task DeleteRefreshToken(int id)
        {
            _refreshTokens.TryRemove(id, out string value);
            return Task.CompletedTask;
        }

        #endregion
    }
}
