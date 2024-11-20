using System.Security.Cryptography.Xml;

namespace SistemaPrestamoEquipos.Models
{
    public class InventarioModel
    {
        public int IdInventario { get; set; }
        public int IdAdmin { get; set; }
        public int NumLabo { get; set; }
        public int NumEquipos { get; set; }

        public int NumComponentes { get; set; }
    }
}