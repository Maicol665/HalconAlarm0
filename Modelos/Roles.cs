using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HalconAlarm0.Modelos
{

    [Table("Roles")]
    public class Roles
    {
        [Key]
        public Guid RolID { get; set; }

        [Required, StringLength(50)]
        public string NombreRol { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Usuarios>? Usuarios { get; set; }    

    }
}
