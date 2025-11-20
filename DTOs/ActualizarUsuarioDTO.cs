namespace HalconAlarm0.DTOs
{
    public class ActualizarUsuarioDTO
    {
        public required string Nombres { get; set; }
        public required string Apellidos { get; set; }
        public required string CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public required Guid RolID { get; set; }
    }
}
