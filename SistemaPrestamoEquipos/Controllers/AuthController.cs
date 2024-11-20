using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamoEquipos.Models;

namespace SistemaPrestamoEquipos.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public AuthController()
        {
            _usuarioService = new UsuarioService(); // Inicializa el servicio
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Eliminar el tipo de usuario de la sesión.Remove("UserType");
            HttpContext.Session.Remove("UserIdRol");
            HttpContext.Session.Remove("UserIdUsuario");
            HttpContext.Session.Remove("TypeRol");

            // Redirigir a la vista de Login
            return RedirectToAction("Login");
        }

        
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Obtener la lista de usuarios desde el servicio
            var (id, rol, idUsuario) = _usuarioService.Login(username, password);

            if (id != -1)
            {
                // Almacenar el tipo de usuario en la sesión

                //Console.WriteLine($"Login exitoso. ID: {id.Value}, Rol: {rol}"); // Imprimir en consola

                HttpContext.Session.SetInt32("UserIdRol", id);
                HttpContext.Session.SetInt32("UserIdUsuario", idUsuario);
                HttpContext.Session.SetString("TypeRol", rol);
                return RedirectToAction("Index", "Home");
            }

            ViewData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
            return View(); // Vuelve a mostrar la vista de login en caso de error
        }
    }
}
