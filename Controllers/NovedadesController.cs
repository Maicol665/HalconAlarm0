using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalconAlarm0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NovedadesController : ControllerBase
    {
        private readonly INovedadesRepositorio _repoNovedades;
        private readonly IHistorialNovedadesRepositorio _repoHistorial;

        public NovedadesController(INovedadesRepositorio repoNovedades, IHistorialNovedadesRepositorio repoHistorial)
        {
            _repoNovedades = repoNovedades;
            _repoHistorial = repoHistorial;
        }

        [Authorize]
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarNovedad([FromBody] NovedadCrearDTO dto)
        {
            var novedad = new Novedades
            {
                UsuarioID = dto.UsuarioID,
                TipoNovedadID = dto.TipoNovedadID,
                Descripcion = dto.Descripcion,
                FechaNovedad = DateTime.Now,
                Estado = "Abierto"
            };

            await _repoNovedades.CrearNovedadAsync(novedad);
            return Ok(new { mensaje = "Novedad registrada", novedad });
        }

        [Authorize(Roles = "Usuario Administrador")]
        [HttpPut("actualizar-estado/{id}")]
        public async Task<IActionResult> ActualizarEstado(Guid id, [FromBody] NovedadEstadoDTO dto)
        {
            bool actualizado = await _repoNovedades.ActualizarEstadoAsync(id, dto.NuevoEstado, dto.Comentario);

            if (!actualizado)
                return NotFound("Novedad no encontrada");

            return Ok("Estado actualizado correctamente");
        }

        [Authorize(Roles = "Usuario Administrador")]
        [HttpGet("historial/{id}")]
        public async Task<IActionResult> ObtenerHistorial(Guid id)
        {
            var historial = await _repoHistorial.ObtenerPorNovedadAsync(id);
            return Ok(historial);
        }

        [Authorize(Roles = "Usuario Administrador")]
        [HttpGet("listar")]
        public async Task<IActionResult> ListarNovedades()
        {
            var novedades = await _repoNovedades.ObtenerTodasAsync();
            return Ok(novedades);
        }

        [Authorize(Roles = "Usuario Administrador")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var novedad = await _repoNovedades.ObtenerPorIdAsync(id);

            if (novedad == null)
                return NotFound("Novedad no encontrada");

            return Ok(novedad);
        }



    }
}
