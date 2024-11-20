using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SistemaPrestamoEquipos.DB;
using SistemaPrestamoEquipos.Models;
using System.ComponentModel;

namespace SistemaPrestamoEquipos.Controllers
{
    public class InventarioController : Controller
    {
        private readonly InventarioService _inventarioService;

        public InventarioController()
        {
            _inventarioService = new InventarioService();
        }

        public IActionResult Index()
        {
            var inventarios = _inventarioService.ListInventarios();
            return View(inventarios);
        }

        [HttpPost]
        public IActionResult VerEquiposPorInventario(int idInventario)
        {
            var equipos = _inventarioService.GetEquiposPorInventario(idInventario);

            //Console.WriteLine("Equipos obtenidos: ");
            foreach (var equipo in equipos)
            {
                Console.WriteLine($"ID: {equipo.IdEquipo}, Nombre: {equipo.Nombre}, Estado: {equipo.Estado}, Disponibilidad: {equipo.Disponibilidad}");
            }

            return Json(equipos);
        }

        [HttpPost]
        public IActionResult SetEquipo(string returnUrl, int idEquipo, int codigoEnLabo, string codigoPantalla, string codigoCpu, string codigoTeclado, string codigoMouse)
        {
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idAdmin = userIdRol.Value;
                string mensajeDb = this._inventarioService.SetEquipo(idAdmin, idEquipo, codigoEnLabo, codigoPantalla, codigoCpu, codigoTeclado, codigoMouse);
                Console.WriteLine(returnUrl);
                Console.WriteLine(mensajeDb);
                TempData["Message"] = mensajeDb;
                return Redirect(returnUrl);
            }
            Console.WriteLine("Error al setEquipo");
            TempData["Message"] = "Error al modificar el equipo";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetEquipo(int idEquipo)
        {
            var equipo = _inventarioService.GetEquipoPorId(idEquipo); // Asegúrate de tener este método en tu servicio

            if (equipo != null)
            {
                return Json(equipo);
            }
            return Json(null); // O manejar el caso en que no se encuentra el equipo
        }

        public IActionResult AddEquipo(int idInventario, int codigoEnLabo, string codigoPantalla, string codigoCpu, string codigoTeclado, string codigoMouse)
        {
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idAdmin = userIdRol.Value;
                string mensajeDb = this._inventarioService.AddEquipo(idAdmin, idInventario, codigoEnLabo, codigoPantalla, codigoCpu, codigoTeclado, codigoMouse);
                TempData["Message"] = mensajeDb;
                return RedirectToAction("Laboratorio", new { idInventario = idInventario });
            }
            TempData["Message"] = "Error al añadir el equipo";
            return RedirectToAction("Laboratorio", new { idInventario = idInventario });
        }

        public IActionResult AddInventario(int numLabo)
        {
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idAdmin = userIdRol.Value;
                string mensajeDb = this._inventarioService.AddInventario(idAdmin, numLabo);
                TempData["Message"] = mensajeDb;
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Error al añadir el inventario"; // Cambia esto
            return RedirectToAction("Index");
        }

        public IActionResult AddComponente(int idEquipo, int idInventario, string tipo, string codigo, string marca)
        {

            Console.WriteLine("-----------------------------------");
            Console.WriteLine(idEquipo);

            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idAdmin = userIdRol.Value;
                string mensajeDb = this._inventarioService.AddComp(idAdmin, idEquipo, idInventario, tipo, codigo, marca);
                TempData["Message"] = mensajeDb;
                return RedirectToAction("Laboratorio", new { idInventario = idInventario });
            }
            TempData["Message"] = "Error al añadir el componente"; // Cambia esto
            return RedirectToAction("Laboratorio", new { idInventario = idInventario });
        }

        public IActionResult SetComp(int idComp, int idEquipo)
        {
            int? userIdRol = HttpContext.Session.GetInt32("UserIdRol");
            if (userIdRol.HasValue)
            {
                int idAdmin = userIdRol.Value;
                string mensajeDb = this._inventarioService.SetComp(idAdmin, idComp, idEquipo);
                TempData["Message"] = mensajeDb;
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Error al añadir el equipo";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult ComponentesPorInventario(int idInventario)
        {
            var componentes = _inventarioService.GetCompsPorInventario(idInventario);
            return Json(componentes);
        }

        [HttpGet]
        public JsonResult GetComp(int idComp)
        {
            var componente = _inventarioService.GetCompPorId(idComp); // Asegúrate de tener este método en tu servicio

            return Json(componente);
        }



        [HttpGet]
        public IActionResult Equipo(int idEquipo)
        {
            var equipo = _inventarioService.GetEquipoPorId(idEquipo);
            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            return View(equipo);
        }

        [HttpGet]
        public IActionResult Componente(int idComponente)
        {
            var equipo = _inventarioService.GetCompPorId(idComponente);
            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            return View(equipo);
        }

        [HttpGet]
        public IActionResult Laboratorio(int idInventario)
        {
            var inventario = _inventarioService.GetInventario(idInventario);
            return View(inventario);
        }


    }
}
