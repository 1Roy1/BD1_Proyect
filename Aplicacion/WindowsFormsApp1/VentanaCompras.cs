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
    public partial class NuevoProducto : Form
    {
        public NuevoProducto()
        {
            InitializeComponent();
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
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
                MessageBox.Show("Ingrese solo numeros", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar) || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo numeros o punto", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void NuevoProducto_Load(object sender, EventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar) || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Ingrese solo numeros o punto", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection connection;
            string servidor = "localhost";
            string bd = "proyecto";
            string usuario = "root";
            string password = "Rod2102777";
            string puerto = "3306";
            string cadenaConexion = "server=" + servidor + ";" + "port=" + puerto + ";" + "user id=" + usuario + ";" + "password=" + password + ";" + "database=" + bd + ";";
            connection = new MySqlConnection(cadenaConexion);

            try
            {
                String id_str = textBox1.Text;
                int id = Convert.ToInt32(id_str);

                String name = textBox2.Text;
                String desc = textBox3.Text;
                String exist_str = textBox4.Text;
                int exist = Convert.ToInt32(exist_str);

                String price_str = textBox5.Text;
                float price = Convert.ToSingle(price_str);

                String cost_str = textBox6.Text;
                float cost = Convert.ToSingle(cost_str);

                String marca = textBox7.Text;

                connection.Open();

                string sqlQuery = $"INSERT INTO inventario(ID, Nombre, Descripcion, Existencias, Precio, Costo, Marca) VALUES (@ID, @Name, @descrip, @exist, @price, @cost, @mar ) ";

                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@descrip", desc);
                cmd.Parameters.AddWithValue("@exist", exist);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@cost", cost);
                cmd.Parameters.AddWithValue("@mar", marca);


                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Datos actualizados correctamente.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se encontró el registro con el ID proporcionado.");
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
    }
}
