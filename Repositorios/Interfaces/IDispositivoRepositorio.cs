using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IDispositivoRepositorio
    {
        Task<IEnumerable<Dispositivo>> ObtenerTodos();
        Task<Dispositivo> CrearDispositivo(Dispositivo dispositivo);
        Task<Dispositivo?> ObtenerPorId(Guid dispositivoId);
        Task<IEnumerable<Dispositivo>> ObtenerPorServicio(Guid servicioId);
        Task<IEnumerable<Dispositivo>> ObtenerPorUsuario(Guid usuarioId);
        Task<bool> Actualizar(Dispositivo dispositivo);
        Task<bool> Eliminar(Guid id);
    }
}
