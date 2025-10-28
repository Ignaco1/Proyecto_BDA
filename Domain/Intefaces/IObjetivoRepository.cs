using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Intefaces
{
    public interface IObjetivoRepository
    {
        Task<Objetivo> GetByIdAsync(int id);
        Task<Objetivo> AddAsync(Objetivo objetivo);
        Task<List<Objetivo>> GetAllAsync();
        Task UpdateAsync(Objetivo objetivo);

    }
}
