using Microsoft.AspNetCore.Mvc;
using ElectroLearn.Data;
using ElectroLearn.Models;
using ElectroLearn.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ElectroLearn.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== LOGIN =====================
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                TempData["Info"] = "Ya iniciaste sesión.";
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Usuarios
                .FirstOrDefault(u => u.Email == model.Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("usuario", user.Nombre);
                HttpContext.Session.SetString("email", user.Email);
                HttpContext.Session.SetString("rol", user.Rol ?? "Usuario");

                TempData["Success"] = "Bienvenido a ElectroLearn ⚡";

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View(model);
        }

        // ===================== REGISTRO =====================
        public IActionResult Registro()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                TempData["Info"] = "Ya iniciaste sesión.";
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Registro(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Usuarios.Any(u => u.Email == model.Email))
            {
                ViewBag.Error = "El correo ya está registrado";
                return View(model);
            }

            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Rol = "Usuario"
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["Success"] = "Cuenta creada correctamente";

            return RedirectToAction("Login");
        }

        // ===================== DASHBOARD =====================
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            var usuario = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                TempData["Error"] = "Debes iniciar sesión";
                return RedirectToAction("Login", "Auth");
            }

            var cursos = _context.Cursos.ToList();

            return View(cursos);
        }

        // ===================== LOGOUT =====================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Info"] = "Sesión cerrada correctamente";
            return RedirectToAction("Login");
        }
    }
}