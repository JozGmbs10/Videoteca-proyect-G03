namespace SistemaPrestamoEquipos.Models
{
    public class ComponenteModel
    {
        public int IdComp { get; set; }

        public int IdInventario { get; set; }

        public int? IdEquipo { get; set; }

        public string Tipo { get; set; } // Debe estar validado en la lógica de negocio.

        public string Estado { get; set; } // Debe estar validado en la lógica de negocio.
        public string Codigo { get; set; }
        public string Marca { get; set; }
        public string Disponibilidad { get; set; }
    }
}
