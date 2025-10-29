    using Business.Interfaces;
using Domain.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login request");
            }
            var response = await _authService.Login(loginRequest);
            if (response.IsAuthenticated)
            {
                return Ok(response);
            }
            else
            {
                return Unauthorized(response.Message);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUser)
        {
            if (createUser == null)
            {
                return BadRequest(new { Message = "Petición inválida." });
            }

            if (string.IsNullOrWhiteSpace(createUser.Name) || string.IsNullOrWhiteSpace(createUser.Email) || string.IsNullOrWhiteSpace(createUser.Password) ||
                string.IsNullOrWhiteSpace(createUser.PhoneNumber))
            {
                return BadRequest(new { Message = "Todos los campos son obligatorios." });
            }

            if (!createUser.Email.Contains('@') || !createUser.Email.Contains('.'))
            {
                return BadRequest(new { Message = "El correo electrónico no tiene un formato válido." });
            }

            try
            {
                var response = await _authService.Register(createUser);
                if (response != null)
                {
                    return CreatedAtAction(nameof(Register), new { id = response.Id }, new { Message = "Usuario registrado correctamente.", response });
                }
                else
                {
                    return BadRequest(new { Message = "No se pudo completar el registro." });
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException!.Message.Contains("duplicate"))
                {
                    return Conflict(new { Message = "Ese correo ya esta siendo utilizado por otro usuario", isSuccess = false });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

    }
}
