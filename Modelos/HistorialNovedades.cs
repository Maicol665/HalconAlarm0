namespace HalconAlarm0.Modelos
{
    public class HistorialNovedades
    {
        public Guid HistorialID { get; set; }
        public Guid NovedadID { get; set; }
        public DateTime FechaCambio { get; set; }
        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;
        public string Comentarios { get; set; } = string.Empty;

        public Novedades? Novedad { get; set; }
    }
}
