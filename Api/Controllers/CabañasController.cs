using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CabañasController(ICabañaService ICabañaService) : ControllerBase
    {
        private readonly ICabañaService ICabañaService = ICabañaService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await ICabañaService.GetAllAsync();
            return Ok(result);
        }
    }
}
