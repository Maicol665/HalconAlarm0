using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface ISolicitudesCotizacionRepositorio
    {
        Task<IEnumerable<SolicitudesCotizacion>> ObtenerTodasAsync();
        Task<SolicitudesCotizacion?> ObtenerPorIdAsync(Guid id);
        Task<bool> ActualizarEstadoAsync(Guid id, string nuevoEstado);

        // <-- nuevo
        Task<SolicitudesCotizacion> CrearSolicitudAsync(SolicitudesCotizacion solicitud);
    }
}
