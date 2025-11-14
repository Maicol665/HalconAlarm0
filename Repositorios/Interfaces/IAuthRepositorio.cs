using System.Threading.Tasks;
using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IAuthRepositorio
    {
        Task<string?> LoginAsync(LoginRequest request);
    }
}