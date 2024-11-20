namespace SistemaPrestamoEquipos.Models
{
    public class EquipoModel
    {
        public int IdEquipo { get; set; }
        public int IdInventario { get; set; }
        public int CodigoEnLabo { get; set; }
        public string Nombre { get; set; }
        public int NumLabo { get; set; }
        public string Estado { get; set; }
        public string Disponibilidad { get; set; }
        public string CodigoPantalla { get; set; }
        public string CodigoCpu { get; set; }
        public string CodigoTeclado { get; set; }
        public string CodigoMouse { get; set; }
    }
}
