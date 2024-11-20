using Microsoft.AspNetCore.Mvc;
using SistemaPrestamoEquipos.DB;
using SistemaPrestamoEquipos.Models;

namespace SistemaPrestamoEquipos.Controllers
{
    public class PerfilController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public PerfilController()
        {
            _usuarioService = new UsuarioService();
        }

        public IActionResult Index()
        {
            int? idUsuario = HttpContext.Session.GetInt32("UserIdUsuario");

            if (idUsuario.HasValue)
            {
                var usuario = _usuarioService.GetUsuario(idUsuario.Value); // Usar .Value para obtener el int
                return View(usuario);
            }

            // Manejo del caso cuando idUsuario es null
            return RedirectToAction("Login", "Auth"); // Rediri
        }

        [HttpPost]
        public IActionResult ChangePassword(string nuevaContrasenia, string repiteContrasenia)
        {
            if (string.IsNullOrEmpty(nuevaContrasenia) || string.IsNullOrEmpty(repiteContrasenia))
            {
                TempData["ErrorMessage"] = "Las contraseñas no son válidas.";
                return RedirectToAction("Index");
            }

            if (nuevaContrasenia != repiteContrasenia)
            {
                TempData["ErrorMessage"] = "Las contraseñas son diferentes.";
                return RedirectToAction("Index");
            }

            int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("UserIdUsuario"));

            // Llama al servicio para cambiar la contraseña
            bool exito = _usuarioService.SetContrasena(idUsuario, nuevaContrasenia);

            if (!exito)
            {
                TempData["ErrorMessage"] = "No se pudo cambiar la contraseña. Inténtalo de nuevo.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "La contraseña ha sido cambiada exitosamente.";
            return RedirectToAction("Index");
        }

    }
}
