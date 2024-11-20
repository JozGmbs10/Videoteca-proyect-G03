using Microsoft.AspNetCore.Mvc;
using SistemaPrestamoEquipos.DB;
using SistemaPrestamoEquipos.Models;
using System.Diagnostics;

namespace SistemaPrestamoEquipos.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly InventarioService _inventarioService;

        public HomeController()
        {
            _usuarioService = new UsuarioService();
            _inventarioService = new InventarioService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SecondView(int id)
        {
            ViewData["ID"] = id;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
