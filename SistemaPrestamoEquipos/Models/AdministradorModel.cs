namespace SistemaPrestamoEquipos.Models
{
    public class AdministradorModel
    {
        public int IdAdmin { get; set; }
        public int IdUsuario { get; set; }

        // Navegación
        public virtual UsuarioModel Usuario { get; set; }
    }
}
