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
    public class CabañaService(ICabañaRepository cabañaRepository, IMapper mapper) : ICabañaService
    {
        private readonly ICabañaRepository _cabañaRepository = cabañaRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<List<CabañaResponseDto>> GetAllAsync()
        {
            var cabañas = await _cabañaRepository.GetAllAsync();
            return _mapper.Map<List<CabañaResponseDto>>(cabañas);
        }
    }
}
