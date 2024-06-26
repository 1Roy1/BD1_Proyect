﻿using MySql.Data.MySqlClient;
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

namespace WindowsFormsApp1
{
    public partial class VentanaVentas : Form
    {
        public VentanaVentas()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void moduloVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaInventario abrir = new VentanaInventario();
            abrir.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                textBox1.Text = selectedRow.Cells["ID"].Value.ToString();
                textBox2.Text = selectedRow.Cells["Nombre"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Descripcion"].Value.ToString();
                textBox4.Text = selectedRow.Cells["Existencia"].Value.ToString();
                textBox6.Text = selectedRow.Cells["Precio"].Value.ToString();
                textBox7.Text = selectedRow.Cells["Marca"].Value.ToString();
            }
        }

        private DataTable originalDataTable;

        private void VentanaVentas_Load(object sender, EventArgs e)
        {
            CargarDatos();
            originalDataTable = ((DataTable)dataGridView1.DataSource).Copy();
        }

        private void CargarDatos()
        {
            string servidor = "localhost";
            string bd = "proyecto";
            string usuario = "root";
            string password = "root123";
            string puerto = "3306";
            string cadenaConexion = $"server={servidor};port={puerto};user id={usuario};password={password};database={bd};";
            MySqlConnection connection = new MySqlConnection(cadenaConexion);
            try
            {
                connection.Open();

                // Consulta ajustada para evitar duplicados
                string sqlQuery = "SELECT DISTINCT pro.ID, pro.Nombre, pro.descripcion, pro.Existencia, pro.Precio, p.Nombre AS marca " +
                                  "FROM producto pro " +
                                  "INNER JOIN detalle_compras dc ON pro.ID = dc.Producto_ID " +
                                  "INNER JOIN compras c ON dc.compras_ID = c.ID " +
                                  "INNER JOIN proveedores p ON c.proveedores_ID = p.ID GROUP BY pro.ID";
                DataTable dataTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqlQuery, connection);
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

                string sqlQuery2 = "SELECT * FROM venta";
                DataTable dataTable2 = new DataTable();
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(sqlQuery2, connection);
                adapter2.Fill(dataTable2);
                dataGridView2.DataSource = dataTable2;

                string sqlQuery3 = "SELECT id, nombre, apellido FROM clientes";
                DataTable dataTable3 = new DataTable();
                MySqlDataAdapter adapter3 = new MySqlDataAdapter(sqlQuery3, connection);
                adapter3.Fill(dataTable3);
                dataGridView3.DataSource = dataTable3;
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

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 abririmenu = new Form1();
            abririmenu.Show();
            this.Hide();
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            string filtro = toolStripTextBox1.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                // Restaurar la fuente de datos original
                dataGridView1.DataSource = originalDataTable;
            }
            else
            {
                // Filtrar las filas basadas en el nombre
                DataTable filteredDataTable = originalDataTable.Clone();
                foreach (DataRow row in originalDataTable.Rows)
                {
                    if (row["Nombre"].ToString().Contains(filtro))
                    {
                        filteredDataTable.ImportRow(row);
                    }
                }

                // Asignar la nueva DataTable al DataGridView
                dataGridView1.DataSource = filteredDataTable;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection connection;
            string servidor = "localhost";
            string bd = "proyecto";
            string usuario = "root";
            string password = "root123";
            string puerto = "3306";
            string cadenaConexion = $"server={servidor};port={puerto};user id={usuario};password={password};database={bd};";
            connection = new MySqlConnection(cadenaConexion);

            try
            {
                int idcliente = Convert.ToInt32(textBox8.Text);
                connection.Open();
                float TotalFinal = 0;

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.IsNewRow) continue;  // Ignorar la fila de nueva entrada

                    int idVenta = Convert.ToInt32(row.Cells["idventa"].Value);
                    int existencias = Convert.ToInt32(row.Cells["existencias"].Value);
                    float total = Convert.ToSingle(row.Cells["total"].Value);
                    TotalFinal += total;

                    // Consulta para obtener la existencia del producto
                    string sqlQuery2 = "SELECT Existencia FROM producto WHERE ID = @ID LIMIT 1";
                    MySqlCommand cmd2 = new MySqlCommand(sqlQuery2, connection);
                    cmd2.Parameters.AddWithValue("@ID", idVenta);

                    object resultado = cmd2.ExecuteScalar();
                    if (resultado == null || resultado == DBNull.Value)
                    {
                        MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int viejoValor = Convert.ToInt32(resultado);
                    int valorFinal = viejoValor - existencias;

                    if (valorFinal < 0)
                    {
                        MessageBox.Show("No hay suficientes existencias del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Actualizar la existencia del producto
                    string sqlQuery3 = "UPDATE producto SET Existencia = @NuevoValor WHERE ID = @ID";
                    MySqlCommand cmd3 = new MySqlCommand(sqlQuery3, connection);
                    cmd3.Parameters.AddWithValue("@NuevoValor", valorFinal);
                    cmd3.Parameters.AddWithValue("@ID", idVenta);
                    cmd3.ExecuteNonQuery();

                    // Eliminar de la tabla venta
                    string sqlQuery4 = "DELETE FROM venta WHERE idventa = @ID";
                    MySqlCommand cmd4 = new MySqlCommand(sqlQuery4, connection);
                    cmd4.Parameters.AddWithValue("@ID", idVenta);
                    cmd4.ExecuteNonQuery();
                }

                // Insertar en la tabla ventas
                string sqlQuery5 = "INSERT INTO ventas(total, clientes_id) VALUES(@total, @cliente_id)";
                MySqlCommand cmd5 = new MySqlCommand(sqlQuery5, connection);
                cmd5.Parameters.AddWithValue("@total", TotalFinal);
                cmd5.Parameters.AddWithValue("@cliente_id", idcliente);
                int rowsAffected3 = cmd5.ExecuteNonQuery();
                if (rowsAffected3 > 0)
                {
                    MessageBox.Show("Compra realizada exitosamente!");
                }
                else
                {
                    MessageBox.Show("No se encontró el registro con el ID proporcionado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Compra Realizada Exitosamente");
            }
            finally
            {
                connection.Close();
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            CargarDatos();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proveedores abrir = new Proveedores();
            abrir.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection connection;
            string servidor = "localhost";
            string bd = "proyecto";
            string usuario = "root";
            string password = "root123";
            string puerto = "3306";
            string cadenaConexion = "server=" + servidor + ";" + "port=" + puerto + ";" + "user id=" + usuario + ";" + "password=" + password + ";" + "database=" + bd + ";";
            connection = new MySqlConnection(cadenaConexion);

            try
            {
                int id = Convert.ToInt32(textBox1.Text);
                float precio = float.Parse(textBox6.Text);
                int nuevoValor = int.Parse(textBox5.Text);
                int viejoValor = int.Parse(textBox4.Text);
                if (viejoValor <= 0)
                {
                    MessageBox.Show("Se han agotado todas las existencias de este producto.");
                }
                else if (viejoValor < nuevoValor)
                {
                    MessageBox.Show("No se tienen las suficientes existencias de este producto.");
                }
                else
                {
                    connection.Open();

                    string sqlQuery = $"INSERT INTO venta(idventa, nombre, existencias, precio, marca, total) VALUES (@idventa, @nombre, @existencias, @precio, @marca, @total)";

                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@idventa", id);
                    cmd.Parameters.AddWithValue("@nombre", textBox2.Text);
                    cmd.Parameters.AddWithValue("@existencias", nuevoValor);
                    cmd.Parameters.AddWithValue("@precio", precio);
                    cmd.Parameters.AddWithValue("@marca", textBox7.Text);
                    cmd.Parameters.AddWithValue("@total", (precio * nuevoValor));

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se agregaron los productos al carrito exitosamente!");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el registro con el ID proporcionado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            CargarDatos();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NuevoProducto abrir = new NuevoProducto();
            abrir.Show();
            this.Hide();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 a = new Form4();
            a.Show();
            this.Hide();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];

                textBox8.Text = selectedRow.Cells["ID"].Value.ToString();
                textBox9.Text = selectedRow.Cells["Nombre"].Value.ToString();
            }
        }
    }
}
