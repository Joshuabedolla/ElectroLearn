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

        // 🔒 VALIDAR SESIÓN
        private bool UsuarioLogueado()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"));
        }

        // 🚨 REDIRECCIÓN LOGIN
        private IActionResult RequiereLogin()
        {
            TempData["Error"] = "Debes iniciar sesión";
            return RedirectToAction("Login", "Auth");
        }

        // 📚 LISTAR CURSOS PÚBLICOS
        public IActionResult Index()
        {
            var cursos = _context.Cursos
                .Include(c => c.Videos)
                .ToList();

            return View(cursos);
        }

        // 👤 MIS CURSOS (SOLO DEL USUARIO)
        public IActionResult MisCursos()
        {
            if (!UsuarioLogueado())
                return RequiereLogin();

            int? userId = HttpContext.Session.GetInt32("usuarioId");

            var cursos = _context.Cursos
                .Include(c => c.Videos)
                .Where(c => c.UsuarioId == userId)
                .ToList();

            return View("Index", cursos);
        }

        // 📄 DETALLE
        public IActionResult Details(int id)
        {
            var curso = _context.Cursos
                .Include(c => c.Videos)
                .FirstOrDefault(c => c.Id == id);

            if (curso == null)
                return RedirectToAction(nameof(Index));

            return View(curso);
        }

        // ➕ CREAR CURSO
        public IActionResult Create()
        {
            if (!UsuarioLogueado())
                return RequiereLogin();

            return View();
        }

        // 💾 GUARDAR CURSO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Curso curso, IFormFile imagenFile)
        {
            if (!UsuarioLogueado())
                return RequiereLogin();

            int? userId = HttpContext.Session.GetInt32("usuarioId");

            if (userId == null)
                return RequiereLogin();

            if (!ModelState.IsValid){
                Console.WriteLine(ModelState["ImagenUrl"]);
                return View(curso);
            }

            ModelState.Remove("ImagenUrl");

            

            // 🖼️ SUBIR IMAGEN
            if (imagenFile != null && imagenFile.Length > 0)
            {
                var carpeta = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/img"
                );

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombre = Guid.NewGuid() +
                             Path.GetExtension(imagenFile.FileName);

                var ruta = Path.Combine(carpeta, nombre);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await imagenFile.CopyToAsync(stream);
                }

                curso.ImagenUrl = "/img/" + nombre;
            }

            
                

            // 🔒 ASIGNAR CURSO AL USUARIO
            curso.UsuarioId = userId.Value;

            _context.Cursos.Add(curso);

            await _context.SaveChangesAsync();

            return RedirectToAction("MisCursos");
        }
    }
}