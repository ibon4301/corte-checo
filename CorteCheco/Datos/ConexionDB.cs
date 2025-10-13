using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using CorteCheco.Modelos; 

namespace CorteCheco.Datos
{
    public class ConexionDB
    {
        
        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CorteCheco;Integrated Security=True";

        #region Usuarios
        public bool ValidarUsuario(string usuario, string password, out string rol, out string nombreUsuario)
        {
            rol = null;
            nombreUsuario = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT Rol, NombreUsuario FROM Usuarios WHERE NombreUsuario = @usuario AND Password = @password";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@password", password);

                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                rol = reader["Rol"].ToString();
                                nombreUsuario = reader["NombreUsuario"].ToString();
                                return true;
                            }
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al validar usuario: " + ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }
        #endregion

        #region Productos
        public List<Producto> GetProductos()
        {
            var listaProductos = new List<Producto>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Codigo, Nombre, Descripcion, Precio, Existencias, IdDepartamento FROM Productos";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
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
                                    Existencias = Convert.ToInt32(reader["Existencias"]),
                                    // Leemos el IdDepartamento, manejando posibles nulos.
                                    IdDepartamento = reader["IdDepartamento"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["IdDepartamento"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener productos: " + ex.Message);
                    }
                }
            }
            return listaProductos;
        }

        public bool InsertarProducto(Producto producto)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Productos (Codigo, Nombre, Descripcion, Precio, Existencias, Imagen, IdDepartamento) " +
                               "VALUES (@codigo, @nombre, @descripcion, @precio, @existencias, @imagen, @idDepartamento)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@existencias", producto.Existencias);
                    cmd.Parameters.AddWithValue("@imagen", producto.Imagen ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@idDepartamento", producto.IdDepartamento ?? (object)DBNull.Value);

                    try
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al insertar el producto: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool ActualizarProducto(Producto producto)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Productos SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, " +
                               "Precio = @precio, Existencias = @existencias, Imagen = @imagen, IdDepartamento = @idDepartamento WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", producto.Id);
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@existencias", producto.Existencias);
                    cmd.Parameters.AddWithValue("@imagen", producto.Imagen ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@idDepartamento", producto.IdDepartamento ?? (object)DBNull.Value);

                    try
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar el producto: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public byte[] GetImagenProductoPorId(int idProducto)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string query = "SELECT Imagen FROM Productos WHERE Id = @id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", idProducto);
                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? (byte[])result : null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener la imagen: " + ex.Message);
                        return null;
                    }
                }
            }
        }

        public bool EliminarProducto(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Productos WHERE Id = @id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public List<Producto> BuscarProductos(string terminoBusqueda)
        {
            var listaProductos = new List<Producto>();
            using (var con = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Codigo, Nombre, Descripcion, Precio, Existencias, IdDepartamento FROM Productos " +
                               "WHERE Nombre LIKE @busqueda OR Codigo LIKE @busqueda";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@busqueda", "%" + terminoBusqueda + "%");
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
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
                                    Existencias = Convert.ToInt32(reader["Existencias"]),
                                    IdDepartamento = reader["IdDepartamento"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["IdDepartamento"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar productos: " + ex.Message);
                    }
                }
            }
            return listaProductos;
        }
        #endregion

        #region Departamentos (¡NUEVO!)
        public List<Departamento> GetDepartamentos()
        {
            var departamentos = new List<Departamento>();
            using (var con = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Nombre FROM Departamentos ORDER BY Nombre";
                using (var cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                departamentos.Add(new Departamento
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Nombre = reader["Nombre"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener departamentos: " + ex.Message);
                    }
                }
            }
            return departamentos;
        }

        public bool InsertarDepartamento(Departamento depto)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Departamentos (Nombre) VALUES (@Nombre)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", depto.Nombre);
                    try
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al insertar departamento: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool ActualizarDepartamento(Departamento depto)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Departamentos SET Nombre = @Nombre WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", depto.Nombre);
                    cmd.Parameters.AddWithValue("@Id", depto.Id);
                    try
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar departamento: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool EliminarDepartamento(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Departamentos WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar departamento: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        #endregion
    }
}