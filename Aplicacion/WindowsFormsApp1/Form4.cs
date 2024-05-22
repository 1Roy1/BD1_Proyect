using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        String cadenaConexion = "server=localhost;port=3306;user id=root;password=root123;database=proyecto";
        public Form4()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            // Utiliza 'using' para asegurar que los recursos se liberen correctamente
            string sqlQuery = @"SELECT Clientes.ID, Nombre AS Cliente, Apellido, Telefono, Direccion 
                        FROM proyecto.telefono 
                        INNER JOIN clientes ON telefono.Clientes_ID = Clientes.ID 
                        INNER JOIN direccion ON direccion.Clientes_ID = Clientes.ID 
                        WHERE Activo = 1";

            // Inicializa el DataTable fuera del bloque 'using' para poder acceder a él después
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, connection);
                    adapter.Fill(dataTable);
                }

                // Establece el DataSource del DataGridView fuera del bloque 'using'
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void CargarDatosDelete()
        {
            // Utiliza 'using' para asegurar que los recursos se liberen correctamente
            string sqlQuery = @"SELECT Clientes.ID, Nombre AS Cliente, Apellido, Telefono, Direccion 
                        FROM proyecto.telefono 
                        INNER JOIN clientes ON telefono.Clientes_ID = Clientes.ID 
                        INNER JOIN direccion ON direccion.Clientes_ID = Clientes.ID 
                        WHERE Activo = 0";

            // Inicializa el DataTable fuera del bloque 'using' para poder acceder a él después
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, connection);
                    adapter.Fill(dataTable);
                }

                // Establece el DataSource del DataGridView fuera del bloque 'using'
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Index != -1)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Verificar si la columna "Proveedor" existe antes de acceder a ella
                if (dataGridView1.Columns.Contains("Cliente"))
                {
                    int indexColumna = dataGridView1.Columns["Cliente"].Index;

                    string id = selectedRow.Cells["ID"].Value?.ToString();
                    string nombre = selectedRow.Cells[indexColumna].Value?.ToString();
                    string Apellidos = selectedRow.Cells["Apellido"].Value?.ToString();
                    string direccion = selectedRow.Cells["Direccion"].Value?.ToString();
                    string telefono = selectedRow.Cells["Telefono"].Value?.ToString();

                    // Verificar si los valores son null antes de asignarlos a los TextBoxes
                    textBox6.Text = id ?? "";
                    textBox4.Text = nombre ?? "";
                    textBox2.Text = Apellidos ?? "";
                    textBox3.Text = direccion ?? "";
                    textBox5.Text = telefono ?? "";
                }
                else
                {
                    // Si la columna "Proveedor" no existe, limpiar los TextBoxes
                    textBox6.Text = "";
                    textBox4.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox5.Text = "";
                }
            }
        }


        private void LimpiarCampos()
        {
            textBox6.Clear();
            textBox4.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox5.Clear();
        }

        private void EliminarCliente(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "UPDATE clientes SET Activo = 0 WHERE ID = @ID";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
                CargarDatos(); // Recargar los datos en el DataGridView después de la actualización
                LimpiarCampos(); // Limpiar los campos de texto después de la actualización
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el estado del proveedor: " + ex.Message);
            }
        }

        private void restablecerCliente(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "UPDATE clientes SET Activo = 1 WHERE ID = @ID";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
                CargarDatos(); // Recargar los datos en el DataGridView después de la actualización
                LimpiarCampos(); // Limpiar los campos de texto después de la actualización
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el estado del cliente: " + ex.Message);
            }
        }

        private void InsertarCliente()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();

                    // Consultar el último ID insertado
                    string getLastIdQuery = "SELECT MAX(ID) FROM clientes";
                    MySqlCommand getLastIdCmd = new MySqlCommand(getLastIdQuery, connection);
                    object lastIdObj = getLastIdCmd.ExecuteScalar();

                    int lastId = 0;
                    if (lastIdObj != null && lastIdObj != DBNull.Value)
                    {
                        lastId = Convert.ToInt32(lastIdObj);
                    }

                    // Incrementar el último ID para la próxima inserción
                    int nextId = lastId + 1;

                    // Insertar el proveedor
                    string sqlQueryCliente = "INSERT INTO clientes(ID, Nombre, Apellido, Activo) VALUES (@ID, @Nombre, @Apellido, @Activo)";
                    MySqlCommand cmdCliente = new MySqlCommand(sqlQueryCliente, connection);
                    cmdCliente.Parameters.AddWithValue("@ID", nextId);
                    cmdCliente.Parameters.AddWithValue("@Nombre", textBox4.Text);
                    cmdCliente.Parameters.AddWithValue("@Apellido", textBox2.Text);
                    cmdCliente.Parameters.AddWithValue("@Activo", 1);
                    cmdCliente.ExecuteNonQuery();
                    

                    // Insertar direccion
                    string sqlQueryDireccion = "INSERT INTO direccion(Direccion, Clientes_ID) VALUES (@Direccion, @ClienteId) ";
                    MySqlCommand cmdDir = new MySqlCommand(sqlQueryDireccion, connection);
                    cmdDir.Parameters.AddWithValue("@Direccion", textBox3.Text);
                    cmdDir.Parameters.AddWithValue("@ClienteId", nextId);
                    cmdDir.ExecuteNonQuery();

                    // Insertar el teléfono
                    string sqlQueryTelefono = "INSERT INTO telefono(Telefono, proveedores_ID, Clientes_ID) VALUES (@Telefono, NULL, @ClienteId)";
                    MySqlCommand cmdTelefono = new MySqlCommand(sqlQueryTelefono, connection);
                    cmdTelefono.Parameters.AddWithValue("@Telefono", textBox5.Text);
                    cmdTelefono.Parameters.AddWithValue("@ClienteId", nextId);
                    cmdTelefono.ExecuteNonQuery();


                }
                CargarDatos(); // Recargar los datos en el DataGridView después de la inserción
                LimpiarCampos(); // Limpiar los campos de texto después de la inserción
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el proveedor: " + ex.Message);
            }
        }

        private void GuardarCliente()
        {
            DialogResult dialogResult = MessageBox.Show("¿Es un nuevo cliente?", "Confirmar acción", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                InsertarCliente();
            }
            else if (dialogResult == DialogResult.No)
            {
                if (!string.IsNullOrEmpty(textBox6.Text))
                {
                    int id = Convert.ToInt32(textBox6.Text);
                    ActualizarCliente(id);
                }
                else
                {
                    MessageBox.Show("Ingrese un ID para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void ActualizarCliente(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();

                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;

                    try
                    {
                        // Actualizar el cliente
                        cmd.CommandText = "UPDATE clientes SET Nombre = @Nombre, Apellido = @Apellido WHERE ID = @ID";
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@Nombre", textBox4.Text);
                        cmd.Parameters.AddWithValue("@Apellido", textBox2.Text);
                        cmd.ExecuteNonQuery();

                        // Actualizar el teléfono
                        cmd.CommandText = "UPDATE telefono SET Telefono = @Telefono WHERE Clientes_ID = @ClienteId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Telefono", textBox5.Text);
                        cmd.Parameters.AddWithValue("@ClienteId", id);
                        cmd.ExecuteNonQuery();

                        // Actualizar la dirección
                        cmd.CommandText = "UPDATE direccion SET Direccion = @Direccion WHERE Clientes_ID = @ClienteId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Direccion", textBox3.Text);
                        cmd.Parameters.AddWithValue("@ClienteId", id);
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                CargarDatos();
                LimpiarCampos();
            }
            catch (MySqlException exSql)
            {
                MessageBox.Show("Error en la base de datos: " + exSql.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el cliente: " + ex.Message);
            }
        }

        private void BuscarPorNombre()
        {
            // Utiliza 'using' para asegurar que los recursos se liberen correctamente
            string consulta = @"SELECT Clientes.ID, Nombre AS Cliente, Apellido, Telefono, Direccion 
                        FROM proyecto.telefono 
                        INNER JOIN clientes ON telefono.Clientes_ID = clientes.ID 
                        INNER JOIN direccion ON direccion.Clientes_ID = clientes.ID 
                        WHERE Activo = 1 AND (Nombre LIKE @busqueda OR Telefono LIKE @busqueda)";

            // Encapsula la lógica de conexión y ejecución en un método separado
            DataTable resultado = EjecutarConsulta(consulta, textBox1.Text);
            if (resultado != null)
            {
                dataGridView1.DataSource = resultado;
            }
            else
            {
                MessageBox.Show("Error al buscar el cliente.");
            }
        }

        private void verificarInActivos()
        {
            // Utiliza 'using' para asegurar que los recursos se liberen correctamente
            string consulta = @"SELECT count(*) FROM proyecto.clientes where Activo = 0;";

            // Encapsula la lógica de conexión y ejecución en un método separado
            DataTable resultado = EjecutarConsulta(consulta, textBox1.Text);
            if (resultado != null)
            {
                checkBox1.Checked = false;
            }

        }

        private DataTable EjecutarConsulta(string consulta, string parametroBusqueda)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(consulta, connection))
                    {
                        string parametroLike = "%" + parametroBusqueda + "%";
                        cmd.Parameters.AddWithValue("@busqueda", parametroLike);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar el cliente: " + ex.Message);
                return null;
            }
            return dataTable;
        }

        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 abrir1 = new Form1();
            abrir1.Show();
            this.Hide();
        }

        private void ventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaVentas abrir1 = new VentanaVentas();
            abrir1.Show();
            this.Hide();
        }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaInventario abrir1 = new VentanaInventario();
            abrir1.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox5.TextLength < 8)
            {
                MessageBox.Show("¡Ingrese un numero válido!", "Advertencia", MessageBoxButtons.OK);
            }
            else
            {
                GuardarCliente();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            BuscarPorNombre(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                int id = Convert.ToInt32(textBox6.Text);
                EliminarCliente(id);
            }
            else
            {
                MessageBox.Show("Ingrese un ID para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proveedores abrir1 = new Proveedores();
            abrir1.Show();
            this.Hide();
        }

        private void cleintesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NuevoProducto abrir = new NuevoProducto();
            abrir.Show();
            this.Hide();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length > 8)
            {
                textBox5.Text = textBox5.Text.Substring(0, 8);
                textBox5.SelectionStart = textBox5.Text.Length; // Mover el cursor al final
                MessageBox.Show("¡Solo se permiten 8 dígitos!", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                if (textBox5.Text.Length < 8)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("¡Solo se permiten 8 dígitos!", "Advertencia", MessageBoxButtons.OK);
                }
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("¡Ingrese solo números!", "Advertencia", MessageBoxButtons.OK);
            }
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {

                CargarDatosDelete();
                pictureBox1.BringToFront();
            }
            else
            {
                CargarDatos();
                pictureBox2.BringToFront();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                int id = Convert.ToInt32(textBox6.Text);
                EliminarCliente(id);
            }
            else
            {
                MessageBox.Show("Ingrese un ID para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            verificarInActivos();
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                int id = Convert.ToInt32(textBox6.Text);
                restablecerCliente(id);
            }
            else
            {
                MessageBox.Show("Ingrese un ID para restaurar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
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


                string sqlQuery = @"SELECT Clientes.ID, Nombre AS Cliente, Apellido, Telefono, Direccion 
                        FROM proyecto.telefono 
                        INNER JOIN clientes ON telefono.Clientes_ID = Clientes.ID 
                        INNER JOIN direccion ON direccion.Clientes_ID = Clientes.ID 
                        WHERE Activo = 1";

                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                Document doc = new Document();
                PdfWriter writer;
                using (saveFileDialog)
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
                Paragraph title = new Paragraph("Clientes Activos\n" + nombreMesActual + " " + añoActual + "\n\n");
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                PdfPTable table = new PdfPTable(dataTable.Columns.Count);
                table.WidthPercentage = 100;
                foreach (DataColumn column in dataTable.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName));
                    table.AddCell(cell);
                }
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (object item in row.ItemArray)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(item.ToString()));
                        table.AddCell(cell);
                    }
                }

                doc.Add(table);
                Paragraph Date = new Paragraph("\nGenerado: " + fecha);
                Date.Alignment = Element.ALIGN_LEFT;
                doc.Add(Date);
                doc.Close();
                MessageBox.Show("El archivo se guardo en '" + saveFileDialog.FileName + "'");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el archivo: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        
    }
    }
}

