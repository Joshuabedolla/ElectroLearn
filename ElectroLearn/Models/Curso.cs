using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectroLearn.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        public string? ImagenUrl { get; set; }

        // ✅ RELACIÓN CORRECTA
        public List<Video> Videos { get; set; } = new List<Video>();
    }
}