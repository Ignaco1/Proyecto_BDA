using Domain.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IClienteService
    {
        Task<List<ClienteResponseDto>> GetClientesAsync();
    }
}
