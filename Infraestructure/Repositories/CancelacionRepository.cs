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
    public class CancelacionRepository(AppDbContext context) : ICancelacionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Cancelacion> AddAsync(Cancelacion cancelacion)
        {
            await _context.Cancelaciones.AddAsync(cancelacion);
            await _context.SaveChangesAsync();
            return cancelacion;
        }

        public async Task<List<Cancelacion>> GetAllAsync()
        {
            return await _context.Cancelaciones
                .Include(c => c.Reserva)
                    .ThenInclude(r => r.Cabaña)
                .Include(c => c.Reserva)
                    .ThenInclude(r => r.Cliente)
                .ToListAsync();
        }

        public async Task<Cancelacion?> GetByReservaIdAsync(int reservaId)
        {
            return await _context.Cancelaciones
                .Include(c => c.Reserva)
                    .ThenInclude(r => r.Cabaña)
                .Include(c => c.Reserva)
                    .ThenInclude(r => r.Cliente)
                .FirstOrDefaultAsync(c => c.ReservaId == reservaId);
        }
    }
}
