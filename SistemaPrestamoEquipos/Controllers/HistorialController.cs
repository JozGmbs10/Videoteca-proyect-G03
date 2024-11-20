using Microsoft.AspNetCore.Mvc;
using SistemaPrestamoEquipos.DB;
using SistemaPrestamoEquipos.Models;

namespace SistemaPrestamoEquipos.Controllers
{
    public class HistorialController : Controller
    {
        private readonly InventarioService _inventarioService;

        public HistorialController()
        {
            _inventarioService = new InventarioService();
        }

    }
}
