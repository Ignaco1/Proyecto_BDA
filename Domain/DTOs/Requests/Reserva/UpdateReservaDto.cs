using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests.Reserva
{
    public class UpdateReservaDto
    {
        [Required]
        public int Id { get; set; }

        public int? IdCabaña { get; set; }
        public int? IdCliente { get; set; }

        public DateTime? FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }

        [Range(0, 9999999.99)]
        public decimal? Total { get; set; }

        // Permitimos cambiar estado (Pendiente/Activa/Finalizada/Cancelada),
        // pero si pasa a Cancelada, la Cancelación se gestiona vía su endpoint.
        public EstadosReserva? Estado { get; set; }
    }
}
