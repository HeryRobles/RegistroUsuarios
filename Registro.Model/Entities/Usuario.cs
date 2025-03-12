namespace Registro.Model.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string? NombreCompleto { get; set; }

        public string? Correo { get; set; }

        public int? IdRol { get; set; }

        public string? Clave { get; set; }

        public string Telefono { get; set; }

        public bool? EsActivo { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
    }
}
