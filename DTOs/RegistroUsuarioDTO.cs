namespace HalconAlarm0.DTOs
{
    public class RegistroUsuarioDTO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public string Contrasena { get; set; }
        public Guid RolID { get; set; }
    }
}
