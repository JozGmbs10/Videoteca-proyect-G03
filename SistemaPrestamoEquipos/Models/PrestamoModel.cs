namespace SistemaPrestamoEquipos.Models
{
    public class PrestamoModel
    {
        public int IdPrestamo { get; set; } // id_prestamo (clave primaria)

        public int NombreEquipo { get; set; }

        public int IdEstudiante { get; set; } // id_estudiante (relacionado con tabla_estudiante)

        public int IdEquipo { get; set; } // id_equipo (relacionado con tabla_equipo)

        public string Estado { get; set; } // estado (pendiente, completado, apurado, cancelado)

        public DateOnly Fecha { get; set; } // fecha (la fecha es la actual al insertar)

        public TimeSpan HoraInicioPedido { get; set; } // hora_inicio_pedido (hora que el usuario ingresa)

        public int TiempoPedido { get; set; } // tiempo_pedido (tiempo que el usuario ingresa en minutos)

        public TimeSpan HoraFinPedido { get; set; }
        public int TiempoUsado { get; set; }
    }
}
