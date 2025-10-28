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
        Task<List<ObjetivoResponseDto>> GetAllObjetivosAsync();
        Task UpdateObjetivoAsync(UpdateObjetivoDto UpdateObjetivoDto);
    }
}
