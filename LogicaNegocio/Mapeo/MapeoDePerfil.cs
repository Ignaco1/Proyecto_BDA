using AutoMapper;
using Dominio.DTOs.Respuestas;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Mapeo
{
    public class MapeoDePerfil:Profile
    {
        public MapeoDePerfil()
        {
            CreateMap<Usuario, RespuestaUsuarioDTO>();
        }
    }
}
