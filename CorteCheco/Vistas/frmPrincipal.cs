using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorteCheco.Vistas
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

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
    }
}
