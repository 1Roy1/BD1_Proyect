﻿using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            

        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            // Crear una instancia de tu ventana de inventario
            VentanaInventario ventanaInventario = new VentanaInventario();

            // Mostrar la ventana de inventario
            ventanaInventario.Show();
            this.Hide();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VentanaVentas abrir1 = new VentanaVentas();
            abrir1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Proveedores abrir1 = new Proveedores();
            abrir1.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            NuevoProducto nuevoProd = new NuevoProducto();
            nuevoProd.Show();
            this.Close();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 abrir1 = new Form4();
            abrir1.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string rutaArchivoPDF = @"C:\Users\luist\OneDrive\Escritorio\1er semestre 2024\Base de datos I\ProyectoBase\BD1_Proyect\Aplicacion\WindowsFormsApp1\Manual de usuario.pdf";

            // Abrir el archivo PDF con la aplicación predeterminada
            Process.Start(rutaArchivoPDF);
        }
    }
}
