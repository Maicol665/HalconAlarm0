using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface INovedadesRepositorio
    {
        Task<Novedades> CrearNovedadAsync(Novedades novedad);
        Task<Novedades> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<Novedades>> ObtenerTodasAsync();
        Task<bool> ActualizarEstadoAsync(Guid novedadID, string nuevoEstado, string comentario);
    }
}
