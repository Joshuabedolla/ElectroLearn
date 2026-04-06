using Microsoft.AspNetCore.Mvc;
using ElectroLearn.Data;
using ElectroLearn.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ElectroLearn.Controllers
{
    public class VideoController : Controller
    {
        private readonly AppDbContext _context;

        public VideoController(AppDbContext context)
        {
            _context = context;
        }

        // 🔐 VALIDAR SI ES ADMIN
        private bool EsAdmin()
        {
            return HttpContext.Session.GetString("rol") == "Admin";
        }

        // ===================== FORMULARIO =====================
        public IActionResult Crear(int? cursoId)
        {
            // 🔒 Solo admin
            if (!EsAdmin())
            {
                return RedirectToAction("Dashboard", "Auth");
            }

            ViewBag.Cursos = new SelectList(_context.Cursos, "Id", "Titulo", cursoId);
            return View();
        }

        // ===================== GUARDAR VIDEO =====================
        [HttpPost]
        public IActionResult Crear(Video video)
        {
            // 🔒 Solo admin
            if (!EsAdmin())
            {
                return RedirectToAction("Dashboard", "Auth");
            }

            if (ModelState.IsValid)
            {
                // 🔥 Convertir URL a embed
                if (video.Url.Contains("watch?v="))
                {
                    video.Url = video.Url.Replace("watch?v=", "embed/");
                }

                _context.Videos.Add(video);
                _context.SaveChanges();

                return RedirectToAction("Detalle", "Curso", new { id = video.CursoId });
            }

            ViewBag.Cursos = new SelectList(_context.Cursos, "Id", "Titulo", video.CursoId);
            return View(video);
        }
    }
}