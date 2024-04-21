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

namespace WindowsFormsApp1
{
    public partial class Proveedores : Form
    {
        string cadenaConexion = "server=localhost;port=3306;user id=root;password=root123;database=proyecto";
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
                    string sqlQuery = "SELECT ID, Nombre as Proveedor, Asesor, Telefono " +
                        "FROM proveedores where Activo = 1";
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
        private void InsertarProveedor()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "INSERT INTO proveedores(Nombre, Asesor, Activo, Telefono) VALUES (@Nombre, @Asesor,@Activo, @Telefono)";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    int numero = Convert.ToInt32(textBox5.Text);
                    cmd.Parameters.AddWithValue("@Nombre", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Asesor", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Telefono", numero);
                    cmd.Parameters.AddWithValue("@Activo", 1);
                    cmd.ExecuteNonQuery();
                }
                CargarDatos(); // Recargar los datos en el DataGridView después de la inserción
                LimpiarCampos(); // Limpiar los campos de texto después de la inserción
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el proveedor: " + ex.Message);
            }
        }

        // Método para limpiar los campos de texto
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
                    string sqlQuery = "SELECT ID, Nombre, Asesor, Telefono " +
                        "FROM proveedores WHERE Nombre LIKE @Nombre";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@Nombre", "%" + textBox1.Text + "%");
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
            Document doc = null; // Definir la variable doc fuera del bloque try

            try
            {
                // Crear un documento PDF
                doc = new Document();

                // Abrir un diálogo para que el usuario seleccione la ubicación del archivo PDF
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Archivo PDF|*.pdf";
                saveDialog.Title = "Guardar PDF";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Crear un archivo PDF en la ubicación seleccionada
                    using (FileStream stream = new FileStream(saveDialog.FileName, FileMode.Create))
                    {
                        PdfWriter.GetInstance(doc, stream);
                        doc.Open();

                        // Crear una tabla con los datos del DataGridView
                        PdfPTable pdfTable = new PdfPTable(dataGridView1.ColumnCount);

                        // Añadir las cabeceras de las columnas
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            pdfTable.AddCell(new Phrase(dataGridView1.Columns[j].HeaderText));
                        }

                        // Añadir las filas de datos
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int k = 0; k < dataGridView1.Columns.Count; k++)
                            {
                                if (dataGridView1[k, i].Value != null)
                                {
                                    pdfTable.AddCell(new Phrase(dataGridView1[k, i].Value.ToString()));
                                }
                            }
                        }

                        // Añadir la tabla al documento
                        doc.Add(pdfTable);

                        // Mostrar un mensaje de éxito
                        MessageBox.Show("PDF generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Asegurarse de cerrar el documento incluso si ocurre una excepción
                if (doc != null)
                {
                    doc.Close();
                }
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
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                textBox4.Text = selectedRow.Cells["ID"].Value.ToString();
                textBox2.Text = selectedRow.Cells["Proveedor"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Asesor"].Value.ToString();
                textBox5.Text = selectedRow.Cells["Telefono"].Value.ToString();
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

        }


        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
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
                MessageBox.Show("¡Ingrese solo numeros!", "Advertencia", MessageBoxButtons.OK);
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
            InsertarProveedor();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
