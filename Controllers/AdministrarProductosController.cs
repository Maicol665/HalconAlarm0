using HalconAlarm0.DTOs;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
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

        // GET: api/AdministrarProductos/listar
        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var productos = await _repositorio.ListarProductos();
            var productosDto = productos.Select(p => new ProductoDto
            {
                ProductoID = p.ProductoID,
                NombreProducto = p.NombreProducto,
                Marca = p.Marca,
                Modelo = p.Modelo,
                ImagenURL = p.ImagenURL
            });

            return Ok(productosDto);
        }

        // GET: api/AdministrarProductos/listarPorId/{id}
        [HttpGet("listarPorId/{id}")]
        public async Task<IActionResult> ObtenerProducto(Guid id)
        {
            var producto = await _repositorio.ObtenerProductoPorId(id);
            if (producto == null) return NotFound();

            var productoDto = new ProductoDto
            {
                ProductoID = producto.ProductoID,
                NombreProducto = producto.NombreProducto,
                Marca = producto.Marca,
                Modelo = producto.Modelo,
                ImagenURL = producto.ImagenURL
            };

            return Ok(productoDto);
        }

        // POST: api/AdministrarProductos/crear
        [HttpPost("crear")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            var productoCreadoDto = new ProductoDto
            {
                ProductoID = producto.ProductoID,
                NombreProducto = producto.NombreProducto,
                Marca = producto.Marca,
                Modelo = producto.Modelo,
                ImagenURL = producto.ImagenURL
            };

            return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.ProductoID }, productoCreadoDto);
        }

        // PUT: api/AdministrarProductos/modificar/{id}
        [HttpPut("modificar/{id}")]
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
                return NotFound();

            return NoContent();
        }

        // DELETE: api/AdministrarProductos/eliminar/{id}
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var resultado = await _repositorio.EliminarProducto(id);
            if (!resultado) return NotFound();

            return NoContent();
        }
    }
}
