using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalconAlarm0.Modelos
{
    public class Contacto
    {
        [Key]
        public Guid ContactoID { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [MaxLength(100)]
        public string? Apellidos { get; set; }

        [Required]
        [MaxLength(150)]
        public string CorreoElectronico { get; set; } = null!;

        [MaxLength(100)]
        public string? Ciudad { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        public Guid? ServicioID { get; set; }

        public Guid? ProductoID { get; set; }

        public DateTime FechaContacto { get; set; } = DateTime.Now;

        // Navegación opcional
        [ForeignKey(nameof(ServicioID))]
        public Servicios? Servicio { get; set; }

        [ForeignKey(nameof(ProductoID))]
        public Productos? Producto { get; set; }
    }
}
