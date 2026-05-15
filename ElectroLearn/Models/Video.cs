using System.ComponentModel.DataAnnotations;

namespace ElectroLearn.Models
{
    public class Video
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        // 🎬 SOLO ESTA (YouTube embed)
        [Required(ErrorMessage = "La URL es obligatoria")]
        public string YoutubeUrl { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un curso")]
        public int CursoId { get; set; }

        public Curso Curso { get; set; }
        public int UsuarioId { get; set; }
    }
}