using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests.Cancelacion
{
    public class AddCancelacionDto
    {
        [Required]
        public int ReservaId { get; set; }

        [Required]
        public MotivosCancelacion MotivosCancelacion { get; set; }
    }
}
