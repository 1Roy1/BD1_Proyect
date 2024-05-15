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
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "SELECT Clientes.ID as ID, Nombre, Apellido, Telefono, Direccion" +
                                      "FROM proyecto.telefono INNER JOIN clientes on telefono.Clientes_ID = Clientes_ID" +
                                      "INNER JOIN direccion on direccion.Clientes_ID = Clientes.ID";
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
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
}
