using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        private readonly string secretKey = config.GetSection("Jwt").GetValue<string>("key")!;

        public string GenerateToken(User user)
        {
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            claims.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
            claims.AddClaim(new Claim("Nombre", user.Name));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMonths(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokencreado = tokenHandler.WriteToken(tokenConfig);

            return tokencreado;
        }
    }
}
