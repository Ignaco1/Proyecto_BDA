using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests.Cancelacion
{
    public class UpdateCancelacionDto
    {
        [Required]
        public int Id { get; set; }

        public MotivosCancelacion? MotivosCancelacion { get; set; }
    }
}
