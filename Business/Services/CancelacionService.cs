using AutoMapper;
using Business.Interfaces;
using Domain.DTOs.Requests.Cancelacion;
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
    public class CancelacionService(ICancelacionRepository cancelacionRepository, IReservaRepository reservaRepository, IMapper mapper) : ICancelacionService
    {
        private readonly ICancelacionRepository _cancelacionRepository = cancelacionRepository;
        private readonly IReservaRepository _reservaRepository = reservaRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<CancelacionResponseDto> CreateAsync(AddCancelacionDto dto)
        {
            var reserva = await _reservaRepository.GetByIdAsync(dto.ReservaId);
            if (reserva == null)
                throw new KeyNotFoundException("La reserva no existe.");

            if (reserva.Estado == EstadosReserva.Cancelada)
                throw new InvalidOperationException("La reserva ya está cancelada.");

            reserva.Estado = EstadosReserva.Cancelada;
            await _reservaRepository.UpdateAsync(reserva);

            var cancelacion = new Cancelacion
            {
                ReservaId = dto.ReservaId,
                Fecha = DateTime.Now,
                MotivosCancelacion = dto.MotivosCancelacion
            };

            var creada = await _cancelacionRepository.AddAsync(cancelacion);
            return _mapper.Map<CancelacionResponseDto>(creada);
        }

        public async Task<List<CancelacionResponseDto>> GetAllAsync()
        {
            var cancelaciones = await _cancelacionRepository.GetAllAsync();
            return _mapper.Map<List<CancelacionResponseDto>>(cancelaciones);
        }

        public async Task<CancelacionResponseDto?> GetByReservaIdAsync(int reservaId)
        {
            var cancel = await _cancelacionRepository.GetByReservaIdAsync(reservaId);
            return cancel == null ? null : _mapper.Map<CancelacionResponseDto>(cancel);
        }
    }
}
