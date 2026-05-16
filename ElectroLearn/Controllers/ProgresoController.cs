using Microsoft.AspNetCore.Mvc;
using ElectroLearn.Data;
using ElectroLearn.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ElectroLearn.Controllers
{
    public class ProgresoController : Controller
    {
        private readonly AppDbContext _context;

        public ProgresoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult MarcarVisto(int videoId)
        {
            var nombre = HttpContext.Session.GetString("usuario");

            if (nombre == null)
                return RedirectToAction("Login", "Auth");

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Nombre == nombre);

            if (usuario == null)
                return RedirectToAction("Login", "Auth");

            var existe = _context.Progresos
                .FirstOrDefault(p => p.UsuarioId == usuario.Id && p.VideoId == videoId);

            if (existe == null)
            {
                var progreso = new Progreso
                {
                    UsuarioId = usuario.Id,
                    VideoId = videoId,
                    Visto = true
                };

                _context.Progresos.Add(progreso);
                _context.SaveChanges();
            }

            var video = _context.Videos.FirstOrDefault(v => v.Id == videoId);

            return RedirectToAction("Detalle", "Curso", new { id = video.CursoId });
        }
    }
}
