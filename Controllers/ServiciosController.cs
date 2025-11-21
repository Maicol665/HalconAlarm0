using HalconAlarm0.Contexto;
using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "3.0 Servicios")]
    [ApiController]

    public class ServiciosController : ControllerBase
    {
        private readonly ContextoHalconAlarm0 _context;

        public ServiciosController(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarServicios()
        {
            var servicios = await _context.Servicios.ToListAsync();
            return Ok(servicios);
        }

        [HttpPost("crear")]
        [AllowAnonymous]
        public async Task<IActionResult> CrearServicio([FromBody] CrearServicioDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var servicio = new Servicios
                {
                    ServicioID = Guid.NewGuid(),
                    NombreServicio = dto.NombreServicio.Trim(),
                    TipoServicio = dto.TipoServicio?.Trim(),
                    Descripcion = dto.Descripcion?.Trim(),
                    Beneficios = dto.Beneficios?.Trim(),
                    Activo = true
                };

                await _context.Servicios.AddAsync(servicio);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Servicio registrado correctamente",
                    servicioID = servicio.ServicioID
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al crear el servicio: " + ex.Message);
            }
        }

        [HttpPut("modificar/{id}")]
        public async Task<IActionResult> ModificarServicio(Guid id, ModificarServicioDTO dto)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
                return NotFound("Servicio no encontrado.");

            servicio.NombreServicio = dto.NombreServicio;
            servicio.Descripcion = dto.Descripcion;
            servicio.TipoServicio = dto.TipoServicio;
            servicio.Beneficios = dto.Beneficios;
            servicio.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return Ok("Servicio modificado correctamente.");
        }

        [HttpPatch("estado/{id}")]
        public async Task<IActionResult> CambiarEstadoServicio(Guid id, [FromQuery] bool activo)
        {
            try
            {
                var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.ServicioID == id);

                if (servicio == null)
                    return NotFound("El servicio no existe.");

                servicio.Activo = activo;

                _context.Servicios.Update(servicio);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = activo ? "Servicio activado correctamente" : "Servicio desactivado correctamente",
                    servicioID = servicio.ServicioID,
                    estadoActual = servicio.Activo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al cambiar el estado del servicio: " + ex.Message);
            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarServicio(Guid id)
        {
            try
            {
                var servicio = await _context.Servicios
                                             .FirstOrDefaultAsync(s => s.ServicioID == id);

                if (servicio == null)
                    return NotFound("El servicio no existe.");

                bool tieneContratos = await _context.ServiciosContratados
                                                   .AnyAsync(c => c.ServicioID == id);

                if (tieneContratos)
                {
                    servicio.Activo = false;
                    _context.Servicios.Update(servicio);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        mensaje = "El servicio está contratado, no puede eliminarse. Fue desactivado.",
                        servicioID = servicio.ServicioID
                    });
                }

                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Servicio eliminado definitivamente.",
                    servicioID = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al eliminar el servicio: " + ex.Message);
            }
        }
    }
}
