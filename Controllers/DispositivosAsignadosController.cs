using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HalconAlarm0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DispositivosAsignadosController : ControllerBase
    {
        private readonly IDispositivosAsignadosRepositorio _repo;

        public DispositivosAsignadosController(IDispositivosAsignadosRepositorio repo)
        {
            _repo = repo;
        }

        // ============================================================
        // GET: /api/DispositivosAsignados/usuario/{usuarioId}
        // ============================================================
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerPorUsuario(Guid usuarioId)
        {
            try
            {
                var asignaciones = await _repo.ObtenerPorUsuario(usuarioId);

                if (asignaciones == null || !asignaciones.Any())
                {
                    return NotFound(new
                    {
                        mensaje = "El usuario no tiene dispositivos asignados."
                    });
                }

                // Mapeo de salida con el dispositivo incluido
                var resultado = asignaciones.Select(x => new
                {
                    x.AsignacionID,
                    x.UsuarioID,
                    x.DispositivoID,
                    x.FechaAsignacion,

                    Dispositivo = new
                    {
                        x.Dispositivo!.DispositivoID,
                        x.Dispositivo.NombreDispositivo,
                        x.Dispositivo.Descripcion,
                        x.Dispositivo.Marca,
                        x.Dispositivo.Serial
                    }
                });

                return Ok(resultado);
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

        // ============================================================
        // POST: /api/DispositivosAsignados/asignar
        // ASIGNA un dispositivo a un usuario
        // ============================================================
        public class AsignarDispositivoDTO
        {
            public Guid UsuarioID { get; set; }
            public Guid DispositivoID { get; set; }
        }

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
