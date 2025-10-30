using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Objetivo
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public int? Mes { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MetaOcupacion { get; set; }

        public TipoObjetivo Tipo { get; set; } = TipoObjetivo.General;

        public int? IdCabaña { get; set; }
        [ForeignKey("IdCabaña")]
        public virtual Cabaña? Cabaña { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
