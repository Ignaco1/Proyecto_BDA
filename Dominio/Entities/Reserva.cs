using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Reserva
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public int IdCabaña { get; set; }
        public Cabaña Cabaña { get; set; } = null!;
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public decimal Precio { get; set; }
        public Estado Estado { get; set; } = Estado.Pendiente;

    }
}
