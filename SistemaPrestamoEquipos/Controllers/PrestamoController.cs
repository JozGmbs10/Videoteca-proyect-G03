using Microsoft.AspNetCore.Mvc;
using SistemaPrestamoEquipos.DB;
using SistemaPrestamoEquipos.Models;

namespace SistemaPrestamoEquipos.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly PrestamoService _prestamoService;
        private readonly InventarioService _inventarioService;

        public PrestamoController()
        {
            _prestamoService = new PrestamoService();
            _inventarioService = new InventarioService();
        }

        public IActionResult Index()
        {
            var equiposDisponibles = this._inventarioService.ListEquiposDisponibles();

            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idEstudiante = userIdRol.Value;
                int idPrestamoVigente = this._prestamoService.GetIdPrestamoVigente(idEstudiante);
                TempData["IdPrestamo"] = idPrestamoVigente;
                return View(equiposDisponibles);
            }
            TempData["IdPrestamo"] = -1;
            return View(equiposDisponibles);
        }

        public IActionResult Prestamo(int idPrestamo)
        {
            string userRol = HttpContext.Session.GetString("TypeRol");

            ViewData["UserRol"] = userRol;
            var prestamo = this._prestamoService.GetPrestamo(idPrestamo);
            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            return View(prestamo);
        }

        [HttpGet]
        public JsonResult ListPrestamosEstudiante(int idEstudiante)
        {
            var prestamos = _prestamoService.ListPrestamoEstudiante(idEstudiante);
            return Json(prestamos);
        }

        public IActionResult HistorialPrestamos()
        {
            int idEstudiante = -1;
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                idEstudiante = userIdRol.Value;
                var listaPrestamos = this._prestamoService.ListPrestamoEstudiante(idEstudiante);
                ViewData["IdEstudiante"] = idEstudiante;
                return View();
            }
            return View();
        }


        [HttpPost]
        public IActionResult AddPrestamo(int idEquipo, TimeSpan horaInicioPedido, int tiempoPedido)
        {
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idEstudiante = userIdRol.Value;
                string mensajeDb = _prestamoService.AddPrestamo(idEstudiante, idEquipo, horaInicioPedido, tiempoPedido);
                TempData["Message"] = mensajeDb;
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Error al añadir el prestamo";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SetPrestamoFinalizar(int idPrestamo)
        {
            string mensajeDb = _prestamoService.SetPrestamoFinalizar(idPrestamo);
            TempData["Message"] = mensajeDb;
            return RedirectToAction("Index");
        }

    }
}
