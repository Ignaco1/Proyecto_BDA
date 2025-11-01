using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Intefaces
{
    public interface IReservaRepository
    {
        Task<Reserva> AddAsync(Reserva reserva);
        Task<Reserva?> GetByIdAsync(int id);
        Task<List<Reserva>> GetAllAsync();
        Task UpdateAsync(Reserva reserva);
        Task<List<Reserva>> GetReservasActivasAsync();
        Task<List<Reserva>> GetReservasPorCabañaAsync(int idCabaña);
        Task<List<int>> GetAñosPorCabañaAsync(int idCabaña);
    }
}
