using Aplicacion.Interfaces;
using Dominio.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios
{
    public class ServicioDeToken(IConfiguration config) : ITokenService
    {
       private readonly string clave = config.GetSection("Jwt").GetValue<string>("Key")!;
        public string GenerarToken(Usuario usuario)
        {
            var BytesClave = Encoding.ASCII.GetBytes(clave);
            var claims = new ClaimsIdentity();
            var nombre = usuario.Nombre + " " + usuario.Apellido;
            claims.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreUsuario));
            claims.AddClaim(new Claim(ClaimTypes.Role, usuario.Grupo.ToString()));
            claims.AddClaim(new Claim("Nombre", nombre));

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMonths(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(BytesClave), SecurityAlgorithms.HmacSha256Signature)
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            var TokenConfig = TokenHandler.CreateToken(TokenDescriptor);

            string tokenCreado = TokenHandler.WriteToken(TokenConfig);

            return tokenCreado;

        }
    }
}
