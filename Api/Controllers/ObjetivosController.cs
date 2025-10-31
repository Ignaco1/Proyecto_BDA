using Business.Interfaces;
using Domain.DTOs.Requests;
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

    }
}
