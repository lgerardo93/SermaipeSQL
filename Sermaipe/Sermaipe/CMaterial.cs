using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Sermaipe
{
    public class CMaterial
    {
        private string datosConexion;
        public CMaterial(string dC)
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
                cmd.Parameters.AddWithValue("@idMaterial", nuevosDatos[0]);
                cmd.Parameters.AddWithValue("@idProveedor", nuevosDatos[1]);
                cmd.Parameters.AddWithValue("@nombre", nuevosDatos[2]);
                cmd.Parameters.AddWithValue("@descripcion", nuevosDatos[3]);
                cmd.Parameters.AddWithValue("@stock", Int32.Parse(nuevosDatos[4]));

                nuevosDatos[5] = nuevosDatos[5].Replace('.',',');
                nuevosDatos[6] = nuevosDatos[6].Replace('.', ',');
                cmd.Parameters.AddWithValue("@precio_compra", Convert.ToDecimal(nuevosDatos[5]));
                cmd.Parameters.AddWithValue("@precio_venta", Convert.ToDecimal(nuevosDatos[6]));

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

            string sqlCmd = "SELECT * FROM INSUMO.MATERIAL";
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
