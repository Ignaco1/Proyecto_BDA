using Dominio.DTOs.Solicitudes;
using LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorDeAutenticacion : ControllerBase
    {
        private readonly IServicioAutenticacion _servicioAutenticacion;

        public ControladorDeAutenticacion(IServicioAutenticacion servicioAutenticacion)
        {
            _servicioAutenticacion = servicioAutenticacion;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginSolicitudDTO loginSolicitud)
        {
            if (loginSolicitud == null)
            {
                return BadRequest("Solicitud de inicio de sesion invalida.");
            }
            
            var respuesta = await _servicioAutenticacion.Login(loginSolicitud);

            if (respuesta.Autenticacion)
            {
                return Ok(respuesta);
            }
            else
            {
                return Unauthorized(respuesta.Mensaje);
            }
        }

        [HttpPost("registrar")]

        public async Task<IActionResult> Registrar([FromBody] CrearUsuarioDTO crearUsuario)
        {
            if (crearUsuario == null)
            {
                return BadRequest("Solicitud de registración invalida.");
            }

            var respuesta = await _servicioAutenticacion.Registrar(crearUsuario);

            if (respuesta != null)
            {
                return CreatedAtAction(nameof(Registrar), new { id = respuesta.Id }, respuesta);
            }
            else
            {
                return BadRequest("Falla de registración.");
            }
        }
    }
}
