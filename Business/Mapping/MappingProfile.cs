using AutoMapper;
using Domain.DTOs.Requests.Cancelacion;
using Domain.DTOs.Requests.Objetivo;
using Domain.DTOs.Requests.Reserva;
using Domain.DTOs.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<Objetivo, ObjetivoResponseDto>()
                .ForMember(dest => dest.NombreCabaña,
                           opt => opt.MapFrom(src => src.Cabaña != null ? src.Cabaña.Nombre : null))
                .ForMember(dest => dest.IdCabaña,
                           opt => opt.MapFrom(src => src.IdCabaña));
            CreateMap<AddObjetivoDto, Objetivo>();
            CreateMap<UpdateObjetivoDto, Objetivo>();
            CreateMap<Cabaña, CabañaResponseDto>();
            CreateMap<Reserva, ReservaResponseDto>()
            .ForMember(d => d.NombreCabaña, o => o.MapFrom(s => s.Cabaña != null ? s.Cabaña.Nombre : null))
            .ForMember(d => d.NombreCliente, o => o.MapFrom(s => s.Cliente != null ? s.Cliente.Nombre : null))
            .ForMember(d => d.FechaCancelacion, o => o.MapFrom(s => s.Cancelacion != null ? s.Cancelacion.Fecha : (DateTime?)null))
            .ForMember(d => d.MotivoCancelacion, o => o.MapFrom(s => s.Cancelacion != null ? s.Cancelacion.MotivosCancelacion : (Domain.Enums.MotivosCancelacion?)null));
            CreateMap<AddReservaDto, Reserva>();
            CreateMap<UpdateReservaDto, Reserva>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Cancelacion, CancelacionResponseDto>()
                .ForMember(d => d.IdCabaña, o => o.MapFrom(s => s.Reserva.IdCabaña))
                .ForMember(d => d.NombreCabaña, o => o.MapFrom(s => s.Reserva.Cabaña.Nombre))
                .ForMember(d => d.IdCliente, o => o.MapFrom(s => s.Reserva.IdCliente))
                .ForMember(d => d.NombreCliente, o => o.MapFrom(s => s.Reserva.Cliente.Nombre))
                .ForMember(d => d.FechaEntrada, o => o.MapFrom(s => s.Reserva.FechaEntrada))
                .ForMember(d => d.FechaSalida, o => o.MapFrom(s => s.Reserva.FechaSalida))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.Reserva.Total));
            CreateMap<AddCancelacionDto, Cancelacion>();
            CreateMap<UpdateCancelacionDto, Cancelacion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
