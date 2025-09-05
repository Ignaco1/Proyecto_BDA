using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Aplicacion.Interfaces
{
    public interface ITokenService
    {
        public string GenerarToken(Usuario usuario);
    }
}
