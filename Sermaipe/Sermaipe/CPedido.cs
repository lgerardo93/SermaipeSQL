using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace Sermaipe
{
    public class CPedido
    {
        private string datosConexion;

        public CPedido(string dC)
        {
            datosConexion = dC;
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

            string sqlCmd = "SELECT * FROM ADMINISTRACION.PEDIDO";
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
