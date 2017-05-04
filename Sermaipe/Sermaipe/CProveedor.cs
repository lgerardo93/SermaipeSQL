using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Sermaipe
{
    public class CProveedor
    {
        private string datosConexion;
        public CProveedor(string dC)
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
                cmd.Parameters.AddWithValue("@idProveedor", nuevosDatos[0]);
                cmd.Parameters.AddWithValue("@nombres", nuevosDatos[1]);
                cmd.Parameters.AddWithValue("@apellidoP", nuevosDatos[2]);
                cmd.Parameters.AddWithValue("@apellidoM", nuevosDatos[3]);
                cmd.Parameters.AddWithValue("@domicilio", nuevosDatos[4]);
                cmd.Parameters.AddWithValue("@telefono", nuevosDatos[5]);
                cmd.Parameters.AddWithValue("@celular", nuevosDatos[6]);
                cmd.Parameters.AddWithValue("@email", nuevosDatos[7]);

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

            string sqlCmd = "SELECT * FROM PERSONA.PROVEEDOR";
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
