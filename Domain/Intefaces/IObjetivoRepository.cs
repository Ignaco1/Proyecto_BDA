using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        Task<bool> ExistsAsync(Expression<Func<Objetivo, bool>> predicate);
        Task<List<Objetivo>> QueryAsync(Expression<Func<Objetivo, bool>> predicate, bool includeCabaña = true);

    }
}
