using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests.Ocupacion
{
    public class OcupacionMensualDto
    {
        public int IdCabaña { get; set; }
        public string NombreCabaña { get; set; } = "";
        public int Año { get; set; }
        public int Mes { get; set; } 
        public int NochesReservadas { get; set; }
        public int NochesDisponibles { get; set; }
        public decimal PorcentajeOcupacion { get; set; }
        public decimal? MetaObjetivo { get; set; } 
        public string Semaforo { get; set; } = "Rojo";
    }
}
