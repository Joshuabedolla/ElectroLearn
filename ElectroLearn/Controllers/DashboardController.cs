using ElectroLearn.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult MisCursos()
    {
        return RedirectToAction("MisCursos", "Curso");
    }

    public IActionResult Videos()
    {
        return RedirectToAction("Index", "Curso");
    }

    public IActionResult Configuracion()
    {
        return View();
    }
}