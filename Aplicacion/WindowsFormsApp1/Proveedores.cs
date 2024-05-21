using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Globalization;

namespace WindowsFormsApp1
{
    public partial class Proveedores : Form
    {
        string cadenaConexion = "server=localhost;port=3306;user id=root;password=Rod2102777;database=proyecto";
        public Proveedores()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void EliminarProveedor(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "UPDATE proveedores SET Activo = 0 WHERE ID = @ID";
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

        private void CargarDatos()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "SELECT p.ID, p.Nombre as Proveedor, p.Asesor, t.Telefono " +
                        "FROM proveedores p INNER JOIN telefono t ON p.ID = t.proveedores_ID " +
                        "WHERE p.Activo = 1";
                    DataTable dataTable = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, connection);
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        

        private void GuardarProveedor()
        {
            DialogResult dialogResult = MessageBox.Show("¿Es un nuevo proveedor?", "Confirmar acción", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                InsertarProveedor();
            }
            else if (dialogResult == DialogResult.No)
            {
                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    int id = Convert.ToInt32(textBox4.Text);
                    ActualizarProveedor(id);
                }
                else
                {
                    MessageBox.Show("Ingrese un ID para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void InsertarProveedor()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();

                    // Obtener el ID máximo actual
                    string maxIdQuery = "SELECT COALESCE(MAX(ID), 0) + 1 FROM proveedores";
                    MySqlCommand maxIdCmd = new MySqlCommand(maxIdQuery, connection);
                    int newId = Convert.ToInt32(maxIdCmd.ExecuteScalar());

                    // Verificar si el nombre o el asesor ya existen
                    string checkQuery = "SELECT ID FROM proveedores WHERE Nombre = @Nombre OR Asesor = @Asesor";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@Nombre", textBox2.Text);
                    checkCmd.Parameters.AddWithValue("@Asesor", textBox3.Text);

                    object result = checkCmd.ExecuteScalar();
                    if (result != null)
                    {
                        throw new Exception("El proveedor con ese nombre o asesor ya existe.");
                    }

                    // Insertar el proveedor con el nuevo ID
                    string sqlQueryProveedor = "INSERT INTO proveedores(ID, Nombre, Asesor, Activo) VALUES (@ID, @Nombre, @Asesor, @Activo)";
                    MySqlCommand cmdProveedor = new MySqlCommand(sqlQueryProveedor, connection);
                    cmdProveedor.Parameters.AddWithValue("@ID", newId);
                    cmdProveedor.Parameters.AddWithValue("@Nombre", textBox2.Text);
                    cmdProveedor.Parameters.AddWithValue("@Asesor", textBox3.Text);
                    cmdProveedor.Parameters.AddWithValue("@Activo", 1);
                    cmdProveedor.ExecuteNonQuery();

                    // Insertar el teléfono
                    string sqlQueryTelefono = "INSERT INTO telefono(Telefono, proveedores_ID) VALUES (@Telefono, @ProveedorId)";
                    MySqlCommand cmdTelefono = new MySqlCommand(sqlQueryTelefono, connection);
                    cmdTelefono.Parameters.AddWithValue("@Telefono", textBox5.Text);
                    cmdTelefono.Parameters.AddWithValue("@ProveedorId", newId);
                    cmdTelefono.ExecuteNonQuery();
                }
                CargarDatos();
                LimpiarCampos();
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("El proveedor con ese nombre o asesor ya existe"))
                {
                    DialogResult dialogResult = MessageBox.Show("El proveedor ya existe. ¿Desea actualizar la información del proveedor existente?", "Proveedor existente", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // Buscar el ID del proveedor existente
                        int idProveedorExistente = ObtenerIdProveedorExistente(textBox2.Text, textBox3.Text);
                        if (idProveedorExistente > 0)
                        {
                            ActualizarProveedor(idProveedorExistente);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo encontrar el proveedor existente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error al insertar el proveedor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int ObtenerIdProveedorExistente(string nombre, string asesor)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "SELECT ID FROM proveedores WHERE Nombre = @Nombre OR Asesor = @Asesor";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Asesor", asesor);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el ID del proveedor existente: " + ex.Message);
            }
            return -1;
        }

        private void ActualizarProveedor(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();

                    // Actualizar el proveedor
                    string sqlQueryProveedor = "UPDATE proveedores SET Nombre = @Nombre, Asesor = @Asesor WHERE ID = @ID";
                    MySqlCommand cmdProveedor = new MySqlCommand(sqlQueryProveedor, connection);
                    cmdProveedor.Parameters.AddWithValue("@ID", id);
                    cmdProveedor.Parameters.AddWithValue("@Nombre", textBox2.Text);
                    cmdProveedor.Parameters.AddWithValue("@Asesor", textBox3.Text);
                    cmdProveedor.ExecuteNonQuery();

                    // Actualizar el teléfono
                    string sqlQueryTelefono = "UPDATE telefono SET Telefono = @Telefono WHERE proveedores_ID = @ProveedorId";
                    MySqlCommand cmdTelefono = new MySqlCommand(sqlQueryTelefono, connection);
                    cmdTelefono.Parameters.AddWithValue("@Telefono", textBox5.Text);
                    cmdTelefono.Parameters.AddWithValue("@ProveedorId", id);
                    cmdTelefono.ExecuteNonQuery();
                }
                CargarDatos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el proveedor: " + ex.Message);
            }
        }
        private void LimpiarCampos()
        {
            textBox4.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox5.Clear();
        }
        // Método para buscar un proveedor por nombre
        private void BuscarPorNombre()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "SELECT p.ID, p.Nombre, p.Asesor, t.Telefono " +
                        "FROM proveedores p INNER JOIN telefono t ON p.ID = t.proveedores_ID " +
                        "WHERE p.Activo = 1 AND (p.Nombre LIKE @Nombre OR t.Telefono LIKE @Telefono)";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@Nombre", "%" + textBox1.Text + "%");
                    cmd.Parameters.AddWithValue("@Telefono", "%" + textBox1.Text + "%");
                    DataTable dataTable = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar el proveedor: " + ex.Message);
            }
        }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaInventario abrir1 = new VentanaInventario();
            abrir1.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            BuscarPorNombre();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ventToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            InsertarProveedor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                int id = Convert.ToInt32(textBox4.Text);
                EliminarProveedor(id);
            }
            else
            {
                MessageBox.Show("Ingrese un ID para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
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

                // obtener la información requerida
                string sqlQuery = @"SELECT p.ID, p.Nombre as Proveedor, p.Asesor, t.Telefono " +
                        "FROM proveedores p INNER JOIN telefono t ON p.ID = t.proveedores_ID " +
                        "WHERE p.Activo = 1";

                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Crear el documento PDF
                Document doc = new Document();
                PdfWriter writer;

                // seleccionar dónde guardar el PDF
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

                // Agregar título al documento pdf
                Paragraph title = new Paragraph("Proveedores Activos\n" + nombreMesActual + " " + añoActual + "\n\n");
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

        private void Proveedores_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Index != -1)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Verificar si la columna "Proveedor" existe antes de acceder a ella
                if (dataGridView1.Columns.Contains("Proveedor"))
                {
                    int indexColumna = dataGridView1.Columns["Proveedor"].Index;

                    string id = selectedRow.Cells["ID"].Value?.ToString();
                    string proveedor = selectedRow.Cells[indexColumna].Value?.ToString();
                    string asesor = selectedRow.Cells["Asesor"].Value?.ToString();
                    string telefono = selectedRow.Cells["Telefono"].Value?.ToString();

                    // Verificar si los valores son null antes de asignarlos a los TextBoxes
                    textBox4.Text = id ?? "";
                    textBox2.Text = proveedor ?? "";
                    textBox3.Text = asesor ?? "";
                    textBox5.Text = telefono ?? "";
                }
                else
                {
                    // Si la columna "Proveedor" no existe, limpiar los TextBoxes
                    textBox4.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox5.Text = "";
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar) || e.KeyChar == ' ')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("¡Ingrese solo numeros!", "Advertencia", MessageBoxButtons.OK);
            }
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
                // Permitir solo 8 dígitos
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar) || e.KeyChar == ' ')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("¡Ingrese solo letras!", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GuardarProveedor();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void comprarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NuevoProducto abrir1 = new NuevoProducto();
            abrir1.Show();
            this.Hide();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 abrir1 = new Form4();
            abrir1.Show();
            this.Hide();
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {

        }
    }

}
