using Domain.DTOs.Requests.Cancelacion;
using Domain.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICancelacionService
    {
        Task<CancelacionResponseDto> CreateAsync(AddCancelacionDto dto);
        Task<List<CancelacionResponseDto>> GetAllAsync();
        Task<CancelacionResponseDto?> GetByReservaIdAsync(int reservaId);
    }
}
