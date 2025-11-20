using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IDispositivosAsignadosRepositorio
    {
        Task<IEnumerable<DispositivosAsignados>> ObtenerTodosAsync();
        Task<DispositivosAsignados?> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<DispositivosAsignados>> ObtenerPorUsuario(Guid usuarioId);
        Task CrearAsync(DispositivosAsignados entidad);
        Task GuardarAsync();
    }
}
