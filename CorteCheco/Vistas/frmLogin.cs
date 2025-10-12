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

        // En frmLogin.cs, reemplaza tu método btnIngresar_Click con este:

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // 1. Obtenemos los datos de la interfaz.
            string usuario = txtUsuario.Text;
            string password = txtPassword.Text;

            // Una validación básica para no hacer una consulta innecesaria a la DB.
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Salimos del método.
            }

            // 2. Creamos una instancia de nuestra clase de conexión.
            ConexionDB conexion = new ConexionDB();

            // 3. Declaramos las variables que nuestro método 'ValidarUsuario' nos va a rellenar.
            string rolDelUsuario;
            string nombreDelUsuario;

            // 4. Llamamos al método que ahora contiene TODA la lógica de la base de datos.
            // Fíjate cómo el 'if' ahora es mucho más claro y descriptivo.
            if (conexion.ValidarUsuario(usuario, password, out rolDelUsuario, out nombreDelUsuario))
            {
                // --- LOGIN CORRECTO ---

                // 5. Guardamos los datos obtenidos en nuestra clase de sesión estática.
                SesionUsuario.IniciarSesion(nombreDelUsuario, rolDelUsuario);

                // Mostramos un mensaje de bienvenida personalizado.
                MessageBox.Show($"¡Bienvenido, {SesionUsuario.NombreUsuario}!", "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Indicamos que el login fue exitoso y cerramos este formulario.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // --- LOGIN INCORRECTO ---
                // El método ValidarUsuario devolvió 'false'.
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUsuario.Focus();
            }
        }
    }
}
