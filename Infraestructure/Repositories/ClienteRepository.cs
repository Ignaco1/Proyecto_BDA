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
    public class ClienteRepository(AppDbContext ctx) : IClienteRepository
    {
        private readonly AppDbContext _ctx = ctx;

        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _ctx.Clientes
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }
    }
}
