using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.Modelos
{
    public class Novedades
    {
        public Guid NovedadID { get; set; }
        public Guid UsuarioID { get; set; }
        public Guid TipoNovedadID { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaNovedad { get; set; }
        public string Estado { get; set; } = "Abierto";

        public Usuarios? Usuario { get; set; }
        public TiposNovedad? TipoNovedad { get; set; }
        public ICollection<HistorialNovedades>? Historial { get; set; }
    }
}