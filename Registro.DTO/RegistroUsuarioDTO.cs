using System.ComponentModel.DataAnnotations;

namespace Registro.DTO
{
    public class RegistroUsuarioDTO
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Correo { get; set; }
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Escribir contraseña")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Clave { get; set; }

        public int? IdRol { get; set; }

        public string? RolDescripcion { get; set; }

    }
}
