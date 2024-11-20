using System.Data.SqlClient;
using System.Data;
using SistemaPrestamoEquipos.Models;
using System.Reflection.PortableExecutable;

namespace SistemaPrestamoEquipos.DB
{
    public class InventarioService
    {
        public List<InventarioModel> ListInventarios()
        {
            var lista = new List<InventarioModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_list_inventarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new InventarioModel()
                        {
                            IdInventario = Convert.ToInt32(dr["id_inventario"]),
                            NumLabo = Convert.ToInt32(dr["num_labo"]),
                            NumEquipos = Convert.ToInt32(dr["num_equipos"]),
                            NumComponentes = Convert.ToInt32(dr["num_componentes"])
                        });
                    }
                }
            }
            return lista;
        }

        public string AddInventario(int idAdmin, int numLabo)
        {
            string mensaje = string.Empty;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_add_inventario", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros obligatorios
                    cmd.Parameters.Add("@id_admin", SqlDbType.Int).Value = idAdmin;
                    cmd.Parameters.Add("@num_labo", SqlDbType.Int).Value = numLabo;

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensaje = (string)mensajeParam.Value;
                }
            }
            return mensaje;
        }

        //#################################################################################33
        //#################################################################################33
        //#################################################################################33

        public List<EquipoModel> GetEquiposPorInventario(int idInventario)
        {

            var lista = new List<EquipoModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_list_equipos_por_inventario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_inventario", SqlDbType.Int).Value = idInventario;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new EquipoModel()
                        {
                            IdEquipo = Convert.ToInt32(dr["id_equipo"]),
                            Estado = (string)dr["estado"],
                            Nombre = (string)dr["nombre"],
                            Disponibilidad = (string)dr["disponibilidad"]
                        });
                    }
                }
            }
            return lista;
        }

        public EquipoModel GetEquipoPorId(int idEquipo)
        {
            EquipoModel? equipo = null;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_get_equipo", conexion)) // Asumiendo que existe este SP
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_equipo", SqlDbType.Int).Value = idEquipo;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            equipo = new EquipoModel()
                            {
                                IdEquipo = Convert.ToInt32(dr["id_equipo"]),
                                IdInventario = Convert.ToInt32(dr["id_inventario"]),
                                CodigoEnLabo = Convert.ToInt32(dr["codigo_en_labo"]),
                                NumLabo = Convert.ToInt32(dr["num_labo"]),
                                Nombre = (string)dr["nombre"],
                                Estado = (string)dr["estado"],
                                Disponibilidad = (string)dr["disponibilidad"],
                                CodigoPantalla = Convert.IsDBNull(dr["codigo_pantalla"]) ? "" : (string)dr["codigo_pantalla"],
                                CodigoMouse = Convert.IsDBNull(dr["codigo_mouse"]) ? "" : (string)dr["codigo_mouse"],
                                CodigoTeclado = Convert.IsDBNull(dr["codigo_teclado"]) ? "" : (string)dr["codigo_teclado"],
                                CodigoCpu = Convert.IsDBNull(dr["codigo_cpu"]) ? "" : (string)dr["codigo_cpu"]
                            };
                        }
                    }
                }
            }
            return equipo;
        }

        public InventarioModel GetInventario(int idInventario)
        {
            InventarioModel? inventario = null;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_get_inventario", conexion)) // Asumiendo que existe este SP
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_inventario", SqlDbType.Int).Value = idInventario;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            inventario = new InventarioModel()
                            {
                                IdInventario = Convert.ToInt32(dr["id_inventario"]),
                                IdAdmin = Convert.ToInt32(dr["id_admin"]),
                                NumLabo = Convert.ToInt32(dr["num_labo"]),
                                NumEquipos = Convert.ToInt32(dr["num_equipos"]),
                                NumComponentes = Convert.ToInt32(dr["num_componentes"])
                            };
                        }
                    }
                }
            }
            return inventario;
        }

        public string SetEquipo(int idAdmin, int idEquipo, int codigoEnLabo, string codigoPantalla, string codigoCpu, string codigoTeclado, string codigoMouse)
        {
            string mensaje = string.Empty;
            var cn = new Conexion();

            Console.WriteLine($"idAdmin: {idAdmin}");
            Console.WriteLine($"idEquipo: {idEquipo}");
            Console.WriteLine($"codigoEnLabo: {codigoEnLabo}");
            Console.WriteLine($"codigoPantalla: {codigoPantalla}");
            Console.WriteLine($"codigoCpu: {codigoCpu}");
            Console.WriteLine($"codigoTeclado: {codigoTeclado}");
            Console.WriteLine($"codigoMouse: {codigoMouse}");

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_set_equipo", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros obligatorios
                    cmd.Parameters.Add("@id_admin", SqlDbType.Int).Value = idAdmin;
                    cmd.Parameters.Add("@id_equipo", SqlDbType.Int).Value = idEquipo;
                    if(codigoEnLabo != -1)
                        cmd.Parameters.Add("@codigo_en_labo", SqlDbType.Int).Value = codigoEnLabo;

                    if (!string.IsNullOrEmpty(codigoPantalla))
                        cmd.Parameters.Add("@codigo_pantalla", SqlDbType.NVarChar, 10).Value = codigoPantalla;

                    if (!string.IsNullOrEmpty(codigoCpu))
                        cmd.Parameters.Add("@codigo_cpu", SqlDbType.NVarChar, 10).Value = codigoCpu;

                    if (!string.IsNullOrEmpty(codigoTeclado))
                        cmd.Parameters.Add("@codigo_teclado", SqlDbType.NVarChar, 10).Value = codigoTeclado;

                    if (!string.IsNullOrEmpty(codigoMouse))
                        cmd.Parameters.Add("@codigo_mouse", SqlDbType.NVarChar, 10).Value = codigoMouse;

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensaje = (string)mensajeParam.Value;
                }
            }
            return mensaje;
        }


        public string AddEquipo(int idAdmin, int idInventario, int codigoEnLabo, string codigoPantalla, string codigoCpu, string codigoTeclado, string codigoMouse)
        {
            string mensaje = string.Empty;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_add_equipo", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros obligatorios
                    cmd.Parameters.Add("@id_inventario", SqlDbType.Int).Value = idInventario;
                    cmd.Parameters.Add("@id_admin", SqlDbType.Int).Value = idAdmin;

                    cmd.Parameters.Add("@codigo_en_labo", SqlDbType.Int).Value = codigoEnLabo;

                    if (!string.IsNullOrEmpty(codigoPantalla))
                        cmd.Parameters.Add("@codigo_pantalla", SqlDbType.NVarChar, 10).Value = codigoPantalla;
                    if (!string.IsNullOrEmpty(codigoCpu))
                        cmd.Parameters.Add("@codigo_cpu", SqlDbType.NVarChar, 10).Value = codigoCpu;
                    if (!string.IsNullOrEmpty(codigoTeclado))
                        cmd.Parameters.Add("@codigo_teclado", SqlDbType.NVarChar, 10).Value = codigoTeclado;
                    if (!string.IsNullOrEmpty(codigoMouse))
                        cmd.Parameters.Add("@codigo_mouse", SqlDbType.NVarChar, 10).Value = codigoMouse;

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensaje = (string)mensajeParam.Value;
                }
            }
            return mensaje;
        }

        //#################################################################################33
        //#################################################################################33
        //#################################################################################33

        public ComponenteModel GetCompPorId(int idComponente)
        {
            ComponenteModel? componente = null;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_get_componente", conexion)) // Asumiendo que existe este SP
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_comp", SqlDbType.Int).Value = idComponente;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            componente = new ComponenteModel()
                            {
                                IdEquipo = Convert.IsDBNull(dr["id_equipo"]) ? -1 : Convert.ToInt32(dr["id_equipo"]),
                                IdInventario = Convert.ToInt32(dr["id_inventario"]),
                                Tipo = (string)dr["tipo"],
                                Estado = (string)dr["estado"],
                                Codigo = (string)dr["codigo"],
                                Marca = Convert.IsDBNull(dr["marca"]) ? "" : (string)dr["marca"],
                                Disponibilidad = (string)dr["disponibilidad"]
                            };
                        }
                    }
                }
            }
            return componente;
        }
        public List<ComponenteModel> GetCompsPorInventario(int idInventario)
        {

            var lista = new List<ComponenteModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_list_componentes_por_inventario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_inventario", SqlDbType.Int).Value = idInventario;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ComponenteModel()
                        {
                            IdComp = Convert.ToInt32(dr["id_comp"]),
                            Tipo = (string)dr["tipo"],
                            Estado = (string)dr["estado"],
                            Codigo = (string)dr["codigo"],
                            Marca = Convert.IsDBNull(dr["marca"]) ? "" : (string)dr["marca"],
                            Disponibilidad = (string)dr["disponibilidad"]
                        });
                    }
                }
            }
            return lista;
        }

        public string AddComp(int idAdmin, int idEquipo, int idInventario, string tipo, string codigo, string marca)
        {
            string mensaje = string.Empty;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_add_componente", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros obligatorios
                    cmd.Parameters.Add("@id_admin", SqlDbType.Int).Value = idAdmin;
                    cmd.Parameters.Add("@id_inventario", SqlDbType.Int).Value = idInventario;

                    if(idEquipo != -1)
                        cmd.Parameters.Add("@id_equipo", SqlDbType.Int).Value = idEquipo;

                    cmd.Parameters.Add("@tipo", SqlDbType.NVarChar, 10).Value = tipo;
                    cmd.Parameters.Add("@codigo", SqlDbType.NVarChar, 10).Value = codigo;
                    cmd.Parameters.Add("@marca", SqlDbType.NVarChar, 10).Value = marca;

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensaje = (string)mensajeParam.Value;
                }
            }
            return mensaje;
        }

        public string SetComp(int idAdmin, int idComp, int idEquipo)
        {
            string mensaje = string.Empty;
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_set_componente", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros obligatorios
                    cmd.Parameters.Add("@id_admin", SqlDbType.Int).Value = idAdmin;
                    cmd.Parameters.Add("@id_comp", SqlDbType.Int).Value = idComp;

                    if (idEquipo == -1)
                        return "El ID Equipo no es válido";
                    
                    cmd.Parameters.Add("@id_equipo", SqlDbType.Int).Value = idEquipo;

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensajeParam);
                    cmd.ExecuteNonQuery();
                    mensaje = (string)mensajeParam.Value;
                }
            }
            return mensaje;
        }


        public List<EquipoModel> ListEquiposDisponibles()
        {
            var lista = new List<EquipoModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_list_equipos_disponibles", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new EquipoModel()
                        {
                            IdEquipo = Convert.ToInt32(dr["id_equipo"]),
                            Nombre = (string)dr["nombre"],
                            NumLabo = Convert.ToInt32(dr["num_labo"])
                        });
                    }
                }
            }
            return lista;
        }







    }

}
