namespace WindowsFormsApp1
{
    partial class ResetPassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ver = new System.Windows.Forms.PictureBox();
            this.ocultar = new System.Windows.Forms.PictureBox();
            this.ocultar2 = new System.Windows.Forms.PictureBox();
            this.ver2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ocultar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ocultar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ver2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.HotPink;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(324, 154);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 58);
            this.button1.TabIndex = 0;
            this.button1.Text = "Comprobar Usuario";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(317, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "RESTABLEZCA SU CONTRASEÑA";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(204, 106);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 30);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(204, 245);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '#';
            this.textBox2.Size = new System.Drawing.Size(200, 30);
            this.textBox2.TabIndex = 3;
            this.textBox2.Visible = false;
            this.textBox2.Leave += new System.EventHandler(this.textBox2_Leave);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(204, 295);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox3.Name = "textBox3";
            this.textBox3.PasswordChar = '#';
            this.textBox3.Size = new System.Drawing.Size(200, 30);
            this.textBox3.TabIndex = 4;
            this.textBox3.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(58, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ingrese su usuario:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 249);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Contraseña nueva:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(44, 300);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Confirme Contraseña:";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.HotPink;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(324, 340);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 58);
            this.button2.TabIndex = 8;
            this.button2.Text = "Actulizar Contraseña";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Image = global::WindowsFormsApp1.Properties.Resources.cerrar2;
            this.button3.Location = new System.Drawing.Point(460, 0);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 36);
            this.button3.TabIndex = 9;
            this.button3.TabStop = false;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ver
            // 
            this.ver.Image = global::WindowsFormsApp1.Properties.Resources.abrir;
            this.ver.Location = new System.Drawing.Point(411, 234);
            this.ver.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ver.Name = "ver";
            this.ver.Size = new System.Drawing.Size(52, 41);
            this.ver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ver.TabIndex = 12;
            this.ver.TabStop = false;
            this.ver.Click += new System.EventHandler(this.ver_Click_1);
            // 
            // ocultar
            // 
            this.ocultar.Image = global::WindowsFormsApp1.Properties.Resources.ocultar;
            this.ocultar.Location = new System.Drawing.Point(411, 234);
            this.ocultar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ocultar.Name = "ocultar";
            this.ocultar.Size = new System.Drawing.Size(52, 41);
            this.ocultar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ocultar.TabIndex = 11;
            this.ocultar.TabStop = false;
            this.ocultar.Click += new System.EventHandler(this.ocultar_Click_1);
            // 
            // ocultar2
            // 
            this.ocultar2.Image = global::WindowsFormsApp1.Properties.Resources.ocultar;
            this.ocultar2.Location = new System.Drawing.Point(411, 295);
            this.ocultar2.Margin = new System.Windows.Forms.Padding(2);
            this.ocultar2.Name = "ocultar2";
            this.ocultar2.Size = new System.Drawing.Size(52, 41);
            this.ocultar2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ocultar2.TabIndex = 13;
            this.ocultar2.TabStop = false;
            this.ocultar2.Click += new System.EventHandler(this.ocultar2_Click);
            // 
            // ver2
            // 
            this.ver2.Image = global::WindowsFormsApp1.Properties.Resources.abrir;
            this.ver2.Location = new System.Drawing.Point(411, 295);
            this.ver2.Margin = new System.Windows.Forms.Padding(2);
            this.ver2.Name = "ver2";
            this.ver2.Size = new System.Drawing.Size(52, 41);
            this.ver2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ver2.TabIndex = 14;
            this.ver2.TabStop = false;
            this.ver2.Click += new System.EventHandler(this.ver2_Click);
            // 
            // ResetPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(495, 452);
            this.Controls.Add(this.ver2);
            this.Controls.Add(this.ocultar2);
            this.Controls.Add(this.ver);
            this.Controls.Add(this.ocultar);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ResetPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ResetPassword";
            ((System.ComponentModel.ISupportInitialize)(this.ver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ocultar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ocultar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ver2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox ver;
        private System.Windows.Forms.PictureBox ocultar;
        private System.Windows.Forms.PictureBox ocultar2;
        private System.Windows.Forms.PictureBox ver2;
    }
}