using Microsoft.AspNetCore.Mvc;
using ElectroLearn.Data;
using ElectroLearn.Models;
using ElectroLearn.ViewModels;
using System.Linq;
using BCrypt.Net;
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
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Usuarios
                .FirstOrDefault(u => u.Email == model.Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                // ✅ Guardar sesión
                HttpContext.Session.SetString("usuario", user.Nombre);
                HttpContext.Session.SetString("rol", user.Rol ?? "User"); // 👈 usa "User" consistente

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View(model);
        }

        // ===================== REGISTRO =====================
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(RegisterViewModel model)
        {
            if (_context.Usuarios.Any(u => u.Email == model.Email))
            {
                ViewBag.Error = "El correo ya está registrado";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Rol = "Usuario" // rol por defecto
                };

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // ===================== DASHBOARD =====================
        public IActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Login");
            }

            ViewBag.Usuario = HttpContext.Session.GetString("usuario");
            ViewBag.Rol = HttpContext.Session.GetString("rol");
            return View();
        }

        // ===================== LOGOUT =====================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}