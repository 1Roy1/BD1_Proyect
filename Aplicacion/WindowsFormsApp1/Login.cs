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
        string passintro = " ";
        string userintro = " ";
        EncryptMD5 encrypt = new EncryptMD5();
        
            // guardar en la base de datos

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

            pass = encrypt.Encrypt("administracion");
            string pass_decrypt = encrypt.Decrypt(pass);

            passintro = textBox2.Text;
            userintro = textBox1.Text;

            if (user == userintro && pass_decrypt == passintro)
            {
                Form1 abrir1 = new Form1();
                abrir1.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas");
                MessageBox.Show(pass);
            }



        }
    }
}
