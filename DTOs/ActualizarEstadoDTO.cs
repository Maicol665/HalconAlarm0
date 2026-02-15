using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class ActualizarEstadoDTO
    {
        [Required(ErrorMessage = "El estado es requerido")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder 50 caracteres")]
        public required string NuevoEstado { get; set; }
    }
}
