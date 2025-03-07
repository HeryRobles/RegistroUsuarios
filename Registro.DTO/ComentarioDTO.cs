using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registro.DTO
{
    public class ComentarioDTO
    {
        public int IdComentario { get; set; }
        public int IdPelicula { get; set; }
        public int NombreUsuario { get; set; }
        public string Texto { get; set; } = string.Empty;
        public double Calificacion { get; set; }
        public DateTime FechaComentario { get; set; } = DateTime.Now;
    }
}
