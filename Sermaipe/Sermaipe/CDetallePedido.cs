using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Sermaipe
{
    public class CDetallePedido
    {
        private string datosConexion;

        public CDetallePedido(string dC)
        {
            datosConexion = dC;
        }

        public string actualizaDatos(string comandoSQL)
        {
            try
            {
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();
                SqlCommand cmd = new SqlCommand(comandoSQL, conexion);
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch (SqlException ex)
            {
                return (ex.Message);
            }
            return ("Exitoso");
        }

        public DataTable obtenerMateriales(string id_pedido)
        {
            SqlCommand cmd;
            SqlConnection conexion;

            SqlDataAdapter adaptador = new SqlDataAdapter();
            DataSet ds = new DataSet();

            string sqlCmd = "SELECT D.idpedido, M.nombre, M.precio_venta, D.cantidad FROM ADMINISTRACION.DETALLE_PEDIDO D, INSUMO.MATERIAL M WHERE D.idmaterial = M.idmaterial AND D.idservicio = 1 AND D.idpedido =" + id_pedido;
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

        public DataTable obtenerServicios(string id_pedido)
        {
            SqlCommand cmd;
            SqlConnection conexion;

            SqlDataAdapter adaptador = new SqlDataAdapter();
            DataSet ds = new DataSet();


            string sqlCmd = "SELECT D.idpedido, S.descripcion FROM ADMINISTRACION.SERVICIO S, ADMINISTRACION.DETALLE_PEDIDO D WHERE D.idservicio = S.idservicio AND D.idmaterial = '1' AND D.idpedido =" + id_pedido;
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
