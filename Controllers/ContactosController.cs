using HalconAlarm0.DTOs.Contactos;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HalconAlarm0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactosRepositorio _repositorio;
        private readonly ISolicitudesCotizacionRepositorio _soliRepositorio;
        private readonly ILogger<ContactosController> _logger;

        public ContactosController(
            IContactosRepositorio repositorio,
            ISolicitudesCotizacionRepositorio soliRepositorio,
            ILogger<ContactosController> logger)
        {
            _repositorio = repositorio;
            _soliRepositorio = soliRepositorio;
            _logger = logger;
        }

        // 📌 Público – cualquier usuario puede registrar una solicitud (RF21)
        [AllowAnonymous]
        [HttpPost("registrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registrar([FromBody] RegistrarContactoDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Payload vacío");

                if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.CorreoElectronico))
                    return BadRequest("Nombre y CorreoElectronico son obligatorios.");

                if (dto.ServicioID == null && dto.ProductoID == null)
                    return BadRequest("Debe seleccionar un Servicio o un Producto de interés.");

                var contacto = await _repositorio.RegistrarContactoAsync(dto);

                var solicitud = new SolicitudesCotizacion
                {
                    ContactoID = contacto.ContactoID,
                    ServicioID = dto.ServicioID,
                    ProductoID = dto.ProductoID,
                    Estado = "Iniciado",
                    FechaSolicitud = DateTime.UtcNow
                };

                var solicitudCreada = await _soliRepositorio.CrearSolicitudAsync(solicitud);

                return Ok(new
                {
                    mensaje = "Contacto registrado y solicitud creada",
                    contacto,
                    solicitud = solicitudCreada
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al registrar contacto");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar contacto");
                return StatusCode(500, "Error interno");
            }
        }

        // 🔒 Solo Admin y Asesor pueden ver la lista completa
        [Authorize(Roles = "Admin,Asesor")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Listar([FromQuery] int? take = 50, [FromQuery] int? skip = 0)
        {
            var lista = await _repositorio.ListarContactosAsync(take, skip);
            return Ok(lista);
        }

        // 🔒 Solo Admin y Asesor pueden consultar contacto por ID
        [Authorize(Roles = "Admin,Asesor")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Obtener(Guid id)
        {
            var contacto = await _repositorio.ObtenerPorIdAsync(id);
            if (contacto == null) return NotFound();
            return Ok(contacto);
        }

        // 🔒 Solo Admin puede eliminar contactos
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var eliminado = await _repositorio.EliminarContactoAsync(id);

            if (!eliminado)
                return NotFound("Contacto no encontrado");

            return Ok(new { mensaje = "Contacto eliminado" });
        }
    }
}
