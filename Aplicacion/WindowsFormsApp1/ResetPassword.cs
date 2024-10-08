﻿using System;
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
    public partial class ResetPassword : Form
    {
        Conexion conexion = new Conexion();
        string pass = " ";
        string pass2 = " ";
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text;

            // Lógica para buscar el usuario en la base de datos
            // Aquí debes implementar la lógica para buscar el usuario en la base de datos
            bool usuarioEncontrado = conexion.BuscarUsuario2(usuario);

            // Si el usuario se encuentra en la base de datos, habilitar los cuadros de texto para la nueva contraseña
            if (usuarioEncontrado)
            {
                textBox2.Visible = true;
                textBox3.Visible = true;
                this.Height += 200;
                this.StartPosition = FormStartPosition.CenterScreen;
                button1.Enabled = false;
                textBox1.Enabled = false;
            }
            else
            {
                MessageBox.Show("Usuario no encontrado");
                textBox1.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text;
            string nuevaContraseña = textBox2.Text;
            string confirmarContraseña = textBox3.Text;

            // Verificar si las contraseñas coinciden
            if (nuevaContraseña != confirmarContraseña)
            {
                MessageBox.Show("Las contraseñas no coinciden");
                textBox2.Clear();
                textBox3.Clear();
                return;
            }

            // Llamar al método para actualizar la contraseña en la base de datos
            bool contraseñaActualizada = conexion.ActualizarContrasenaUsuario(usuario, nuevaContraseña);

            if (contraseñaActualizada)
            {
                MessageBox.Show("Contraseña actualizada exitosamente");
                this.Close();   
            }
            else
            {
                MessageBox.Show("No se pudo actualizar la contraseña");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
          
        }
        private void ver2_Click(object sender, EventArgs e)
        {
            ocultar2.BringToFront();
            textBox3.PasswordChar = '\0';
        }

        private void ocultar2_Click(object sender, EventArgs e)
        {
            ver2.BringToFront();
            textBox3.PasswordChar = '#';
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {

        }

        private void ocultar1_Click(object sender, EventArgs e)
        {
            ver1.BringToFront();
            textBox2.PasswordChar = '#';
        }

        private void ver1_Click(object sender, EventArgs e)
        {
            ocultar1.BringToFront();
            textBox2.PasswordChar = '\0';
        }

        private void ResetPassword_Load(object sender, EventArgs e)
        {

        }
    }
}
