using Business.Interfaces;
using Business.Services;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ObjetivosController(IObjetivoService IObjetivoService) : ControllerBase
    {
        private readonly IObjetivoService _IObjetivoService = IObjetivoService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var objetivos = await _IObjetivoService.GetAllObjetivosAsync();
            return Ok(objetivos);
        }

        [HttpGet("anuales")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAnuales()
        {
            var items = await _IObjetivoService.GetObjetivosAnualesAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var objetivo = await _IObjetivoService.GetObjetivoByIdAsync(id);
            if (objetivo == null)
            {
                return NotFound();
            }
            return Ok(objetivo);
        }

        [HttpPost]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] AddObjetivoDto objetivoDto)
        {
            ModelState.ClearValidationState(nameof(AddObjetivoDto));

            try
            {
                var created = await _IObjetivoService.CreateObjetivoAsync(objetivoDto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return new ContentResult
                {
                    StatusCode = 400,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ContentResult
                {
                    StatusCode = 409,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    StatusCode = 500,
                    Content = "Error interno: " + ex.Message,
                    ContentType = "text/plain"
                };
            }
        }

        [HttpPost("anuales")]
        public async Task<IActionResult> CreateAnual([FromBody] AddObjetivoDto dto)
        {
            try
            {
                dto.Tipo = TipoObjetivo.Anual;
                dto.Mes = null; 
                var created = await _IObjetivoService.CreateObjetivoAnualAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateObjetivoDto objetivoDto)
        {
            if (id != objetivoDto.Id)
                return BadRequest("El ID no coincide con el objetivo." );

            try
            {
                await _IObjetivoService.UpdateObjetivoAsync(objetivoDto);
                return Ok("Objetivo actualizado correctamente." );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message );
            }
        }

        [HttpGet("mensuales")]
        public async Task<ActionResult<List<ObjetivoResponseDto>>> GetMensuales()
        {
            var data = await _IObjetivoService.GetObjetivosMensualesAsync();
            return Ok(data);
        }

        [HttpPost("mensuales")]
        public async Task<ActionResult<ObjetivoResponseDto>> CreateMensual([FromBody] AddObjetivoDto dto)
        {
            try
            {
                var creado = await _IObjetivoService.CreateObjetivoMensualAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }

        [HttpPut("mensuales")]
        public async Task<IActionResult> UpdateMensual([FromBody] UpdateObjetivoDto dto)
        {
            try
            {
                await _IObjetivoService.UpdateObjetivoMensualAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }

        [HttpGet("mensuales/cabana/{idCabaña:int}/years")]
        public async Task<ActionResult<List<int>>> GetAñosMensuales(int idCabaña)
        {
            var años = await _IObjetivoService.GetAñosDisponiblesMensualAsync(idCabaña);
            return Ok(años);
        }


        [HttpGet("mensuales/cabana/{idCabaña:int}/{año:int}")]
        public async Task<ActionResult<List<ObjetivoResponseDto>>> GetMensualesPorCabanaYAño(int idCabaña, int año)
        {
            var lista = await _IObjetivoService.GetObjetivosMensualesPorCabañaYAñoAsync(idCabaña, año);
            return Ok(lista);
        }

    }
}
