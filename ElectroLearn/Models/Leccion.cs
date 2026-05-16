namespace ElectroLearn.Models
{
    public class Leccion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }

        public int CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}