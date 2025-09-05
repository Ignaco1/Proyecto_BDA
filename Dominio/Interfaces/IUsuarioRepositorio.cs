using Dominio.DTOs.Respuestas;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> ObtenerUsuarioPorId(int Id);
        Task<IEnumerable<Usuario>> ObtenerTodos();
        Task<Usuario> CrearUsuario(Usuario usuario);
        Task<Usuario> ObtenerUsuarioPorNombre(string nombreUsuario);
        Task<Usuario> UpdateUsuario(Usuario usuario);
        Task<Usuario> ObtenerUsuarioPorEmail(string email);
    }
}
