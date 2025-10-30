using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Business.Interfaces;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Enums;
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

        public async Task<ObjetivoResponseDto> CreateObjetivoAsync(AddObjetivoDto dto)
        {
            if (dto == null) throw new ArgumentException("El objetivo no puede ser nulo.");

            if (dto.Año <= 0) throw new ArgumentException("El campo 'Año' debe ser mayor que cero.");
            if (dto.Año > DateTime.Now.Year) throw new ArgumentException("El año no puede ser mayor al actual.");
            if (dto.MetaOcupacion <= 0) throw new ArgumentException("La 'Meta de Ocupación' debe ser mayor que cero.");
            if (dto.MetaOcupacion > 100) throw new ArgumentException("La 'Meta de Ocupación' no puede superar el 100%.");

            var existentes = await _objetivoRepository.GetAllAsync();

            switch (dto.Tipo)
            {
                case TipoObjetivo.General:
                    dto.Mes = null;
                    dto.IdCabaña = null;

                    if (existentes.Any(x => x.Tipo == TipoObjetivo.General && x.Año == dto.Año))
                        throw new InvalidOperationException($"Ya existe un objetivo general para el año {dto.Año}.");

                    foreach (var obj in existentes.Where(x => x.Tipo == TipoObjetivo.General && x.IsActive))
                    {
                        obj.IsActive = false;
                        await _objetivoRepository.UpdateAsync(obj);
                    }
                    break;

                case TipoObjetivo.Anual:
                    if (dto.IdCabaña is null) throw new ArgumentException("Debe seleccionar una cabaña.");
                    dto.Mes = null;
                    if (existentes.Any(x => x.Tipo == TipoObjetivo.Anual && x.Año == dto.Año && x.IdCabaña == dto.IdCabaña))
                        throw new InvalidOperationException($"Ya existe un objetivo anual para la cabaña {dto.IdCabaña} en {dto.Año}.");
                    break;

                case TipoObjetivo.Mensual:
                    if (dto.IdCabaña is null) throw new ArgumentException("Debe seleccionar una cabaña.");
                    if (!dto.Mes.HasValue || dto.Mes < 1 || dto.Mes > 12)
                        throw new ArgumentException("El campo 'Mes' debe estar entre 1 y 12.");
                    if (existentes.Any(x => x.Tipo == TipoObjetivo.Mensual && x.Año == dto.Año && x.Mes == dto.Mes && x.IdCabaña == dto.IdCabaña))
                        throw new InvalidOperationException($"Ya existe un objetivo mensual para la cabaña {dto.IdCabaña} en {dto.Mes}/{dto.Año}.");
                    break;
            }

            var nuevo = _mapper.Map<Objetivo>(dto);
            nuevo.FechaCreacion = DateTime.Now;
            nuevo.IsActive = true;

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

        public async Task UpdateObjetivoAsync(UpdateObjetivoDto dto)
        {
            var existing = await _objetivoRepository.GetByIdAsync(dto.Id);
            if (existing == null) throw new Exception("El objetivo no existe.");


            if (dto.Año <= 0) throw new ArgumentException("El campo 'Año' debe ser mayor que cero.");
            if (dto.Año > DateTime.Now.Year) throw new ArgumentException("El año no puede ser mayor al actual.");
            if (dto.MetaOcupacion <= 0) throw new ArgumentException("La 'Meta de Ocupación' debe ser mayor que cero.");
            if (dto.MetaOcupacion > 100) throw new ArgumentException("La 'Meta de Ocupación' no puede superar el 100%.");

            var todos = await _objetivoRepository.GetAllAsync();

            switch (existing.Tipo)
            {
                case TipoObjetivo.General:
                    dto.Mes = null;
                    dto.IdCabaña = null;
                    if (todos.Any(o => o.Id != dto.Id && o.Tipo == TipoObjetivo.General && o.Año == dto.Año))
                        throw new Exception($"Ya existe un objetivo general para el año {dto.Año}.");
                    break;

                case TipoObjetivo.Anual:
                    if (dto.IdCabaña is null) throw new ArgumentException("Debe seleccionar una cabaña.");
                    dto.Mes = null;
                    if (todos.Any(o => o.Id != dto.Id && o.Tipo == TipoObjetivo.Anual && o.Año == dto.Año && o.IdCabaña == dto.IdCabaña))
                        throw new Exception($"Ya existe un objetivo anual para esa cabaña en {dto.Año}.");
                    break;

                case TipoObjetivo.Mensual:
                    if (dto.IdCabaña is null) throw new ArgumentException("Debe seleccionar una cabaña.");
                    if (!dto.Mes.HasValue || dto.Mes < 1 || dto.Mes > 12)
                        throw new ArgumentException("El campo 'Mes' debe estar entre 1 y 12.");
                    if (todos.Any(o => o.Id != dto.Id && o.Tipo == TipoObjetivo.Mensual && o.Año == dto.Año && o.Mes == dto.Mes && o.IdCabaña == dto.IdCabaña))
                        throw new Exception($"Ya existe un objetivo mensual para esa cabaña en {dto.Mes}/{dto.Año}.");
                    break;
            }

            existing.Año = dto.Año;
            existing.Mes = dto.Mes;
            existing.MetaOcupacion = dto.MetaOcupacion;
            existing.IdCabaña = dto.IdCabaña;

            await _objetivoRepository.UpdateAsync(existing);
        }

    }
}
