using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalconAlarm0.Modelos
{
    [Table("ServiciosContratados")]
    public class ServiciosContratados
    {
        [Key]
        public Guid ContratoID { get; set; }

        [Required]
        public Guid UsuarioID { get; set; }

        [Required]
        public Guid ServicioID { get; set; }

        public DateTime FechaContratacion { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ServicioID))]
        public Servicios? Servicio { get; set; }
    }
}
