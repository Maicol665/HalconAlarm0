using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class AsignarDispositivoDTO
    {
        [Required(ErrorMessage = "El UsuarioID es requerido")]
        public Guid UsuarioID { get; set; }

        [Required(ErrorMessage = "El DispositivoID es requerido")]
        public Guid DispositivoID { get; set; }
    }
}
