namespace HalconAlarm0.DTOs
{
    public class ModificarServicioDTO
    {
        public string NombreServicio { get; set; } = string.Empty;
        public string? TipoServicio { get; set; }
        public string? Descripcion { get; set; }
        public string? Beneficios { get; set; }
        public bool Activo { get; set; }
    }
}
