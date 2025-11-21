using HalconAlarm0.Contexto;
using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace HalconAlarm0.Controllers
{

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

        // ============================================================
        // 🔹 SOLO EL ADMINISTRADOR REGISTRA SERVICIOS PARA UN CLIENTE
        // ============================================================
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] ContratarServicioDTO dto)
        {
            try
            {
                if (dto == null || dto.ServicioID == Guid.Empty || dto.UsuarioID == Guid.Empty)
                    return BadRequest("UsuarioID y ServicioID son obligatorios.");

                var servicio = await _context.Servicios
                    .FirstOrDefaultAsync(s => s.ServicioID == dto.ServicioID && s.Activo);

                if (servicio == null)
                    return BadRequest("Servicio no encontrado o inactivo.");

                var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioID == dto.UsuarioID);
                if (!usuarioExiste)
                    return BadRequest("El usuario no existe.");

                var yaExiste = await _repositorio.ExisteContrato(dto.UsuarioID, dto.ServicioID);
                if (yaExiste)
                    return Conflict("Este usuario ya tiene contratado este servicio.");

                var contrato = new ServiciosContratados
                {
                    ContratoID = Guid.NewGuid(),
                    UsuarioID = dto.UsuarioID,
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
                    usuarioId = dto.UsuarioID,
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

        // ============================================================
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerServiciosPorUsuario(Guid usuarioId)
        {
            try
            {
                if (usuarioId == Guid.Empty)
                    return BadRequest("UsuarioID es obligatorio.");
                var serviciosContratados = await _repositorio.ObtenerPorUsuario(usuarioId);
                if (serviciosContratados == null || !serviciosContratados.Any())
                    return NotFound("No se encontraron servicios contratados para este usuario.");
                var resultado = serviciosContratados.Select(sc => new
                {
                    sc.ContratoID,
                    sc.FechaContratacion,
                    Servicio = new
                    {
                        sc.Servicio!.ServicioID,
                        sc.Servicio.NombreServicio,
                        sc.Servicio.TipoServicio,
                        sc.Servicio.Descripcion
                    }
                });
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener los servicios contratados: " + ex.Message);
            }
        }
        // ============================================================
        [HttpDelete("eliminar/{contratoId}")]
        public async Task<IActionResult> EliminarContrato(Guid contratoId)
        {
            try
            {
                if (contratoId == Guid.Empty)
                    return BadRequest("ContratoID es obligatorio.");
                var eliminado = await _repositorio.EliminarContrato(contratoId);
                if (!eliminado)
                    return NotFound("Contrato no encontrado o ya eliminado.");
                return Ok(new { mensaje = "Contrato eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al eliminar el contrato: " + ex.Message);
            }


        }

        [HttpPost] 
        [Route("ObtenerTodosLosContratos")]
        public async Task<IActionResult> ObtenerTodosLosContratos()
        {
            try
            {
                var contratos = await _repositorio.ObtenerTodosLosContratos();
                var resultado = contratos.Select(sc => new
                {
                    sc.ContratoID,
                    sc.FechaContratacion,
                    Servicio = new
                    {
                        sc.Servicio!.ServicioID,
                        sc.Servicio.NombreServicio,
                        sc.Servicio.TipoServicio,
                        sc.Servicio.Descripcion
                    }
                });
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener todos los contratos: " + ex.Message);
            }
        }
    }
}