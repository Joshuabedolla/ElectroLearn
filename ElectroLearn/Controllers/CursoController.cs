using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectroLearn.Data;
using ElectroLearn.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ElectroLearn.Controllers
{
    public class CursoController : Controller
    {
        private readonly AppDbContext _context;

        public CursoController(AppDbContext context)
        {
            _context = context;
        }

        // 🔒 SESIÓN
        private bool UsuarioLogueado()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"));
        }

        // 📚 LISTAR CURSOS
        public IActionResult Index()
        {
            var cursos = _context.Cursos
                .Include(c => c.Videos)
                .ToList();

            return View(cursos);
        }

        // 👤 MIS CURSOS
        public IActionResult MisCursos()
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Login", "Auth");

            var cursos = _context.Cursos
                .Include(c => c.Videos)
                .ToList();

            return View("Index", cursos);
        }

        // 📄 DETALLE (🔥 ESTE ES EL MÁS IMPORTANTE)
        public IActionResult Details(int id)
        {
            var curso = _context.Cursos
                .Include(c => c.Videos)
                .FirstOrDefault(c => c.Id == id);

            if (curso == null)
                return RedirectToAction(nameof(Index));

            return View(curso); // ✔ ahora coincide con la vista
        }

        // ➕ CREAR CURSO
        public IActionResult Create()
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // 💾 GUARDAR CURSO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Curso curso, IFormFile imagenFile)
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Login", "Auth");

            ModelState.Remove("ImagenUrl");

            if (!ModelState.IsValid)
                return View(curso);

            if (imagenFile != null && imagenFile.Length > 0)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombre = Guid.NewGuid() + Path.GetExtension(imagenFile.FileName);
                var ruta = Path.Combine(carpeta, nombre);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await imagenFile.CopyToAsync(stream);
                }

                curso.ImagenUrl = "/img/" + nombre;
            }

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}