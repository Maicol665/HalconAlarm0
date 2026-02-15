using System;
using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs.Contactos
{
    public class RegistrarContactoDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres")]
        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string CorreoElectronico { get; set; } = null!;

        [StringLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
        public string? Ciudad { get; set; }

        [Phone(ErrorMessage = "El teléfono no es válido")]
        public string? Telefono { get; set; }

        public Guid? ServicioID { get; set; }

        public Guid? ProductoID { get; set; }
    }
}
