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
using System.Data.Common;
using System.Transactions;

namespace WindowsFormsApp1
{
    public partial class NuevoProducto : Form
    {
        private MySqlTransaction transaction;
        private MySqlConnection connection;
        private const string logSubFolder = "Bitacora";
        private readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logSubFolder, "transaction_log.txt");
        private bool transaccionactiva = false;
        int lista_id = 0;
        String cadenaConexion = "server=127.0.0.1;port=4001;user id=root;password=;database=proyecto";

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
            
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            

        }

        private void IniciarConexion()
        {
            connection = new MySqlConnection(cadenaConexion);
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SET autocommit = 0;", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la conexión: " + ex.Message);
            }
        }

        private void IniciarTransaccion()
        {
            try
            {
                using (var cmd = new MySqlCommand("START TRANSACTION", connection))
                {
                    cmd.ExecuteNonQuery();
                }
                RegistrarTransaccion("Transacción iniciada.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar la transacción: " + ex.Message);
            }
        }

        private void CargarDatos()
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                IniciarConexion();
            }

            string sqlQuery = @"
           SELECT 
                        p.ID, 
                        p.Nombre, 
                        p.Descripcion, 
                        p.Existencia, 
                        p.Precio, 
                        (SELECT dc.Costo 
                         FROM detalle_compras dc 
                         WHERE dc.Producto_ID = p.ID 
                         ORDER BY dc.Compras_ID DESC 
                         LIMIT 1) AS Costo,
                        ROUND(SUM(c.Total), 2) AS Total 
                    FROM 
                        producto p
                    LEFT JOIN 
                        detalle_compras dc ON p.ID = dc.Producto_ID
                    LEFT JOIN 
                        compras c ON dc.Compras_ID = c.ID
                    GROUP BY 
                        p.ID, 
                        p.Nombre, 
                        p.Descripcion, 
                        p.Existencia, 
                        p.Precio;";

                DataTable dataTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, connection);
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            
            
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

        private void RegistrarTransaccion(string mensaje)
        {
            try
            {
                int numeroTransaccion = 1;

               
                if (File.Exists(logFilePath))
                {
                    var lastLine = File.ReadLines(logFilePath).LastOrDefault();
                    if (lastLine != null)
                    {
                 
                        var lastNumberStr = lastLine.Split(':').FirstOrDefault();
                        if (int.TryParse(lastNumberStr, out int lastNumber))
                        {
                            numeroTransaccion = lastNumber + 1;
                        }
                    }
                }

              
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"{numeroTransaccion}: {DateTime.Now}: {mensaje}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al escribir en el archivo de log: " + ex.Message);
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

            try
            {
                connection.Open();
                string sqlQuery = "SELECT ID, Nombre FROM proveedores WHERE Activo = 1";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                comboBox1.DataSource = dataTable;
                comboBox1.DisplayMember = "Nombre"; // Mostrar el nombre del proveedor
                comboBox1.ValueMember = "ID";       // Usar el ID del proveedor como valor

                comboBox1.SelectedIndex = -1; // Opcional: para que no haya ninguna selección al inicio
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los nombres de los proveedores: " + ex.Message);
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
            if (transaccionactiva)
            {
                using (var cmdRollback = new MySqlCommand("ROLLBACK", connection))
                {
                    cmdRollback.ExecuteNonQuery();
                }
       
                transaccionactiva = false;

            }
            VentanaInventario abrir1 = new VentanaInventario();
            abrir1.Show();
            this.Hide();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (transaccionactiva)
            {
                using (var cmdRollback = new MySqlCommand("ROLLBACK", connection))
                {
                    cmdRollback.ExecuteNonQuery();
                }

                transaccionactiva = false;

            }
            Proveedores abrir1 = new Proveedores();
            abrir1.Show();
            this.Hide();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (transaccionactiva)
            {
                using (var cmdRollback = new MySqlCommand("ROLLBACK", connection))
                {
                    cmdRollback.ExecuteNonQuery();
                }

                transaccionactiva = false;

            }
            Form4 abrir1 = new Form4();
            abrir1.Show();
            this.Hide();
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (transaccionactiva)
            {
                using (var cmdRollback = new MySqlCommand("ROLLBACK", connection))
                {
                    cmdRollback.ExecuteNonQuery();
                }

                transaccionactiva = false;

            }
            VentanaVentas abrir1 = new VentanaVentas();
            abrir1.Show();
            this.Hide();
        }

        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (transaccionactiva)
            {
                using (var cmdRollback = new MySqlCommand("ROLLBACK", connection))
                {
                    cmdRollback.ExecuteNonQuery();
                }
            
                transaccionactiva = false;

            }
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
           
        }

        private void textBox7_KeyPress_1(object sender, KeyPressEventArgs e)
        {
           
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Por favor, seleccione un proveedor.");
                    return;
                }

                int proveedorId = Convert.ToInt32(comboBox1.SelectedValue);

                if (!transaccionactiva)
                {
                    IniciarTransaccion();
                    transaccionactiva = true;
                }

                int cantidad = Convert.ToInt32(textBox4.Text);
                decimal precio = Convert.ToDecimal(textBox5.Text);  
                decimal costo = Convert.ToDecimal(textBox6.Text); 
                decimal costoTotal = cantidad * costo;

                // Verificar si el producto ya existe en la tabla producto
                string sql = "SELECT ID FROM producto WHERE Nombre = @nombre";
                MySqlCommand cmdCheck = new MySqlCommand(sql, connection, transaction);
                cmdCheck.Parameters.AddWithValue("@nombre", textBox2.Text);
                object result = cmdCheck.ExecuteScalar();

                int productoId;

                if (result != null)
                {
                    // El producto ya existe, obtenemos el ID
                    productoId = Convert.ToInt32(result);

                    // Actualizar existencia, descripción, y precio si el producto ya existe
                    sql = "UPDATE producto SET Descripcion = @descripcion, Precio = @precio, Existencia = Existencia + @cantidad WHERE ID = @id";
                    MySqlCommand cmdUpdate = new MySqlCommand(sql, connection, transaction);
                    cmdUpdate.Parameters.AddWithValue("@id", productoId);
                    cmdUpdate.Parameters.AddWithValue("@descripcion", textBox3.Text);
                    cmdUpdate.Parameters.AddWithValue("@precio", precio); // Asegurándonos de actualizar el precio
                    cmdUpdate.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdUpdate.ExecuteNonQuery();
                }
                else
                {
                    // Insertar un nuevo producto si no existe
                    sql = "INSERT INTO producto (Nombre, Descripcion, Precio, Existencia) VALUES (@nombre, @descripcion, @precio, @cantidad)";
                    MySqlCommand cmdInsert = new MySqlCommand(sql, connection, transaction);
                    cmdInsert.Parameters.AddWithValue("@nombre", textBox2.Text);
                    cmdInsert.Parameters.AddWithValue("@descripcion", textBox3.Text);
                    cmdInsert.Parameters.AddWithValue("@precio", precio);
                    cmdInsert.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdInsert.ExecuteNonQuery();

                    // Obtener el ID del producto recién insertado
                    productoId = (int)cmdInsert.LastInsertedId;
                }

                // Insertar en la tabla compras, incluyendo el ID del proveedor
                sql = "INSERT INTO compras (Fecha, Total, proveedores_ID) VALUES (@fecha, @total, @proveedor_id)";
                MySqlCommand cmdCompras = new MySqlCommand(sql, connection, transaction);
                cmdCompras.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmdCompras.Parameters.AddWithValue("@total", costoTotal);
                cmdCompras.Parameters.AddWithValue("@proveedor_id", proveedorId);
                cmdCompras.ExecuteNonQuery();

                long compraId = cmdCompras.LastInsertedId;

                // Insertar en la tabla detalle_compras, actualizando el costo
                sql = "INSERT INTO detalle_compras (Producto_ID, Compras_ID, Cantidad, Costo) VALUES (@producto_id, @compras_id, @cantidad, @costo)";
                MySqlCommand cmdDetalleCompras = new MySqlCommand(sql, connection, transaction);
                cmdDetalleCompras.Parameters.AddWithValue("@producto_id", productoId);
                cmdDetalleCompras.Parameters.AddWithValue("@compras_id", compraId);
                cmdDetalleCompras.Parameters.AddWithValue("@cantidad", cantidad);
                cmdDetalleCompras.Parameters.AddWithValue("@costo", costo); 
                cmdDetalleCompras.ExecuteNonQuery();

                // Refrescar la interfaz de usuario con los datos actualizados
                CargarDatos();

                MessageBox.Show("Producto añadido al carrito de compras.");
            }
            catch (MySqlException ex)
            {
                using (var cmd = new MySqlCommand("ROLLBACK", connection, transaction))
                {
                    cmd.ExecuteNonQuery();
                }
                RegistrarTransaccion("Transacción revertida por error ");
                LimpiarCampos();
                CargarDatos();
                MessageBox.Show("Error de base de datos: " + ex.Message);
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (var cmd = new MySqlCommand("COMMIT", connection, transaction))
                {
                    cmd.ExecuteNonQuery();
                }
                transaccionactiva = false;
                RegistrarTransaccion("Transacción comiteada.");
                MessageBox.Show("Compra Realizada correctamente!!!.");
            }
            catch (MySqlException ex)
            {
                using (var cmd = new MySqlCommand("ROLLBACK", connection, transaction))
                {
                    cmd.ExecuteNonQuery();
                }
                RegistrarTransaccion("Transacción revertida por error ");
                LimpiarCampos();
                CargarDatos();
                MessageBox.Show("Error de base de datos: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection = null;
                }
                LimpiarCampos();
                CargarDatos();
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                
                    using (var cmd = new MySqlCommand("ROLLBACK", connection, transaction))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    transaccionactiva = false; 
                    RegistrarTransaccion("Transacción revertida.");
                    MessageBox.Show("Compras revertidas exitosamente.");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante el rollback: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection = null;
                }
                LimpiarCampos();
                CargarDatos();
            }
        }


    }
}
