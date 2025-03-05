using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registro.Model.Entities
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual ICollection<MenuRol> MenuRoles { get; set; } = new List<MenuRol>();

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
