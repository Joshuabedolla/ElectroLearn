using ElectroLearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Dapper;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        // 🔒 VALIDAR SESIÓN
        var usuario = HttpContext.Session.GetString("usuario");

        if (string.IsNullOrEmpty(usuario))
        {
            TempData["Error"] = "Debes iniciar sesión";
            return RedirectToAction("Login", "Auth");
        }

        return View();
    }

    public IActionResult MisCursos()
    {
        // 🔒 VALIDAR SESIÓN
        var usuario = HttpContext.Session.GetString("usuario");

        if (string.IsNullOrEmpty(usuario))
        {
            TempData["Error"] = "Debes iniciar sesión";
            return RedirectToAction("Login", "Auth");
        }

        return RedirectToAction("MisCursos", "Curso");
    }

    public IActionResult Videos()
    {
        // 🔒 VALIDAR SESIÓN
        var usuario = HttpContext.Session.GetString("usuario");

        if (string.IsNullOrEmpty(usuario))
        {
            TempData["Error"] = "Debes iniciar sesión";
            return RedirectToAction("Login", "Auth");
        }

        return RedirectToAction("Index", "Curso");
    }

    // ✅ GET CONFIGURACIÓN
    public IActionResult Configuracion()
    {
        var usuario = HttpContext.Session.GetString("usuario");

        if (string.IsNullOrEmpty(usuario))
        {
            TempData["Error"] = "Debes iniciar sesión";
            return RedirectToAction("Login", "Auth");
        }

        return RedirectToAction("Index", "Configuracion");

        return View();
    }

    // ✅ POST CONFIGURACIÓN
    [HttpPost]
    public IActionResult Configuracion(string PasswordActual, string NuevaPassword, string ConfirmarPassword)
    {
        var usuario = HttpContext.Session.GetString("usuario");

        if (string.IsNullOrEmpty(usuario))
        {
            TempData["Error"] = "Debes iniciar sesión";
            return RedirectToAction("Login", "Auth");
        }

        // ✅ Validar campos
        if (string.IsNullOrEmpty(PasswordActual) ||
            string.IsNullOrEmpty(NuevaPassword) ||
            string.IsNullOrEmpty(ConfirmarPassword))
        {
            TempData["Error"] = "Todos los campos son obligatorios";
            return RedirectToAction("Configuracion");
        }

        // ✅ Validar coincidencia
        if (NuevaPassword != ConfirmarPassword)
        {
            
            TempData["Error"] = "Las contraseñas no coinciden";
            return RedirectToAction("Configuracion");
        }

        // ✅ Connection String
        string connectionString =
            "Server=sql.bsite.net\\MSSQL2016;Database=tl25_electrolearn;User Id=tl25_electrolearn;Password=Baul12345;MultipleActiveResultSets=true;TrustServerCertificate=true;";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // ✅ Buscar usuario
            var usuarioDb = connection.QueryFirstOrDefault<Usuario>(
                "SELECT * FROM Usuarios WHERE Nombre = @Nombre",
                new { Nombre = usuario });

            // ✅ Usuario no encontrado
            if (usuarioDb == null)
            {
                TempData["Error"] = "Usuario no encontrado";
                return RedirectToAction("Configuracion");
            }

            // ✅ Verificar contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(PasswordActual, usuarioDb.PasswordHash))
            {
                TempData["Error"] = "La contraseña actual es incorrecta";
                return RedirectToAction("Configuracion");
            }

            // ✅ Nuevo hash
            string nuevaPasswordHash =
                BCrypt.Net.BCrypt.HashPassword(NuevaPassword);

            // ✅ Actualizar contraseña
            connection.Execute(
                "UPDATE Usuarios SET PasswordHash = @PasswordHash WHERE Nombre = @Nombre",
                new
                {
                    PasswordHash = nuevaPasswordHash,
                    Nombre = usuario
                });
        }

        TempData["Success"] = "Contraseña cambiada correctamente";

        return RedirectToAction("Configuracion");
    }
}