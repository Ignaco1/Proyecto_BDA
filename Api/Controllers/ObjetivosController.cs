using Business.Interfaces;
using Domain.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> Create([FromBody] AddObjetivoDto objetivoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdObjetivo = await _IObjetivoService.CreateObjetivoAsync(objetivoDto);
            return CreatedAtAction(nameof(GetById), new { id = createdObjetivo.Id }, createdObjetivo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateObjetivoDto objetivoDto)
        {
            if (id != objetivoDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _IObjetivoService.UpdateObjetivoAsync(objetivoDto);
            return NoContent();
        }

    }
}
