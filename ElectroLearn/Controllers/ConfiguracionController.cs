using Microsoft.AspNetCore.Mvc;
using ElectroLearn.Data;
using ElectroLearn.Models;
using System.Linq;
using BCrypt.Net;

namespace ElectroLearn.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly AppDbContext _context;

        public ConfiguracionController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== VISTA =====================
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                TempData["Error"] = "Debes iniciar sesión";
                return RedirectToAction("Login", "Auth");
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == userId);

            if (usuario == null)
                return NotFound();

            var model = new ConfiguracionViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email
            };

            return View(model);
        }

        // ===================== PERFIL =====================
        [HttpPost]
        public IActionResult GuardarPerfil(ConfiguracionViewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                TempData["Error"] = "Debes iniciar sesión";
                return RedirectToAction("Login", "Auth");
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == userId);

            if (usuario == null)
                return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Email = model.Email;

            _context.SaveChanges();

            TempData["Success"] = "Perfil actualizado correctamente";
            return RedirectToAction("Index");
        }

        // ===================== PASSWORD =====================
        [HttpPost]
        public IActionResult CambiarPassword(ConfiguracionViewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                TempData["Error"] = "Debes iniciar sesión";
                return RedirectToAction("Login", "Auth");
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == userId);

            if (usuario == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(model.PasswordActual) ||
                string.IsNullOrWhiteSpace(model.NuevaPassword))
            {
                TempData["Error"] = "Debes completar todos los campos";
                return RedirectToAction("Index");
            }

            if (!BCrypt.Net.BCrypt.Verify(model.PasswordActual, usuario.PasswordHash))
            {
                TempData["Error"] = "Contraseña actual incorrecta";
                return RedirectToAction("Index");
            }

            if (model.NuevaPassword != model.ConfirmarPassword)
            {
                TempData["Error"] = "Las contraseñas no coinciden";
                return RedirectToAction("Index");
            }

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NuevaPassword);

            _context.SaveChanges();

            TempData["Success"] = "Contraseña actualizada correctamente";
            return RedirectToAction("Index");
        }
    }
}