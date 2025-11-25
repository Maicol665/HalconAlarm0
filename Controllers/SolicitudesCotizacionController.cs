using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalconAlarm0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesCotizacionController : ControllerBase
    {
        private readonly ISolicitudesCotizacionRepositorio _repositorio;

        public SolicitudesCotizacionController(ISolicitudesCotizacionRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [Authorize(Roles = "Usuario Administrador")]
        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var solicitudes = await _repositorio.ObtenerTodasAsync();
            return Ok(solicitudes);
        }

        [Authorize(Roles = "Usuario Administrador")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var solicitud = await _repositorio.ObtenerPorIdAsync(id);

            if (solicitud == null)
                return NotFound("Solicitud no encontrada.");

            return Ok(solicitud);
        }

        
        [Authorize(Roles = "Usuario Administrador")]
        [HttpPut("actualizar-estado/{id}")]
        public async Task<IActionResult> ActualizarEstado(Guid id, [FromBody] ActualizarEstadoDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.NuevoEstado))
                return BadRequest("Debe enviar el estado.");

            var estadosValidos = new List<string>
            {
                "Iniciado",
                "En progreso",
                "Completado"
            };

            if (!estadosValidos.Contains(dto.NuevoEstado))
                return BadRequest("Estado inválido.");

            bool actualizado = await _repositorio.ActualizarEstadoAsync(id, dto.NuevoEstado);

            if (!actualizado)
                return NotFound("Solicitud no encontrada.");

            return Ok("Estado actualizado exitosamente.");
        }
    }
}
