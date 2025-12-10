using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalconAlarm0.Modelos
{
    [Table("Usuarios")]
    [Index(nameof(CorreoElectronico), IsUnique = true)]
    public class Usuarios
    {
        [Key]
        public Guid UsuarioID { get; set; }

        [Required, StringLength(100)]
        public required string Nombres { get; set; }

        [Required, StringLength(100)]
        public required string Apellidos { get; set; }

        [Required, StringLength(150)]
        public required string CorreoElectronico { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [Required, StringLength(255)]
        public required string ContrasenaHash { get; set; }

        [Required, StringLength(255)]
        public required string ContrasenaSalt { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string? TokenRestablecimiento { get; set; }
        public DateTime? TokenExpiracion { get; set; }


        public Guid RolID { get; set; }

        [ForeignKey(nameof(RolID))]
        public Roles? Rol { get; set; }
    }
}
