using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class ModificarServicioDTO
    {
        [Required(ErrorMessage = "El nombre del servicio es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string NombreServicio { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "El tipo de servicio no puede exceder 50 caracteres")]
        public string? TipoServicio { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [StringLength(500, ErrorMessage = "Los beneficios no pueden exceder 500 caracteres")]
        public string? Beneficios { get; set; }

        [Required(ErrorMessage = "El estado activo es requerido")]
        public bool Activo { get; set; }
    }
}
