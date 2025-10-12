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
            // L�gica de validaci�n temporal (m�s adelante la conectaremos a una base de datos)
            // Leemos el texto de los TextBox y comprobamos si son los correctos.
            if (txtUsuario.Text == "admin" && txtPassword.Text == "1234")
            {
                // --- SI EL LOGIN ES CORRECTO ---

                // 1. Le decimos a este formulario que el resultado de su "di�logo" es OK.
                //    Esta es la se�al que nuestro Program.cs est� esperando para continuar.
                this.DialogResult = DialogResult.OK;

                // 2. Cerramos este formulario de login. Al hacerlo, el flujo de ejecuci�n
                //    vuelve a Program.cs, que ver� el resultado "OK" y abrir� frmPrincipal.
                this.Close();
            }
            else
            {
                // --- SI EL LOGIN ES INCORRECTO ---

                // 1. Mostramos una ventana emergente profesional con un mensaje de error.
                MessageBox.Show("Usuario o contrase�a incorrectos.",
                                "Error de Autenticaci�n",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                // 2. Limpiamos el campo de la contrase�a
                //    y ponemos el cursor (foco) de nuevo en el usuario para que lo intente otra vez.
                txtPassword.Clear();
                txtUsuario.Focus();
            }
        }
    }
}
