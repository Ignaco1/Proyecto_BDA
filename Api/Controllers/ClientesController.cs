using Business.Interfaces;
using Domain.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController(IClienteService svc, ILogger<ClientesController> logger) : ControllerBase
    {
        private readonly IClienteService _svc = svc;
        private readonly ILogger<ClientesController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponseDto>>> GetAll()
        {
            try
            {
                var data = await _svc.GetClientesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                return Problem(detail: ex.Message, statusCode: 500, title: "Error al obtener clientes");
            }
        }
    }
}
