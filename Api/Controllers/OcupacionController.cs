using Business.Interfaces;
using Domain.DTOs.Requests.Ocupacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcupacionController(IOcupacionService svc, ILogger<OcupacionController> logger) : ControllerBase
    {
        private readonly IOcupacionService _svc = svc;
        private readonly ILogger<OcupacionController> _logger = logger;

        // GET: api/ocupacion/cabana/5/years
        [HttpGet("cabana/{idCabaña:int}/years")]
        [HttpGet("cabaña/{idCabaña:int}/years")]
        [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears(int idCabaña)
        {
            try
            {
                var años = await _svc.GetAñosConReservasAsync(idCabaña);
                return Ok(años);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetYears: {Id}", idCabaña);
                return Problem(detail: ex.Message, statusCode: 500, title: "Error en años de ocupación");
            }
        }

        // GET: api/ocupacion/cabana/5/anual
        [HttpGet("cabana/{idCabaña:int}/anual")]
        [HttpGet("cabaña/{idCabaña:int}/anual")]
        [ProducesResponseType(typeof(List<OcupacionAnualDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnualTodos(int idCabaña)
        {
            try
            {
                var data = await _svc.GetOcupacionesAnualesAsync(idCabaña);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAnualTodos: {Id}", idCabaña);
                return Problem(detail: ex.Message, statusCode: 500, title: "Error en ocupación anual");
            }
        }

        // GET: api/ocupacion/cabana/5/mensual/2024
        [HttpGet("cabana/{idCabaña:int}/mensual/{año:int}")]
        [HttpGet("cabaña/{idCabaña:int}/mensual/{año:int}")]
        [ProducesResponseType(typeof(List<OcupacionMensualDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMensual(int idCabaña, int año)
        {
            try
            {
                var data = await _svc.GetOcupacionMensualAsync(idCabaña, año);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetMensual: {Id} {Año}", idCabaña, año);
                return Problem(detail: ex.Message, statusCode: 500, title: "Error en ocupación mensual");
            }
        }

        // GET: api/ocupacion/cabana/5/diaria/2025/12
        [HttpGet("cabaña/{idCabaña:int}/diaria/{año:int}/{mes:int}")]
        [HttpGet("cabana/{idCabaña:int}/diaria/{año:int}/{mes:int}")]
        public async Task<ActionResult<List<OcupacionDiariaDto>>> GetDiaria(int idCabaña, int año, int mes)
        {
            try
            {
                var data = await _svc.GetOcupacionDiariaAsync(idCabaña, año, mes);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetDiaria: {Id}-{Año}-{Mes}", idCabaña, año, mes);
                return Problem(detail: ex.Message, statusCode: 500, title: "Error en ocupación diaria");
            }
        }

    }
}
