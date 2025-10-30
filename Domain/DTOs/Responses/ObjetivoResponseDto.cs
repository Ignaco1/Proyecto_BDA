using Domain.Enums; 
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTOs.Responses
{
    public class ObjetivoResponseDto
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public int? Mes { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MetaOcupacion { get; set; }

        public TipoObjetivo Tipo { get; set; } = TipoObjetivo.General; 
        public int? IdCabaña { get; set; }
        public string? NombreCabaña { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
