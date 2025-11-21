using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.Modelos
{
    public class Productos
    {
        
            [Key]
            public Guid ProductoID { get; set; }

            [Required]
            public string NombreProducto { get; set; } = string.Empty;

            public string? Marca { get; set; }

            public string? ImagenURL { get; set; }  // ✅ ahora coincide con la base de datos y el JSON
            public string? Modelo { get; set; }    // ✅ también debe ser string

    }
}
