using AutoMapper;
using Domain.DTOs.Requests;
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
                           opt => opt.MapFrom(src => src.Cabaña != null ? src.Cabaña.Nombre : null));
            CreateMap<AddObjetivoDto, Objetivo>();
            CreateMap<UpdateObjetivoDto, Objetivo>();
            CreateMap<Cabaña, CabañaResponseDto>();

        }
    }
}
