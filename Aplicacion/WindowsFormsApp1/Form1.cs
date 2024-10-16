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
     
            VentanaInventario ventanaInventario = new VentanaInventario();


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

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Ruta del archivo .txt
                string rutaArchivo = @"C:\Users\USER\OneDrive\Documentos\GitHub\BD1_Proyect\Aplicacion\WindowsFormsApp1\obj\Debug\Bitacora\transaction_log.txt";

                // Verificar si el archivo existe
                if (System.IO.File.Exists(rutaArchivo))
                {
                    // Leer todo el contenido del archivo
                    string contenido = System.IO.File.ReadAllText(rutaArchivo);

                   MessageBox.Show(contenido, "Contenido del archivo");
                }
                else
                {
                    MessageBox.Show("El archivo no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
