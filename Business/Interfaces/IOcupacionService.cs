using Domain.DTOs.Requests.Ocupacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IOcupacionService
    {
        Task<List<int>> GetAñosConReservasAsync(int idCabaña);
        Task<List<OcupacionAnualDto>> GetOcupacionesAnualesAsync(int idCabaña);
        Task<List<OcupacionAnualDto>> GetOcupacionAnualAsync(int idCabaña, int año);
        Task<List<OcupacionMensualDto>> GetOcupacionMensualAsync(int idCabaña, int año);
        Task<List<OcupacionDiariaDto>> GetOcupacionDiariaAsync(int idCabaña, int año, int mes);
    }
}
