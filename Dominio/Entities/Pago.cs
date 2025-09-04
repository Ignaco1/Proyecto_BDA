using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Pago
    {
        public int Id { get; set; }
        public int IdReserva { get; set; }
        public Reserva Reserva { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = null;
        public string IdTransaccion { get; set; } = null;
    }
}
