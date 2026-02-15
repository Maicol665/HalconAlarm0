using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IServiciosRepositorio
    {
        Task<IEnumerable<Servicios>> ObtenerTodos();
        Task<Servicios?> ObtenerPorId(Guid servicioId);
        Task<Servicios> Crear(Servicios servicio);
        Task<bool> Actualizar(Servicios servicio);
        Task<bool> Actualizar(Guid servicioId, string nombreServicio, string? tipoServicio, string? descripcion, string? beneficios, bool activo);
        Task<bool> ActualizarEstado(Guid servicioId, bool activo);
        Task<bool> Eliminar(Guid servicioId);
        Task<bool> TieneContratos(Guid servicioId);
    }
}
