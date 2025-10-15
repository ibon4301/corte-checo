using System;
using System.Windows.Forms;
using CorteCheco.Datos;

namespace CorteCheco.Vistas
{
    public partial class frmRegistroUsuarios : Form
    {
        private readonly ConexionDB _conexionDB;


        public frmRegistroUsuarios()
        {
            InitializeComponent();
            _conexionDB = new ConexionDB();

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtUsuario.Text;
            string contraseña = txtPassword.Text;
            string confirmarContraseña = txtConfirmarPassword.Text;

            // 2. Validaciones de la interfaz
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contraseña) || string.IsNullOrWhiteSpace(confirmarContraseña))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (contraseña != confirmarContraseña)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor, verifícalas.", "Error de Contraseña", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (contraseña.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Contraseña Corta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Lógica con la Base de Datos
            try
            {
                if (_conexionDB.UsuarioYaExiste(nombreUsuario))
                {
                    MessageBox.Show("El nombre de usuario ya está en uso. Por favor, elige otro.", "Usuario Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool registroExitoso = _conexionDB.RegistrarUsuario(nombreUsuario, contraseña);

                // 4. Feedback final
                if (registroExitoso)
                {
                    MessageBox.Show("¡Cuenta creada exitosamente! Ya puedes iniciar sesión.", "Registro Completo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Volver al login
                }
                else
                {
                    MessageBox.Show("Ocurrió un error inesperado al crear la cuenta.", "Error de Registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión con la base de datos: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        // Si tienes un botón "Volver" o similar, conéctalo a este evento.
        // Si no, puedes borrar este método.
        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnVolver_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
    

