using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Intefaces
{
    public interface ICancelacionRepository
    {
        Task<Cancelacion> AddAsync(Cancelacion cancelacion);
        Task<List<Cancelacion>> GetAllAsync();
        Task<Cancelacion?> GetByReservaIdAsync(int reservaId);
    }
}
