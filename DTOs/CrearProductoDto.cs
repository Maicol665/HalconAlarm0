using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class CrearProductoDto
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string NombreProducto { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "La marca no puede exceder 50 caracteres")]
        public string? Marca { get; set; }

        [StringLength(50, ErrorMessage = "El modelo no puede exceder 50 caracteres")]
        public string? Modelo { get; set; }

        [Url(ErrorMessage = "La URL de imagen debe ser válida")]
        public string? ImagenURL { get; set; }
    }
}
