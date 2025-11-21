using HalconAlarm0.Contexto;
using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HalconAlarm0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "3.1 Servicios Contratados")]
    [ApiController]
    public class ServiciosContratadosController : ControllerBase
    {
        private readonly ContextoHalconAlarm0 _context;
        private readonly IServiciosContratadosRepositorio _repositorio;

        public ServiciosContratadosController(IServiciosContratadosRepositorio repositorio,
                                              ContextoHalconAlarm0 context)
        {
            _repositorio = repositorio;
            _context = context;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] ContratarServicioDTO dto)
        {
            try
            {
                if (dto == null || dto.ServicioID == Guid.Empty)
                    return BadRequest("ServicioID es obligatorio.");

                var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(usuarioIdString) || !Guid.TryParse(usuarioIdString, out var usuarioID))
                    return Unauthorized("Usuario no autenticado correctamente.");

                var servicio = await _context.Servicios
                    .FirstOrDefaultAsync(s => s.ServicioID == dto.ServicioID && s.Activo);

                if (servicio == null)
                    return BadRequest("Servicio no encontrado o inactivo.");

                var yaExiste = await _repositorio.ExisteContrato(usuarioID, dto.ServicioID);
                if (yaExiste)
                    return Conflict("Ya tienes registrado este servicio.");

                var contrato = new ServiciosContratados
                {
                    ContratoID = Guid.NewGuid(),
                    UsuarioID = usuarioID,
                    ServicioID = dto.ServicioID,
                    FechaContratacion = DateTime.UtcNow
                };

                var registrado = await _repositorio.Registrar(contrato);
                if (!registrado)
                    return StatusCode(500, "Error registrando el servicio contratado.");

                return Ok(new
                {
                    mensaje = "Servicio contratado exitosamente",
                    contratoId = contrato.ContratoID,
                    servicio = new
                    {
                        servicio.ServicioID,
                        servicio.NombreServicio,
                        servicio.TipoServicio,
                        servicio.Descripcion
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al registrar el servicio contratado: " + ex.Message);
            }
        }
    }
}
