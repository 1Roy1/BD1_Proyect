﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class NuevoProducto : Form
    {
        string cadenaConexion = "server=localhost;port=3306;user id=root;password=Rod2102777;database=proyecto";
        public NuevoProducto()
        {
            InitializeComponent();
            CargarNombresProveedores();
            CargarNombresProductos();


        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
    
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false ;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo numeros", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo numeros", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar) || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo numeros o punto", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void NuevoProducto_Load(object sender, EventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar) || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo numeros o punto", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(cadenaConexion);

            try
            {
                connection.Open();

                // Obtener el ID del proveedor seleccionado
                string selectedProveedor = comboBox1.SelectedItem.ToString();
                MySqlCommand cmdProveedor = new MySqlCommand("SELECT ID FROM proveedores WHERE Nombre = @nombreProveedor", connection);
                cmdProveedor.Parameters.AddWithValue("@nombreProveedor", selectedProveedor);
                int proveedorID = Convert.ToInt32(cmdProveedor.ExecuteScalar());

                // Obtener la fecha
                string fecha = textBox9.Text;

                // Obtener el total
                decimal total = Convert.ToDecimal(textBox7.Text);

                // Si se seleccionó un producto
                if (!string.IsNullOrEmpty(comboBox2.Text))
                {
                    // Obtener el ID del producto seleccionado
                    string selectedProducto = comboBox2.SelectedItem.ToString();
                    MySqlCommand cmdProducto = new MySqlCommand("SELECT ID, Existencia, Precio FROM producto WHERE Nombre = @nombreProducto", connection);
                    cmdProducto.Parameters.AddWithValue("@nombreProducto", selectedProducto);
                    MySqlDataReader reader = cmdProducto.ExecuteReader();

                    // Si el producto existe en la base de datos
                    if (reader.Read())
                    {
                        int productoID = Convert.ToInt32(reader["ID"]);
                        int existenciaExistente = Convert.ToInt32(reader["Existencia"]);
                        decimal precioProducto = Convert.ToDecimal(reader["Precio"]);
                        reader.Close();

                        // Actualizar el precio del producto si es diferente
                        if (precioProducto != Convert.ToDecimal(textBox5.Text))
                        {
                            MySqlCommand cmdUpdatePrecio = new MySqlCommand("UPDATE producto SET Precio = @precio WHERE ID = @productoID", connection);
                            cmdUpdatePrecio.Parameters.AddWithValue("@precio", Convert.ToDecimal(textBox5.Text));
                            cmdUpdatePrecio.Parameters.AddWithValue("@productoID", productoID);
                            cmdUpdatePrecio.ExecuteNonQuery();
                        }

                        // Actualizar la existencia del producto
                        int nuevaExistencia = existenciaExistente + Convert.ToInt32(textBox4.Text);
                        MySqlCommand cmdUpdateExistencia = new MySqlCommand("UPDATE producto SET Existencia = @existencia WHERE ID = @productoID", connection);
                        cmdUpdateExistencia.Parameters.AddWithValue("@existencia", nuevaExistencia);
                        cmdUpdateExistencia.Parameters.AddWithValue("@productoID", productoID);
                        cmdUpdateExistencia.ExecuteNonQuery();

                        // Insertar en la tabla de compras
                        InsertarCompra(connection, fecha, total, proveedorID);

                        // Obtener el ID de la compra recién insertada
                        MySqlCommand cmdUltimaCompra = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
                        int compraID = Convert.ToInt32(cmdUltimaCompra.ExecuteScalar());

                        // Insertar en la tabla de detalle_compras
                        InsertarDetalleCompra(connection, compraID, productoID, Convert.ToDecimal(textBox6.Text), Convert.ToInt32(textBox4.Text));
                    }
                }
                else
                {
                    // Si no se seleccionó un producto, insertar uno nuevo
                    int nuevoProductoID = InsertarProducto(connection, textBox2.Text, textBox3.Text, textBox5.Text, Convert.ToInt32(textBox4.Text), Convert.ToDecimal(textBox6.Text));
                    InsertarCompra(connection, fecha, total, proveedorID);
                    MySqlCommand cmdUltimaCompra = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
                    int compraID = Convert.ToInt32(cmdUltimaCompra.ExecuteScalar());
                    InsertarDetalleCompra(connection, compraID, nuevoProductoID, Convert.ToDecimal(textBox6.Text), Convert.ToInt32(textBox4.Text));
                }

                MessageBox.Show("Datos actualizados correctamente.");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox9.Clear();
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private int InsertarProducto(MySqlConnection connection, string nombreProducto, string descripcion, string precio, int cantidad, decimal costo)
        {
            MySqlCommand cmdInsertProducto = new MySqlCommand("INSERT INTO producto (Nombre, Descripcion, Existencia, Precio) VALUES (@nombre, @descripcion, @cantidad, @precio)", connection);
            cmdInsertProducto.Parameters.AddWithValue("@nombre", nombreProducto);
            cmdInsertProducto.Parameters.AddWithValue("@descripcion", descripcion);
            cmdInsertProducto.Parameters.AddWithValue("@cantidad", cantidad);
            cmdInsertProducto.Parameters.AddWithValue("@precio", precio);
            cmdInsertProducto.ExecuteNonQuery();

            // Obtener el ID del producto recién insertado
            MySqlCommand cmdUltimoProducto = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
            return Convert.ToInt32(cmdUltimoProducto.ExecuteScalar());
        }

        private void InsertarCompra(MySqlConnection connection, string fecha, decimal total, int proveedorID)
        {
            MySqlCommand cmdCompras = new MySqlCommand("INSERT INTO compras (Fecha, Total, proveedores_ID) VALUES (@fecha, @total, @proveedorID)", connection);
            cmdCompras.Parameters.AddWithValue("@fecha", fecha);
            cmdCompras.Parameters.AddWithValue("@total", total);
            cmdCompras.Parameters.AddWithValue("@proveedorID", proveedorID);
            cmdCompras.ExecuteNonQuery();
        }

        private void InsertarDetalleCompra(MySqlConnection connection, int compraID, int productoID, decimal costo, int cantidad)
        {
            MySqlCommand cmdDetalleCompras = new MySqlCommand("INSERT INTO detalle_compras (compras_ID, Producto_ID, Costo, Cantidad) VALUES (@compraID, @productoID, @costo, @cantidad)", connection);
            cmdDetalleCompras.Parameters.AddWithValue("@compraID", compraID);
            cmdDetalleCompras.Parameters.AddWithValue("@productoID", productoID);
            cmdDetalleCompras.Parameters.AddWithValue("@costo", costo);
            cmdDetalleCompras.Parameters.AddWithValue("@cantidad", cantidad);
            cmdDetalleCompras.ExecuteNonQuery();
        }
        private void CalcularTotal()
        {
            if (!string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrEmpty(textBox4.Text))
            {
                decimal costo = Convert.ToDecimal(textBox6.Text);
                int cantidad = Convert.ToInt32(textBox4.Text);
                decimal total = costo * cantidad;
                textBox7.Text = total.ToString();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void CargarNombresProveedores()
        {
            MySqlConnection connection = new MySqlConnection(cadenaConexion);
            comboBox1.Items.Clear(); // Limpiar los elementos existentes en el ComboBox antes de cargar nuevos

            try
            {
                connection.Open();
                string sqlQuery = "SELECT Nombre FROM proveedores WHERE Activo = 1";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string nombreProveedor = reader["Nombre"].ToString();
                    comboBox1.Items.Add(nombreProveedor); // Agregar el nombre del proveedor al ComboBox
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los nombres de los proveedores: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void CargarNombresProductos()
        {
            MySqlConnection connection = new MySqlConnection(cadenaConexion);
            comboBox2.Items.Clear(); // Limpiar los elementos existentes en el ComboBox antes de cargar nuevos

            try
            {
                connection.Open();
                string sqlQuery = "SELECT Nombre FROM producto";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string nombreProveedor = reader["Nombre"].ToString();
                    comboBox2.Items.Add(nombreProveedor); // Agregar el nombre del proveedor al ComboBox
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los nombres de los proveedores: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            // Obtener la fecha seleccionada del MonthCalendar
            DateTime selectedDate = monthCalendar1.SelectionStart;

            // Obtener la fecha y hora actual
            DateTime currentDateTime = DateTime.Now;

            // Crear una nueva fecha combinando la fecha seleccionada del MonthCalendar con la hora actual
            DateTime combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day,
                                                     currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);

            // Asignar la fecha y hora combinadas al TextBox en el formato deseado
            textBox9.Text = combinedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProduct = comboBox2.SelectedItem.ToString(); // Obtener el nombre del producto seleccionado

            MySqlConnection connection = new MySqlConnection(cadenaConexion);

            try
            {
                connection.Open();
                string sqlQuery = "SELECT ID, Nombre, Descripcion, Precio FROM producto WHERE Nombre = @nombreProducto";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@nombreProducto", selectedProduct);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Rellenar los textBox con la información del producto seleccionado
                    textBox1.Text = reader["ID"].ToString();
                    textBox2.Text = reader["Nombre"].ToString();
                    textBox3.Text = reader["Descripcion"].ToString();
                    textBox5.Text = reader["Precio"].ToString();
                
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la información del producto: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        private void textBox7_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
           
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox9.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

        }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaInventario abrir1 = new VentanaInventario();
            abrir1.Show();
            this.Hide();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proveedores abrir1 = new Proveedores();
            abrir1.Show();
            this.Hide();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 abrir1 = new Form5();
            abrir1.Show();
            this.Hide();
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaVentas abrir1 = new VentanaVentas();
            abrir1.Show();
            this.Hide();
        }

        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 abrir1 = new Form1();
            abrir1.Show();
            this.Hide();
        }
    }
}
