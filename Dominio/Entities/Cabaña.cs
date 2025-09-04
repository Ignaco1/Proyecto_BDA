using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Cabaña
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Activa { get; set; } = true;
        public string? ImagenUrl { get; set; }
    }
}
