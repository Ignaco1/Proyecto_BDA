using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Responses
{
    public class ReservaResponseDto
    {
        public int Id { get; set; }

        public int IdCabaña { get; set; }
        public string? NombreCabaña { get; set; }

        public int IdCliente { get; set; }
        public string? NombreCliente { get; set; }

        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }

        public Domain.Enums.EstadosReserva Estado { get; set; }

        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }

        public bool Cancelada => Estado == Domain.Enums.EstadosReserva.Cancelada;
        public DateTime? FechaCancelacion { get; set; }
        public MotivosCancelacion? MotivoCancelacion { get; set; }
    }
}
