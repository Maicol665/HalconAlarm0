using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.Modelos
{
    public class Novedades
    {
        private Guid _novedadID;
        private Guid _usuarioID;
        private Guid _tipoNovedadID;
        private string _descripcion;
        private DateTime _fechaNovedad;
        private string _estado;

        public Guid NovedadID
        {
            get => _novedadID;
            set => _novedadID = value;
        }

        public Guid UsuarioID
        {
            get => _usuarioID;
            set => _usuarioID = value;
        }

        public Guid TipoNovedadID
        {
            get => _tipoNovedadID;
            set => _tipoNovedadID = value;
        }

        public string Descripcion
        {
            get => _descripcion;
            set => _descripcion = value;
        }

        public DateTime FechaNovedad
        {
            get => _fechaNovedad;
            set => _fechaNovedad = value;
        }

        public string Estado
        {
            get => _estado;
            set => _estado = value;
        }

        public Usuarios Usuario { get; set; }
        public TiposNovedad TipoNovedad { get; set; }
        public ICollection<HistorialNovedades> Historial { get; set; }
    }
}
