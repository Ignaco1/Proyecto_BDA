using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string? PhoneNumber { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Employee;
    }
}
