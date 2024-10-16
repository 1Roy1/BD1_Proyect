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

namespace WindowsFormsApp1
{
    public partial class VentanaInventario : Form
    {
        public VentanaInventario()
        {
            InitializeComponent();
        }

        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 abririmenu = new Form1();
            abririmenu.Show();
            this.Hide();
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

        private void button2_Click(object sender, EventArgs e)
        {
            Conexion objetoconexion = new Conexion();
            objetoconexion.establecerconexion();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void VentanaInventario_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }
        private void CargarDatos()
        {
            string servidor = "127.0.0.1";
            string bd = "proyecto";
            string usuario = "root";
            string password = "";
            string puerto = "4001";
            string cadenaConexion = "server=" + servidor + ";" + "port=" + puerto + ";" + "user id=" + usuario + ";" + "password=" + password + ";" + "database=" + bd + ";";
            MySqlConnection connection = new MySqlConnection(cadenaConexion);
            try
            {
                connection.Open();
                string sqlQuery = "SELECT pro.ID, pro.Nombre, pro.Descripcion, pro.Existencia, pro.Precio, MAX(p.Nombre) AS Marca " +
                   "FROM producto pro " +
                   "LEFT JOIN detalle_compras dc ON pro.ID = dc.Producto_ID " +
                   "LEFT JOIN compras c ON dc.Compras_ID = c.ID " +
                   "LEFT JOIN proveedores p ON c.Proveedores_ID = p.ID " +
                   "GROUP BY pro.ID, pro.Nombre, pro.Descripcion, pro.Existencia, pro.Precio";

                if (comboBox1.SelectedItem != null)
                {
                    string sortBy = comboBox1.SelectedItem.ToString();
                    if (sortBy == "Nombre de producto")
                    {
                        sqlQuery += " ORDER BY Nombre ASC";
                    }
                    else if (sortBy == "Existencia de Producto (Mayor)")
                    {
                        sqlQuery += " ORDER BY Existencia DESC";
                    }
                    else if (sortBy == "Existencia de Producto (Menor)")
                    {
                        sqlQuery += " ORDER BY Existencia ASC";
                    }
                    else if (sortBy == "ID")
                    {
                        sqlQuery += " ORDER BY ID ASC";
                    }
                    else if (sortBy == "Costo (Mayor)")
                    {
                        sqlQuery += " ORDER BY Costo DESC";
                    }
                    else if (sortBy == "Costo (Menor)")
                    {
                        sqlQuery += " ORDER BY Costo ASC";
                    }
                    else if (sortBy == "Precio (Mayor)")
                    {
                        sqlQuery += " ORDER BY Precio DESC";
                    }
                    else if (sortBy == "Precio (Menor)")
                    {
                        sqlQuery += " ORDER BY Precio ASC";
                    }
                }

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NuevoProducto nuevoProd = new NuevoProducto();
            nuevoProd.ShowDialog();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filtro = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(filtro))
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Nombre LIKE '%{0}%'", filtro);
            }
            else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proveedores abrir1 = new Proveedores();
            abrir1.Show();
            this.Hide();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NuevoProducto nuevoProd = new NuevoProducto();
            nuevoProd.ShowDialog();
        }

        private void comprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NuevoProducto prod = new NuevoProducto();
            prod.Show();
            this.Hide();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 abrir = new Form4();
            abrir.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string id = selectedRow.Cells["Codigo"].Value.ToString();
                string currentNombre = selectedRow.Cells["Nombre"].Value.ToString();
                string currentDescripcion = selectedRow.Cells["Descripcion"].Value.ToString();

                ActualizarProductoForm actualizarProductoForm = new ActualizarProductoForm(currentNombre, currentDescripcion);

                if (actualizarProductoForm.ShowDialog() == DialogResult.OK)
                {
                    string nuevoNombre = actualizarProductoForm.Nombre;
                    string nuevaDescripcion = actualizarProductoForm.Descripcion;

                    string connectionString = "server=localhost;port=3306;user id=root;password=root123;database=proyecto;";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        string query = "UPDATE producto SET Nombre = @Nombre, descripcion = @Descripcion WHERE ID = @ID";

                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Nombre", nuevoNombre);
                        command.Parameters.AddWithValue("@Descripcion", nuevaDescripcion);
                        command.Parameters.AddWithValue("@ID", id);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Producto actualizado exitosamente.");

                            // Recargar los datos en el DataGridView
                            CargarDatos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al actualizar el producto: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para actualizar.");
            }
        }
    }
}
