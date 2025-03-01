using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CasaLannister.Models;

namespace CasaLannister.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index( )
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // Clase auxiliar para el controlador de errores
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}