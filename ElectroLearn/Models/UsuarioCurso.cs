using Microsoft.AspNetCore.Identity;

namespace ElectroLearn.Models
{
    public class UsuarioCurso
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }

        public int CursoId { get; set; }
        public Curso Curso { get; set; }

        public int Progreso { get; set; } = 0;
    }
}