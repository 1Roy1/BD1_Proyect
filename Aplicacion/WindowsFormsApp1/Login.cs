using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
     
        string user = " ";
        string pass = " ";
        string user2 = " ";
        string pass2 = " ";
        string passintro = "";
        string userintro = "";
        bool igual = false;
        EncryptMD5 encrypt = new EncryptMD5();
        Conexion conexion = new Conexion();

        // guardar en la base de datos

        public Login()
        {
            InitializeComponent();
            conexion.establecerconexion();
            //user2 = "Hola";
            //pass2 = encrypt.Encrypt("hola123");
            //conexion.InsertarUsuario(user2, pass2);
            
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Usuario";
            textBox1.ForeColor = Color.Gray;
            textBox2.PasswordChar = '\0';
            textBox2.Text = "Contraseña";
            textBox2.ForeColor = Color.Gray;

            


        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.ForeColor = Color.Black;
            textBox2.PasswordChar = '#';
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            user = textBox1.Text;
            if (user.Equals("Usuario"))
            {
                textBox1.Text = "Usuario";
                textBox1.ForeColor = Color.Gray;
            }
            else
            {
                if (user.Equals(""))
                {
                    textBox1.Text = "Usuario";
                    textBox1.ForeColor = Color.Gray;
                }
                else
                {
                    textBox1.Text = user;
                    textBox1.ForeColor = Color.Black;
                }
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            pass = textBox2.Text;
            if (pass.Equals("Contraseña"))
            {
                textBox2.Text = "Contraseña";
                textBox2.ForeColor = Color.Gray;
            }
            else
            {
                if (pass.Equals(""))
                {
                    textBox2.PasswordChar = '\0';
                    textBox2.Text = "Contraseña";
                    textBox2.ForeColor = Color.Gray;
                }
                else
                {
                    textBox2.PasswordChar = '#';
                    textBox2.Text = pass;
                    textBox2.ForeColor = Color.Black;
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {

            string passintro = textBox2.Text;
            string userintro = textBox1.Text;

            // Llamar al método BuscarUsuario para verificar las credenciales
            (string usuarioEncontrado, string contraseñaEncontrada) = conexion.BuscarUsuario(userintro, passintro);

            // Verificar si las credenciales son correctas
            if (userintro == usuarioEncontrado && passintro == contraseñaEncontrada)
            {
                // Abrir el formulario principal si las credenciales son correctas
                Form1 abrir1 = new Form1();
                abrir1.ShowDialog();
                this.Hide();
            }
            else
            {
                // Mostrar un mensaje de error si las credenciales son incorrectas
                MessageBox.Show("Credenciales incorrectas");
            }


        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ver_Click(object sender, EventArgs e)
        {
            ocultar.BringToFront();
            textBox2.PasswordChar = '\0';
        }

        private void ocultar_Click(object sender, EventArgs e)
        {
            ver.BringToFront();
            textBox2.PasswordChar = '#';

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetPassword abrir = new ResetPassword();
            abrir.ShowDialog();
            
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Realizar la verificación de las credenciales aquí
                string passintro = textBox2.Text;
                string userintro = textBox1.Text;

                // Llamar al método BuscarUsuario para verificar las credenciales
                (string usuarioEncontrado, string contraseñaEncontrada) = conexion.BuscarUsuario(userintro, passintro);

                // Verificar si las credenciales son correctas
                if (userintro == usuarioEncontrado && passintro == contraseñaEncontrada)
                {
                    // Abrir el formulario principal si las credenciales son correctas
                    Form1 abrir1 = new Form1();
                    abrir1.ShowDialog();
                    this.Hide();
                }
                else
                {
                    // Mostrar un mensaje de error si las credenciales son incorrectas
                    MessageBox.Show("Credenciales incorrectas");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string rutaArchivoPDF = @"C:\Users\luist\OneDrive\Escritorio\1er semestre 2024\Base de datos I\ProyectoBase\BD1_Proyect\Aplicacion\WindowsFormsApp1\Manual de usuario.pdf";

            // Abrir el archivo PDF con la aplicación predeterminada
            Process.Start(rutaArchivoPDF);
        }
    }
}
