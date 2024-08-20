using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace WindowsFormsApp1
{
    public partial class NuevoProducto : Form
    {
        int lista_id = 0;
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo números enteros.", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo números enteros o decimales.", "Advertencia", MessageBoxButtons.OK);
            }
            // Permitir solo un punto decimal
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo un punto decimal.", "Advertencia", MessageBoxButtons.OK);
            }

        }

        private void CargarDatos()
        {
            MySqlConnection connection = new MySqlConnection(cadenaConexion);
            try
            {
                connection.Open();

                // Consulta ajustada para evitar duplicados
                string sqlQuery = "SELECT * from lista_compras";
                DataTable dataTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, connection);
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
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

        private void NuevoProducto_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo números enteros o decimales.", "Advertencia", MessageBoxButtons.OK);
            }
           
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo un punto decimal.", "Advertencia", MessageBoxButtons.OK);
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
            DialogResult result = MessageBox.Show("¿Estás seguro de limpiar los campos?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox9.Clear();

                if (comboBox1.SelectedIndex != -1)
                {
                    comboBox1.SelectedIndex = -1;
                }
            }
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
            Form4 abrir1 = new Form4();
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

        private void imprimircompras_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(cadenaConexion);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            // obtener mes y año
            string nombreMesActual = DateTime.Now.ToString("MMMM", CultureInfo.CurrentCulture);
            int añoActual = DateTime.Now.Year;
            string fecha = DateTime.Now.ToString();

            try
            {
                connection.Open();

                // Consulta SQL con INNER JOIN para obtener la información requerida
                string sqlQuery = @"SELECT c.Fecha, c.Total, p.Nombre AS NombreProveedor, dc.Cantidad, dc.Costo, pr.Nombre AS NombreProducto
                            FROM compras c
                            INNER JOIN detalle_compras dc ON c.ID = dc.compras_ID
                            INNER JOIN producto pr ON dc.Producto_ID = pr.ID
                            INNER JOIN proveedores p ON c.proveedores_ID = p.ID";

                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Crear el documento PDF
                Document doc = new Document();
                PdfWriter writer;

                // Iniciar el SaveFileDialog para seleccionar dónde guardar el PDF
                using (saveFileDialog )
                {
                    saveFileDialog.Filter = "PDF Files|*.pdf";
                    saveFileDialog.DefaultExt = "pdf";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        writer = PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    }
                    else
                    {
                        throw new Exception("Operación cancelada por el usuario.");
                    }
                }

                doc.Open();

                // Agregar título al documento
                Paragraph title = new Paragraph("Historial de Compras\n" +nombreMesActual+" "+añoActual+"\n\n");
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                // Crear tabla para mostrar los datos
                PdfPTable table = new PdfPTable(dataTable.Columns.Count);
                table.WidthPercentage = 100;

                // Agregar encabezados de columnas
                foreach (DataColumn column in dataTable.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName));
                    table.AddCell(cell);
                }

                // Agregar filas con datos
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (object item in row.ItemArray)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(item.ToString()));
                        table.AddCell(cell);
                    }
                }

                doc.Add(table);

                // AGregar fecha de generacion
                Paragraph Date = new Paragraph("\nGenerado: " + fecha);
                Date.Alignment = Element.ALIGN_LEFT;
                doc.Add(Date);

                doc.Close();

                MessageBox.Show("Historial de compras se guardo en '" + saveFileDialog.FileName + "'");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el historial de compras: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void textBox7_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo números enteros o decimales.", "Advertencia", MessageBoxButtons.OK);
            }
          
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo un punto decimal.", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(cadenaConexion);

            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand("InsertarCompraCompleta", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Verificar si se ha seleccionado un proveedor
                if (comboBox1.SelectedItem == null)
                {
                    // Manejar el caso cuando no se ha seleccionado ningún proveedor
                    MessageBox.Show("No se ha seleccionado ningún proveedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Salir del método sin continuar con la ejecución
                }

                string nombreProducto;
                string descripcionProducto;

                // Verificar si se ha seleccionado un producto existente
                if (comboBox2.SelectedItem != null && !string.IsNullOrWhiteSpace(comboBox2.SelectedItem.ToString()))
                {
                    // Se seleccionó un producto existente en comboBox2
                    nombreProducto = comboBox2.SelectedItem.ToString();
                    descripcionProducto = textBox3.Text;
                }
                else
                {
                    // Es un nuevo producto, por lo tanto, insertar el nombre y la descripción en la tabla producto
                    nombreProducto = textBox2.Text;
                    descripcionProducto = textBox3.Text;

                    // Insertar el nuevo producto
                    int nuevoProductoID = InsertarProducto(connection, nombreProducto, descripcionProducto, textBox5.Text, Convert.ToInt32(textBox4.Text), Convert.ToDecimal(textBox6.Text));
                    if (nuevoProductoID == -1)
                    {
                        // Ocurrió un error al insertar el nuevo producto
                        MessageBox.Show("Error al insertar el nuevo producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Pasar los parámetros del procedimiento almacenado
                cmd.Parameters.AddWithValue("@p_nombreProveedor", comboBox1.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@p_nombreProducto", nombreProducto);
                cmd.Parameters.AddWithValue("@p_descripcionProducto", descripcionProducto);
                cmd.Parameters.AddWithValue("@p_fecha", textBox9.Text);
                cmd.Parameters.AddWithValue("@p_total", Convert.ToDecimal(textBox7.Text));
                cmd.Parameters.AddWithValue("@p_precioProducto", Convert.ToDecimal(textBox5.Text));
                cmd.Parameters.AddWithValue("@p_cantidadProducto", Convert.ToInt32(textBox4.Text));
                cmd.Parameters.AddWithValue("@p_costoProducto", Convert.ToDecimal(textBox6.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Compra Realizada correctamente!");
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Error: " + ex.Message); // Comentado para evitar mostrar el mensaje de error
            }
            finally
            {
                connection.Close();
            }
        }

        private void LimpiarCampos()
        {
            DialogResult result = MessageBox.Show("¿Estás seguro de limpiar los campos?", "Confirmación",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Limpia los campos de texto
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox9.Clear();
                // comboBox1.SelectedIndex = -1;
                // comboBox2.SelectedIndex = -1;

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(cadenaConexion);
            connection.Open();
            MySqlCommand cmdInsertProducto = new MySqlCommand("INSERT INTO lista_compras (id, Nombre, Descripcion, Precio, costo, total, cantidad) VALUES (@id, @nombre, @descripcion, @precio, @costo, @total, @cantidad)", connection);
            if (textBox1.Text is null)
            {
                //temporal se cambiara luego
                lista_id += 1;
                cmdInsertProducto.Parameters.AddWithValue("@id", lista_id);
            }
            else
            {
                cmdInsertProducto.Parameters.AddWithValue("@id", Convert.ToInt32(textBox1.Text));
            }
            cmdInsertProducto.Parameters.AddWithValue("@nombre", textBox2.Text);
            cmdInsertProducto.Parameters.AddWithValue("@descripcion", textBox3.Text);
            cmdInsertProducto.Parameters.AddWithValue("@cantidad", Convert.ToInt32(textBox4.Text));
            cmdInsertProducto.Parameters.AddWithValue("@precio", Convert.ToDecimal(textBox5.Text));
            cmdInsertProducto.Parameters.AddWithValue("@costo", Convert.ToDecimal(textBox6.Text));
            cmdInsertProducto.Parameters.AddWithValue("@total", Convert.ToDecimal(textBox7.Text));
            cmdInsertProducto.ExecuteNonQuery();
            connection.Close();
            CargarDatos();
        }
    }
}
