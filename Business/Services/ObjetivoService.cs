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
    public class ObjetivoService(IObjetivoRepository objetivoRepository, IReservaRepository reservaRepository, IMapper mapper, ICabañaRepository cabañaRepository) : IObjetivoService
    {
        private readonly IObjetivoRepository _objetivoRepository = objetivoRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly ICabañaRepository _cabañaRepository = cabañaRepository;
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

                    foreach (var obj in existentes.Where(x => x.Tipo == TipoObjetivo.General && x.IsActive == true))
                    {
                        obj.IsActive = false;
                        await _objetivoRepository.UpdateAsync(obj);
                    }
                    break;

                case TipoObjetivo.Anual:
                    if (dto.IdCabaña is null) throw new ArgumentException("Debe seleccionar una cabaña.");
                    dto.Mes = null;
                    var nombreAnual = await GetNombreCabañaAsync(dto.IdCabaña);
                    if (existentes.Any(x => x.Tipo == TipoObjetivo.Anual && x.Año == dto.Año && x.IdCabaña == dto.IdCabaña))
                        throw new InvalidOperationException($"Ya existe un objetivo anual para la{nombreAnual} en {dto.Año}.");
                    break;

                case TipoObjetivo.Mensual:
                    if (dto.IdCabaña is null) throw new ArgumentException("Debe seleccionar una cabaña.");
                    if (!dto.Mes.HasValue || dto.Mes < 1 || dto.Mes > 12)
                        throw new ArgumentException("El campo 'Mes' debe estar entre 1 y 12.");
                    var nombreMensual = await GetNombreCabañaAsync(dto.IdCabaña);
                    if (existentes.Any(x => x.Tipo == TipoObjetivo.Mensual && x.Año == dto.Año && x.Mes == dto.Mes && x.IdCabaña == dto.IdCabaña && x.IsActive == true))
                        throw new InvalidOperationException($"Ya existe un objetivo mensual para la {nombreMensual} en {dto.Mes}/{dto.Año}.");
                    break;
            }

            var nuevo = _mapper.Map<Objetivo>(dto);
            nuevo.FechaCreacion = DateTime.Now;
            nuevo.IsActive = true;

            var creado = await _objetivoRepository.AddAsync(nuevo);
            return _mapper.Map<ObjetivoResponseDto>(creado);
        }

        private async Task<string> GetNombreCabañaAsync(int? idCabaña)
        {
            if (!idCabaña.HasValue) return "(sin cabaña)";
            var nombre = await _cabañaRepository.GetNombreByIdAsync(idCabaña.Value);
            return string.IsNullOrWhiteSpace(nombre) ? $"ID {idCabaña.Value}" : nombre;
        }

        public async Task<ObjetivoResponseDto> CreateObjetivoAnualAsync(AddObjetivoDto Dto)
        {
            if (Dto == null) throw new ArgumentException("El objetivo no puede ser nulo.");

            if (Dto.IdCabaña is null || Dto.IdCabaña <= 0)
                throw new ArgumentException("Debe seleccionar una cabaña para un objetivo anual.");
            if (Dto.Año <= 0) throw new ArgumentException("El campo 'Año' debe ser mayor que cero.");
            if (Dto.Año > DateTime.Now.Year) throw new ArgumentException("El año no puede ser mayor al actual.");

            Dto.Tipo = TipoObjetivo.Anual;
            Dto.Mes = null;                

            if (Dto.MetaOcupacion <= 0) throw new ArgumentException("La 'Meta de Ocupación' debe ser mayor que cero.");
            if (Dto.MetaOcupacion > 100) throw new ArgumentException("La 'Meta de Ocupación' no puede superar el 100%.");

            var existentes = await _objetivoRepository.GetAllAsync();
            var duplicado = existentes.Any(o =>
                o.Tipo == TipoObjetivo.Anual &&
                o.IdCabaña == Dto.IdCabaña &&
                o.Año == Dto.Año);

            if (duplicado)
                throw new InvalidOperationException($"Ya existe un objetivo anual para la cabaña {Dto.IdCabaña} en {Dto.Año}.");

            var entity = _mapper.Map<Objetivo>(Dto);
            entity.FechaCreacion = DateTime.Now;
            entity.IsActive = true;

            var creado = await _objetivoRepository.AddAsync(entity);
            return _mapper.Map<ObjetivoResponseDto>(creado);
        }

        public async Task<List<ObjetivoResponseDto>> GetAllObjetivosAsync()
        {
            var objetivos = await _objetivoRepository.GetAllAsync();
            return _mapper.Map<List<ObjetivoResponseDto>>(objetivos);
        }
        public async Task<List<ObjetivoResponseDto>> GetObjetivosAnualesAsync()
        {
            var all = await _objetivoRepository.GetAllAsync();
            var anuales = all.Where(o => o.Tipo == TipoObjetivo.Anual).ToList();
            return _mapper.Map<List<ObjetivoResponseDto>>(anuales);
        }

        public async Task<List<ObjetivoResponseDto>> GetObjetivosMensualesAsync()
        {
            var all = await _objetivoRepository.GetAllAsync();
            var mensuales = all.Where(o => o.Tipo == TipoObjetivo.Mensual).OrderBy(o => o.IdCabaña).ThenBy(o => o.Año).ThenBy(o => o.Mes).ToList();
            return _mapper.Map<List<ObjetivoResponseDto>>(mensuales);
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
                    if (dto.IdCabaña is null || dto.IdCabaña <= 0) throw new ArgumentException("Debe seleccionar una cabaña.");
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

        public async Task<ObjetivoResponseDto> CreateObjetivoMensualAsync(AddObjetivoDto dto)
        {
            if (dto.IdCabaña is null || dto.IdCabaña <= 0)
                throw new ArgumentException("Debe seleccionar una cabaña.");

            if (dto.Año <= 0)
                throw new ArgumentException("Debe indicar un año válido.");

            if (!dto.Mes.HasValue || dto.Mes < 1 || dto.Mes > 12)
                throw new ArgumentException("Debe indicar un mes entre 1 y 12.");

            if (dto.MetaOcupacion < 0 || dto.MetaOcupacion > 100)
                throw new ArgumentException("La meta debe estar entre 0 y 100.");

            var existe = await _objetivoRepository.ExistsAsync(o =>
                o.Tipo == TipoObjetivo.Mensual &&
                o.IdCabaña == dto.IdCabaña &&
                o.Año == dto.Año &&
                o.Mes == dto.Mes);

            if (existe)
                throw new InvalidOperationException("Ya existe un objetivo mensual para esa cabaña, año y mes.");

            var entity = _mapper.Map<Objetivo>(dto);
            entity.Tipo = TipoObjetivo.Mensual;
            entity.IsActive = true;
            entity.FechaCreacion = DateTime.Now;

            var creado = await _objetivoRepository.AddAsync(entity);
            return _mapper.Map<ObjetivoResponseDto>(creado);
        }

        public async Task UpdateObjetivoMensualAsync(UpdateObjetivoDto dto)
        {
            var entity = await _objetivoRepository.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Objetivo no encontrado.");

            if (entity.Tipo != TipoObjetivo.Mensual)
                throw new InvalidOperationException("El objetivo no es mensual.");

            // 🔹 No permitimos cambiar cabaña/año/mes
            if (dto.IdCabaña.HasValue && dto.IdCabaña != entity.IdCabaña)
                throw new InvalidOperationException("No se puede cambiar la cabaña en la edición del objetivo mensual.");

            if (dto.Año > 0 && dto.Año != entity.Año)
                throw new InvalidOperationException("No se puede cambiar el año en la edición del objetivo mensual.");

            if (dto.Mes.HasValue && dto.Mes != entity.Mes)
                throw new InvalidOperationException("No se puede cambiar el mes en la edición del objetivo mensual.");

            // 🔹 Solo se puede actualizar la meta y el estado
            if (dto.MetaOcupacion.HasValue)
            {
                var meta = dto.MetaOcupacion.Value;
                if (meta < 0 || meta > 100)
                    throw new ArgumentException("La meta debe estar entre 0 y 100.");
                entity.MetaOcupacion = meta;
            }

            if (dto.IsActive.HasValue)
                entity.IsActive = dto.IsActive.Value;

            await _objetivoRepository.UpdateAsync(entity);
        }

        public async Task<List<int>> GetAñosDisponiblesMensualAsync(int idCabaña)
        {
            var anuales = await _objetivoRepository.QueryAsync(
                o => o.Tipo == TipoObjetivo.Anual && o.IdCabaña == idCabaña);
            var añosAnuales = anuales.Select(o => o.Año).Distinct();

            var añosReservas = _reservaRepository != null
                ? await _reservaRepository.GetAniosPorCabañaAsync(idCabaña)
                : new List<int>();

            var union = añosAnuales.Union(añosReservas).Distinct().OrderByDescending(x => x).ToList();

            if (!union.Any())
                union = new List<int> { DateTime.Now.Year };

            return union;
        }

        public async Task<List<ObjetivoResponseDto>> GetObjetivosMensualesPorCabañaYAñoAsync(int idCabaña, int año)
        {
            var mensuales = await _objetivoRepository.QueryAsync(
                o => o.Tipo == TipoObjetivo.Mensual
                  && o.IdCabaña == idCabaña
                  && o.Año == año);

            mensuales = mensuales.OrderBy(o => o.Mes).ToList();

            return _mapper.Map<List<ObjetivoResponseDto>>(mensuales);
        }
    }
}
