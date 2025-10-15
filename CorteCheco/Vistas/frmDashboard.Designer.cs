namespace CorteCheco.Vistas
{
    partial class frmDashboard
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            flpDashboard = new FlowLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pnlFiltros = new Guna.UI2.WinForms.Guna2Panel();
            btnBuscar = new Guna.UI2.WinForms.Guna2Button();
            cmbDepartamentos = new Guna.UI2.WinForms.Guna2ComboBox();
            txtBuscar = new Guna.UI2.WinForms.Guna2TextBox();
            flpDashboard.SuspendLayout();
            pnlFiltros.SuspendLayout();
            SuspendLayout();
            // 
            // flpDashboard
            // 
            flpDashboard.AutoScroll = true;
            flpDashboard.Controls.Add(flowLayoutPanel1);
            flpDashboard.Dock = DockStyle.Fill;
            flpDashboard.Location = new Point(0, 0);
            flpDashboard.Name = "flpDashboard";
            flpDashboard.Padding = new Padding(20);
            flpDashboard.Size = new Size(965, 748);
            flpDashboard.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(23, 23);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(200, 0);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // pnlFiltros
            // 
            pnlFiltros.Controls.Add(btnBuscar);
            pnlFiltros.Controls.Add(cmbDepartamentos);
            pnlFiltros.Controls.Add(txtBuscar);
            pnlFiltros.CustomizableEdges = customizableEdges7;
            pnlFiltros.Dock = DockStyle.Top;
            pnlFiltros.Location = new Point(0, 0);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.ShadowDecoration.CustomizableEdges = customizableEdges8;
            pnlFiltros.Size = new Size(965, 50);
            pnlFiltros.TabIndex = 1;
            // 
            // btnBuscar
            // 
            btnBuscar.BackgroundImage = Properties.Resources.search;
            btnBuscar.BorderRadius = 15;
            btnBuscar.CustomizableEdges = customizableEdges1;
            btnBuscar.DisabledState.BorderColor = Color.DarkGray;
            btnBuscar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnBuscar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnBuscar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnBuscar.FillColor = Color.LightGray;
            btnBuscar.Font = new Font("Segoe UI", 9F);
            btnBuscar.ForeColor = Color.FromArgb(45, 45, 48);
            btnBuscar.Image = Properties.Resources.search;
            btnBuscar.Location = new Point(12, 11);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnBuscar.Size = new Size(44, 36);
            btnBuscar.TabIndex = 4;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // cmbDepartamentos
            // 
            cmbDepartamentos.BackColor = Color.Transparent;
            cmbDepartamentos.BorderRadius = 15;
            cmbDepartamentos.CustomizableEdges = customizableEdges3;
            cmbDepartamentos.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDepartamentos.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDepartamentos.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbDepartamentos.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbDepartamentos.Font = new Font("Segoe UI", 10F);
            cmbDepartamentos.ForeColor = Color.FromArgb(68, 88, 112);
            cmbDepartamentos.ItemHeight = 30;
            cmbDepartamentos.Location = new Point(268, 11);
            cmbDepartamentos.Name = "cmbDepartamentos";
            cmbDepartamentos.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cmbDepartamentos.Size = new Size(250, 36);
            cmbDepartamentos.TabIndex = 0;
            cmbDepartamentos.SelectedIndexChanged += cmbDepartamentos_SelectedIndexChanged;
            // 
            // txtBuscar
            // 
            txtBuscar.BorderRadius = 15;
            txtBuscar.CustomizableEdges = customizableEdges5;
            txtBuscar.DefaultText = "";
            txtBuscar.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtBuscar.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtBuscar.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtBuscar.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtBuscar.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtBuscar.Font = new Font("Segoe UI", 9F);
            txtBuscar.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtBuscar.Location = new Point(62, 11);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar por nombre o código...";
            txtBuscar.SelectedText = "";
            txtBuscar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtBuscar.Size = new Size(200, 36);
            txtBuscar.TabIndex = 3;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
            // 
            // frmDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(965, 748);
            Controls.Add(pnlFiltros);
            Controls.Add(flpDashboard);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmDashboard";
            Load += frmDashboard_Load;
            flpDashboard.ResumeLayout(false);
            pnlFiltros.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpDashboard;
        private FlowLayoutPanel flowLayoutPanel1;
        private Guna.UI2.WinForms.Guna2Panel pnlFiltros;
        private Guna.UI2.WinForms.Guna2ComboBox cmbDepartamentos;
        private Guna.UI2.WinForms.Guna2Button btnBuscar;
        private Guna.UI2.WinForms.Guna2TextBox txtBuscar;
    }
}