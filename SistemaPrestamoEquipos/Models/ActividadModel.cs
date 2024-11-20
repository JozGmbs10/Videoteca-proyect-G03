namespace SistemaPrestamoEquipos.Models
{
    public class ActividadModel
    {
        public int IdActividad { get; set; }

        public int IdAdmin { get; set; }

        public string Descripcion { get; set; }

        public string Tipo { get; set; }

        public DateTime Fecha { get; set; }
    }
}
