


using SistemaPrestamoEquipos.DB;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaPrestamoEquipos.Models
{
    public class UsuarioService
    {


        public UsuarioModel GetUsuario(int idUsuario)
        {
            UsuarioModel usuario = null;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_get_usuario", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new UsuarioModel
                            {
                                IdUsuario = Convert.ToInt32(dr["id_usuario"]),
                                Nombre = (string)dr["nombre"],
                                Correo = (string)dr["correo"],
                                Rol = (string)dr["rol"]
                            };
                        }
                    }
                }
            }
            return usuario;
        }


        public List<EstudianteModel> ListEstudiantes()
        {
            var lista = new List<EstudianteModel>();
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_list_estudiantes", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var estudiante = new EstudianteModel
                            {
                                IdEstudiante = Convert.ToInt32(dr["id_estudiante"]),
                                Estado = (string)dr["estado"],
                                Usuario = new UsuarioModel
                                {
                                    Nombre = (string)dr["nombre"],
                                    Correo = (string)dr["correo"]
                                }
                            };
                            lista.Add(estudiante);
                        }
                    }
                }
            }
            return lista;
        }



        public (int id, string rol, int idUsuario) Login(string correo, string contrasenia)
        {
            int id = -1; // Valor por defecto si no se encuentra
            string rol = string.Empty; // Inicializar rol como null
            int idUsuario = -1; // Valor por defecto si no se encuentra
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_login", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.Add("@correo", SqlDbType.NVarChar, 100).Value = correo;
                    cmd.Parameters.Add("@contrasenia", SqlDbType.NVarChar, 100).Value = contrasenia;

                    // Parámetros de salida
                    SqlParameter idParameter = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(idParameter);

                    SqlParameter rolParameter = new SqlParameter("@rol", SqlDbType.NVarChar, 20)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(rolParameter);

                    SqlParameter idUsuarioParameter = new SqlParameter("@id_usuario", SqlDbType.Int){Direction = ParameterDirection.Output};
                    cmd.Parameters.Add(idUsuarioParameter);

                    try
                    {
                        // Ejecutar el comando
                        cmd.ExecuteNonQuery();

                        // Capturar las salidas
                        id = (int) idParameter.Value;
                        rol = (string) rolParameter.Value;
                        idUsuario = (int) idUsuarioParameter.Value;
                    }
                    catch (SqlException ex)
                    {
                        // Manejo de errores
                        Console.WriteLine($"Error al verificar login: {ex.Message}");
                    }
                }
            }

            return (id, rol, idUsuario); // Devolver id, rol y id_usuario
        }

        public bool SetContrasena(int idUsuario, string nuevaContrasenia)
        {
            bool exito = false;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_set_contrasenia", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = idUsuario;
                    cmd.Parameters.Add("@nueva_contrasenia", SqlDbType.NVarChar, 100).Value = nuevaContrasenia;
                    //cmd.ExecuteNonQuery();

                    try
                    {
                        // Ejecutar el comando
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        Console.WriteLine($"Filas afectadas: {filasAfectadas}");

                        exito = filasAfectadas > 0; // Si se actualizaron filas, la operación fue exitosa
                    }
                    catch (SqlException ex)
                    {
                        // Manejo de errores: puedes registrar el error o lanzar una excepción personalizada
                        Console.WriteLine($"Error al cambiar la contraseña: {ex.Message}");
                    }

                }
            }
            return exito;
        }


        public string AddEstudiante(int idAdmin, string nombre, string correo)
        {
            var cn = new Conexion();

            string mensajeDb = "La conexión a la BD falló.";

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_add_estudiante", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id_admin", SqlDbType.Int).Value = idAdmin;
                    cmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
                    cmd.Parameters.Add("@correo", SqlDbType.NVarChar, 100).Value = correo;

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensajeDb = (string)mensajeParam.Value;
                }
            }
            return mensajeDb;
        }

        public string InhabilitarEstudiante(int idAdmin, int idEstudiante)
        {
            var cn = new Conexion();

            string mensajeDb = "La conexión a la BD falló.";

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_set_estudiante_inhabilitar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id_admin", SqlDbType.Int).Value = idAdmin;
                    cmd.Parameters.Add("@id_estudiante", SqlDbType.Int).Value = idEstudiante;

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensajeDb = (string)mensajeParam.Value;
                }
            }
            return mensajeDb;
        }

        public EstudianteModel GetEstudiante(int idEstudiante)
        {
            EstudianteModel estudiante = null;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_get_estudiante", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_estudiante", idEstudiante);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            estudiante = new EstudianteModel
                            {
                                IdEstudiante = Convert.ToInt32(dr["id_estudiante"]),
                                Estado = (string)dr["estado"],

                                Usuario = new UsuarioModel
                                {
                                    /*
                                     *         id_usuario, 
        nombre, 
        correo, 
        rol,
        id_estudiante, 
        estado
                                     * */
                                    IdUsuario = (int)dr["id_usuario"],
                                    Nombre = (string)dr["nombre"],
                                    Correo = (string)dr["correo"],
                                    Rol = (string)dr["rol"]
                                }
                            };
                        }
                    }
                }
            }
            return estudiante;
        }

        public string AddAdmin(int idAdmin, string nombre, string correo, string contrasenia)
        {
            var cn = new Conexion();

            string mensajeDb = "La conexión a la BD falló.";

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_add_admin", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id_admin", SqlDbType.Int).Value = idAdmin;
                    cmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
                    cmd.Parameters.Add("@correo", SqlDbType.NVarChar, 100).Value = correo;
                    cmd.Parameters.Add("@contrasenia", SqlDbType.NVarChar, 100).Value = contrasenia;

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensajeDb = (string)mensajeParam.Value;
                }
            }
            return mensajeDb;
        }

    }
}
