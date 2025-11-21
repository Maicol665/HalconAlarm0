namespace HalconAlarm0.DTOs
{
    public class DispositivoAsignadoDTO
    {
        public Guid AsignacionID { get; set; }
        public Guid UsuarioID { get; set; }
        public Guid DispositivoID { get; set; }
        public string NombreDispositivo { get; set; }
        public string Marca { get; set; }
        public string Serial { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
