using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Responses
{
    public class CancelacionResponseDto
    {
        public int Id { get; set; }
        public int ReservaId { get; set; }

        public DateTime Fecha { get; set; }

        public MotivosCancelacion MotivosCancelacion { get; set; }

        public int IdCabaña { get; set; }
        public string? NombreCabaña { get; set; }
        public int IdCliente { get; set; }
        public string? NombreCliente { get; set; }

        public DateTime FechaIngreso { get; set; }
        public DateTime FechaEgreso { get; set; }
        public decimal Total { get; set; }
    }
}
