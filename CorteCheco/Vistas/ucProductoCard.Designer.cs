namespace CorteCheco.Vistas
{
    partial class ucProductoCard
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2MouseStateHandler1 = new Guna.UI2.WinForms.Guna2MouseStateHandler(components);
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            lblStock = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblPrecio = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblNombre = new Guna.UI2.WinForms.Guna2HtmlLabel();
            picImagen = new Guna.UI2.WinForms.Guna2PictureBox();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picImagen).BeginInit();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BorderColor = Color.FromArgb(80, 80, 80);
            guna2Panel1.BorderRadius = 15;
            guna2Panel1.Controls.Add(lblStock);
            guna2Panel1.Controls.Add(lblPrecio);
            guna2Panel1.Controls.Add(lblNombre);
            guna2Panel1.Controls.Add(picImagen);
            guna2Panel1.CustomizableEdges = customizableEdges3;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.FillColor = Color.FromArgb(60, 63, 81);
            guna2Panel1.ForeColor = Color.FromArgb(255, 192, 0);
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Panel1.Size = new Size(204, 241);
            guna2Panel1.TabIndex = 0;
            // 
            // lblStock
            // 
            lblStock.BackColor = Color.Transparent;
            lblStock.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblStock.ForeColor = Color.Silver;
            lblStock.Location = new Point(115, 223);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(77, 15);
            lblStock.TabIndex = 3;
            lblStock.Text = "Existencias: 50";
            // 
            // lblPrecio
            // 
            lblPrecio.BackColor = Color.Transparent;
            lblPrecio.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrecio.ForeColor = Color.FromArgb(255, 192, 0);
            lblPrecio.Location = new Point(12, 215);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(56, 23);
            lblPrecio.TabIndex = 2;
            lblPrecio.Text = "99,99 €";
            // 
            // lblNombre
            // 
            lblNombre.BackColor = Color.Transparent;
            lblNombre.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(3, 146);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(198, 22);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre del Producto Largo";
            // 
            // picImagen
            // 
            picImagen.CustomizableEdges = customizableEdges1;
            picImagen.Dock = DockStyle.Top;
            picImagen.ImageRotate = 0F;
            picImagen.Location = new Point(0, 0);
            picImagen.Name = "picImagen";
            picImagen.ShadowDecoration.CustomizableEdges = customizableEdges2;
            picImagen.Size = new Size(204, 140);
            picImagen.SizeMode = PictureBoxSizeMode.Zoom;
            picImagen.TabIndex = 0;
            picImagen.TabStop = false;
            // 
            // ucProductoCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(guna2Panel1);
            Name = "ucProductoCard";
            Size = new Size(204, 241);
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picImagen).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2MouseStateHandler guna2MouseStateHandler1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2PictureBox picImagen;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNombre;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPrecio;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStock;
    }
}