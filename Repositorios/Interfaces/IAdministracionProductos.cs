using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IAdministracionProductos
    {
        Task<IEnumerable<Productos>> ListarProductos();
        Task<Productos?> ObtenerProductoPorId(Guid id);
        Task<bool> CrearProducto(Productos producto);
        Task<bool> ModificarProducto(Guid id, Productos productoActualizado);
        Task<bool> EliminarProducto(Guid id);
        Task<IEnumerable<Productos>> FiltrarProductos(string? nombre, string? marca, string? modelo);

    }
}
