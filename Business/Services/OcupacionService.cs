using Business.Interfaces;
using Domain.DTOs.Requests.Ocupacion;
using Domain.Entities;
using Domain.Enums;
using Domain.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class OcupacionService(IReservaRepository reservaRepository, IObjetivoRepository objetivoRepository) : IOcupacionService
    {
        private readonly IReservaRepository _reservaRepository = reservaRepository;
        private readonly IObjetivoRepository _objetivoRepository = objetivoRepository;

        public async Task<List<int>> GetAñosConReservasAsync(int idCabaña)
        {
            var reservasAll = await _reservaRepository.GetAllAsync();
            var reservasCab = reservasAll?
                .Where(r => r.IdCabaña == idCabaña && (r.Estado != EstadosReserva.Cancelada))
                .ToList() ?? new List<Reserva>();

            if (reservasCab.Count == 0) return new List<int>();

       
            return reservasCab
                .SelectMany(r => new[] { r.FechaEntrada.Year, r.FechaSalida.Year })
                .Distinct()
                .OrderBy(y => y)
                .ToList();
        }

        public async Task<List<OcupacionAnualDto>> GetOcupacionAnualAsync(int idCabaña, int año)
        {
            var reservasAll = await _reservaRepository.GetAllAsync();

           
            var inicio = new DateTime(año, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var finExcl = inicio.AddYears(1);
            var diasDelAño = (int)(finExcl - inicio).TotalDays; 

        
            var reservasCab = (reservasAll ?? Enumerable.Empty<Reserva>())
                .Where(r => r.IdCabaña == idCabaña
                         && ((r.Estado != EstadosReserva.Cancelada))
                         && r.FechaSalida > inicio    
                         && r.FechaEntrada < finExcl) 
                .ToList();

            if (reservasCab.Count == 0)
                return new List<OcupacionAnualDto>(); 

       
            int nochesReservadas = reservasCab.Sum(r =>
            {
                var desde = r.FechaEntrada < inicio ? inicio : r.FechaEntrada;
                var hasta = r.FechaSalida > finExcl ? finExcl : r.FechaSalida;
                var diff = (int)(hasta - desde).TotalDays;
                return Math.Max(diff, 0);
            });

            var nochesDisponibles = Math.Max(diasDelAño - nochesReservadas, 0);
            decimal porcentaje = diasDelAño > 0
                ? Math.Round((decimal)nochesReservadas * 100m / diasDelAño, 2)
                : 0m;

        
            var objetivosAll = await _objetivoRepository.GetAllAsync();
            decimal? meta = (objetivosAll ?? Enumerable.Empty<Objetivo>())
                .Where(o => o.Tipo == TipoObjetivo.Anual
                         && o.IdCabaña == idCabaña
                         && (o.IsActive ?? true)
                         && o.Año == año)
                .OrderByDescending(o => o.FechaCreacion)
                .Select(o => (decimal?)o.MetaOcupacion)
                .FirstOrDefault();

            var semaforo = CalcularSemaforo(porcentaje, meta);

            return new List<OcupacionAnualDto>
            {
                new OcupacionAnualDto
                {
                    Año = año,
                    NochesReservadas = nochesReservadas,
                    NochesDisponibles = nochesDisponibles,
                    PorcentajeOcupacion = porcentaje,
                    MetaObjetivoAnual = meta,
                    Semaforo = semaforo
                }
            };
        }

        public async Task<List<OcupacionAnualDto>> GetOcupacionesAnualesAsync(int idCabaña)
        {
            var años = await GetAñosConReservasAsync(idCabaña); 
            var list = new List<OcupacionAnualDto>();
            foreach (var año in años)
            {
     
                var uno = await GetOcupacionAnualAsync(idCabaña, año);
                if (uno is { Count: > 0 })
                    list.AddRange(uno);
            }
            return list.OrderByDescending(x => x.Año).ToList();
        }

        public async Task<List<OcupacionMensualDto>> GetOcupacionMensualAsync(int idCabaña, int año)
        {
      
            var reservas = await _reservaRepository.GetReservasPorCabañaYAñoAsync(idCabaña, año)
                           ?? new List<Reserva>();

    
            reservas = reservas
                .Where(r => (r.Estado != EstadosReserva.Cancelada))
                .ToList();

            var nombreCabaña = reservas.FirstOrDefault()?.Cabaña?.Nombre ?? string.Empty;

         
            var objetivosAll = await _objetivoRepository.GetAllAsync() ?? new List<Objetivo>();

            decimal? metaAnual = objetivosAll
                .Where(o => o.Tipo == TipoObjetivo.Anual
                         && o.IdCabaña == idCabaña
                         && (o.IsActive ?? true)
                         && o.Año == año)
                .OrderByDescending(o => o.FechaCreacion)
                .Select(o => (decimal?)o.MetaOcupacion)
                .FirstOrDefault();

            var result = new List<OcupacionMensualDto>();

            for (int mes = 1; mes <= 12; mes++)
            {
                var mDesde = new DateTime(año, mes, 1);
                var mHasta = (mes == 12) ? new DateTime(año + 1, 1, 1) : new DateTime(año, mes + 1, 1);
                var diasMes = (mHasta - mDesde).Days;

     
                var rangosMes = reservas
                    .Where(r => r.FechaSalida > mDesde && r.FechaEntrada < mHasta)
                    .Select(r => (desde: r.FechaEntrada < mDesde ? mDesde : r.FechaEntrada,
                                  hasta: r.FechaSalida > mHasta ? mHasta : r.FechaSalida))
                    .Where(t => t.desde < t.hasta)
                    .ToList();

                var unidos = UnirRangos(rangosMes);
                var noches = unidos.Sum(x => (x.hasta - x.desde).Days);
                var disp = Math.Max(diasMes - noches, 0);
                var pct = diasMes == 0 ? 0m : Math.Round(noches * 100m / diasMes, 2);

       
                decimal? metaMensual = objetivosAll
                    .Where(o => o.Tipo == TipoObjetivo.Mensual
                             && o.IdCabaña == idCabaña
                             && (o.IsActive ?? true)
                             && o.Año == año
                             && o.Mes == mes)
                    .OrderByDescending(o => o.FechaCreacion)
                    .Select(o => (decimal?)o.MetaOcupacion)
                    .FirstOrDefault();

                var meta = metaMensual ?? metaAnual;

                result.Add(new OcupacionMensualDto
                {
                    IdCabaña = idCabaña,
                    NombreCabaña = nombreCabaña,
                    Año = año,
                    Mes = mes,
                    NochesReservadas = noches,
                    NochesDisponibles = disp,
                    PorcentajeOcupacion = pct,
                    MetaObjetivo = meta,
                    Semaforo = CalcularSemaforo(pct, meta)
                });
            }

            return result;
        }

        public async Task<List<OcupacionDiariaDto>> GetOcupacionDiariaAsync(int idCabaña, int año, int mes)
        {

            var reservas = await _reservaRepository.GetReservasPorCabañaYAñoAsync(idCabaña, año);
 
            reservas = reservas.Where(r => r.Estado != EstadosReserva.Cancelada).ToList();

   
            var inicioMes = new DateTime(año, mes, 1);
            var inicioMesSig = (mes == 12) ? new DateTime(año + 1, 1, 1) : new DateTime(año, mes + 1, 1);
            int diasMes = (inicioMesSig - inicioMes).Days;


            var ocupados = new HashSet<DateTime>();

            foreach (var r in reservas)
            {

                var desde = r.FechaEntrada < inicioMes ? inicioMes : r.FechaEntrada;
                var hasta = r.FechaSalida > inicioMesSig ? inicioMesSig : r.FechaSalida;


                for (var d = desde.Date; d < hasta.Date; d = d.AddDays(1))
                {

                    if (d >= inicioMes && d < inicioMesSig)
                        ocupados.Add(d);
                }
            }

            var nombreCab = reservas.FirstOrDefault()?.Cabaña?.Nombre ?? ""; 

            var lista = new List<OcupacionDiariaDto>(diasMes);
            for (int dia = 1; dia <= diasMes; dia++)
            {
                var fecha = new DateTime(año, mes, dia);
                bool esOcupado = ocupados.Contains(fecha);

                lista.Add(new OcupacionDiariaDto
                {
                    IdCabaña = idCabaña,
                    NombreCabaña = nombreCab,
                    Año = año,
                    Mes = mes,
                    Dia = dia,
                    Fecha = fecha,
                    Ocupada = esOcupado,
                    Estado = esOcupado ? "Ocupada" : "Desocupada"
                });
            }

            return lista;
        }

        private static List<(DateTime desde, DateTime hasta)> UnirRangos(List<(DateTime desde, DateTime hasta)> rangos)
        {
            if (rangos.Count == 0) return rangos;
            var orden = rangos.OrderBy(r => r.desde).ToList();
            var res = new List<(DateTime, DateTime)> { orden[0] };

            for (int i = 1; i < orden.Count; i++)
            {
                var last = res[^1];
                var cur = orden[i];
                if (cur.desde <= last.Item2) 
                    res[^1] = (last.Item1, cur.hasta > last.Item2 ? cur.hasta : last.Item2);
                else
                    res.Add(cur);
            }
            return res;
        }

        private static string CalcularSemaforo(decimal porcentaje, decimal? meta)
        {
            if (meta is null) return "Rojo";            
            var objetivo = meta.Value;

            if (porcentaje >= objetivo) return "Verde";

            var diff = objetivo - porcentaje;

            if (diff <= 7m) return "Naranja";

            return "Rojo";
        }
    }
}
