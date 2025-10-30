using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Responses
{
    public class CabañaResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Capacidad { get; set; } = string.Empty;
        public decimal PrecioPorNoche { get; set; }
        public string? Descripcion { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
