using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Sermaipe
{
    public class CServicio
    {
         private string datosConexion;

        public CServicio(string dC)
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
                cmd.Parameters.AddWithValue("@tipoServicio", nuevosDatos[1]);
                cmd.Parameters.AddWithValue("@descripcion", nuevosDatos[2]);
                nuevosDatos[3] = nuevosDatos[3].Replace('.', ',');
                cmd.Parameters.AddWithValue("@costo", Convert.ToDecimal(nuevosDatos[3]));
                

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

        public DataTable obtenerDatos()
        {
            SqlCommand cmd;
            SqlConnection conexion;

            SqlDataAdapter adaptador = new SqlDataAdapter();
            DataSet ds = new DataSet();

            string sqlCmd = "SELECT * FROM ADMINISTRACION.SERVICIO";
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
            catch (SqlException)
            {
                return null;
            }
        }
    }
}
