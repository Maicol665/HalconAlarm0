using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class NovedadEstadoDTO
    {
        [Required(ErrorMessage = "El nuevo estado es requerido")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder 50 caracteres")]
        public required string NuevoEstado { get; set; }

        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "El comentario debe tener entre 3 y 500 caracteres")]
        public required string Comentario { get; set; }
    }
}
