using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HalconAlarm0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "4.1 Dispositivos Asignados")]
    public class DispositivosAsignadosController : ControllerBase
    {
        private readonly IDispositivosAsignadosRepositorio _repo;

        public DispositivosAsignadosController(IDispositivosAsignadosRepositorio repo)
        {
            _repo = repo;
        }

        // ============================================================
        // GET: /api/DispositivosAsignados/asignados-al-usuario/{id}
        // Solo Admin y Tecnico
        // ============================================================
        [Authorize(Roles = "Admin,Tecnico")]
        [HttpGet("asignados-al-usuario/{id}")]
        public async Task<IActionResult> ObtenerAsignadosPorUsuario(Guid id)
        {
            try
            {
                var asignaciones = await _repo.ObtenerPorUsuario(id);

                if (asignaciones == null || !asignaciones.Any())
                {
                    return NotFound(new
                    {
                        mensaje = "El usuario no tiene dispositivos asignados."
                    });
                }

                var resultado = asignaciones.Select(a => new DispositivoAsignadoDTO
                {
                    AsignacionID = a.AsignacionID,
                    UsuarioID = a.UsuarioID,
                    DispositivoID = a.DispositivoID,
                    FechaAsignacion = a.FechaAsignacion,
                    NombreDispositivo = a.Dispositivo?.NombreDispositivo ?? "",
                    Marca = a.Dispositivo?.Marca ?? "",
                    Serial = a.Dispositivo?.Serial ?? "",
                    Descripcion = a.Dispositivo?.Descripcion ?? ""
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error en el servidor", detalle = ex.Message });
            }
        }


        // ============================================================
        // POST: /api/DispositivosAsignados/asignar
        // Solo Admin y Tecnico
        // ============================================================
        [Authorize(Roles = "Admin,Tecnico")]
        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarDispositivo([FromBody] AsignarDispositivoDTO dto)
        {
            try
            {
                var resultado = await _repo.AsignarDispositivo(dto.UsuarioID, dto.DispositivoID);

                if (!resultado)
                {
                    return BadRequest(new
                    {
                        mensaje = "No se pudo asignar el dispositivo. Verifique que el usuario y el dispositivo existan."
                    });
                }

                return Ok(new
                {
                    mensaje = "Dispositivo asignado correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error en el servidor",
                    detalle = ex.Message
                });
            }
        }
    }
}
