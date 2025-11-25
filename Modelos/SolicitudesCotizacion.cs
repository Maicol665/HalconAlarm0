using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalconAlarm0.Modelos
{
    public class SolicitudesCotizacion
    {
        [Key]
        public Guid SolicitudID { get; set; } = Guid.NewGuid();

        // FK obligatoria
        [Required]
        public Guid ContactoID { get; set; }

        // FKs opcionales
        public Guid? ServicioID { get; set; }
        public Guid? ProductoID { get; set; }

        // Estado
        [Required]
        [MaxLength(50)]
        public string Estado { get; set; } = "Iniciado";

        // Fecha
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

        // Relaciones
        [ForeignKey("ContactoID")]
        public Contacto? Contacto { get; set; }

        [ForeignKey("ServicioID")]
        public Servicios? Servicio { get; set; }

        [ForeignKey("ProductoID")]
        public Productos? Producto { get; set; }
    }
}
