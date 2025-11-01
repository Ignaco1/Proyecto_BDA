using Business.Interfaces;
using Domain.DTOs.Requests;
using Domain.DTOs.Requests.Cancelacion;
using Domain.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class CancelacionesController(ICancelacionService cancelacionService) : ControllerBase
    {
        private readonly ICancelacionService _cancelacionService = cancelacionService;

        // GET: api/cancelaciones
        [HttpGet]
        public async Task<ActionResult<List<CancelacionResponseDto>>> GetAll()
        {
            var data = await _cancelacionService.GetAllAsync();
            return Ok(data);
        }

        // GET: api/cancelaciones/por-reserva/5
        [HttpGet("por-reserva/{reservaId:int}", Name = "GetCancelacionByReservaId")]
        [AllowAnonymous]
        public async Task<ActionResult<CancelacionResponseDto>> GetByReservaId(int reservaId)
        {
            var item = await _cancelacionService.GetByReservaIdAsync(reservaId);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST: api/cancelaciones
        // Crea la cancelación y cambia estado de la reserva a Cancelada
        [HttpPost]
        public async Task<ActionResult<CancelacionResponseDto>> Create([FromBody] AddCancelacionDto dto)
        {
            try
            {
                var creada = await _cancelacionService.CreateAsync(dto);
                return CreatedAtRoute("GetCancelacionByReservaId", new { reservaId = dto.ReservaId }, creada);
            }
            catch (KeyNotFoundException) { return NotFound("La reserva no existe."); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }
    }
}
