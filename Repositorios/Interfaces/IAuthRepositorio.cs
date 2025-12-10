using System.Threading.Tasks;
using HalconAlarm0.Modelos;

namespace HalconAlarm0.Repositorios.Interfaces
{
    public interface IAuthRepositorio
    {
        // Login normal con hash + salt
        Task<string?> LoginAsync(LoginRequest request);

        // RF09: Verificar si el correo existe
        Task<bool> VerificarCorreoExistenteAsync(string correo);

        // NUEVO: Generar token seguro y enviar correo (flujo seguro)
        Task<bool> GenerarTokenRestablecimientoAsync(string correo);

        // NUEVO: Restablecer contraseña usando token seguro
        Task<bool> RestablecerContrasenaConTokenAsync(string token, string nuevaContrasena);


    }
}