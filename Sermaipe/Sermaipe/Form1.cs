using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Sermaipe
{
    public partial class Principal : Form
    {
        private string datosConexion;
        private string idEmpleado;
        private string idCliente;
        private string idProveedor;
        private string idMaterial;
        private string idServicio;
        private string idPedido;

        private CEmpleado empleado;
        private CCliente cliente;
        private CProveedor proveedor;
        private CMaterial material;
        private CServicio servicio;
        private CServicioEmpleado servicioEmpleado;
        private CPedido pedido;
        private CDetallePedido detallePedido;

        DateTime fecha = DateTime.Today;

        public Principal()
        {
            InitializeComponent();
        }

        private void boton_sesion_Click(object sender, EventArgs e)
        {
            boton_sesion.Visible = false;
            tabControl1.Visible = true;
        }


        private void Principal_Load(object sender, EventArgs e)
        {
            datosConexion = @"Data Source = DESKTOP-INCBI9H\SQLEXPRESS; Initial Catalog = SERMAIPE ; Integrated Security = true;";

            //datosConexion = @"Data Source = KARLAZM\SQLEXPRESS;" + "Initial Catalog = PROYECTO_SERMAIPE ; Integrated Security=true;";

            empleado = new CEmpleado(datosConexion);
            cliente = new CCliente(datosConexion);
            proveedor = new CProveedor(datosConexion);
            material = new CMaterial(datosConexion);
            servicio = new CServicio(datosConexion);
            servicioEmpleado = new CServicioEmpleado(datosConexion);
            pedido = new CPedido(datosConexion);
            detallePedido = new CDetallePedido(datosConexion);

            idEmpleado = "";
            idCliente = "";
            idProveedor = "";
            idMaterial = "";
            idServicio = "";
            idPedido = "";
            dataGridEmpleados.MultiSelect = false;
            dataGridEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridClientes.MultiSelect = false;
            dataGridClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridProveedores.MultiSelect = false;
            dataGridProveedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridMateriales.MultiSelect = false;
            dataGridMateriales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridPedidos.MultiSelect = false;
            dataGridPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridServicios.MultiSelect = false;
            dataGridServicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridServicioEmpleado.MultiSelect = false;
            dataGridServicioEmpleado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridPedidos.MultiSelect = false;
            dataGridPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridMateriales_pedido.MultiSelect = false;
            dataGridMateriales_pedido.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridServicios_pedido.MultiSelect = false;
            dataGridServicios_pedido.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            muestraTablas();
            cargaProveedores();
            cargaServicios();
            cargaEmpleado();
            cargaClientes();
            cargaMaterialesPedido();
            cargaServiciosPedido();
            //obtenerDatosPedidosMateriales();
            //obtenerDatosPedidosServicios();
        }


        private void cargaServiciosPedido()
        {
            t_idServicio_pedido.Clear();
            comboServicios.Items.Clear();
            SqlConnection conexion = new SqlConnection(datosConexion);
            SqlCommand cmd = new SqlCommand("Select * from ADMINISTRACION.SERVICIO", conexion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                comboServicios.Items.Add(ds.Tables[0].Rows[i][1]);
        }

        private void cargaMaterialesPedido()
        {
            t_idMaterial_pedido.Clear();
            comboMateriales.Items.Clear();
            SqlConnection conexion = new SqlConnection(datosConexion);
            SqlCommand cmd = new SqlCommand("Select * from INSUMO.MATERIAL", conexion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                comboMateriales.Items.Add(ds.Tables[0].Rows[i][2]);
        }

        //Carga los proveedores en el textbox de la pestaña materiales
        public void cargaProveedores()
        {
            comboProveedores.Items.Clear();
            SqlConnection conexion = new SqlConnection(datosConexion);
            SqlCommand cmd = new SqlCommand("Select * from PERSONA.PROVEEDOR",conexion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                comboProveedores.Items.Add(ds.Tables[0].Rows[i][1] + "_" + ds.Tables[0].Rows[i][2] + "_" + ds.Tables[0].Rows[i][3]);
        }

        public void cargaClientes()
        {
            comboClientes.Items.Clear();
            SqlConnection conexion = new SqlConnection(datosConexion);
            SqlCommand cmd = new SqlCommand("Select * from PERSONA.CLIENTE", conexion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                comboClientes.Items.Add(ds.Tables[0].Rows[i][1] + "_" + ds.Tables[0].Rows[i][2] + "_" + ds.Tables[0].Rows[i][3]);
        }

        private void cargaServicios()
        {
            comboBoxServicios.Items.Clear();
            comboBoxServicios.Items.Add("Instalacion");
            comboBoxServicios.Items.Add("Mantenimiento");
            comboBoxServicios.Items.Add("Reparacion");
        }

        /*private void cargaServiciosPedido()
        {
            comboServicios.Items.Clear();
            SqlConnection conexion = new SqlConnection(datosConexion);
            SqlCommand cmd = new SqlCommand("Select * from ADMINISTRACION.SERVICIO", conexion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                comboServicios.Items.Add(ds.Tables[0].Rows[i][2]);
        }*/

        public void cargaEmpleado()
        {
            comboEmpleados.Items.Clear();
            SqlConnection conexion = new SqlConnection(datosConexion);
            SqlCommand cmd = new SqlCommand("Select * from PERSONA.EMPLEADO", conexion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                comboEmpleados.Items.Add(ds.Tables[0].Rows[i][1] + "_" + ds.Tables[0].Rows[i][2] + "_" + ds.Tables[0].Rows[i][3]);
        }

        /*public void cargaClientes()
        {
            comboCliente.Items.Clear();
            SqlConnection conexion = new SqlConnection(datosConexion);
            SqlCommand cmd = new SqlCommand("Select * from PERSONA.CLIENTE", conexion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                comboCliente.Items.Add(ds.Tables[0].Rows[i][1] + "_" + ds.Tables[0].Rows[i][2] + "_" + ds.Tables[0].Rows[i][3]);
        }*/

        /*public void cargaMaterial()  //Carga los materiales en el combo box en la pestaña de pedidos
        {
            comboMaterial.Items.Clear();
            SqlConnection conexion = new SqlConnection(datosConexion);
            SqlCommand cmd = new SqlCommand("Select * from INSUMO.MATERIAL", conexion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                comboMaterial.Items.Add(ds.Tables[0].Rows[i][2]);
            }
        }*/



        private void muestraTablas()
        {
            dataGridEmpleados.DataSource = empleado.obtenerDatos();
            dataGridClientes.DataSource = cliente.obtenerDatos();
            dataGridProveedores.DataSource = proveedor.obtenerDatos();
            dataGridMateriales.DataSource = material.obtenerDatos();
            dataGridServicios.DataSource = servicio.obtenerDatos();
            dataGridPedidos.DataSource = pedido.obtenerDatos();
            while (dataGridServicioEmpleado.Rows.Count > 0)
                dataGridServicioEmpleado.Rows.RemoveAt(0);
            dataGridServicioEmpleado.DataSource = servicioEmpleado.obtenerDatos(idServicio);
            dataGridMateriales_pedido.DataSource = detallePedido.obtenerMateriales(idPedido);
            dataGridServicios_pedido.DataSource = detallePedido.obtenerServicios(idPedido);
            
            
        }

        private void dataGridEmpleados_MouseClick(object sender, MouseEventArgs e)
        {   
            if (dataGridEmpleados.SelectedCells.Count > 1)
            {
                idEmpleado = dataGridEmpleados.SelectedRows[0].Cells[0].Value.ToString();

                this.t_idEmpleado.Text = dataGridEmpleados.SelectedCells[0].Value.ToString();
                this.t_nombresEmpleado.Text = dataGridEmpleados.SelectedCells[1].Value.ToString();
                this.t_apellidoP_empleado.Text = dataGridEmpleados.SelectedCells[2].Value.ToString();
                this.t_apellidoM_empleado.Text = dataGridEmpleados.SelectedCells[3].Value.ToString();
                string[] domicilio = dataGridEmpleados.SelectedCells[4].Value.ToString().Split('_');
                this.t_colonia_empleado.Text = domicilio[0];
                this.t_calle_empleado.Text = domicilio[1];
                this.t_numero_empleado.Text = domicilio[2];
                this.t_telefono_empleado.Text = dataGridEmpleados.SelectedCells[5].Value.ToString();
                this.t_celular_empleado.Text = dataGridEmpleados.SelectedCells[6].Value.ToString();
                this.t_sueldo_empleado.Text = dataGridEmpleados.SelectedCells[7].Value.ToString();
            }
        }

        private void dataGridClientes_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridClientes.SelectedCells.Count > 1)
            {
                idCliente = dataGridClientes.SelectedRows[0].Cells[0].Value.ToString();

                this.t_idCliente.Text = dataGridClientes.SelectedCells[0].Value.ToString();
                this.t_nombresCliente.Text = dataGridClientes.SelectedCells[1].Value.ToString();
                this.t_apellidoP_cliente.Text = dataGridClientes.SelectedCells[2].Value.ToString();
                this.t_apellidoM_cliente.Text = dataGridClientes.SelectedCells[3].Value.ToString();
                string[] domicilio = dataGridClientes.SelectedCells[4].Value.ToString().Split('_');
                this.t_colonia_cliente.Text = domicilio[0];
                this.t_calle_cliente.Text = domicilio[1];
                this.t_numero_cliente.Text = domicilio[2];
                this.t_telefono_cliente.Text = dataGridClientes.SelectedCells[5].Value.ToString();
                this.t_celular_cliente.Text = dataGridClientes.SelectedCells[6].Value.ToString();
                this.t_credito_cliente.Text = dataGridClientes.SelectedCells[7].Value.ToString();
            }
        }

        private void dataGridProveedores_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridProveedores.SelectedCells.Count > 1)
            {
                idProveedor = dataGridProveedores.SelectedRows[0].Cells[0].Value.ToString();

                this.t_idProveedor.Text = dataGridProveedores.SelectedCells[0].Value.ToString();
                this.t_nombresProveedor.Text = dataGridProveedores.SelectedCells[1].Value.ToString();
                this.t_apellidoP_proveedor.Text = dataGridProveedores.SelectedCells[2].Value.ToString();
                this.t_apellidoM_proveedor.Text = dataGridProveedores.SelectedCells[3].Value.ToString();
                string[] domicilio = dataGridProveedores.SelectedCells[4].Value.ToString().Split('_');
                this.t_colonia_proveedor.Text = domicilio[0];
                this.t_calle_proveedor.Text = domicilio[1];
                this.t_numero_proveedor.Text = domicilio[2];
                this.t_telefono_proveedor.Text = dataGridProveedores.SelectedCells[5].Value.ToString();
                this.t_celular_proveedor.Text = dataGridProveedores.SelectedCells[6].Value.ToString();
                this.t_email_proveedor.Text = dataGridProveedores.SelectedCells[7].Value.ToString();
            }
        }

        private void limpia_camposEmpleado()
        {
            t_idEmpleado.Clear();
            t_nombresEmpleado.Clear();
            t_apellidoP_empleado.Clear();
            t_apellidoM_empleado.Clear();
            t_colonia_empleado.Clear();
            t_calle_empleado.Clear();
            t_numero_empleado.Clear();
            t_telefono_empleado.Clear();
            t_celular_empleado.Clear();
            t_sueldo_empleado.Clear();

            idEmpleado = "";
        }

        private void limpia_camposCliente()
        {
            t_idCliente.Clear();
            t_nombresCliente.Clear();
            t_apellidoP_cliente.Clear();
            t_apellidoM_cliente.Clear();
            t_colonia_cliente.Clear();
            t_calle_cliente.Clear();
            t_numero_cliente.Clear();
            t_telefono_cliente.Clear();
            t_celular_cliente.Clear();
            t_credito_cliente.Clear();

            idCliente = "";
        }

        private void limpia_camposProveedor()
        {
            t_idProveedor.Clear();
            t_nombresProveedor.Clear();
            t_apellidoP_proveedor.Clear();
            t_apellidoM_proveedor.Clear();
            t_colonia_proveedor.Clear();
            t_calle_proveedor.Clear();
            t_numero_proveedor.Clear();
            t_telefono_proveedor.Clear();
            t_celular_proveedor.Clear();
            t_email_proveedor.Clear();

            idProveedor = "";
        }

        private void boton_guardarEmpleado_Click(object sender, EventArgs e)
        {
            if (idEmpleado == "")
            {
                if (validaCamposEmpleados())
                {
                    string comandoSQL = "INSERT INTO PERSONA.EMPLEADO(nombres, apellidoP, apellidoM, domicilio, telefono, celular, sueldo) VALUES('" + t_nombresEmpleado.Text + "', '" + t_apellidoP_empleado.Text + "', '" + t_apellidoM_empleado.Text + "', '" + t_colonia_empleado.Text + '_' + t_calle_empleado.Text + '_' + t_numero_empleado.Text + "', " + t_telefono_empleado.Text + ", " + t_celular_empleado.Text + ", " + t_sueldo_empleado.Text + ")";
                    if (empleado.insertaDatos(comandoSQL) == "Exitoso")
                    {
                        MessageBox.Show("Se ha insertado exitosamente");
                        limpia_camposEmpleado();
                        muestraTablas();
                        limpia_camposServicioEmpleado();
                        cargaEmpleado();
                    }
                    else
                        MessageBox.Show("No se pudo insertar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("No se pudo insertar el mismo registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string> dameCamposCliente()
        {
            List<string> listaCampos = new List<string>();

            listaCampos.Add(t_idCliente.Text);
            listaCampos.Add(t_nombresCliente.Text);
            listaCampos.Add(t_apellidoP_cliente.Text);
            listaCampos.Add(t_apellidoM_cliente.Text);
            listaCampos.Add(t_colonia_cliente.Text + '_' + t_calle_cliente.Text + '_' + t_numero_cliente.Text);
            listaCampos.Add(t_telefono_cliente.Text);
            listaCampos.Add(t_celular_cliente.Text);
            listaCampos.Add(t_credito_cliente.Text);

            return listaCampos;
        }

        private List<string> dameCamposMaterial()
        {
            List<string> listaCampos = new List<string>();

            listaCampos.Add(t_idMaterial.Text);
            listaCampos.Add(t_MidProveedor.Text);
            listaCampos.Add(t_nombreMaterial.Text);
            listaCampos.Add(t_descripcion_material.Text);
            listaCampos.Add(t_disponibles.Text);
            listaCampos.Add(t_precio_compra.Text);
            listaCampos.Add(t_precio_venta.Text);

            return listaCampos;
        }

        

        private List<string> dameCamposEmpleado()
        {
            List<string> listaCampos = new List<string>();

            listaCampos.Add(t_idEmpleado.Text);
            listaCampos.Add(t_nombresEmpleado.Text);
            listaCampos.Add(t_apellidoP_empleado.Text);
            listaCampos.Add(t_apellidoM_empleado.Text);
            listaCampos.Add(t_colonia_empleado.Text+'_'+t_calle_empleado.Text+'_'+t_numero_empleado.Text);
            listaCampos.Add(t_telefono_empleado.Text);
            listaCampos.Add(t_celular_empleado.Text);
            listaCampos.Add(t_sueldo_empleado.Text);

            return listaCampos;
        }

        private List<string> dameCamposProveedor()
        {
            List<string> listaCampos = new List<string>();

            listaCampos.Add(t_idProveedor.Text);
            listaCampos.Add(t_nombresProveedor.Text);
            listaCampos.Add(t_apellidoP_proveedor.Text);
            listaCampos.Add(t_apellidoM_proveedor.Text);
            listaCampos.Add(t_colonia_proveedor.Text + '_' + t_calle_proveedor.Text + '_' + t_numero_proveedor.Text);
            listaCampos.Add(t_telefono_proveedor.Text);
            listaCampos.Add(t_celular_proveedor.Text);
            listaCampos.Add(t_email_proveedor.Text);

            return listaCampos;
        }
        private List<string> dameCamposServicios()
        {
            List<string> listaCampos = new List<string>();

            listaCampos.Add(textIdServicios.Text);
            listaCampos.Add(comboBoxServicios.Text);
            listaCampos.Add(textDescripcionServicios.Text);
            listaCampos.Add(textCostoServicios.Text);

            return listaCampos;
        }

        private void boton_actualizaEmpleado_Click(object sender, EventArgs e)
        {

            if (idEmpleado != "")
            {
                if (validaCamposEmpleados())
                {
                    string comandoSQL = "UPDATE PERSONA.EMPLEADO set nombres=@nombres, apellidoP=@apellidoP, apellidoM=@apellidoM, domicilio=@domicilio, telefono=@telefono, celular=@celular, sueldo=@sueldo WHERE idEmpleado = " + idEmpleado;
                    if (empleado.actualizaDatos(comandoSQL, dameCamposEmpleado()) == "Exitoso")
                    {
                        MessageBox.Show("Se ha actualizado exitosamente");
                        limpia_camposEmpleado();
                        muestraTablas();
                        cargaEmpleado();
                    }
                    else
                        MessageBox.Show("No se pudo actualizar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No hay registro por actualizar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool validaCamposEmpleados()
        {
            double d;
            float f;
            int i;

            if (string.IsNullOrEmpty(t_nombresEmpleado.Text) || string.IsNullOrEmpty(t_apellidoP_empleado.Text) || string.IsNullOrEmpty(t_apellidoM_empleado.Text))
                return false;
            if (string.IsNullOrEmpty(t_colonia_empleado.Text) || string.IsNullOrEmpty(t_calle_empleado.Text) || string.IsNullOrEmpty(t_numero_empleado.Text))
                return false;
            if (!Int32.TryParse(t_numero_empleado.Text, out i))
            {
                t_numero_empleado.Focus();
                return false;
            }
            if (!Double.TryParse(t_telefono_empleado.Text, out d) || t_telefono_empleado.Text.Length!=7)
            {
                t_telefono_empleado.Focus();
                return false;
            }
            if (!Double.TryParse(t_celular_empleado.Text, out d) || t_celular_empleado.Text.Length!=10)
            {
                t_celular_empleado.Focus();
                return false;
            }
            if (!float.TryParse(t_sueldo_empleado.Text, out f) || float.Parse(t_sueldo_empleado.Text)<0)
            {
                t_sueldo_empleado.Focus();
                return false;
            }
            return true;
        }

        private bool validaCamposClientes()
        {
            double d;
            int i;

            if (string.IsNullOrEmpty(t_nombresCliente.Text) || string.IsNullOrEmpty(t_apellidoP_cliente.Text) || string.IsNullOrEmpty(t_apellidoM_cliente.Text))
                return false;
            if (string.IsNullOrEmpty(t_colonia_cliente.Text) || string.IsNullOrEmpty(t_calle_cliente.Text) || string.IsNullOrEmpty(t_numero_cliente.Text))
                return false;
            if (!Int32.TryParse(t_numero_cliente.Text, out i))
            {
                t_numero_cliente.Focus();
                return false;
            }
            if (!Double.TryParse(t_telefono_cliente.Text, out d) || t_telefono_cliente.Text.Length != 7)
            {
                t_telefono_cliente.Focus();
                return false;
            }
            if (!Double.TryParse(t_celular_cliente.Text, out d) || t_celular_cliente.Text.Length != 10)
            {
                t_celular_cliente.Focus();
                return false;
            }
            if (!Int32.TryParse(t_credito_cliente.Text, out i))
            {
                t_credito_cliente.Focus();
                return false;
            }
            return true;
        }

        private bool validaCamposServicios()
        {
            float f;
            if (string.IsNullOrEmpty(comboBoxServicios.SelectedItem.ToString()))
                return false;
            if (string.IsNullOrEmpty(textDescripcionServicios.Text))
                return false;
            if(!float.TryParse(textCostoServicios.Text, out f) || float.Parse(textCostoServicios.Text)<0)
                return false;
            return true;
        }

        private bool validaCamposMateriales()
        {
            float d;
            int i;

            if (comboProveedores.SelectedItem == null || string.IsNullOrEmpty(comboProveedores.SelectedItem.ToString()))
                return false;
            if (string.IsNullOrEmpty(t_nombreMaterial.Text) || string.IsNullOrEmpty(t_descripcion_material.Text))
                return false;
            if (!Int32.TryParse(t_disponibles.Text, out i))
            {
                t_disponibles.Focus();
                return false;
            }
            if (!float.TryParse(t_precio_compra.Text, out d) || float.Parse(t_precio_compra.Text)<0)
            {
                t_precio_compra.Focus();
                return false;
            }
            if (!float.TryParse(t_precio_venta.Text, out d) || float.Parse(t_precio_venta.Text)<0)
            {
                t_precio_venta.Focus();
                return false;
            }
            return true;
        }

        private bool validaCamposProveedores()
        {
            double d;
            int i;

            if (string.IsNullOrEmpty(t_nombresProveedor.Text) || string.IsNullOrEmpty(t_apellidoP_proveedor.Text) || string.IsNullOrEmpty(t_apellidoM_proveedor.Text))
                return false;
            if (string.IsNullOrEmpty(t_colonia_proveedor.Text) || string.IsNullOrEmpty(t_calle_proveedor.Text) || string.IsNullOrEmpty(t_numero_proveedor.Text))
                return false;
            if (!Int32.TryParse(t_numero_proveedor.Text, out i))
            {
                t_numero_proveedor.Focus();
                return false;
            }
            if (!Double.TryParse(t_telefono_proveedor.Text, out d) || t_telefono_proveedor.Text.Length > 7)
            {
                t_telefono_proveedor.Focus();
                return false;
            }
            if (!Double.TryParse(t_celular_proveedor.Text, out d) || t_celular_proveedor.Text.Length > 10)
            {
                t_celular_proveedor.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(t_nombresProveedor.Text))
            {
                t_email_proveedor.Focus();
                return false;
            }
            else
            {
                string cadena = t_email_proveedor.Text;
                string regex = @"^((([\w]+\.[\w]+)+)|([\w]+))@(([\w]+\.)+)([A-Za-z]{1,3})$";
                if (ValidarDato(regex, cadena) == false)
                {
                    t_email_proveedor.Focus();
                    return false;
                }
            }

            return true;
        }

        public bool ValidarDato(string regex, string dato)
        {
            // Creación del objecto Regex
            Regex r = new Regex(regex);
            // Validación
            return r.IsMatch(dato);
        }

        

        private void eliminacionFisicaEnCascadaEmpleado()
        {
            limpia_camposServicioEmpleado();
            //Eliminacion física en cascada de Empleado ----
            string comandoSQL = "DELETE FROM ADMINISTRACION.SERVICIO_EMPLEADO WHERE idEmpleado=" + idEmpleado + "";
            if (servicioEmpleado.eliminaDatos(comandoSQL) != "Exitoso")
                MessageBox.Show("No se pudieron eliminar todos los registros");
            //----------------------------------------------
        }

        private void boton_eliminaEmpleado_Click(object sender, EventArgs e)
        {
            eliminacionFisicaEnCascadaEmpleado();
            string comandoSQL = "DELETE FROM PERSONA.EMPLEADO WHERE idEmpleado=" + idEmpleado + "";
            if (empleado.eliminaDatos(comandoSQL) == "Exitoso")
            {
                MessageBox.Show("Se ha eliminado exitosamente");
                limpia_camposEmpleado();
                cargaEmpleado();
                muestraTablas();
            }
            else
                MessageBox.Show("No se pudo eliminar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void boton_nuevoEmpleado_Click(object sender, EventArgs e)
        {
            limpia_camposEmpleado();
            t_nombresEmpleado.Focus();
        }

        private void boton_nuevoCliente_Click(object sender, EventArgs e)
        {
            limpia_camposCliente();
            t_nombresCliente.Focus();
        }

        private void boton_nuevoProveedor_Click(object sender, EventArgs e)
        {
            limpia_camposProveedor();
            t_nombresProveedor.Focus();
        }

        private void boton_guardar_cliente_Click(object sender, EventArgs e)
        {
            if (idCliente == "")
            {
                if (validaCamposClientes())
                {
                    string comandoSQL = "INSERT INTO PERSONA.CLIENTE(nombres, apellidoP, apellidoM, domicilio, telefono, celular, credito) VALUES('" + t_nombresCliente.Text + "', '" + t_apellidoP_cliente.Text + "', '" + t_apellidoM_cliente.Text + "', '" + t_colonia_cliente.Text + '_' + t_calle_cliente.Text + '_' + t_numero_cliente.Text + "', " + t_telefono_cliente.Text + ", " + t_celular_cliente.Text + ", " + t_credito_cliente.Text + ")";
                    if (cliente.insertaDatos(comandoSQL) == "Exitoso")
                    {
                        MessageBox.Show("Se ha insertado exitosamente");
                        limpia_camposCliente();
                        muestraTablas();
                    }
                    else
                        MessageBox.Show("No se pudo insertar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("No se pudo insertar el mismo registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void boton_eliminar_cliente_Click(object sender, EventArgs e)
        {
            string comandoSQL = "DELETE FROM PERSONA.CLIENTE WHERE idCliente=" + idCliente + "";
            if (cliente.eliminaDatos(comandoSQL) == "Exitoso")
            {
                MessageBox.Show("Se ha eliminado exitosamente");
                limpia_camposCliente();
                muestraTablas();
            }
            else
                MessageBox.Show("No se pudo eliminar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void boton_actualizar_cliente_Click(object sender, EventArgs e)
        {

            if (idCliente != "")
            {
                if (validaCamposClientes())
                {
                    string comandoSQL = "UPDATE PERSONA.CLIENTE set nombres=@nombres, apellidoP=@apellidoP, apellidoM=@apellidoM, domicilio=@domicilio, telefono=@telefono, celular=@celular, credito=@credito WHERE idCliente = " + idCliente;
                    if (cliente.actualizaDatos(comandoSQL, dameCamposCliente()) == "Exitoso")
                    {
                        MessageBox.Show("Se ha actualizado exitosamente");
                        limpia_camposCliente();
                        muestraTablas();
                    }
                    else
                        MessageBox.Show("No se pudo actualizar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No hay registro por actualizar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void eliminacionFisicaCascadaProveedor()
        {
            limpia_camposMaterial();
            //Eliminacion física en cascada de Proveedor ----
            string comandoSQL = "DELETE FROM INSUMO.MATERIAL WHERE idProveedor=" + idProveedor + "";
            if (material.eliminaDatos(comandoSQL) != "Exitoso")
                MessageBox.Show("No se pudieron eliminar todos los registros");
            //----------------------------------------------
        }

        private void boton_eliminaProveedor_Click(object sender, EventArgs e)
        {
            eliminacionFisicaCascadaProveedor();
            string comandoSQL = "DELETE FROM PERSONA.PROVEEDOR WHERE idProveedor=" + idProveedor + "";
            if (proveedor.eliminaDatos(comandoSQL) == "Exitoso")
            {
                MessageBox.Show("Se ha eliminado exitosamente");
                limpia_camposProveedor();
                cargaProveedores();
                muestraTablas();
            }
            else
                MessageBox.Show("No se pudo eliminar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        

        private void boton_guardarProveedor_Click(object sender, EventArgs e)
        {
            if (idProveedor == "")
            {
                if (validaCamposProveedores())
                {
                    string comandoSQL = "INSERT INTO PERSONA.PROVEEDOR(nombres, apellidoP, apellidoM, domicilio, telefono, celular, email) VALUES('" + t_nombresProveedor.Text + "', '" + t_apellidoP_proveedor.Text + "', '" + t_apellidoM_proveedor.Text + "', '" + t_colonia_proveedor.Text + '_' + t_calle_proveedor.Text + '_' + t_numero_proveedor.Text + "', " + t_telefono_proveedor.Text + ", " + t_celular_proveedor.Text + ", '" + t_email_proveedor.Text + "')";
                    if (proveedor.insertaDatos(comandoSQL) == "Exitoso")
                    {
                        MessageBox.Show("Se ha insertado exitosamente");
                        limpia_camposProveedor();
                        muestraTablas();
                        cargaProveedores();
                    }
                    else
                        MessageBox.Show("No se pudo insertar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("No se pudo insertar el mismo registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void boton_actualizarProveedor_Click(object sender, EventArgs e)
        {
            if (idProveedor != "")
            {
                if (validaCamposProveedores())
                {
                    string comandoSQL = "UPDATE PERSONA.PROVEEDOR set nombres=@nombres, apellidoP=@apellidoP, apellidoM=@apellidoM, domicilio=@domicilio, telefono=@telefono, celular=@celular, email=@email WHERE idProveedor = " + idProveedor;
                    if (proveedor.actualizaDatos(comandoSQL, dameCamposProveedor()) == "Exitoso")
                    {
                        MessageBox.Show("Se ha actualizado exitosamente");
                        limpia_camposProveedor();
                        muestraTablas();
                        cargaProveedores();
                    }
                    else
                        MessageBox.Show("No se pudo actualizar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No hay registro por actualizar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void comboProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboProveedores.SelectedItem != null && comboProveedores.SelectedItem != "")
            {
                string idProveedores = "";
                string[] nombreCompleto = comboProveedores.SelectedItem.ToString().Split('_');
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();
                string SQL = "Select * FROM PERSONA.PROVEEDOR WHERE nombres='" + nombreCompleto[0] + "' AND apellidoP='" + nombreCompleto[1] + "' AND apellidoM='" + nombreCompleto[2]+"'";
                SqlCommand sqlCmd = new SqlCommand(SQL, conexion);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                

                if(reader.HasRows)
                    while(reader.Read())
                        idProveedores = reader[0].ToString();
                reader.Close();
                conexion.Close();

                t_MidProveedor.Text = idProveedores;
            }
        }

        private void boton_nuevoMaterial_Click(object sender, EventArgs e)
        {
            limpia_camposMaterial();
            t_nombreMaterial.Focus();
        }

        public void limpia_camposMaterial()
        {
            t_idMaterial.Clear();
            t_MidProveedor.Clear();
            t_nombreMaterial.Clear();
            t_descripcion_material.Clear();
            t_disponibles.Value = 0;
            t_precio_compra.Clear();
            t_precio_venta.Clear();
            comboProveedores.Items.Clear();
            cargaProveedores();

            idMaterial = "";
        }


        private void boton_guardarMaterial_Click(object sender, EventArgs e)
        {
            if (idMaterial == "")
            {
                if (validaCamposMateriales())
                {
                    string comandoSQL = "INSERT INTO INSUMO.MATERIAL(idProveedor,nombre, descripcion, stock, precio_compra, precio_venta) VALUES(" + t_MidProveedor.Text + ",'" + t_nombreMaterial.Text + "','" + t_descripcion_material.Text + "'," + t_disponibles.Value.ToString() + "," + t_precio_compra.Text + "," + t_precio_venta.Text + ")";
                    if (material.insertaDatos(comandoSQL) == "Exitoso")
                    {
                        MessageBox.Show("Se ha insertado exitosamente");
                        limpia_camposMaterial();
                        muestraTablas();
                    }
                    else
                        MessageBox.Show("No se pudo insertar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("No se pudo insertar el mismo registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void boton_actualizarMaterial_Click(object sender, EventArgs e)
        {
            if (idMaterial != "")
            {
                if (validaCamposMateriales())
                {
                    
                    string comandoSQL = "UPDATE INSUMO.MATERIAL set idProveedor=@idProveedor, nombre=@nombre, descripcion=@descripcion, stock=@stock, precio_compra=@precio_compra, precio_venta=@precio_venta WHERE idMaterial = " + idMaterial;
                    if (material.actualizaDatos(comandoSQL, dameCamposMaterial()) == "Exitoso")
                    {
                        MessageBox.Show("Se ha actualizado exitosamente");
                        limpia_camposMaterial();
                        muestraTablas();
                        //cargaMaterial();
                    }
                    else
                        MessageBox.Show("No se pudo actualizar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No hay registro por actualizar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void boton_eliminarMaterial_Click(object sender, EventArgs e)
        {
            string comandoSQL = "DELETE FROM INSUMO.MATERIAL WHERE idMaterial=" + idMaterial + "";
            if (material.eliminaDatos(comandoSQL) == "Exitoso")
            {
                MessageBox.Show("Se ha eliminado exitosamente");
                limpia_camposMaterial();
                muestraTablas();
            }
            else
                MessageBox.Show("No se pudo eliminar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridMateriales_MouseClick(object sender, MouseEventArgs e)
        {
            string nombreCompleto = "";
            if (dataGridMateriales.SelectedCells.Count > 1)
            {
                idMaterial = dataGridMateriales.SelectedRows[0].Cells[0].Value.ToString();
                this.t_idMaterial.Text = dataGridMateriales.SelectedCells[0].Value.ToString();
                this.t_MidProveedor.Text = dataGridMateriales.SelectedCells[1].Value.ToString();

                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();
                string SQL = "Select nombres, apellidoP, apellidoM FROM PERSONA.PROVEEDOR WHERE idProveedor="+t_MidProveedor.Text;
                SqlCommand sqlCmd = new SqlCommand(SQL, conexion);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        nombreCompleto = reader[0].ToString() + '_' + reader[1].ToString() + '_' + reader[2].ToString(); ;
                reader.Close();
                conexion.Close();
                this.comboProveedores.SelectedItem = nombreCompleto;

                this.t_nombreMaterial.Text = dataGridMateriales.SelectedCells[2].Value.ToString();
                this.t_descripcion_material.Text = dataGridMateriales.SelectedCells[3].Value.ToString();
                this.t_disponibles.Value = decimal.Parse(dataGridMateriales.SelectedCells[4].Value.ToString());
                this.t_precio_compra.Text = dataGridMateriales.SelectedCells[5].Value.ToString();
                this.t_precio_venta.Text = dataGridMateriales.SelectedCells[6].Value.ToString();
            }
        }

        private void limpia_camposServicioEmpleado()
        {
            textIdEmpleado.Clear();
            cargaEmpleado();
        }

        public void limpia_camposServicio()
        {
            textIdServicios.Clear();
            textCostoServicios.Clear();
            textDescripcionServicios.Clear();
            cargaServicios();
            //La parte de servicio_empleado
            limpia_camposServicioEmpleado();

            idServicio = "";
        }

        private void guardarServicio_Click(object sender, EventArgs e)
        {
            if (idServicio == "")
            {
                if (validaCamposServicios())
                {
                    string comandoSQL = "INSERT INTO ADMINISTRACION.SERVICIO(tipoServicio,descripcion, costo) VALUES('" + comboBoxServicios.SelectedItem + "','" + textDescripcionServicios.Text + "'," + textCostoServicios.Text + ")";
                    if (servicio.insertaDatos(comandoSQL) == "Exitoso")
                    {
                        MessageBox.Show("Se ha insertado exitosamente");
                        limpia_camposServicio();
                        muestraTablas();
                    }
                    else
                        MessageBox.Show("No se pudo insertar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se pudo insertar el mismo registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void actualizarServicio_Click(object sender, EventArgs e)
        {
            if (idServicio != "")
            {
                if (validaCamposServicios())
                {
                    string comandoSQL = "UPDATE ADMINISTRACION.SERVICIO set tipoServicio=@tipoServicio, descripcion=@descripcion, costo=@costo WHERE idServicio = " + idServicio;
                    if (servicio.actualizaDatos(comandoSQL, dameCamposServicios()) == "Exitoso")
                    {
                        MessageBox.Show("Se ha actualizado exitosamente");
                        limpia_camposServicio();
                        muestraTablas();
                    }
                    else
                        MessageBox.Show("No se pudo actualizar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No hay registro por actualizar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void eliminarServicio_Click(object sender, EventArgs e)
        {
            string comandoSQL = "DELETE FROM ADMINISTRACION.SERVICIO WHERE idServicio=" + idServicio + "";
            if (servicio.eliminaDatos(comandoSQL) == "Exitoso")
            {
                MessageBox.Show("Se ha eliminado exitosamente");
                limpia_camposServicio();
                muestraTablas();
                idServicio = "";
            }
            else
                MessageBox.Show("No se pudo eliminar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void botLimpiaServicios_Click(object sender, EventArgs e)
        {
            limpia_camposServicio();
            comboBoxServicios.Focus();

            while (dataGridServicioEmpleado.Rows.Count > 0)
                dataGridServicioEmpleado.Rows.RemoveAt(0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboEmpleados.SelectedItem != null)
            {
                string idEmpleado = "";
                string[] nombreCompleto = comboEmpleados.SelectedItem.ToString().Split('_');
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();
                string SQL = "Select * FROM PERSONA.EMPLEADO WHERE nombres='" + nombreCompleto[0] + "' AND apellidoP='" + nombreCompleto[1] + "' AND apellidoM='" + nombreCompleto[2] + "'";
                SqlCommand sqlCmd = new SqlCommand(SQL, conexion);
                SqlDataReader reader = sqlCmd.ExecuteReader();


                if (reader.HasRows)
                    while (reader.Read())
                        idEmpleado = reader[0].ToString();
                reader.Close();
                conexion.Close();

                textIdEmpleado.Text = idEmpleado;
            }
        }

        private void dataGridServicios_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridServicios.SelectedCells.Count > 1)
            {
                idServicio = dataGridServicios.SelectedRows[0].Cells[0].Value.ToString();

                this.textIdServicios.Text = dataGridServicios.SelectedCells[0].Value.ToString();
                this.comboBoxServicios.Text = dataGridServicios.SelectedCells[3].Value.ToString();
                this.textDescripcionServicios.Text = dataGridServicios.SelectedCells[1].Value.ToString();
                this.textCostoServicios.Text = dataGridServicios.SelectedCells[2].Value.ToString();

                dataGridServicioEmpleado.DataSource = servicioEmpleado.obtenerDatos(idServicio);
            }
        }


        private void agregaEmpleado_servicio_Click(object sender, EventArgs e)
        {
            if (idServicio != "")
            {
                if (comboEmpleados.SelectedItem!=null && comboEmpleados.SelectedItem.ToString()!="")
                {
                    if (!servicioEmpleado.existeEmpleado(idServicio, textIdEmpleado.Text))
                    {
                        string comandoSQL = "INSERT INTO ADMINISTRACION.SERVICIO_EMPLEADO(idServicio, idEmpleado) VALUES(" + textIdServicios.Text + ", " + textIdEmpleado.Text + ")";
                        if (servicioEmpleado.insertaDatos(comandoSQL) == "Exitoso")
                        {
                            MessageBox.Show("Se ha insertado exitosamente");
                            limpia_camposServicioEmpleado();
                            muestraTablas();
                        }
                        else
                            MessageBox.Show("No se pudo insertar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Ya se ha registrado el empleado anteriormente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Seleccione un empleado de la lista", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningun servicio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        


        private void eliminaEmpleado_servicio_Click(object sender, EventArgs e)
        {
            if (idServicio != "")
            {
                if (idEmpleado != "")
                {
                    string comandoSQL = "DELETE FROM ADMINISTRACION.SERVICIO_EMPLEADO WHERE idServicio=" + idServicio + "AND idEmpleado=" + idEmpleado;
                    if (servicioEmpleado.eliminaDatos(comandoSQL) == "Exitoso")
                    {
                        MessageBox.Show("Se ha eliminado exitosamente");
                        limpia_camposServicioEmpleado();
                        muestraTablas();
                        idEmpleado = "";
                    }
                    else
                        MessageBox.Show("No se pudo eliminar el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("No se ha seleccionado ningun empleado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No se ha seleccionado ningun servicio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridServicioEmpleado_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridServicioEmpleado.SelectedCells.Count > 1)
            {
                string[] nombreCompleto = dataGridServicioEmpleado.SelectedCells[1].Value.ToString().Split('_');
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();
                string SQL = "Select * FROM PERSONA.EMPLEADO WHERE nombres='" + nombreCompleto[0] + "' AND apellidoP='" + nombreCompleto[1] + "' AND apellidoM='" + nombreCompleto[2] + "'";
                SqlCommand sqlCmd = new SqlCommand(SQL, conexion);
                SqlDataReader reader = sqlCmd.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        idEmpleado = reader[0].ToString();
                reader.Close();
                conexion.Close();
            }
        }

        private void comboClientes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboClientes_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboClientes.SelectedItem != null && comboClientes.SelectedItem != "")
            {
                string[] nombreCompleto = comboClientes.SelectedItem.ToString().Split('_');
                if (!recuperaIDcliente(nombreCompleto))
                        MessageBox.Show("Error al intentar recuperar: 'idcliente'");
            }
        }

        private bool recuperaIDcliente(string[] nombreCompleto)
        {
            string idClientes = "";
            try
            {
                //Lectura READER DESDE SQLSERVER
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM PERSONA.CLIENTE WHERE nombres='" + nombreCompleto[0] + "' AND apellidop='" + nombreCompleto[1] + "' AND apellidom='" + nombreCompleto[2] + "'", conexion);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                        idClientes = reader[0].ToString();

                reader.Close();
                conexion.Close();
                if (idClientes != "")
                {
                    t_idClientePedido.Text = idClientes;
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private void boton_generarPedido_Click(object sender, EventArgs e)
        {
            if (idPedido == "")
            {
                if (validaCamposPedido())
                {
                    string comandoSQL = "INSERT INTO ADMINISTRACION.PEDIDO(idCliente, fechaPedido, fechaEntrega, total, cargoServicio) VALUES(" + t_idClientePedido.Text + ",'" + t_fechaPedido.Value.ToString("yyyy-MM-dd") + "','" + t_fechaEntrega.Value.ToString("yyyy-MM-dd") + "',0,0)";
                    if (pedido.insertaDatos(comandoSQL) == "Exitoso")
                    {
                        MessageBox.Show("Se ha insertado exitosamente");
                        limpia_camposPedido();
                        muestraTablas();
                    }
                    else
                        MessageBox.Show("No se pudo insertar correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Favor de completar correctamente todos los campos", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
                MessageBox.Show("No se pudo insertar el mismo registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool validaCamposPedido()
        {
            float f;
            if (string.IsNullOrEmpty(t_idClientePedido.Text))
                return false;
            if (t_fechaEntrega.Value < t_fechaPedido.Value)
                return false;
            return true;
        }

        private void comboMateriales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboMateriales.SelectedItem != null && comboMateriales.SelectedItem != "")
            {
                string nombre = comboMateriales.SelectedItem.ToString();
                if (!recuperaIDmaterial(nombre))
                    MessageBox.Show("No se pudo recuperar los datos del material");
            }
        }

        private bool recuperaIDmaterial(string nombreCompleto)
        {
            string idMateriales = "";
            try
            {
                //Lectura READER DESDE SQLSERVER
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM INSUMO.MATERIAL WHERE nombre='" + nombreCompleto + "'", conexion);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                        idMateriales = reader[0].ToString();

                    reader.Close();
                    conexion.Close();
                    t_idMaterial_pedido.Text = idMateriales;
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private bool recuperaIDservicio(string descripcion)
        {
            string idServicios = "";
            try
            {
                //Lectura READER DESDE SQLSERVER
                SqlConnection conexion = new SqlConnection(datosConexion);
                conexion.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM ADMINISTRACION.SERVICIO WHERE descripcion='" + descripcion + "'", conexion);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                        idServicios = reader[0].ToString();

                    reader.Close();
                    conexion.Close();
                    t_idServicio_pedido.Text = idServicios;
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private void crearPedido_Click(object sender, EventArgs e)
        {
            limpia_camposPedido();
            comboClientes.Focus();
            activa_controles_pedido();
        }

        public void activa_controles_pedido()
        {
            comboClientes.Enabled = true;
            t_fechaEntrega.Enabled = true;
        }

        public void desactiva_controles_pedido()
        {
            comboClientes.Enabled = false;
            t_fechaEntrega.Enabled = false;
        }

        public void limpia_camposPedido()
        {
            t_idPedido.Clear();
            t_idClientePedido.Clear();
            t_fechaPedido.Value = DateTime.Now;
            t_fechaEntrega.Value = DateTime.Now;
            t_idMaterial_pedido.Clear();
            t_idServicio_pedido.Clear();
            t_cantidadMaterial.Value = 0;


            cargaClientes(); //Limpia el combobox y los genera nuevamente
            cargaMaterialesPedido(); //Limpia el combobox y los genera nuevamente
            cargaServiciosPedido(); //Limpia el combobox y los genera nuevamente

            t_total.Clear();

            idPedido = "";
            obtenerDatosPedidosMateriales();
            obtenerDatosPedidosServicios();
        }

        public void obtenerDatosPedidosMateriales()
        {
            while (dataGridMateriales_pedido.Rows.Count > 0)
                dataGridMateriales_pedido.Rows.RemoveAt(0);
            dataGridMateriales_pedido.DataSource = detallePedido.obtenerMateriales(idPedido);
        }

        public void obtenerDatosPedidosServicios()
        {
            while (dataGridServicios_pedido.Rows.Count > 0)
                dataGridServicios_pedido.Rows.RemoveAt(0);
            dataGridServicios_pedido.DataSource = detallePedido.obtenerServicios(idPedido);
        }

        private void dataGridPedidos_MouseClick(object sender, MouseEventArgs e)
        {
            idPedido = dataGridPedidos.SelectedRows[0].Cells[0].Value.ToString();
            idCliente = dataGridPedidos.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridPedidos.SelectedCells.Count > 1)
            {
                desactiva_controles_pedido();
                t_idPedido.Text = idPedido;
                if (!recuperaNOMBREcliente())
                {
                    MessageBox.Show("No se pudo recuperar los datos del cliente");
                    t_idClientePedido.Clear();
                    cargaClientes();
                }

                t_fechaPedido.Text = dataGridPedidos.SelectedRows[0].Cells[2].Value.ToString();
                t_fechaEntrega.Text = dataGridPedidos.SelectedRows[0].Cells[3].Value.ToString();
                t_total.Text = dataGridPedidos.SelectedRows[0].Cells[4].Value.ToString();
                cargaMaterialesPedido();
                cargaServiciosPedido();
                obtenerDatosPedidosMateriales();
                obtenerDatosPedidosServicios();
                //actualizaTotal();
            }
        }

        public bool recuperaNOMBREcliente()
        {
            //string idCliente = dataGridPedidos.SelectedRows[0].Cells[1].Value.ToString();
            SqlConnection conexion = new SqlConnection(datosConexion);
            try
            {
                conexion.Open();
                string SQL = "Select nombres, apellidop, apellidom FROM PERSONA.CLIENTE WHERE idcliente='" + idCliente +"'";
                SqlCommand sqlCmd = new SqlCommand(SQL, conexion);
                SqlDataReader reader = sqlCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        t_idClientePedido.Text = idCliente;
                        comboClientes.Text = reader[0].ToString() + '_' + reader[1].ToString() + '_' + reader[2].ToString();
                    }
                    reader.Close();
                    conexion.Close();
                    return true;
                }
                else
                    return false;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        private void boton_agregarMaterial_pedido_Click(object sender, EventArgs e)
        {
            if (t_idPedido.Text != "")
            {
                if (comboMateriales.SelectedItem != null && comboMateriales.SelectedItem.ToString() != "" && t_cantidadMaterial.Value > 0)
                {
                    string comandoSQL = "INSERT INTO ADMINISTRACION.DETALLE_PEDIDO(idpedido, idmaterial, cantidad, idservicio) VALUES(" + idPedido + ", " + t_idMaterial_pedido.Text+ ", " + t_cantidadMaterial.Value.ToString() + ", 1)";
                    try
                    {
                        SqlConnection conexion = new SqlConnection(datosConexion);
                        conexion.Open();

                        SqlCommand cmd = new SqlCommand(comandoSQL, conexion);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            conexion.Close();
                            muestraTablas();
                        }
                        catch (SqlException ex)
                        {
                            conexion.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                    MessageBox.Show("Seleccione un material y una cantidad", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No se ha seleccionado ningun pedido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void boton_eliminarMaterial_pedido_Click(object sender, EventArgs e)
        {
            if (t_idPedido.Text != "")
            {
                if (t_idMaterial_pedido.Text != "")
                {
                    string comandoSQL = "DELETE FROM ADMINISTRACION.DETALLE_PEDIDO WHERE idMaterial="+t_idMaterial_pedido.Text+" AND idPedido="+t_idPedido.Text;
                    try
                    {
                        SqlConnection conexion = new SqlConnection(datosConexion);
                        conexion.Open();

                        SqlCommand cmd = new SqlCommand(comandoSQL, conexion);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            conexion.Close();
                            muestraTablas();
                        }
                        catch (SqlException ex)
                        {
                            conexion.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                    MessageBox.Show("No ha seleccionado ningun material", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No se ha seleccionado ningun pedido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void comboServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboServicios.SelectedItem != null && comboServicios.SelectedItem != "")
            {
                string descripcion = comboServicios.SelectedItem.ToString();
                if (!recuperaIDservicio(descripcion))
                    MessageBox.Show("No se pudo recuperar los datos del material");
            }
        }

        private void boton_agregarServicio_pedido_Click(object sender, EventArgs e)
        {
            if (t_idPedido.Text != "")
            {
                if (comboServicios.SelectedItem != null && comboServicios.SelectedItem.ToString() != "")
                {
                    string comandoSQL = "INSERT INTO ADMINISTRACION.DETALLE_PEDIDO(idpedido, idmaterial, cantidad, idservicio) VALUES(" + idPedido + ", 1, 0, "+t_idServicio_pedido.Text+")";
                    try
                    {
                        SqlConnection conexion = new SqlConnection(datosConexion);
                        conexion.Open();

                        SqlCommand cmd = new SqlCommand(comandoSQL, conexion);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            conexion.Close();
                            muestraTablas();
                        }
                        catch (SqlException ex)
                        {
                            conexion.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                    MessageBox.Show("Seleccione un servicio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No se ha seleccionado ningun pedido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void boton_eliminarServicio_pedido_Click(object sender, EventArgs e)
        {
            if (t_idPedido.Text != "")
            {
                if (t_idServicio_pedido.Text != "")
                {
                    string comandoSQL = "DELETE FROM ADMINISTRACION.DETALLE_PEDIDO WHERE idServicio=" + t_idServicio_pedido.Text + " AND idPedido=" + t_idPedido.Text;
                    try
                    {
                        SqlConnection conexion = new SqlConnection(datosConexion);
                        conexion.Open();

                        SqlCommand cmd = new SqlCommand(comandoSQL, conexion);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            conexion.Close();
                            muestraTablas();
                        }
                        catch (SqlException ex)
                        {
                            conexion.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                    MessageBox.Show("No ha seleccionado ningun servicio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No se ha seleccionado ningun pedido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridMateriales_pedido_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridMateriales_pedido.SelectedCells.Count > 1)
                this.comboMateriales.SelectedItem = dataGridMateriales_pedido.SelectedRows[0].Cells[1].Value;
        }

        private void dataGridServicios_pedido_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridServicios_pedido.SelectedCells.Count > 1)
                this.comboServicios.SelectedItem = dataGridServicios_pedido.SelectedRows[0].Cells[1].Value;
        }

    }
}
