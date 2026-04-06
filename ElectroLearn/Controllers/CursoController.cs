using Microsoft.AspNetCore.Mvc;
using ElectroLearn.Data;
using ElectroLearn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ElectroLearn.Controllers
{
    public class CursoController : Controller
    {
        private readonly AppDbContext _context;

        public CursoController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== LISTA DE CURSOS =====================
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cursos = _context.Cursos.ToList();
            return View(cursos);
        }

        // ===================== DETALLE =====================
        public IActionResult Detalle(int id)
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var curso = _context.Cursos
                .Include(c => c.Videos) // 👈 relación con videos
                .FirstOrDefault(c => c.Id == id);

            if (curso == null)
            {
                return NotFound(); // 👈 evita error si no existe
            }

            return View(curso);
        }
    }
}