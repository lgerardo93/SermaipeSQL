using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Sermaipe
{
    public class CServicioEmpleado
    {
        private string datosConexion;

        public CServicioEmpleado(string dC)
        {
            datosConexion = dC;
        }

        public string actualizaDatos(string comandoSQL, List<string> nuevosDatos)
        {
            try
            {
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();
                SqlCommand cmd = new SqlCommand(comandoSQL, conexion);
                cmd.Parameters.AddWithValue("@idServicio", nuevosDatos[0]);
                cmd.Parameters.AddWithValue("@idEmpleado", nuevosDatos[1]);
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                return (ex.Message);
            }
            return ("Exitoso");
        }

        public string eliminaDatos(string comandoSQL)
        {
            try
            {
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();
                SqlCommand cmd = new SqlCommand(comandoSQL, conexion);
                cmd.Connection = conexion;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
            return "Exitoso";
        }

        public string insertaDatos(string comandoSQL)
        {
            try
            {
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();

                string sqlCmd = comandoSQL;
                SqlCommand cmd = new SqlCommand(sqlCmd, conexion);
                try
                {
                    cmd.ExecuteNonQuery();
                    conexion.Close();
                }
                catch (SqlException ex)
                {
                    conexion.Close();
                    return ex.Message;
                }
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
            return "Exitoso";
        }

        public bool existeEmpleado(string idServicio, string idEmpleado)
        {
            int count;
            SqlCommand cmd;
            SqlConnection conexion = new SqlConnection(datosConexion);
            conexion.Open();
            SqlDataAdapter adaptador = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string sqlCmd = "SELECT COUNT(*) FROM ADMINISTRACION.SERVICIO_EMPLEADO SE, PERSONA.EMPLEADO E WHERE SE.idEmpleado=" + idEmpleado + " AND SE.idServicio=" + idServicio;
            try
            {
                cmd = new SqlCommand(sqlCmd, conexion);
                count = (int)cmd.ExecuteScalar();

                conexion.Close();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException)
            {
                conexion.Close();
                return false;
            }
        }

        public DataTable obtenerDatos(string idServicio)
        {
            SqlCommand cmd;
            SqlConnection conexion;

            SqlDataAdapter adaptador = new SqlDataAdapter();
            DataSet ds = new DataSet();

            string sqlCmd = "SELECT SE.idServicio, E.nombres+'_'+E.apellidoP+'_'+E.apellidoM AS 'Empleados encargados' FROM ADMINISTRACION.SERVICIO_EMPLEADO SE, PERSONA.EMPLEADO E WHERE SE.idEmpleado=E.idEmpleado AND SE.idServicio=" + idServicio;
            try
            {
                conexion = new SqlConnection(datosConexion);
                conexion.Open();
                cmd = new SqlCommand(sqlCmd, conexion);

                adaptador.SelectCommand = cmd;
                adaptador.Fill(ds);
                adaptador.Dispose();
                cmd.Dispose();
                conexion.Close();
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                return null;
            }
        }
    }
}
