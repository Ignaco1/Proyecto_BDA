using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOs.Respuestas
{
    public class LoginRespuestaDTO
    {
        public bool Autenticacion {  get; set; }
        public string Token { get; set; }
        public string Mensaje { get; set; }
    }
}
