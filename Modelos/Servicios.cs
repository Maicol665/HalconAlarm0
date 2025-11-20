using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalconAlarm0.Modelos
{
    [Table("Servicios")]
    public class Servicios
    {
        [Key]
        public Guid ServicioID { get; set; }

        [Required, StringLength(100)]
        public required string NombreServicio { get; set; }

        public string? TipoServicio { get; set; }
        public string? Descripcion { get; set; }
        public string? Beneficios { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<ServiciosContratados>? ServiciosContratados { get; set; }
    }
}
