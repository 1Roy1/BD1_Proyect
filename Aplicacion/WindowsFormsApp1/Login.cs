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
    public partial class Login : Form
    {
        string user = " ";
        string pass = " ";

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Usuario";
            textBox1.ForeColor = Color.Gray;
            textBox2.PasswordChar = '\0';
            textBox2.Text = "Contraseña";
            textBox2.ForeColor = Color.Gray;

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EncryptMD5 encrypt = new EncryptMD5();
            pass = encrypt.Encrypt("soyElAdmin");
            // guardar en la base de datos

            string pass_decrypt = encrypt.Decrypt(pass);

            
            
            if (user == "Admin" && pass == pass_decrypt)
            {
                Form1 abrir1 = new Form1();
                abrir1.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas");
            }


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
    }
}
