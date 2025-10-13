using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CorteCheco.Modelos; // ¡La necesitamos para conocer la clase Producto!

namespace CorteCheco.Datos
{
    public class ConexionDB
    {
        // Corregido el nombre de la base de datos
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CorteCheco;Integrated Security=True";

        // (Los métodos AbrirConexion, CerrarConexion y ValidarUsuario se quedan igual que los tenías)
        #region Conexión y Usuarios (Sin Cambios)
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

        public bool ValidarUsuario(string usuario, string password, out string rol, out string nombreUsuario)
        {
            rol = null;
            nombreUsuario = null;
            SqlConnection conexion = null;
            try
            {
                conexion = AbrirConexion();
                string query = "SELECT Rol, NombreUsuario FROM Usuarios WHERE NombreUsuario = @usuario AND Password = @password";
                SqlCommand command = new SqlCommand(query, conexion);
                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@password", password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        rol = reader["Rol"].ToString();
                        nombreUsuario = reader["NombreUsuario"].ToString();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar usuario: " + ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conexion != null)
                {
                    CerrarConexion(conexion);
                }
            }
        }
        #endregion

        public List<Producto> GetProductos()
        {
            List<Producto> listaProductos = new List<Producto>();
            SqlConnection conexion = null;
            try
            {
                conexion = AbrirConexion();
                string query = "SELECT Id, Codigo, Nombre, Descripcion, Precio, Existencias FROM Productos";
                SqlCommand command = new SqlCommand(query, conexion);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaProductos.Add(new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Codigo = reader["Codigo"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Existencias = Convert.ToInt32(reader["Existencias"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener productos: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    CerrarConexion(conexion);
                }
            }
            return listaProductos;
        }

        // 'InsertarProducto' ahora recibe UN SOLO objeto Producto.
        public bool InsertarProducto(Producto producto)
        {
            SqlConnection conexion = null;
            try
            {
                conexion = AbrirConexion();
                string query = "INSERT INTO Productos (Codigo, Nombre, Descripcion, Precio, Existencias, Imagen) " +
                               "VALUES (@codigo, @nombre, @descripcion, @precio, @existencias, @imagen)";
                SqlCommand command = new SqlCommand(query, conexion);

                command.Parameters.AddWithValue("@codigo", producto.Codigo);
                command.Parameters.AddWithValue("@nombre", producto.Nombre);
                command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                command.Parameters.AddWithValue("@precio", producto.Precio);
                command.Parameters.AddWithValue("@existencias", producto.Existencias);
                command.Parameters.AddWithValue("@imagen", producto.Imagen ?? (object)DBNull.Value); // Manejo de imagen nula

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el producto: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexion != null)
                {
                    CerrarConexion(conexion);
                }
            }
        }

        // 'ActualizarProducto' ahora recibe UN SOLO objeto Producto.
        public bool ActualizarProducto(Producto producto)
        {
            SqlConnection conexion = null;
            try
            {
                conexion = AbrirConexion();
                string query = "UPDATE Productos SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, " +
                               "Precio = @precio, Existencias = @existencias, Imagen = @imagen WHERE Id = @id";
                SqlCommand command = new SqlCommand(query, conexion);

                command.Parameters.AddWithValue("@id", producto.Id);
                command.Parameters.AddWithValue("@codigo", producto.Codigo);
                command.Parameters.AddWithValue("@nombre", producto.Nombre);
                command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                command.Parameters.AddWithValue("@precio", producto.Precio);
                command.Parameters.AddWithValue("@existencias", producto.Existencias);
                command.Parameters.AddWithValue("@imagen", producto.Imagen ?? (object)DBNull.Value);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexion != null)
                {
                    CerrarConexion(conexion);
                }
            }
        }

        // (Los métodos GetImagenProductoPorId, EliminarProducto y BuscarProductos se quedan igual)
        #region Métodos sin Cambios (Por ahora)
        public byte[] GetImagenProductoPorId(int idProducto)
        {
            SqlConnection conexion = null;
            try
            {
                conexion = AbrirConexion();
                string query = "SELECT Imagen FROM Productos WHERE Id = @id";
                SqlCommand command = new SqlCommand(query, conexion);
                command.Parameters.AddWithValue("@id", idProducto);
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return (byte[])result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la imagen: " + ex.Message);
            }
            finally
            {
                if (conexion != null)
                {
                    CerrarConexion(conexion);
                }
            }
            return null;
        }

        public bool EliminarProducto(int id)
        {
            // ... tu código original aquí
            SqlConnection conexion = null;
            try
            {
                conexion = AbrirConexion();
                string query = "DELETE FROM Productos WHERE Id = @id";
                SqlCommand command = new SqlCommand(query, conexion);
                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexion != null)
                {
                    CerrarConexion(conexion);
                }
            }
        }

        public List<Producto> BuscarProductos(string terminoBusqueda)
        {
            List<Producto> listaProductos = new List<Producto>();
            SqlConnection conexion = null;
            try
            {
                conexion = AbrirConexion();
                string query = "SELECT Id, Codigo, Nombre, Descripcion, Precio, Existencias FROM Productos " +
                               "WHERE Nombre LIKE @busqueda OR Codigo LIKE @busqueda";

                // Usamos SqlCommand y SqlDataReader, igual que en GetProductos
                SqlCommand command = new SqlCommand(query, conexion);
                command.Parameters.AddWithValue("@busqueda", "%" + terminoBusqueda + "%");

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Por cada fila encontrada, creamos un objeto Producto y lo añadimos a la lista.
                        listaProductos.Add(new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Codigo = reader["Codigo"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Existencias = Convert.ToInt32(reader["Existencias"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion != null)
                {
                    CerrarConexion(conexion);
                }
            }
            return listaProductos;
        }
        #endregion
    }
}