using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.ServiciosExternos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalconAlarm0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "2.Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly Repositorios.Interfaces.IUsuariosRepositorio _usuariosRepositorio;
        private readonly PasswordService _passwordService;

        public UsuariosController(
            Repositorios.Interfaces.IUsuariosRepositorio usuariosRepositorio,
            PasswordService passwordService)
        {
            _usuariosRepositorio = usuariosRepositorio;
            _passwordService = passwordService;
        }

        // SOLO ADMIN
        [Authorize(Roles = "Usuario Administrador")]
        [HttpGet("obtenerUsuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await _usuariosRepositorio.ObtenerTodosLosUsuarios();
                if (usuarios == null || usuarios.Count == 0)
                    return NotFound("No se encontraron usuarios.");

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los usuarios: {ex.Message}");
            }
        }

        // SOLO ADMIN
        [Authorize(Roles = "Usuario Administrador")]
        [HttpGet("obtenerUsuariosPorID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ObtenerUsuarioPorID(Guid usuarioID)
        {
            try
            {
                var usuario = await _usuariosRepositorio.ObtenerUsuarioPorID(usuarioID);

                if (usuario == null)
                    return NotFound("Usuario no encontrado.");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el usuario: {ex.Message}");
            }
        }

        // REGISTRO ES PUBLICO
        [AllowAnonymous]
        [HttpPost("RegistrarUsuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroUsuarioDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("La solicitud está vacía.");

                if (string.IsNullOrWhiteSpace(dto.Nombres) ||
                    string.IsNullOrWhiteSpace(dto.Apellidos) ||
                    string.IsNullOrWhiteSpace(dto.CorreoElectronico) ||
                    string.IsNullOrWhiteSpace(dto.Contrasena))
                {
                    return BadRequest("Todos los campos obligatorios deben estar llenos.");
                }

                var usuarioExistente = await _usuariosRepositorio.ObtenerUsuarioPorCorreo(dto.CorreoElectronico);
                if (usuarioExistente != null)
                    return BadRequest("Usuario ya existente.");

                var salt = _passwordService.GenerarSalt();
                var hash = _passwordService.GenerarHash(dto.Contrasena, salt);

                var usuario = new Usuarios
                {
                    UsuarioID = Guid.NewGuid(),
                    Nombres = dto.Nombres.Trim(),
                    Apellidos = dto.Apellidos.Trim(),
                    CorreoElectronico = dto.CorreoElectronico.Trim(),
                    Telefono = dto.Telefono?.Trim(),
                    ContrasenaHash = hash,
                    ContrasenaSalt = salt,
                    RolID = dto.RolID,
                    Activo = true,
                    FechaRegistro = DateTime.Now
                };

                var resultado = await _usuariosRepositorio.RegistrarUsuario(usuario);
                if (!resultado)
                    return StatusCode(500, "Error registrando usuario.");

                return Ok("Usuario registrado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar el usuario: {ex.Message}");
            }
        }

        // SOLO ADMIN
        [Authorize(Roles = "Usuario Administrador")]
        [HttpPut("actualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ActualizarUsuario(Guid id, [FromBody] ActualizarUsuarioDTO dto)
        {
            if (dto == null)
                return BadRequest("La solicitud está vacía.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _usuariosRepositorio.ObtenerUsuarioPorID(id);

            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            usuario.Nombres = dto.Nombres;
            usuario.Apellidos = dto.Apellidos;
            usuario.CorreoElectronico = dto.CorreoElectronico;
            usuario.Telefono = dto.Telefono;
            usuario.RolID = dto.RolID;

            var actualizado = await _usuariosRepositorio.ActualizarUsuario(usuario);

            if (!actualizado)
                return StatusCode(500, "Error al actualizar el usuario.");

            return Ok(new
            {
                mensaje = "Usuario actualizado correctamente.",
                usuario = new
                {
                    usuario.UsuarioID,
                    usuario.Nombres,
                    usuario.Apellidos,
                    usuario.CorreoElectronico,
                    usuario.Telefono,
                    usuario.RolID
                }
            });
        }

        // SOLO ADMIN
        [Authorize(Roles = "Usuario Administrador")]
        [HttpDelete("EliminarUsuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> EliminarUsuario(Guid usuarioID)
        {
            try
            {
                var resultado = await _usuariosRepositorio.EliminarUsuario(usuarioID);
                if (!resultado)
                    return StatusCode(500, "Error al eliminar el usuario.");

                return Ok("Usuario eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el usuario: {ex.Message}");
            }
        }
    }
}
