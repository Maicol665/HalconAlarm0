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
            if (request == null) return BadRequest("La solicitud está vacía.");

            var token = await _authRepositorio.LoginAsync(request);
            if (string.IsNullOrEmpty(token)) return Unauthorized("Correo o contraseña incorrectos.");

            return Ok(new { Token = token });
        }

        [HttpPost("VerificarCorreo")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerificarCorreo([FromBody] string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return BadRequest("Debe ingresar un correo.");

            var existe = await _authRepositorio.VerificarCorreoExistenteAsync(correo);
            if (!existe) return NotFound(new { mensaje = "Por favor ingrese un nuevo correo electrónico." });

            return Ok(new { mensaje = "Correo encontrado. Puede continuar con la recuperación." });
        }

        

        [HttpPost("SolicitarRestablecimiento")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SolicitarRestablecimiento([FromBody] SolicitarRestablecimientoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CorreoElectronico)) return BadRequest("Debe ingresar un correo.");

            var resultado = await _authRepositorio.GenerarTokenRestablecimientoAsync(request.CorreoElectronico);
            if (!resultado) return NotFound("Correo no encontrado.");

            return Ok("Se ha enviado un código por correo para restablecer la contraseña.");
        }

        [HttpPost("RestablecerContrasenaSeguro")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RestablecerContrasenaSeguro([FromBody] RestablecerContrasenaSeguroRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NuevaContrasena))
                return BadRequest("Código y nueva contraseña son requeridos.");

            var resultado = await _authRepositorio.RestablecerContrasenaConTokenAsync(request.Token, request.NuevaContrasena);
            if (!resultado) return BadRequest("Código inválido o expirado.");

            return Ok("Contraseña restablecida correctamente. Diríjase al Login.");
        }
    }
}
