using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cancelacion
    {
        public int Id { get; set; }
        public int ReservaId { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("ReservaId")]
        public virtual Reserva Reserva { get; set; } = null!;
        public MotivosCancelacion MotivosCancelacion { get; set; }
    }
}
