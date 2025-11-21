using HalconAlarm0.Contexto;
using HalconAlarm0.Modelos;
using HalconAlarm0.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Repositorios
{
    public class AdministracionProductos : IAdministracionProductos
    {
        private readonly ContextoHalconAlarm0 _context;

        public AdministracionProductos(ContextoHalconAlarm0 context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IEnumerable<Productos>> ListarProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        // OBTENER POR ID
        public async Task<Productos?> ObtenerProductoPorId(Guid id)
        {
            return await _context.Productos.FindAsync(id);
        }

        // CREAR
        public async Task<bool> CrearProducto(Productos producto)
        {
            await _context.Productos.AddAsync(producto);
            return await _context.SaveChangesAsync() > 0;
        }

        // MODIFICAR
        public async Task<bool> ModificarProducto(Guid id, Productos productoActualizado)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            producto.NombreProducto = productoActualizado.NombreProducto;
            producto.Marca = productoActualizado.Marca;
            producto.Modelo = productoActualizado.Modelo;
            producto.ImagenURL = productoActualizado.ImagenURL;

            _context.Productos.Update(producto);
            return await _context.SaveChangesAsync() > 0;
        }

        // ELIMINAR
        public async Task<bool> EliminarProducto(Guid id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Productos>> FiltrarProductos(string? nombre, string? marca, string? modelo)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => p.NombreProducto.Contains(nombre));

            if (!string.IsNullOrWhiteSpace(marca))
                query = query.Where(p => p.Marca.Contains(marca));

            if (!string.IsNullOrWhiteSpace(modelo))
                query = query.Where(p => p.Modelo.Contains(modelo));

            return await query.ToListAsync();
        }

    }
}
