using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registro.DTO
{
    public class PeliculaDTO
    {
        public int IdPelicula { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public string Reseña { get; set; }
        public string ImagenUrl { get; set; }
        public string TrailerUrl { get; set; }
        public double Calificacion { get; set; } = 0.0;

        public virtual ICollection<ComentarioDTO> Comentarios { get; set; } = new List<ComentarioDTO>();
    }
}
