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
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            string user = " ";
            string pass = " ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 abrir1 = new Form1();
            abrir1.ShowDialog();
            this.Close();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "Usuario";
            textBox1.ForeColor = Color.Gray;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = "Usuario";
            textBox2.ForeColor = Color.Gray;
        }
    }
}
