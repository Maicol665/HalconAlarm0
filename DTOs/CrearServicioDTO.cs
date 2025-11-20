namespace HalconAlarm0.DTOs
{
    public class CrearServicioDTO
    {
        public string NombreServicio { get; set; } = string.Empty;
        public string? TipoServicio { get; set; }
        public string? Descripcion { get; set; }
        public string? Beneficios { get; set; }
    }
}
