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
            if (objetivoDto == null)
                throw new ArgumentException("El objetivo no puede ser nulo.");

            if (objetivoDto.Año <= 0)
                throw new ArgumentException("El campo 'Año' es obligatorio y debe ser mayor que cero.");
            if (objetivoDto.Año > DateTime.Now.Year)
                throw new ArgumentException("El año no puede ser mayor al actual.");

            if (!objetivoDto.Mes.HasValue)
                throw new ArgumentException("El campo 'Mes' es obligatorio.");
            if (objetivoDto.Mes < 1 || objetivoDto.Mes > 12)
                throw new ArgumentException("El campo 'Mes' debe estar entre 1 y 12.");

            if (objetivoDto.MetaOcupacion <= 0)
                throw new ArgumentException("La 'Meta de Ocupación' debe ser mayor que cero.");
            if (objetivoDto.MetaOcupacion > 100)
                throw new ArgumentException("La 'Meta de Ocupación' no puede superar el 100%.");


            var existentes = await _objetivoRepository.GetAllAsync();
            if (existentes.Any(x => x.Año == objetivoDto.Año && x.Mes == objetivoDto.Mes))
                throw new InvalidOperationException($"Ya existe un objetivo para {objetivoDto.Mes}/{objetivoDto.Año}.");

            foreach (var obj in existentes.Where(x => x.IsActive))
            {
                obj.IsActive = false;
                await _objetivoRepository.UpdateAsync(obj);
            }

            var nuevoObjetivo = _mapper.Map<Objetivo>(objetivoDto);
            nuevoObjetivo.FechaCreacion = DateTime.Now;
            nuevoObjetivo.IsActive = true;

            var creado = await _objetivoRepository.AddAsync(nuevoObjetivo);
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
            var existing = await _objetivoRepository.GetByIdAsync(ObjetivoDto.Id);
            
            if (existing == null)
                throw new Exception("El objetivo no existe.");


            if (ObjetivoDto.Año <= 0)
                throw new ArgumentException("El campo 'Año' es obligatorio y debe ser mayor que cero.");
            if (ObjetivoDto.Año > DateTime.Now.Year)
                throw new ArgumentException("El año no puede ser mayor al actual.");

            if (!ObjetivoDto.Mes.HasValue)
                throw new ArgumentException("El campo 'Mes' es obligatorio.");
            if (ObjetivoDto.Mes < 1 || ObjetivoDto.Mes > 12)
                throw new ArgumentException("El campo 'Mes' debe estar entre 1 y 12.");

            if (ObjetivoDto.MetaOcupacion <= 0)
                throw new ArgumentException("La 'Meta de Ocupación' debe ser mayor que cero.");
            if (ObjetivoDto.MetaOcupacion > 100)
                throw new ArgumentException("La 'Meta de Ocupación' no puede superar el 100%.");


            var allObjetivos = await _objetivoRepository.GetAllAsync();
            var duplicado = allObjetivos.Any(o =>
                o.Id != ObjetivoDto.Id &&               
                o.Año == ObjetivoDto.Año &&
                o.Mes == ObjetivoDto.Mes);

            if (duplicado)
                throw new Exception($"Ya existe un objetivo para {ObjetivoDto.Mes}/{ObjetivoDto.Año}.");

            existing.Año = ObjetivoDto.Año;
            existing.Mes = ObjetivoDto.Mes;
            existing.MetaOcupacion = ObjetivoDto.MetaOcupacion;

            await _objetivoRepository.UpdateAsync(existing);
        }
    }
}
