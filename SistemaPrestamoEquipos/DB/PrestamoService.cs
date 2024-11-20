using SistemaPrestamoEquipos.DB;
using SistemaPrestamoEquipos.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaPrestamoEquipos.DB
{
    public class PrestamoService
    {
        public PrestamoModel GetPrestamo(int idPrestamo)
        {
            PrestamoModel prestamo = null;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_get_prestamo", conexion)) // Asumiendo que existe este SP
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_prestamo", SqlDbType.Int).Value = idPrestamo;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            prestamo = new PrestamoModel()
                            {
                                IdPrestamo = Convert.ToInt32(dr["id_prestamo"]),
                                IdEstudiante = Convert.ToInt32(dr["id_estudiante"]),
                                IdEquipo = Convert.ToInt32(dr["id_equipo"]),
                                Estado = (string) dr["estado"],
                                Fecha = DateOnly.FromDateTime(Convert.ToDateTime(dr["fecha"])),
                                HoraInicioPedido = (TimeSpan)dr["hora_inicio_pedido"],
                                TiempoPedido = Convert.ToInt32(dr["tiempo_pedido"]),
                                HoraFinPedido = (TimeSpan)dr["hora_fin_pedido"],
                                TiempoUsado = Convert.ToInt32(dr["tiempo_usado"])
                            };
                        }
                    }
                }
            }
            return prestamo;
        }

        public int GetIdPrestamoVigente(int idEstudiante)
        {
            int idPrestamo = -1;
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_get_prestamo_vigente", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.Add("@id_estudiante", SqlDbType.Int).Value = idEstudiante;

                    // Agregar el parámetro de salida
                    var param = new SqlParameter("@id_prestamo", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(param);
                    cmd.ExecuteNonQuery();
                    idPrestamo = (int)param.Value;
                }
            }
            return idPrestamo;
        }

        public List<PrestamoModel> ListPrestamos()
        {
            var lista = new List<PrestamoModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                // Crear el comando para ejecutar la stored procedure
                SqlCommand cmd = new SqlCommand("sp_list_prestamos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Ejecutar el comando y obtener los datos
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PrestamoModel()
                        {
                            IdPrestamo = Convert.ToInt32(dr["id_prestamo"]),
                            IdEstudiante = Convert.ToInt32(dr["id_estudiante"]),
                            IdEquipo = Convert.ToInt32(dr["id_equipo"]),
                            Estado = (string)dr["estado"],
                            Fecha = DateOnly.FromDateTime(Convert.ToDateTime(dr["fecha"])),
                            HoraInicioPedido = (TimeSpan)dr["hora_inicio_pedido"],
                            TiempoPedido = Convert.ToInt32(dr["tiempo_pedido"]),
                            TiempoUsado = Convert.ToInt32(dr["tiempo_usado"])
                        });
                    }
                }
            }
            return lista;
        }
        
        public List<PrestamoModel> ListPrestamoEstudiante(int idEstudiante)
        {
            var lista = new List<PrestamoModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                // Crear el comando para ejecutar la stored procedure
                SqlCommand cmd = new SqlCommand("sp_list_prestamos_estudiante", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Añadir el parámetro para la stored procedure
                cmd.Parameters.AddWithValue("@id_estudiante", idEstudiante);

                // Ejecutar el comando y obtener los datos
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PrestamoModel()
                        {
                            IdPrestamo = Convert.ToInt32(dr["id_prestamo"]),
                            IdEstudiante = Convert.ToInt32(dr["id_estudiante"]),
                            IdEquipo = Convert.ToInt32(dr["id_equipo"]),
                            Estado = (string)dr["estado"],
                            Fecha = DateOnly.FromDateTime(Convert.ToDateTime(dr["fecha"])),
                            HoraInicioPedido = (TimeSpan) dr["hora_inicio_pedido"],
                            TiempoUsado = Convert.ToInt32(dr["tiempo_usado"])
                        });
                    }
                }
            }
            return lista;
        }
        public string SetPrestamoFinalizar(int idPrestamo)
        {
            string mensaje = string.Empty;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_set_prestamo_finalizar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.Add("@id_prestamo", SqlDbType.Int).Value = idPrestamo;

                    // Agregar el parámetro de salida
                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensaje = (string)mensajeParam.Value;
                }
            }

            return mensaje; // Devolver tupla
        }

        public string AddPrestamo(int idEstudiante, int idEquipo, TimeSpan horaInicioPedido, int tiempoPedido)
        {
            string mensaje = string.Empty;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_add_prestamo", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.Add("@id_estudiante", SqlDbType.Int).Value = idEstudiante;
                    cmd.Parameters.Add("@id_equipo", SqlDbType.Int).Value = idEquipo;
                    cmd.Parameters.Add("@hora_inicio_pedido", SqlDbType.Time).Value = horaInicioPedido;
                    cmd.Parameters.Add("@tiempo_pedido", SqlDbType.Int).Value = tiempoPedido;

                    // Agregar el parámetro de salida
                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensaje = (string)mensajeParam.Value;
                }
            }

            return mensaje;
        }

        public List<PrestamoModel> BuscarPorCorreoFiltrarPrestamos(string cadenaBuscada, string filtro)
        {
            var lista = new List<PrestamoModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                // Crear el comando para ejecutar la stored procedure
                SqlCommand cmd = new SqlCommand("sp_list_prestamos_estudiante", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Añadir el parámetro para la stored procedure
                if(cadenaBuscada != String.Empty)
                    cmd.Parameters.AddWithValue("@cadena_buscada", cadenaBuscada);
                if(filtro != String.Empty)
                    cmd.Parameters.AddWithValue("@filtro", filtro);

                // Ejecutar el comando y obtener los datos
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PrestamoModel()
                        {
                            IdPrestamo = Convert.ToInt32(dr["id_prestamo"]),
                            IdEstudiante = Convert.ToInt32(dr["id_estudiante"]),
                            IdEquipo = Convert.ToInt32(dr["id_equipo"]),
                            Estado = (string)dr["estado"],
                            Fecha = DateOnly.FromDateTime(Convert.ToDateTime(dr["fecha"])),
                            HoraInicioPedido = (TimeSpan)dr["hora_inicio_pedido"],
                            TiempoPedido = Convert.ToInt32(dr["tiempo_pedido"]),
                            TiempoUsado = Convert.ToInt32(dr["tiempo_usado"])
                        });
                    }
                }
            }
            return lista;
        }
    }
}
