using Aplicacion.Interfaces;
using AutoMapper;
using Dominio.DTOs.Respuestas;
using Dominio.DTOs.Solicitudes;
using Dominio.Entities;
using Dominio.Interfaces;
using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Servicios
{
    public class ServicioAutenticacion : IServicioAutenticacion
    {
        private readonly ITokenService _TokenService;
        private readonly IUsuarioRepositorio _UsuarioRepositorio;
        private readonly IMapper _mapper;

        public ServicioAutenticacion(ITokenService tokenService, IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
        {
            _TokenService = tokenService;
            _UsuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        public async Task<LoginRespuestaDTO> Login(LoginSolicitudDTO loginSolicitud)
        {
            var usuario = await _UsuarioRepositorio.ObtenerUsuarioPorNombre(loginSolicitud.NombreUsuario);
            var autenticacion = usuario != null && BCrypt.Net.BCrypt.Verify(loginSolicitud.Contraseña, usuario.Contraseña);
            if (autenticacion && usuario!.Activo)
            {
                var token = _TokenService.GenerarToken(usuario!);
                return new LoginRespuestaDTO
                {
                    Autenticacion = true,
                    Token = token,
                    Mensaje = "Inicio de sesion exitoso."
                };
            }
            else
            {
                return new LoginRespuestaDTO
                {
                    Autenticacion = false,
                    Token = string.Empty,
                    Mensaje = "Credenciales incorrectas."
                };
            }
        }

        public async Task<RespuestaUsuarioDTO> Registrar(CrearUsuarioDTO crearUsuario)
        {
            var usuario = new Usuario
            {
                Nombre = crearUsuario.Nombre,
                Apellido = crearUsuario.Apellido,
                Email = crearUsuario.Email,
                NombreUsuario = crearUsuario.NombreUsuario,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(crearUsuario.Contraseña),
                Telefono = crearUsuario.Telefono,
                Grupo = crearUsuario.Grupo,
            };

            var usuarioCreado = await _UsuarioRepositorio.CrearUsuario(usuario);
            return _mapper.Map<RespuestaUsuarioDTO>(usuarioCreado);
        }
    }
}
