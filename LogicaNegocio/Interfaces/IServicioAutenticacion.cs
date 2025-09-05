using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.DTOs.Solicitudes;
using Dominio.DTOs.Respuestas;

namespace LogicaNegocio.Interfaces
{
    public interface IServicioAutenticacion
    {
        Task<LoginRespuestaDTO> Login(LoginSolicitudDTO loginSolicitud);

        Task<RespuestaUsuarioDTO> Registrar(CrearUsuarioDTO crearUsuario);
    }
}
