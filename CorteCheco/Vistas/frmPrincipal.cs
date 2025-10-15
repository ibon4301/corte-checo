using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CorteCheco.Logica;

namespace CorteCheco.Vistas
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            ConfigurarInterfazPorRol();
        }

        private void ConfigurarInterfazPorRol()
        {
            if (SesionUsuario.Rol == "Usuario")
            {
                // --- USUARIO NORMAL ---
                btnProductos.Visible = false;
                btnDepartamentos.Visible = false;
                btnDashboard.Text = "Ver Artículos";
                AbrirFormularioEnPanel(new frmDashboard());
            }
            else // Administrador
            {
                // --- ADMINISTRADOR ---
                btnProductos.Visible = true;
                btnDepartamentos.Visible = true;
                btnDashboard.Visible = true;
                AbrirFormularioEnPanel(new frmProductos());
            }
        }

        private void pnlContenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AbrirFormularioEnPanel(object formularioHijo)
        {
            // Nos aseguramos de que el panel de contenido esté limpio antes de abrir un nuevo formulario.
            if (this.pnlContenedor.Controls.Count > 0)
                this.pnlContenedor.Controls.RemoveAt(0);

            // Convertimos el objeto recibido a un Formulario.
            Form fh = formularioHijo as Form;

            // Configuramos el formulario hijo para que se comporte como un panel, no como una ventana.
            fh.TopLevel = false; // Le decimos que no es una ventana de nivel superior.
            fh.Dock = DockStyle.Fill; // Hacemos que ocupe todo el espacio del panel.

            // Lo añadimos a la colección de controles de nuestro panel.
            this.pnlContenedor.Controls.Add(fh);
            this.pnlContenedor.Tag = fh;

            // con esto lo mostramos.
            fh.Show();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new frmProductos());
        }

        private void btnDepartamentos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new frmDepartamento());
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new frmDashboard());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Estás seguro de que quieres cerrar la sesión?",
                                             "Confirmar Cierre de Sesión",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

            // Si el usuario hace clic en "Sí"...
            if (resultado == DialogResult.Yes)
            {
                // 1. Limpiamos los datos de la sesión actual.
                SesionUsuario.CerrarSesion();

                // 2. Reiniciamos la aplicación.
                Application.Restart();
            }
        }
    }
}
