using Microsoft.AspNetCore.Mvc;
using ElectroLearn.Data;
using ElectroLearn.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;

namespace ElectroLearn.Controllers
{
    public class VideoController : Controller
    {
        private readonly AppDbContext _context;

        public VideoController(AppDbContext context)
        {
            _context = context;
        }

        private bool EsAdmin()
        {
            return HttpContext.Session.GetString("rol") == "Admin";
        }

        // =========================
        // 🎬 CREAR VIDEO
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Video video)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            ModelState.Remove("Curso");

            if (video.CursoId == 0)
            {
                TempData["Error"] = "CursoId no recibido";
                return RedirectToAction("Index", "Curso");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos";
                return RedirectToAction("Details", "Curso", new { id = video.CursoId });
            }

            // 🔥 FIX REAL
            video.YoutubeUrl = ConvertirYoutube(video.YoutubeUrl.Trim());

            _context.Videos.Add(video);
            _context.SaveChanges();

            TempData["Success"] = "Video guardado correctamente";

            return RedirectToAction("Details", "Curso", new { id = video.CursoId });
        }

        // =========================
        // ✏️ EDITAR VIDEO
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Video video)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            ModelState.Remove("Curso");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos";
                return RedirectToAction("Details", "Curso", new { id = video.CursoId });
            }

            var existente = _context.Videos.Find(video.Id);

            if (existente == null)
            {
                TempData["Error"] = "Video no encontrado";
                return RedirectToAction("Details", "Curso", new { id = video.CursoId });
            }

            existente.Titulo = video.Titulo;
            existente.YoutubeUrl = ConvertirYoutube(video.YoutubeUrl.Trim());

            _context.SaveChanges();

            TempData["Success"] = "Video actualizado";

            return RedirectToAction("Details", "Curso", new { id = video.CursoId });
        }

        // =========================
        // 🗑 ELIMINAR VIDEO
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id, int cursoId)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Auth");

            var video = _context.Videos
                .FirstOrDefault(v => v.Id == id);

            if (video == null)
            {
                TempData["Error"] = "Video no encontrado";
                return RedirectToAction("Details", "Curso", new { id = cursoId });
            }

            try
            {
                _context.Videos.Remove(video);
                _context.SaveChanges();

                TempData["Success"] = "Video eliminado";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", "Curso", new { id = cursoId });
        }

        // =========================
        // 🔥 CONVERTIDOR YOUTUBE (FIX TOTAL)
        // =========================
        private string ConvertirYoutube(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            try
            {
                string videoId = "";

                // 👉 Caso 1: youtube.com/watch?v=
                if (url.Contains("watch?v="))
                {
                    var uri = new Uri(url);
                    var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
                    videoId = query["v"];
                }
                // 👉 Caso 2: youtu.be/
                else if (url.Contains("youtu.be/"))
                {
                    videoId = url.Split('/').Last().Split('?')[0];
                }
                // 👉 Caso 3: ya es embed
                else if (url.Contains("embed/"))
                {
                    return url;
                }

                if (!string.IsNullOrEmpty(videoId))
                    return $"https://www.youtube.com/embed/{videoId}";

                return url;
            }
            catch
            {
                return url;
            }
        }
    }
}