using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IHistorialNovedadesRepositorio
    {
        Task CrearHistorialAsync(HistorialNovedades historial);
        Task<IEnumerable<HistorialNovedades>> ObtenerPorNovedadAsync(Guid novedadID);
    }
}
