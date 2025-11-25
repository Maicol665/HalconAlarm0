using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HalconAlarm0.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "4.0 Dispositivos")]
    public class DispositivosController : ControllerBase
    {
        private readonly IDispositivoRepositorio _repo;

        public DispositivosController(IDispositivoRepositorio repo)
        {
            _repo = repo;
        }

        
        // LISTAR TODOS LOS DISPOSITIVOS (PUBLICO)
       
        [AllowAnonymous]
        [HttpGet("listar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerTodosLosDispositivos()
        {
            var dispositivos = await _repo.ObtenerTodos();
            return Ok(dispositivos);
        }

        
        // LISTAR POR SERVICIO (PUBLICO)
        
        [AllowAnonymous]
        [HttpGet("ListarServicioPorId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerDispositivosPorServicio(Guid id)
        {
            var dispositivos = await _repo.ObtenerPorServicio(id);
            return Ok(dispositivos);
        }

        
        // RF14 - REGISTRAR DISPOSITIVO (Admin)
        
        [Authorize(Roles = "Usuario Administrador")]
        [HttpPost("registrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarDispositivo([FromBody] DispositivoCrearDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dispositivo = new Dispositivo
            {
                NombreDispositivo = dto.NombreDispositivo,
                Descripcion = dto.Descripcion,
                Marca = dto.Marca,
                Serial = dto.Serial,
                ServicioID = dto.ServicioID
            };

            await _repo.CrearDispositivo(dispositivo);

            return Ok(new { mensaje = "Equipo registrado correctamente." });
        }

        // RF15 - MODIFICAR DISPOSITIVO (Admin)
        
        [Authorize(Roles = "Usuario Administrador")]
        [HttpPut("actualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarDispositivo(Guid id, [FromBody] DispositivoActualizarDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dispositivo = await _repo.ObtenerPorId(id);
            if (dispositivo == null)
                return NotFound(new { mensaje = "Dispositivo no encontrado." });

            dispositivo.NombreDispositivo = dto.NombreDispositivo;
            dispositivo.Descripcion = dto.Descripcion;
            dispositivo.Marca = dto.Marca;
            dispositivo.Serial = dto.Serial;

            await _repo.Actualizar(dispositivo);

            return Ok(new { mensaje = "Dispositivo actualizado correctamente." });
        }

        
        // RF15 - ELIMINAR DISPOSITIVO (Admin)
        
        [Authorize(Roles = "Usuario Administrador")]
        [HttpDelete("eliminar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarDispositivo(Guid id)
        {
            var eliminado = await _repo.Eliminar(id);

            if (!eliminado)
                return NotFound(new { mensaje = "Dispositivo no encontrado." });

            return Ok(new { mensaje = "Dispositivo eliminado correctamente." });
        }
    }
}
