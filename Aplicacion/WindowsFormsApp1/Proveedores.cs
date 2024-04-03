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
        string cadenaConexion = "server=localhost;port=3306;user id=root;password=Rod2102777;database=proyecto";
        public Proveedores()
        {
            InitializeComponent();
            CargarDatos();
        }
        private void CargarDatos()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM proveedores";
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
                    string sqlQuery = "INSERT INTO proveedores(ID, Proveedor, Asesor, Numero) VALUES (@ID, @Proveedor, @Asesor, @Numero)";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    int id = Convert.ToInt32(textBox4.Text);
                    int numero = Convert.ToInt32(textBox5.Text);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Proveedor", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Asesor", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Numero", numero);
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

        // Método para eliminar un proveedor de la base de datos
        private void EliminarProveedor()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "DELETE FROM proveedores WHERE ID = @ID";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@ID", textBox4.Text);
                    cmd.ExecuteNonQuery();
                }
                CargarDatos(); // Recargar los datos en el DataGridView después de la eliminación
                LimpiarCampos(); // Limpiar los campos de texto después de la eliminación
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el proveedor: " + ex.Message);
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
                    string sqlQuery = "SELECT * FROM proveedores WHERE Proveedor LIKE @Nombre";
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
            EliminarProveedor();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Crear un documento PDF
            Document doc = new Document();
            try
            {
                // Abrir un diálogo para que el usuario seleccione la ubicación del archivo PDF
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Archivo PDF|*.pdf";
                saveDialog.Title = "Guardar PDF";
                saveDialog.ShowDialog();

                // Si el usuario cancela, sal del método
                if (saveDialog.FileName == "")
                    return;

                // Crear un archivo PDF en la ubicación seleccionada
                PdfWriter.GetInstance(doc, new FileStream(saveDialog.FileName, FileMode.Create));
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

                // Cerrar el documento
                doc.Close();

                // Mostrar un mensaje de éxito
                MessageBox.Show("PDF generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Asegurarse de cerrar el documento incluso si ocurre una excepción
                doc.Close();
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
                textBox5.Text = selectedRow.Cells["Numero"].Value.ToString();
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
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("¡Ingrese solo letras!", "Advertencia", MessageBoxButtons.OK);
            }
        }
    }
}
