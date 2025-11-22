using HalconAlarm0.DTOs.Contactos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HalconAlarm0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactosRepositorio _repositorio;
        private readonly ILogger<ContactosController> _logger;

        public ContactosController(IContactosRepositorio repositorio, ILogger<ContactosController> logger)
        {
            _repositorio = repositorio;
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

                var contacto = await _repositorio.RegistrarContactoAsync(dto);

                return Ok(new { mensaje = "Contacto registrado", contacto });
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
    }
}
