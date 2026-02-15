using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class RegistroUsuarioDTO
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

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$", ErrorMessage = "La contraseña debe contener minúsculas, mayúsculas, dígitos y caracteres especiales")]
        public required string Contrasena { get; set; }

        [Required(ErrorMessage = "El RolID es requerido")]
        public Guid RolID { get; set; }
    }
}
