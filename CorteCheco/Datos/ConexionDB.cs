using System.Data;
using System.Data.SqlClient; // ¡Muy importante! Este es el proveedor para SQL Server.

namespace CorteCheco.Datos
{
    public class ConexionDB
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CorteChecoDB;Integrated Security=True";

        // --- TU CÓDIGO ACTUAL (ESTÁ PERFECTO) ---
        public SqlConnection AbrirConexion()
        {
            SqlConnection conexion = new SqlConnection(connectionString);
            if (conexion.State == ConnectionState.Closed)
            {
                conexion.Open();
            }
            return conexion;
        }

        public void CerrarConexion(SqlConnection conexion)
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }
        // --- FIN DE TU CÓDIGO ACTUAL ---


        // --- AÑADE ESTE NUEVO MÉTODO AQUÍ DENTRO DE LA CLASE ---
        public bool ValidarUsuario(string nombreUsuario, string password, out string rol, out string nombre)
        {
            rol = null;
            nombre = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT NombreUsuario, Rol FROM Usuarios WHERE NombreUsuario = @user AND Password = @pass";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@user", nombreUsuario);
                    cmd.Parameters.AddWithValue("@pass", password); // En un futuro, esto debe ser un hash seguro.

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) // Si la consulta devuelve filas, el usuario existe.
                        {
                            reader.Read(); // Leemos la primera fila.
                            nombre = reader["NombreUsuario"].ToString();
                            rol = reader["Rol"].ToString();
                            return true; // Login exitoso.
                        }
                        else
                        {
                            return false; // Login fallido.
                        }
                    }
                }
            }
        }
    }
}