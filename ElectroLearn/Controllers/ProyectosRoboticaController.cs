using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ElectroLearn.Controllers
{
    public class ProyectosRoboticaController : Controller
    {
        // Modelo interno simple para proyectos
        public class Proyecto
        {
            public string Titulo { get; set; }
            public string VideoPath { get; set; }
        }

        public IActionResult Index()
        {
            // Lista de proyectos con rutas relativas a videos en wwwroot/videos/
            var proyectos = new List<Proyecto>
            {
                new Proyecto { Titulo = "Robot Seguidor de Línea", VideoPath = "/videos/robot-seguidor-linea.mp4" },
                new Proyecto { Titulo = "Brazo Robótico Controlado", VideoPath = "/videos/brazo-robotico.mp4" },
                new Proyecto { Titulo = "Robot Evita Obstáculos", VideoPath = "/videos/robot-evita-obstaculos.mp4" }
            };

            return View(proyectos);
        }
    }
}
