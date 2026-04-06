using Microsoft.AspNetCore.Mvc;
using ElectroLearn.Data;
using ElectroLearn.Models;

namespace ElectroLearn.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult CrearCurso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearCurso(Curso c)
        {
            _context.Cursos.Add(c);
            _context.SaveChanges();
            return RedirectToAction("Panel");
        }

        public IActionResult CrearVideo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearVideo(Video v)
        {
            _context.Videos.Add(v);
            _context.SaveChanges();
            return RedirectToAction("Panel");
        }
    }
}
