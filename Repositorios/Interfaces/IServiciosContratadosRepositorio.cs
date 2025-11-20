using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IServiciosContratadosRepositorio
    {
        Task<bool> ExisteContrato(Guid usuarioId, Guid servicioId);
        Task<bool> Registrar(ServiciosContratados contrato);
    }
}
