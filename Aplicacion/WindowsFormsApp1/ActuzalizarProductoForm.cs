using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ActualizarProductoForm : Form
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ActualizarProductoForm(string nombre, string descripcion)
        {
            InitializeComponent();
            textBoxNombre.Text = nombre;
            textBoxDescripcion.Text = descripcion;
            this.BackColor = Color.FromArgb(255, 182, 193); // Color de fondo rosado pálido
            this.Icon = SystemIcons.Information; // Usar un icono por defecto de Windows
            this.MaximizeBox = false; // Deshabilitar el botón maximizar
            this.StartPosition = FormStartPosition.CenterScreen; // Centrar el formulario en la pantalla
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            Nombre = textBoxNombre.Text;
            Descripcion = textBoxDescripcion.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.labelNombre = new System.Windows.Forms.Label();
            this.labelDescripcion = new System.Windows.Forms.Label();
            this.buttonAceptar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxNombre
            // 
            this.textBoxNombre.Location = new System.Drawing.Point(100, 30);
            this.textBoxNombre.Name = "textBoxNombre";
            this.textBoxNombre.Size = new System.Drawing.Size(200, 20);
            this.textBoxNombre.TabIndex = 0;
            // 
            // textBoxDescripcion
            // 
            this.textBoxDescripcion.Location = new System.Drawing.Point(100, 70);
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.Size = new System.Drawing.Size(200, 20);
            this.textBoxDescripcion.TabIndex = 1;
            // 
            // labelNombre
            // 
            this.labelNombre.AutoSize = true;
            this.labelNombre.Location = new System.Drawing.Point(20, 30);
            this.labelNombre.Name = "labelNombre";
            this.labelNombre.Size = new System.Drawing.Size(47, 13);
            this.labelNombre.TabIndex = 2;
            this.labelNombre.Text = "Nombre:";
            // 
            // labelDescripcion
            // 
            this.labelDescripcion.AutoSize = true;
            this.labelDescripcion.Location = new System.Drawing.Point(20, 70);
            this.labelDescripcion.Name = "labelDescripcion";
            this.labelDescripcion.Size = new System.Drawing.Size(66, 13);
            this.labelDescripcion.TabIndex = 3;
            this.labelDescripcion.Text = "Descripción:";
            // 
            // buttonAceptar
            // 
            this.buttonAceptar.Location = new System.Drawing.Point(225, 110);
            this.buttonAceptar.Name = "buttonAceptar";
            this.buttonAceptar.Size = new System.Drawing.Size(75, 23);
            this.buttonAceptar.TabIndex = 4;
            this.buttonAceptar.Text = "Aceptar";
            this.buttonAceptar.UseVisualStyleBackColor = true;
            this.buttonAceptar.Click += new System.EventHandler(this.buttonAceptar_Click);
            this.buttonAceptar.FlatStyle = FlatStyle.Flat;
            this.buttonAceptar.FlatAppearance.BorderSize = 0;
            this.buttonAceptar.BackColor = Color.LightPink;
            this.buttonAceptar.ForeColor = Color.Black; // Color de texto negro
            this.buttonAceptar.Font = new Font(this.buttonAceptar.Font.FontFamily, 10, FontStyle.Bold); // Texto negrita y tamaño más grande
            this.buttonAceptar.MouseEnter += new EventHandler(this.buttonAceptar_MouseEnter);
            this.buttonAceptar.MouseLeave += new EventHandler(this.buttonAceptar_MouseLeave);
            this.buttonAceptar.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, buttonAceptar.Width, buttonAceptar.Height, 20, 20));

            // 
            // ActualizarProductoForm
            // 
            this.ClientSize = new System.Drawing.Size(320, 160);
            this.Controls.Add(this.buttonAceptar);
            this.Controls.Add(this.labelDescripcion);
            this.Controls.Add(this.labelNombre);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.textBoxNombre);
            this.Name = "ActualizarProductoForm";
            this.Text = "Actualizar Producto";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox textBoxNombre;
        private TextBox textBoxDescripcion;
        private Label labelNombre;
        private Label labelDescripcion;
        private Button buttonAceptar;

        // Agrega las referencias necesarias para la función de redondeo
        [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateRoundRectRgn(
     int nLeftRect,
     int nTopRect,
     int nRightRect,
     int nBottomRect,
     int nWidthEllipse,
     int nHeightEllipse);

        private void buttonAceptar_MouseEnter(object sender, EventArgs e)
        {
            buttonAceptar.BackColor = Color.DeepPink;
        }

        private void buttonAceptar_MouseLeave(object sender, EventArgs e)
        {
            buttonAceptar.BackColor = Color.LightPink;
        }
    }
}
