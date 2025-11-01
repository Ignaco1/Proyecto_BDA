using Domain.Entities;
using Domain.Enums;
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
    public class ReservaRepository(AppDbContext context) : IReservaRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Reserva> AddAsync(Reserva reserva)
        {
            await _context.Reservas.AddAsync(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task<List<Reserva>> GetAllAsync()
        {
            return await _context.Reservas
                .Include(r => r.Cabaña)
                .Include(r => r.Cliente)
                .Include(r => r.Cancelacion)
                .ToListAsync();
        }

        public async Task<List<int>> GetAñosPorCabañaAsync(int idCabaña)
        {
            return await _context.Reservas
                .Where(r => r.IdCabaña == idCabaña)
                .Select(r => r.FechaEntrada.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Cabaña)
                .Include(r => r.Cliente)
                .Include(r => r.Cancelacion)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reserva>> GetReservasActivasAsync()
        {
            var hoy = DateTime.Today;
            return await _context.Reservas
                .Include(r => r.Cabaña)
                .Include(r => r.Cliente)
                .Where(r => r.Estado != EstadosReserva.Cancelada &&
                           r.FechaEntrada <= hoy && r.FechaSalida >= hoy)
                .ToListAsync();
        }

        public async Task<List<Reserva>> GetReservasPorCabañaAsync(int idCabaña)
        {
            return await _context.Reservas
                .Include(r => r.Cabaña)
                .Include(r => r.Cliente)
                .Where(r => r.IdCabaña == idCabaña)
                .ToListAsync();
        }

        public async Task UpdateAsync(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();
        }
    }
}
