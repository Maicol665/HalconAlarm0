using System;

namespace HalconAlarm0.DTOs.Contactos
{
    public class RegistrarContactoDTO
    {
        public string Nombre { get; set; } = null!;
        public string? Apellidos { get; set; }
        public string CorreoElectronico { get; set; } = null!;
        public string? Ciudad { get; set; }
        public string? Telefono { get; set; }
        public Guid? ServicioID { get; set; }
        public Guid? ProductoID { get; set; }
    }
}
