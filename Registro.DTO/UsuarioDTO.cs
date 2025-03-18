using System.ComponentModel.DataAnnotations;

namespace Registro.DTO
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string? Correo { get; set; }
        public string Telefono { get; set; }

        public int? IdRol { get; set; }

        public string? RolDescripcion { get; set; }

        [Required(ErrorMessage = "Escribir contraseña")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(100)]
        public string? Clave { get; set; }

        public int? EsActivo { get; set; }

    }
}
