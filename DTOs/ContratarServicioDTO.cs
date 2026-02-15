using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class ContratarServicioDTO
    {
        [Required(ErrorMessage = "El ServicioID es requerido")]
        public Guid ServicioID { get; set; }

        [Required(ErrorMessage = "El UsuarioID es requerido")]
        public Guid UsuarioID { get; set; }
    }
}
