using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalconAlarm0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly Repositorios.Interfaces.IUsuariosRepositorio _usuariosRepositorio;
        private readonly PasswordService _passwordService;

        public UsuariosController(Repositorios.Interfaces.IUsuariosRepositorio usuariosRepositorio)
        {
            _usuariosRepositorio = usuariosRepositorio;
            _passwordService = new PasswordService();
        }

        [HttpGet("obtenerUsuarios")]
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
                return StatusCode(500, "Error al obtener los usuarios: " + ex.Message);
            }
        }

        [HttpGet("obtenerUsuariosPorID")]
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
                return StatusCode(500, "Error al obtener el usuario: " + ex.Message);
            }
        }

        [HttpPost("RegistrarUsuarios")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarUsuario(RegistroUsuarioDTO dto)
        {
            try
            {
                // 1️⃣ Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(dto.Nombres) ||
                    string.IsNullOrWhiteSpace(dto.Apellidos) ||
                    string.IsNullOrWhiteSpace(dto.CorreoElectronico) ||
                    string.IsNullOrWhiteSpace(dto.TipoUsuario) ||
                    string.IsNullOrWhiteSpace(dto.Contrasena))
                {
                    return BadRequest("Todos los campos obligatorios deben estar llenos.");
                }

                // 2️⃣ Verificar si el correo ya existe
                var usuarioExistente = await _usuariosRepositorio.ObtenerUsuarioPorCorreo(dto.CorreoElectronico);
                if (usuarioExistente != null)
                    return BadRequest("Usuario ya existente");

                // 3️⃣ Generar salt y hash
                var salt = _passwordService.GenerarSalt();
                var hash = _passwordService.GenerarHash(dto.Contrasena, salt);

                // 4️⃣ Crear usuario
                var usuario = new Usuarios
                {
                    UsuarioID = Guid.NewGuid(),
                    Nombres = dto.Nombres.Trim(),
                    Apellidos = dto.Apellidos.Trim(),
                    CorreoElectronico = dto.CorreoElectronico.Trim(),
                    Telefono = dto.Telefono?.Trim(),
                    TipoUsuario = dto.TipoUsuario.Trim(),
                    ContrasenaHash = hash,
                    ContrasenaSalt = salt,
                    RolID = dto.RolID,
                    Activo = true,
                    FechaRegistro = DateTime.Now
                };

                // 5️⃣ Guardar en base de datos
                var resultado = await _usuariosRepositorio.RegistrarUsuario(usuario);
                if (!resultado)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error registrando usuario");

                return Ok("Usuario registrado exitosamente");
            }
            catch (Exception ex)
            {
                // ⚠️ Aquí puedes agregar ex.InnerException?.Message para más detalle si quieres
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al registrar el usuario: " + ex.Message);
            }
        }


        [HttpPut("ActualizarUsuarios")]
        public async Task<IActionResult> ActualizarUsuario(Usuarios usuario)
        {
            try
            {
                var resultado = await _usuariosRepositorio.ActualizarUsuario(usuario);
                if (!resultado)
                    return StatusCode(500, "Error al actualizar el usuario.");

                return Ok("Usuario actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al actualizar el usuario: " + ex.Message);
            }
        }

        [HttpDelete("EliminarUsuarios")]
        public async Task<IActionResult> EliminarUsuario(Guid usuarioID)
        {
            try
            {
                var resultado = await _usuariosRepositorio.EliminarUsuario(usuarioID);
                if (!resultado)
                    return StatusCode(500, "Error al eliminar el usuario.");

                return Ok("Usuario eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al eliminar el usuario: " + ex.Message);
            }
        }
    }
}
