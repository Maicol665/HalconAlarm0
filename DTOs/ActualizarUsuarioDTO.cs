using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class ActualizarUsuarioDTO
    {
        [Required(ErrorMessage = "Los nombres son requeridos")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 100 caracteres")]
        public required string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres")]
        public required string Apellidos { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public required string CorreoElectronico { get; set; }

        [Phone(ErrorMessage = "El teléfono no es válido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El RolID es requerido")]
        public required Guid RolID { get; set; }
    }
}
