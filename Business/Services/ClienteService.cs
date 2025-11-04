using AutoMapper;
using Business.Interfaces;
using Domain.DTOs.Responses;
using Domain.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClienteService(IClienteRepository repo, IMapper mapper) : IClienteService
    {
        private readonly IClienteRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ClienteResponseDto>> GetClientesAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<List<ClienteResponseDto>>(entities);
        }
    }
}
