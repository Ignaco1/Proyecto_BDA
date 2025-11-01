using Business.Interfaces;
using Domain.DTOs.Requests;
using Domain.DTOs.Requests.Reserva;
using Domain.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ReservasController(IReservaService reservaService) : ControllerBase
    {
        private readonly IReservaService _reservaService = reservaService;

        // GET: api/reservas
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReservaResponseDto>>> GetAll()
        {
            var data = await _reservaService.GetAllAsync();
            return Ok(data);
        }

        // GET: api/reservas/5
        [HttpGet("{id:int}", Name = "GetReservaById")]
        [AllowAnonymous]
        public async Task<ActionResult<ReservaResponseDto>> GetById(int id)
        {
            var item = await _reservaService.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // GET: api/reservas/cabana/3
        [HttpGet("cabana/{idCabaña:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReservaResponseDto>>> GetByCabaña(int idCabaña)
        {
            var data = await _reservaService.GetByCabañaAsync(idCabaña);
            return Ok(data);
        }

        // GET: api/reservas/cabana/3/years
        [HttpGet("cabana/{idCabaña:int}/years")]
        [AllowAnonymous]
        public async Task<ActionResult<List<int>>> GetAñosPorCabaña(int idCabaña)
        {
            var años = await _reservaService.GetAñosPorCabañaAsync(idCabaña);
            return Ok(años);
        }

        // POST: api/reservas
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<ReservaResponseDto>> Create([FromBody] AddReservaDto dto)
        {
            try
            {
                var creada = await _reservaService.CreateReservaAsync(dto);
                return CreatedAtRoute("GetReservaById", new { id = creada.Id }, creada);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }

        // PUT: api/reservas/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReservaDto dto)
        {
            if (id != dto.Id) return BadRequest("El ID no coincide con la reserva.");

            try
            {
                await _reservaService.UpdateAsync(dto);
                return Ok("Reserva actualizada correctamente.");
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }
    }
}
