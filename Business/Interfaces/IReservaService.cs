using Domain.DTOs.Requests.Reserva;
using Domain.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IReservaService
    {
        Task<ReservaResponseDto> CreateReservaAsync(AddReservaDto dto);
        Task<List<ReservaResponseDto>> GetAllAsync();
        Task<ReservaResponseDto?> GetByIdAsync(int id);
        Task UpdateAsync(UpdateReservaDto dto);
        Task<List<ReservaResponseDto>> GetByCabañaAsync(int idCabaña);
        Task<List<int>> GetAñosPorCabañaAsync(int idCabaña);
    }
}
