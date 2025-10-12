using System.Data.SqlClient;
using CorteCheco.Datos;
using CorteCheco.Logica;


namespace CorteCheco
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        // En frmLogin.cs, reemplaza tu m�todo btnIngresar_Click con este:

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // 1. Obtenemos los datos de la interfaz.
            string usuario = txtUsuario.Text;
            string password = txtPassword.Text;

            // Una validaci�n b�sica para no hacer una consulta innecesaria a la DB.
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, ingrese usuario y contrase�a.", "Campos Vac�os", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Salimos del m�todo.
            }

            // 2. Creamos una instancia de nuestra clase de conexi�n.
            ConexionDB conexion = new ConexionDB();

            // 3. Declaramos las variables que nuestro m�todo 'ValidarUsuario' nos va a rellenar.
            string rolDelUsuario;
            string nombreDelUsuario;

            // 4. Llamamos al m�todo que ahora contiene TODA la l�gica de la base de datos.
            // F�jate c�mo el 'if' ahora es mucho m�s claro y descriptivo.
            if (conexion.ValidarUsuario(usuario, password, out rolDelUsuario, out nombreDelUsuario))
            {
                // --- LOGIN CORRECTO ---

                // 5. Guardamos los datos obtenidos en nuestra clase de sesi�n est�tica.
                SesionUsuario.IniciarSesion(nombreDelUsuario, rolDelUsuario);

                // Mostramos un mensaje de bienvenida personalizado.
                MessageBox.Show($"�Bienvenido, {SesionUsuario.NombreUsuario}!", "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Indicamos que el login fue exitoso y cerramos este formulario.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // --- LOGIN INCORRECTO ---
                // El m�todo ValidarUsuario devolvi� 'false'.
                MessageBox.Show("Usuario o contrase�a incorrectos.", "Error de Autenticaci�n", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUsuario.Focus();
            }
        }
    }
}
