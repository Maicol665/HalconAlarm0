using System.ComponentModel.DataAnnotations;

namespace HalconAlarm0.DTOs
{
    public class DispositivoAsignadoDTO
    {
        [Required(ErrorMessage = "El AsignacionID es requerido")]
        public Guid AsignacionID { get; set; }

        [Required(ErrorMessage = "El UsuarioID es requerido")]
        public Guid UsuarioID { get; set; }

        [Required(ErrorMessage = "El DispositivoID es requerido")]
        public Guid DispositivoID { get; set; }

        [Required(ErrorMessage = "El nombre del dispositivo es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public required string NombreDispositivo { get; set; }

        [Required(ErrorMessage = "La marca es requerida")]
        [StringLength(50, ErrorMessage = "La marca no puede exceder 50 caracteres")]
        public required string Marca { get; set; }

        [Required(ErrorMessage = "El serial es requerido")]
        [StringLength(50, ErrorMessage = "El serial no puede exceder 50 caracteres")]
        public required string Serial { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public required string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de asignación es requerida")]
        public DateTime FechaAsignacion { get; set; }
    }
}
