using Microsoft.AspNetCore.Mvc;
using SistemaPrestamoEquipos.DB;
using SistemaPrestamoEquipos.Models;

namespace SistemaPrestamoEquipos.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly PrestamoService _prestamoService;
        private readonly InventarioService _inventarioService;
        private readonly UsuarioService _usuarioService;

        public PrestamoController()
        {
            _prestamoService = new PrestamoService();
            _inventarioService = new InventarioService();
            _usuarioService = new UsuarioService();
        }

        public IActionResult Index()
        {
            // Obtener el ID del estudiante de la sesión
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");

            if (!userIdRol.HasValue)
            {
                // Redirigir a login si no hay sesión
                TempData["Message"] = "Debe iniciar sesión para realizar un préstamo.";
                return RedirectToAction("Login", "Cuenta");
            }

            // Obtener información del estudiante
            var estudiante = _usuarioService.GetEstudiante(userIdRol.Value);
            ViewData["Estudiante"] = estudiante;

            // Verificar estado del estudiante
            if (estudiante == null || estudiante.Estado == "inhabilitado")
            {
                TempData["Message"] = "Su cuenta está inhabilitada. No puede realizar préstamos.";
                return View(new List<EquipoModel>());
            }

            // Obtener equipos disponibles
            var equiposDisponibles = _inventarioService.ListEquiposDisponibles();

            // Verificar préstamo vigente
            int idPrestamoVigente = _prestamoService.GetIdPrestamoVigente(userIdRol.Value);
            TempData["IdPrestamo"] = idPrestamoVigente;

            return View(equiposDisponibles);
        }

        public IActionResult Prestamo(int idPrestamo)
        {
            // Validar sesión
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (!userIdRol.HasValue)
            {
                TempData["Message"] = "Debe iniciar sesión para ver un préstamo.";
                return RedirectToAction("Login", "Cuenta");
            }

            // Obtener rol de la sesión
            string userRol = HttpContext.Session.GetString("TypeRol");
            ViewData["UserRol"] = userRol;

            // Obtener detalles del préstamo
            var prestamo = _prestamoService.GetPrestamo(idPrestamo);

            // Validar acceso al préstamo
            if (prestamo == null ||
                (userRol != "admin" && prestamo.IdEstudiante != userIdRol))
            {
                TempData["Message"] = "No tiene permiso para ver este préstamo.";
                return RedirectToAction("Index");
            }

            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            return View(prestamo);
        }

        [HttpGet]
        public JsonResult ListPrestamosEstudiante(int idEstudiante)
        {
            // Validar permiso de acceso
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            string userRol = HttpContext.Session.GetString("TypeRol");

            if (!userIdRol.HasValue ||
                (userRol != "admin" && idEstudiante != userIdRol))
            {
                return Json(new { error = "No tiene permiso para ver estos préstamos." });
            }

            var prestamos = _prestamoService.ListPrestamoEstudiante(idEstudiante);
            return Json(prestamos);
        }

        public IActionResult HistorialPrestamos()
        {
            // Validar sesión
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (!userIdRol.HasValue)
            {
                TempData["Message"] = "Debe iniciar sesión para ver el historial de préstamos.";
                return RedirectToAction("Login", "Cuenta");
            }

            // Obtener préstamos del estudiante
            var listaPrestamos = _prestamoService.ListPrestamoEstudiante(userIdRol.Value);
            ViewData["IdEstudiante"] = userIdRol.Value;

            return View(listaPrestamos);
        }

        [HttpPost]
        public IActionResult AddPrestamo(int idEquipo, TimeSpan horaInicioPedido, int tiempoPedido)
        {
            // Validar sesión
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (!userIdRol.HasValue)
            {
                TempData["Message"] = "Debe iniciar sesión para realizar un préstamo.";
                return RedirectToAction("Index");
            }

            // Validar estado del estudiante
            var estudiante = _usuarioService.GetEstudiante(userIdRol.Value);
            if (estudiante == null || estudiante.Estado == "inhabilitado")
            {
                TempData["Message"] = "Su cuenta está inhabilitada. No puede realizar préstamos.";
                return RedirectToAction("Index");
            }

            // Validar si ya tiene un préstamo vigente
            int prestamoVigente = _prestamoService.GetIdPrestamoVigente(userIdRol.Value);
            if (prestamoVigente > 0)
            {
                TempData["Message"] = "Ya tiene un préstamo en curso. No puede realizar otro.";
                return RedirectToAction("Index");
            }

            // Validar que la hora de inicio sea posterior a la actual
            var horaActual = DateTime.Now.TimeOfDay;
            if (horaInicioPedido <= horaActual)
            {
                TempData["Message"] = "La hora de inicio debe ser posterior a la hora actual.";
                return RedirectToAction("Index");
            }

            // Validar que la hora de inicio + tiempo de préstamo no exceda las horas de operación
            // Asumiendo que el laboratorio opera hasta las 22:00 (puedes ajustar esto)
            var horaFinPrestamo = horaInicioPedido.Add(TimeSpan.FromMinutes(tiempoPedido));
            var horaMaxima = new TimeSpan(22, 0, 0); // 22:00 horas

            if (horaFinPrestamo > horaMaxima)
            {
                TempData["Message"] = "El préstamo excede el horario de operación del laboratorio (hasta las 22:00).";
                return RedirectToAction("Index");
            }

            // Realizar el préstamo
            int idEstudiante = userIdRol.Value;
            string mensajeDb = _prestamoService.AddPrestamo(idEstudiante, idEquipo, horaInicioPedido, tiempoPedido);

            TempData["Message"] = mensajeDb;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SetPrestamoFinalizar(int idPrestamo)
        {
            // Validar sesión
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            string userRol = HttpContext.Session.GetString("TypeRol");

            if (!userIdRol.HasValue)
            {
                TempData["Message"] = "Debe iniciar sesión para finalizar un préstamo.";
                return RedirectToAction("Index");
            }

            // Validar permiso de finalización
            var prestamo = _prestamoService.GetPrestamo(idPrestamo);
            if (prestamo == null ||
                (userRol != "admin" && prestamo.IdEstudiante != userIdRol))
            {
                TempData["Message"] = "No tiene permiso para finalizar este préstamo.";
                return RedirectToAction("Index");
            }

            // Finalizar préstamo
            string mensajeDb = _prestamoService.SetPrestamoFinalizar(idPrestamo);
            TempData["Message"] = mensajeDb;

            return RedirectToAction("Index");
        }
    }
}