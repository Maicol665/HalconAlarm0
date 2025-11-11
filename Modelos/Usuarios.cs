using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Modelos
{
    [Table("Usuarios")]
    [Index(nameof(CorreoElectronico), IsUnique = true)] // Índice único en CorreoElectronico
    public class Usuarios
    {
        [Key]
        public Guid UsuarioID { get; set; }
        [Required, StringLength(100)]
        public string Nombres { get; set; }
        [Required, StringLength(100)]
        public string Apellidos { get; set; }
        [Required, StringLength(150)]
        public string CorreoElectronico { get; set; }
        [StringLength(20)]
        public string? Telefono { get; set; }
        [Required, StringLength(50)]
        public string TipoUsuario { get; set; }= "Local";
        [Required, StringLength(255)]
        public string ContrasenaHash { get; set; }
        [Required, StringLength(255)]
        public string ContrasenaSalt { get; set; }
        
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public Guid RolID { get; set; }
        [ForeignKey(nameof(RolID))]

        public  Roles? Rol { get; set; }


    }
}
