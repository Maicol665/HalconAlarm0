using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HalconAlarm0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase

    {
        private readonly Repositorios.Interfaces.IUsuariosRepositorio _usuariosRepositorio; // inyección de dependencia del repositorio de usuarios

        public UsuariosController(Repositorios.Interfaces.IUsuariosRepositorio usuariosRepositorio) // constructor que recibe el repositorio de usuarios
        {
            _usuariosRepositorio = usuariosRepositorio;
        }

        [HttpGet("obtenerUsuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ObtenerUsuarios() // método para obtener todos los usuarios
        {
            try
            {
                var usuarios = await _usuariosRepositorio.ObtenerTodosLosUsuarios();
                if (usuarios == null || usuarios.Count == 0)
                {
                    return NotFound("No se encontraron usuarios.");
                }
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Aquí se podría registrar el error en un sistema de logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los usuarios: " + ex.Message);
            }

        }
        [HttpGet("obtenerUsuariosPorid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerUsuarioPorID(Guid usuarioID) // método para obtener un usuario por su ID
        {
            try
            {
                var usuario = await _usuariosRepositorio.ObtenerUsuarioPorID(usuarioID);
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado.");
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // Aquí se podría registrar el error en un sistema de logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener el usuario: " + ex.Message);
            }
        }
        [HttpPost("RegistrarUsuarios")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarUsuario(Modelos.Usuarios usuario) // método para registrar un nuevo usuario
        {
            try
            {
                var resultado = await _usuariosRepositorio.RegistrarUsuario(usuario);
                if (!resultado)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al registrar el usuario.");
                }
                return Ok("Usuario registrado exitosamente.");
            }
            catch (Exception ex)
            {
                // Aquí se podría registrar el error en un sistema de logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al registrar el usuario: " + ex.Message);
            }
        }
        [HttpPut ("ActualizarUsuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarUsuario(Modelos.Usuarios usuario) // método para actualizar un usuario existente
        {
            try
            {
                var resultado = await _usuariosRepositorio.ActualizarUsuario(usuario);
                if (!resultado)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el usuario.");
                }
                return Ok("Usuario actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                // Aquí se podría registrar el error en un sistema de logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el usuario: " + ex.Message);
            }
        }

        [HttpDelete("EliminarUsuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarUsuario(Guid usuarioID) // método para eliminar un usuario por su ID
        {
            try
            {
                var resultado = await _usuariosRepositorio.EliminarUsuario(usuarioID);
                if (!resultado)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el usuario.");
                }
                return Ok("Usuario eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                // Aquí se podría registrar el error en un sistema de logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el usuario: " + ex.Message);
            }
        }
    }
}