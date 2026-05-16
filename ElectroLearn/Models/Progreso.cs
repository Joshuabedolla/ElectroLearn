namespace ElectroLearn.Models
{
    public class Progreso
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int VideoId { get; set; }
        public Video Video { get; set; }

        public bool Visto { get; set; } = true;
    }
}
