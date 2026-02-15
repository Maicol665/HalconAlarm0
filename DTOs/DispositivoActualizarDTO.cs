using System.ComponentModel.DataAnnotations;

public class DispositivoActualizarDTO
{
    [Required(ErrorMessage = "El nombre del dispositivo es requerido")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public required string NombreDispositivo { get; set; }

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string? Descripcion { get; set; }

    [StringLength(50, ErrorMessage = "La marca no puede exceder 50 caracteres")]
    public string? Marca { get; set; }

    [StringLength(50, ErrorMessage = "El serial no puede exceder 50 caracteres")]
    public string? Serial { get; set; }
}
