using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOs.Solicitudes
{
    public class CrearUsuarioDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; } = string.Empty;
        public string Telefono { get; set; }
        public Grupo Grupo { get; set; }
    }
}
