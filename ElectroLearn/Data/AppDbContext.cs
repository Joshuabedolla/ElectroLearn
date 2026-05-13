using Microsoft.EntityFrameworkCore;
using ElectroLearn.Models;

namespace ElectroLearn.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Leccion> Lecciones { get; set; }
        public DbSet<Progreso> Progresos { get; set; }
        public DbSet<UsuarioCurso> UsuarioCursos { get; set; }
    }
}
