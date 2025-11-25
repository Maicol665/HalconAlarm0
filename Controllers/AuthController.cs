using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HalconAlarm0.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "1.Login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepositorio _authRepositorio;

        public AuthController(IAuthRepositorio authRepositorio)
        {
            _authRepositorio = authRepositorio;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest("La solicitud está vacía.");

            try
            {
                var token = await _authRepositorio.LoginAsync(request);

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Correo o contraseña incorrectos.");

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Ocurrió un error al procesar la solicitud: {ex.Message}"
                );
            }
        }
    }
}
