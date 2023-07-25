using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using vacinacao_backend.Models;

namespace vacinacao_backend.Services {
    public class TokenService {

        private readonly string jwtKey;

        public TokenService(IConfiguration configuration) {
            jwtKey = configuration["JwtSettings:Key"]!;
        }

        public string GenerateAccessToken(Usuario usuario) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            var claims = new List<Claim> {
                new("isAdmin", usuario.IsAdmin.ToString().ToLower()),
                new(JwtRegisteredClaimNames.Email, usuario.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Issuer = "id.teste.com",
                Audience = "vacinacao.teste.com",
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        public string GenerateRandomHexToken() {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
