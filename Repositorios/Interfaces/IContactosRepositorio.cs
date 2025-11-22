using HalconAlarm0.DTOs.Contactos;
using HalconAlarm0.Modelos;
using System.Threading.Tasks;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IContactosRepositorio
    {
        Task<Contacto> RegistrarContactoAsync(RegistrarContactoDTO dto);
        Task<IEnumerable<Contacto>> ListarContactosAsync(int? take = null, int? skip = null);
        Task<Contacto?> ObtenerPorIdAsync(Guid contactoId);
    }
}
