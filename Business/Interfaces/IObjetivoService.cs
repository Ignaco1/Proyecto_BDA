using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IObjetivoService
    {
        Task<ObjetivoResponseDto> GetObjetivoByIdAsync(int id);
        Task<ObjetivoResponseDto> CreateObjetivoAsync(AddObjetivoDto AddObjetivoDto);
        Task<ObjetivoResponseDto> CreateObjetivoAnualAsync(AddObjetivoDto AddObjetivoDto);
        Task<List<ObjetivoResponseDto>> GetObjetivosAnualesAsync();
        Task<ObjetivoResponseDto> CreateObjetivoMensualAsync(AddObjetivoDto AddObjetivoDto);
        Task UpdateObjetivoMensualAsync(UpdateObjetivoDto UpdateObjetivoDto);
        Task<List<int>> GetAñosDisponiblesMensualAsync(int idCabaña);
        Task<List<ObjetivoResponseDto>> GetObjetivosMensualesPorCabañaYAñoAsync(int idCabaña, int año);

        Task<List<ObjetivoResponseDto>> GetAllObjetivosAsync();
        Task<List<ObjetivoResponseDto>> GetObjetivosMensualesAsync();
        Task UpdateObjetivoAsync(UpdateObjetivoDto UpdateObjetivoDto);
    }
}
