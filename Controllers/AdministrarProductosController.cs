using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalconAlarm0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrarProductosController : ControllerBase
    {
        private readonly IAdministracionProductos _repositorio;

        public AdministrarProductosController(IAdministracionProductos repositorio)
        {
            _repositorio = repositorio;
        }

        // ✔ CUALQUIER USUARIO LOGUEADO
        [Authorize]
        [HttpGet("listar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Listar()
        {
            var productos = await _repositorio.ListarProductos();

            return Ok(productos.Select(p => new ProductoDto
            {
                ProductoID = p.ProductoID,
                NombreProducto = p.NombreProducto,
                Marca = p.Marca,
                Modelo = p.Modelo,
                ImagenURL = p.ImagenURL
            }));
        }

        // ✔ CUALQUIER USUARIO LOGUEADO
        [Authorize]
        [HttpGet("listarPorId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerProducto(Guid id)
        {
            var producto = await _repositorio.ObtenerProductoPorId(id);
            if (producto == null) return NotFound();

            return Ok(new ProductoDto
            {
                ProductoID = producto.ProductoID,
                NombreProducto = producto.NombreProducto,
                Marca = producto.Marca,
                Modelo = producto.Modelo,
                ImagenURL = producto.ImagenURL
            });
        }

        // ✔ SOLO ADMIN
        [Authorize(Roles = "Usuario Administrador")]
        [HttpPost("crear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Crear([FromBody] CrearProductoDto productoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producto = new Productos
            {
                ProductoID = Guid.NewGuid(),
                NombreProducto = productoDto.NombreProducto,
                Marca = productoDto.Marca,
                Modelo = productoDto.Modelo,
                ImagenURL = productoDto.ImagenURL
            };

            var resultado = await _repositorio.CrearProducto(producto);
            if (!resultado)
                return StatusCode(500, "No se pudo crear el producto");

            return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.ProductoID }, producto);
        }

        // ✔ SOLO ADMIN
        [Authorize(Roles = "Usuario Administrador")]
        [HttpPut("modificar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Modificar(Guid id, [FromBody] ActualizarProductoDto productoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productoActualizado = new Productos
            {
                NombreProducto = productoDto.NombreProducto,
                Marca = productoDto.Marca,
                Modelo = productoDto.Modelo,
                ImagenURL = productoDto.ImagenURL
            };

            var resultado = await _repositorio.ModificarProducto(id, productoActualizado);
            if (!resultado)
                return NotFound("Producto no encontrado");

            return NoContent();
        }

        // ✔ SOLO ADMIN
        [Authorize(Roles = "Usuario Administrador")]
        [HttpDelete("eliminar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var resultado = await _repositorio.EliminarProducto(id);
            if (!resultado) return NotFound();

            return NoContent();
        }

        // ✔ CUALQUIER USUARIO LOGUEADO
        [Authorize]
        [HttpGet("filtrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Filtrar(
            [FromQuery] string? nombre,
            [FromQuery] string? marca,
            [FromQuery] string? modelo)
        {
            var productos = await _repositorio.FiltrarProductos(nombre, marca, modelo);

            return Ok(productos.Select(p => new ProductoDto
            {
                ProductoID = p.ProductoID,
                NombreProducto = p.NombreProducto,
                Marca = p.Marca,
                Modelo = p.Modelo,
                ImagenURL = p.ImagenURL
            }));
        }
    }
}
