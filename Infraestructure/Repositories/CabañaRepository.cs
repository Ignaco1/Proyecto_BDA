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
    public class CabañaRepository(AppDbContext context) : ICabañaRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Cabaña>> GetAllAsync()
        {
            return await _context.Cabañas.ToListAsync();
        }
    }
}
