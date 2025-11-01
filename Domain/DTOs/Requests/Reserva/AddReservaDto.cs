using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests.Reserva
{
    public class AddReservaDto
    {
        [Required]
        public int IdCabaña { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public DateTime FechaEntrada { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        [Range(0, 9999999.99)]
        public decimal Total { get; set; }

        public EstadosReserva Estado { get; set; } = EstadosReserva.Pendiente;
    }
}

