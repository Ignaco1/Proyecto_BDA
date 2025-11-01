using AutoMapper;
using Business.Interfaces;
using Domain.DTOs.Requests.Reserva;
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
    public class ReservaService(IReservaRepository reservaRepository, IMapper mapper) : IReservaService
    {
        private readonly IReservaRepository _reservaRepository = reservaRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ReservaResponseDto> CreateReservaAsync(AddReservaDto dto)
        {
            if (dto.FechaEntrada >= dto.FechaSalida)
                throw new ArgumentException("La fecha de entrada debe ser anterior a la de salida.");

            var entity = _mapper.Map<Reserva>(dto);
            entity.FechaCreacion = DateTime.Now;
            entity.Estado = EstadosReserva.Pendiente;

            var creada = await _reservaRepository.AddAsync(entity);
            return _mapper.Map<ReservaResponseDto>(creada);
        }

        public async Task<List<ReservaResponseDto>> GetAllAsync()
        {
            var reservas = await _reservaRepository.GetAllAsync();
            ActualizarEstadosDinamicos(reservas);
            return _mapper.Map<List<ReservaResponseDto>>(reservas);
        }

        public async Task<List<int>> GetAñosPorCabañaAsync(int idCabaña)
        {
            return await _reservaRepository.GetAñosPorCabañaAsync(idCabaña);
        }

        public async Task<List<ReservaResponseDto>> GetByCabañaAsync(int idCabaña)
        {
            var reservas = await _reservaRepository.GetReservasPorCabañaAsync(idCabaña);
            ActualizarEstadosDinamicos(reservas);
            return _mapper.Map<List<ReservaResponseDto>>(reservas);
        }

        public async Task<ReservaResponseDto?> GetByIdAsync(int id)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id);
            if (reserva == null)
                return null;

            ActualizarEstadoDinamico(reserva);
            return _mapper.Map<ReservaResponseDto>(reserva);
        }

        public async Task UpdateAsync(UpdateReservaDto dto)
        {
            var existing = await _reservaRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("La reserva no existe.");

            if (existing.Estado == EstadosReserva.Cancelada)
                throw new InvalidOperationException("No se puede modificar una reserva cancelada.");

            if (dto.FechaEntrada.HasValue && dto.FechaSalida.HasValue &&
                dto.FechaEntrada >= dto.FechaSalida)
                throw new ArgumentException("La fecha de entrada debe ser anterior a la de salida.");

            _mapper.Map(dto, existing);
            await _reservaRepository.UpdateAsync(existing);
        }

        private static void ActualizarEstadosDinamicos(IEnumerable<Reserva> reservas)
        {
            foreach (var r in reservas)
                ActualizarEstadoDinamico(r);
        }
        private static void ActualizarEstadoDinamico(Reserva r)
        {
            if (r.Estado == EstadosReserva.Cancelada)
                return;

            var hoy = DateTime.Today;

            if (r.FechaEntrada > hoy)
                r.Estado = EstadosReserva.Pendiente;
            else if (r.FechaSalida < hoy)
                r.Estado = EstadosReserva.Finalizada;
            else
                r.Estado = EstadosReserva.Activa;
        }

    }
}
