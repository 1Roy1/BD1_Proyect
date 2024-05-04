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
        string cadenaConexion = "server=localhost;port=3306;user id=root;password=root123;database=proyecto";
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

            MySqlConnection connection = new MySqlConnection(cadenaConexion);

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void CargarNombresProveedores()
        {
            MySqlConnection connection = new MySqlConnection(cadenaConexion);
            comboBox1.Items.Clear(); // Limpiar los elementos existentes en el ComboBox antes de cargar nuevos

            try
            {
                connection.Open();
                string sqlQuery = "SELECT Nombre FROM proveedores WHERE Activo = 1";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string nombreProveedor = reader["Nombre"].ToString();
                    comboBox1.Items.Add(nombreProveedor); // Agregar el nombre del proveedor al ComboBox
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los nombres de los proveedores: " + ex.Message);
            }
            finally
            {
                connection.Close();
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
            finally
            {
                connection.Close();
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

        }
    }
}
