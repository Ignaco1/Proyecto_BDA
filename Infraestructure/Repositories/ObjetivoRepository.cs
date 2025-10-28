using Domain.Entities;
using Domain.Intefaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Objetivo>> GetAllAsync()
        {
            return await _context.Objetivos.ToListAsync();
        }

        public async Task<Objetivo> GetByIdAsync(int id)
        {
            return (await _context.Objetivos.FindAsync(id));
        }

        public async Task UpdateAsync(Objetivo objetivo)
        {
            _context.Objetivos.Update(objetivo);
            await _context.SaveChangesAsync();
        }
    }
}
