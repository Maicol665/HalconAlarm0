using System;

namespace HalconAlarm0.Modelos
{
    public class Dispositivo
    {
        public Guid DispositivoID { get; set; }
        public string NombreDispositivo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public string? Serial { get; set; }

        // Relación
        public Guid ServicioID { get; set; }
        public Servicios Servicio { get; set; }
    }
}
