using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalconAlarm0.Modelos
{
    [Table("DispositivosAsignados")]
    public class DispositivosAsignados
    {
        [Key]
        public Guid AsignacionID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioID { get; set; }

        [Required]
        public Guid DispositivoID { get; set; }

        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

        [ForeignKey("DispositivoID")]
        public Dispositivo? Dispositivo { get; set; }

        [ForeignKey("UsuarioID")]
        public Usuarios? Usuario { get; set; }


    }
}
