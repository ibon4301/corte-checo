using System.Data;
using System.Data.SqlClient; // ¡Muy importante! Este es el proveedor para SQL Server.

namespace CorteCheco.Datos
{
    public class ConexionDB
    {
        // Pega aquí la cadena de conexión que copiaste de las propiedades.
        // Esto debes verlo buscando en la ventana de propiedades al hacer click en la base de datos en el explorador de servidores.
        // Busca "Cadena de conexión" y cópiala aquí.
        // ¡¡¡IMPORTANTE!!! SI VES ERRORES DE COMPILACION EN "SqlConnection" O "ConnectionState", ES QUE TE FALTA EL PAQUETE NUGET "System.Data.SqlClient"
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CorteChecoDB;Integrated Security=True";

        // Este método abrirá la conexión a la base de datos.
        public SqlConnection AbrirConexion()
        {
            SqlConnection conexion = new SqlConnection(connectionString);
            if (conexion.State == ConnectionState.Closed)
            {
                conexion.Open();
            }
            return conexion;
        }

        // Este método cerrará la conexión.
        public void CerrarConexion(SqlConnection conexion)
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }
    }
}