using HalconAlarm0.DTOs.Contactos;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarContactoDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Payload vacío");

                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.CorreoElectronico))
                    return BadRequest("Nombre y CorreoElectronico son obligatorios.");

                if (dto.ServicioID == null && dto.ProductoID == null)
                    return BadRequest("Debe seleccionar un Servicio o un Producto de interés.");

                // 1) Registrar el contacto (asumo que tu repositorio acepta el DTO)
                var contacto = await _repositorio.RegistrarContactoAsync(dto);
                // contacto.ContactoID debe contener el id generado

                // 2) Crear la solicitud asociada (RF21: "Registrar y enviar solicitud")
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

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int? take = 50, [FromQuery] int? skip = 0)
        {
            var lista = await _repositorio.ListarContactosAsync(take, skip);
            return Ok(lista);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Obtener(Guid id)
        {
            var contacto = await _repositorio.ObtenerPorIdAsync(id);
            if (contacto == null) return NotFound();
            return Ok(contacto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var eliminado = await _repositorio.EliminarContactoAsync(id);

            if (!eliminado)
                return NotFound("Contacto no encontrado");

            return Ok(new { mensaje = "Contacto eliminado" });
        }
    }
}
