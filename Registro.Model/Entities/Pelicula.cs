namespace Registro.Model.Entities
{
    public class Pelicula
    {
        public int IdPelicula { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public string Reseña { get; set; }
        public string ImagenUrl { get; set; }
        public string TrailerUrl { get; set; }
        public double Calificacion { get; set; } = 0.0;
        public bool EsActiva { get; set; }
        public DateTime FechaEstreno { get; set; }

        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
