using CorteCheco.Datos;
using System.Data.SqlClient;

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
            // Creamos una instancia de nuestra clase de conexi�n para usar sus m�todos.
            ConexionDB conexionDB = new ConexionDB();
            SqlConnection conexion = null; // Inicializamos la conexi�n como nula para poder usarla en el 'finally'.

            try
            {
                // Abrimos la conexi�n usando el m�todo de nuestra clase.
                conexion = conexionDB.AbrirConexion();

                // Creamos el comando SQL. Es MUY IMPORTANTE usar par�metros (@usuario, @password)
                // para prevenir un ataque de seguridad llamado "Inyecci�n SQL".
                string query = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @usuario AND Password = @password";

                SqlCommand command = new SqlCommand(query, conexion);
                // Asignamos los valores de los TextBox a los par�metros del query.
                command.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                command.Parameters.AddWithValue("@password", txtPassword.Text);

                // ExecuteScalar se usa cuando la consulta devuelve un �nico valor (en este caso, un n�mero).
                // Convertimos el resultado a un entero.
                int count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    // --- LOGIN CORRECTO ---
                    // El usuario y la contrase�a existen en la base de datos.
                    MessageBox.Show("�Bienvenido!", "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // --- LOGIN INCORRECTO ---
                    // No se encontr� ninguna coincidencia.
                    MessageBox.Show("Usuario o contrase�a incorrectos.", "Error de Autenticaci�n", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                // Si ocurre cualquier error con la base de datos, lo capturamos y mostramos.
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error Cr�tico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // El bloque 'finally' se ejecuta SIEMPRE, tanto si hay �xito como si hay error.
                // Es el lugar perfecto para asegurarnos de que la conexi�n SIEMPRE se cierre.
                if (conexion != null)
                {
                    conexionDB.CerrarConexion(conexion);
                }
            }
        }
    }
}
