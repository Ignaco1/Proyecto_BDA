using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Business.Interfaces;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ObjetivoService(IObjetivoRepository objetivoRepository, IMapper mapper) : IObjetivoService
    {
        private readonly IObjetivoRepository _objetivoRepository = objetivoRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ObjetivoResponseDto> CreateObjetivoAsync(AddObjetivoDto objetivoDto)
        {
            var existentes = await _objetivoRepository.GetAllAsync();
            if (existentes.Any(o => o.Año == objetivoDto.Año && o.Mes == objetivoDto.Mes))
            {
                throw new InvalidOperationException("Ya existe un objetivo para ese período (año y mes).");
            }

            var nuevo = _mapper.Map<Objetivo>(objetivoDto);

            nuevo.FechaCreacion = DateTime.Now;
            nuevo.IsActive = true;

            var activoAnterior = existentes.FirstOrDefault(o => o.Año == objetivoDto.Año && o.IsActive);
            if (activoAnterior != null)
            {
                activoAnterior.IsActive = false;
                await _objetivoRepository.UpdateAsync(activoAnterior);
            }

            var creado = await _objetivoRepository.AddAsync(nuevo);

            return _mapper.Map<ObjetivoResponseDto>(creado);
        }

        public async Task<List<ObjetivoResponseDto>> GetAllObjetivosAsync()
        {
            var objetivos = await _objetivoRepository.GetAllAsync();
            return _mapper.Map<List<ObjetivoResponseDto>>(objetivos);
        }

        public async Task<ObjetivoResponseDto> GetObjetivoByIdAsync(int id)
        {
            var objetivo = await _objetivoRepository.GetByIdAsync(id);
            return _mapper.Map<ObjetivoResponseDto>(objetivo);
        }

        public async Task UpdateObjetivoAsync(UpdateObjetivoDto ObjetivoDto)
        {
            var objetivo = _mapper.Map<Objetivo>(ObjetivoDto);
            await _objetivoRepository.UpdateAsync(objetivo);
        }
    }
}
