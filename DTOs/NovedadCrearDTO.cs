using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class NovedadCrearDTO
    {
        [Required(ErrorMessage = "El UsuarioID es requerido")]
        public Guid UsuarioID { get; set; }

        [Required(ErrorMessage = "El TipoNovedadID es requerido")]
        public Guid TipoNovedadID { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 1000 caracteres")]
        public required string Descripcion { get; set; }
    }
}
