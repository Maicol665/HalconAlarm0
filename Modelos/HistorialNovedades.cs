namespace HalconAlarm0.Modelos
{
    public class HistorialNovedades
    {
        private Guid _historialID;
        private Guid _novedadID;
        private DateTime _fechaCambio;
        private string _estadoAnterior;
        private string _estadoNuevo;
        private string _comentarios;

        public Guid HistorialID
        {
            get => _historialID;
            set => _historialID = value;
        }

        public Guid NovedadID
        {
            get => _novedadID;
            set => _novedadID = value;
        }

        public DateTime FechaCambio
        {
            get => _fechaCambio;
            set => _fechaCambio = value;
        }

        public string EstadoAnterior
        {
            get => _estadoAnterior;
            set => _estadoAnterior = value;
        }

        public string EstadoNuevo
        {
            get => _estadoNuevo;
            set => _estadoNuevo = value;
        }

        public string Comentarios
        {
            get => _comentarios;
            set => _comentarios = value;
        }

        public Novedades Novedad { get; set; }
    }
}
