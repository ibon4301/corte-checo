namespace CorteCheco
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // Lógica de validación temporal (más adelante la conectaremos a una base de datos)
            // Leemos el texto de los TextBox y comprobamos si son los correctos.
            if (txtUsuario.Text == "admin" && txtPassword.Text == "1234")
            {
                // --- SI EL LOGIN ES CORRECTO ---

                // 1. Le decimos a este formulario que el resultado de su "diálogo" es OK.
                //    Esta es la señal que nuestro Program.cs está esperando para continuar.
                this.DialogResult = DialogResult.OK;

                // 2. Cerramos este formulario de login. Al hacerlo, el flujo de ejecución
                //    vuelve a Program.cs, que verá el resultado "OK" y abrirá frmPrincipal.
                this.Close();
            }
            else
            {
                // --- SI EL LOGIN ES INCORRECTO ---

                // 1. Mostramos una ventana emergente profesional con un mensaje de error.
                MessageBox.Show("Usuario o contraseña incorrectos.",
                                "Error de Autenticación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                // 2. Limpiamos el campo de la contraseña
                //    y ponemos el cursor (foco) de nuevo en el usuario para que lo intente otra vez.
                txtPassword.Clear();
                txtUsuario.Focus();
            }
        }
    }
}
