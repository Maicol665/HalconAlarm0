namespace HalconAlarm0.DTOs
{
    public class ActualizarProductoDto
    {
        public string NombreProducto { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? ImagenURL { get; set; }
    }
}
