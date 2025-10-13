using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CorteCheco.Datos;      
using CorteCheco.Modelos;    

namespace CorteCheco.Vistas
{
    public partial class frmDepartamento : Form
    {
        private readonly ConexionDB _conexionDB = new ConexionDB();
        private Departamento _deptoSeleccionado = null;

        public frmDepartamento()
        {
            InitializeComponent();
        }

        private void frmDepartamentos_Load(object sender, EventArgs e)
        {
            // Al cargar el formulario, lo primero es mostrar los datos.
            CargarDepartamentos();
            ConfigurarDataGridView();
        }

        private void CargarDepartamentos()
        {
            // Obtenemos la lista de departamentos desde la base de datos.
            List<Departamento> listaDepartamentos = _conexionDB.GetDepartamentos();

            // Asignamos la lista como fuente de datos para el DataGridView.
            dgvDepartamentos.DataSource = listaDepartamentos;
        }

        private void ConfigurarDataGridView()
        {
            // Opcional: Mejorar la apariencia y funcionalidad del DataGridView.
            dgvDepartamentos.RowHeadersVisible = false; // Ocultar la columna de la izquierda
            dgvDepartamentos.Columns["Id"].Visible = false; // Ocultar la columna de ID, no es necesaria para el usuario
            dgvDepartamentos.Columns["Nombre"].HeaderText = "Nombre del Departamento"; // Cambiar título de la columna
            dgvDepartamentos.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Hacer que la columna ocupe todo el espacio

            // Estilo visual (opcional pero recomendado)
            dgvDepartamentos.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            dgvDepartamentos.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.FromArgb(255, 192, 0);
            dgvDepartamentos.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(60, 60, 63);
            dgvDepartamentos.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.White;
            dgvDepartamentos.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(80, 80, 85);
        }
    }
}