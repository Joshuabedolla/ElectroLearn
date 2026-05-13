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
            public string YouTubeUrl { get; set; }
        }

        public IActionResult Index()
        {
            // Lista de proyectos con URLs de YouTube (formato embed)
            var proyectos = new List<Proyecto>
            {
                new Proyecto
                {
                    Titulo = "Brazo Robotico",
                    YouTubeUrl = "https://www.youtube.com/embed/JBl7gwf7ORU?si=RB3cjUDbCMj-DFRzite",
                },
                new Proyecto
                {
                    Titulo = "Brazo Robotico controlado por Botones",
                    YouTubeUrl = "https://www.youtube.com/embed/JBvj6cAB6ck?si=eGf6hyirQ6yeT7jW"
                },
                new Proyecto
                {
                    Titulo = "Semaforo con Arduino",
                    YouTubeUrl = "https://www.youtube.com/embed/CIL8iKiQqSI?si=dmKLVozeCzwDQxjA"
                },
                new Proyecto
                {
                    Titulo = "LCD 16X2 con Arduino Uno",
                    YouTubeUrl = "https://www.youtube.com/embed/tMoJhRPPiFE?si=bv12h9N3obueAsAn"
                },
                new Proyecto
                {
                    Titulo = "Programar ESP32-CAM con Arduino Uno",
                    YouTubeUrl = "https://www.youtube.com/embed/pYAdUmzGnJk?si=T0ee8cxk79CmMfgc"
                },
                new Proyecto
                {
                    Titulo = "Dectector de Luz con Arduino Uno",
                    YouTubeUrl = "https://www.youtube.com/embed/dPAf4h_G6UI?si=bXMHUWOctnxVDGVh"
                },
                new Proyecto
                {
                    Titulo = "Proyecto DYF con Arduino Uno",
                    YouTubeUrl = "https://www.youtube.com/embed/42YSuO3MfNg?si=KkOIB1msl_06ncjY"
                },
                 new Proyecto
                {
                    Titulo = "Carro Bluetooth Arduino",
                    YouTubeUrl = "https://www.youtube.com/embed/k6zf60cpILw?si=-1Y1Sgwo_7KOznxK"
                },
                 new Proyecto
                {
                    Titulo = "Alarma Antirrobo Arduino",
                    YouTubeUrl = "https://www.youtube.com/embed/K7-EaUG4fxc?si=NOOM7iEdKdtE45uG"
                }
            };

            return View(proyectos);
        }
    }
}