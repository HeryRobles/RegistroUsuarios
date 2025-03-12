namespace Registro.Model.Entities
{
    public class Menu
    {
        public int IdMenu { get; set; }

        public string? Nombre { get; set; }

        public string? Icono { get; set; }

        public string? Url { get; set; }

        public virtual ICollection<MenuRol> MenuRoles { get; set; } = new List<MenuRol>();
    }
}
