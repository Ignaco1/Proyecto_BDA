using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests.Ocupacion
{
    public class OcupacionDiariaDto
    {
        public int IdCabaña { get; set; }
        public string? NombreCabaña { get; set; }

        public int Año { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }

        public DateTime Fecha { get; set; }

        public bool Ocupada { get; set; }

        // texto opcional para mostrar (por ejemplo en chip)
        public string? Estado { get; set; }
    }
}
