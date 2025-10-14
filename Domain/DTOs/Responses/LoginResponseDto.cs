using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Responses
{
    public class LoginResponseDto
    {
        public bool IsAuthenticated { get; set; }

        public string Token { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
