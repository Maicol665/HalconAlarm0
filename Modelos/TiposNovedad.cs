namespace HalconAlarm0.Modelos
{
    public class TiposNovedad
    {
        private Guid _tipoNovedadID;
        private string _nombreTipo;

        public Guid TipoNovedadID
        {
            get => _tipoNovedadID;
            set => _tipoNovedadID = value;
        }

        public string NombreTipo
        {
            get => _nombreTipo;
            set => _nombreTipo = value;
        }
    }
}
