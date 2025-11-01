using Domain.Entities;
using Domain.Intefaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class ObjetivoRepository(AppDbContext context) : IObjetivoRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Objetivo> AddAsync(Objetivo objetivo)
        {
            await _context.Objetivos.AddAsync(objetivo);
            await _context.SaveChangesAsync();
            return objetivo;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Objetivo, bool>> predicate)
        {
            return await _context.Objetivos.AnyAsync(predicate);
        }

        public async Task<List<Objetivo>> GetAllAsync()
        {
            return await _context.Objetivos
                .Include(o => o.Cabaña)
                .ToListAsync();
        }

        public async Task<Objetivo> GetByIdAsync(int id)
        {
            return await _context.Objetivos
                .Include(o => o.Cabaña)
                .FirstOrDefaultAsync(o => o.Id == id);  
        }

        public async Task<List<Objetivo>> QueryAsync(Expression<Func<Objetivo, bool>> predicate, bool includeCabaña = true)
        {
            var query = _context.Objetivos.AsQueryable();
            if (includeCabaña)
                query = query.Include(o => o.Cabaña);

            return await query.Where(predicate).ToListAsync();
        }

        public async Task UpdateAsync(Objetivo objetivo)
        {
            _context.Objetivos.Update(objetivo);
            await _context.SaveChangesAsync();
        }
    }
}
