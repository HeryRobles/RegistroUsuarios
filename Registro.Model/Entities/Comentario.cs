using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registro.Model.Entities
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public int IdPelicula { get; set; }
        public int IdUsuario { get; set; }
        public string Texto { get; set; } = string.Empty;
        public double Calificacion { get; set; }
        public DateTime FechaComentario { get; set; } = DateTime.Now;
        public Pelicula Pelicula { get; set; }
        public Usuario Usuario { get; set; }
    }
}
