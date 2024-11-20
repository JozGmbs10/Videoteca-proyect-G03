using Microsoft.AspNetCore.Mvc;
using SistemaPrestamoEquipos.DB;
using SistemaPrestamoEquipos.Models;

namespace SistemaPrestamoEquipos.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController()
        {
            _usuarioService = new UsuarioService();
        }

        public IActionResult Index()
        {
            var usuarios = this._usuarioService.ListEstudiantes();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Estudiante(int idEstudiante)
        {
            var equipo = _usuarioService.GetEstudiante(idEstudiante);
            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            return View(equipo);
        }

        [HttpPost]
        public IActionResult AddEstudiante(string nombre, string correo)
        {
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idAdmin = userIdRol.Value;
                string mensajeDb = this._usuarioService.AddEstudiante(idAdmin, nombre, correo);
                TempData["Message"] = mensajeDb;
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Error al añadir el estudiante";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult InhabilitarEstudiante(int idEstudiante)
        {
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idAdmin = userIdRol.Value;
                string mensajeDb = this._usuarioService.InhabilitarEstudiante(idAdmin, idEstudiante);
                TempData["Message"] = mensajeDb;
                Console.WriteLine(mensajeDb);
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Error al modificar el equipo";
            return RedirectToAction("Index");
        }

    }
}
