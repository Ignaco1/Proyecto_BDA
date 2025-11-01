using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reserva
    {
        public int Id { get; set; }
        
        public int IdCabaña { get; set; }
        [ForeignKey("IdCabaña")]
        public virtual Cabaña Cabaña { get; set; } = null!;

        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; } = null!;
       
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }

        public EstadosReserva Estado { get; set; } = EstadosReserva.Pendiente;

        public decimal Total { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public Cancelacion? Cancelacion { get; set; }
    }
}
