using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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


        private void BuscarPorNombre()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    string sqlQuery = "SELECT Clientes.ID, Nombre AS Cliente, Apellido, Telefono, Direccion " +
                                      "FROM proyecto.telefono INNER JOIN clientes on telefono.Clientes_ID = Clientes_ID " +
                                      "INNER JOIN direccion on direccion.Clientes_ID = Clientes.ID WHERE Activo = 1 " +
                                      " AND (Nombre LIKE @Nombre OR Telefono LIKE @Telefono)";
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
            InsertarCliente();
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
    }
}
