namespace SistemaPrestamoEquipos.Models
{
    public class EstudianteModel
    {
        public int IdEstudiante { get; set; }
        public string Estado { get; set; } // 'habilitado' o 'inhabilitado'
        public UsuarioModel Usuario { get; set; } // Navegación manual
    }
}
