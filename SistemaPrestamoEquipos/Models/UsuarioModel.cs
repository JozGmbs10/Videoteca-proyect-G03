namespace SistemaPrestamoEquipos.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; } // Agregar rol si es necesario
    }
}
