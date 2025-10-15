using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using CorteCheco.Modelos; 

namespace CorteCheco.Datos
{
    public class ConexionDB
    {

        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CorteCheco;Integrated Security=True;Encrypt=False";
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


        // --- PEGA ESTOS DOS MÉTODOS DENTRO DE TU CLASE ConexionDB ---
        // --- PUEDEN IR JUSTO DEBAJO DE TU MÉTODO ValidarUsuario ---

        /// <summary>
        /// Verifica si un nombre de usuario ya existe en la base de datos.
        /// </summary>
        /// <param name="nombreUsuario">El nombre de usuario a verificar.</param>
        /// <returns>True si el usuario ya existe, false en caso contrario.</returns>
        public bool UsuarioYaExiste(string nombreUsuario)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario.Trim());
                    try
                    {
                        con.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al verificar usuario: " + ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Inserta un nuevo usuario en la base de datos con el rol fijo 'Usuario' y contraseña en texto plano.
        /// </summary>
        /// <param name="nombreUsuario">El nombre del nuevo usuario.</param>
        /// <param name="contraseña">La contraseña en texto plano.</param>
        /// <returns>True si el registro fue exitoso, false si falló.</returns>
        public bool RegistrarUsuario(string nombreUsuario, string contraseña)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Usuarios (NombreUsuario, Password, Rol) VALUES (@NombreUsuario, @Password, 'Usuario')";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario.Trim());
                    cmd.Parameters.AddWithValue("@Password", contraseña); // Guardamos la contraseña tal cual

                    try
                    {
                        con.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al registrar usuario: " + ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }


        #endregion

        #region Productos

        public Producto GetProductoPorId(int id)
        {
            Producto producto = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                // Usamos la misma consulta JOIN para obtener también el nombre del departamento
                string query = @"
            SELECT p.*, ISNULL(d.Nombre, 'Sin Departamento') AS NombreDepartamento
            FROM Productos p
            LEFT JOIN Departamentos d ON p.IdDepartamento = d.Id
            WHERE p.Id = @id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                producto = new Producto
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Codigo = reader["Codigo"].ToString(),
                                    Nombre = reader["Nombre"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"]),
                                    Existencias = Convert.ToInt32(reader["Existencias"]),
                                    IdDepartamento = reader["IdDepartamento"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["IdDepartamento"]),
                                    NombreDepartamento = reader["NombreDepartamento"].ToString(),
                                    // Leemos la imagen directamente
                                    Imagen = reader["Imagen"] == DBNull.Value ? null : (byte[])reader["Imagen"]
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener el producto: " + ex.Message);
                    }
                }
            }
            return producto;
        }

        public List<Producto> GetProductos()
        {
            var listaProductos = new List<Producto>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                // --- CONSULTA MODIFICADA CON INNER JOIN ---
                string query = @"
            SELECT 
                p.Id, p.Codigo, p.Nombre, p.Descripcion, p.Precio, p.Existencias, p.IdDepartamento,
                ISNULL(d.Nombre, 'Sin Departamento') AS NombreDepartamento
            FROM 
                Productos p
            LEFT JOIN 
                Departamentos d ON p.IdDepartamento = d.Id";

                // Nota: Usamos LEFT JOIN en lugar de INNER JOIN. Esto asegura que si un producto
                // por alguna razón NO tiene un departamento asignado, AÚN APARECERÁ en la lista.
                // ISNULL() se encarga de mostrar "Sin Departamento" en esos casos.

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
                                    IdDepartamento = reader["IdDepartamento"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["IdDepartamento"]),

                                    // --- LECTURA DEL NUEVO CAMPO ---
                                    NombreDepartamento = reader["NombreDepartamento"].ToString()
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
                string query = @"
            INSERT INTO Productos (Codigo, Nombre, Descripcion, Precio, Existencias, Imagen, IdDepartamento)
            VALUES (@codigo, @nombre, @descripcion, @precio, @existencias, @imagen, @idDepartamento)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@existencias", producto.Existencias);
                    cmd.Parameters.AddWithValue("@idDepartamento", producto.IdDepartamento ?? (object)DBNull.Value);

                    // 🔹 Aquí especificamos explícitamente el tipo de dato binario
                    var imagenParam = cmd.Parameters.Add("@imagen", System.Data.SqlDbType.VarBinary);
                    imagenParam.Value = (object)producto.Imagen ?? DBNull.Value;

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
                string query = @"
            UPDATE Productos
            SET Codigo = @codigo,
                Nombre = @nombre,
                Descripcion = @descripcion,
                Precio = @precio,
                Existencias = @existencias,
                Imagen = @imagen,
                IdDepartamento = @idDepartamento
            WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", producto.Id);
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@existencias", producto.Existencias);
                    cmd.Parameters.AddWithValue("@idDepartamento", producto.IdDepartamento ?? (object)DBNull.Value);

                    // 🔹 Parche crítico: forzamos el tipo binario
                    var imagenParam = cmd.Parameters.Add("@imagen", System.Data.SqlDbType.VarBinary);
                    imagenParam.Value = (object)producto.Imagen ?? DBNull.Value;

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


        public List<Departamento> BuscarDepartamentos(string terminoBusqueda)
        {
            var departamentos = new List<Departamento>();
            using (var con = new SqlConnection(_connectionString))
            {
                // La consulta busca cualquier departamento cuyo nombre contenga el texto buscado.
                string query = "SELECT Id, Nombre FROM Departamentos WHERE Nombre LIKE @busqueda ORDER BY Nombre";

                using (var cmd = new SqlCommand(query, con))
                {
                    // Usamos parámetros para evitar inyección SQL. Es la forma segura.
                    cmd.Parameters.AddWithValue("@busqueda", "%" + terminoBusqueda + "%");
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
                        MessageBox.Show("Error al buscar departamentos: " + ex.Message);
                    }
                }
            }
            return departamentos;
        }

        public List<Producto> BuscarProductos(string terminoBusqueda)
        {
            var listaProductos = new List<Producto>();
            using (var con = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                p.Id, p.Codigo, p.Nombre, p.Descripcion, p.Precio, p.Existencias, p.IdDepartamento,
                ISNULL(d.Nombre, 'Sin Departamento') AS NombreDepartamento
            FROM 
                Productos p
            LEFT JOIN 
                Departamentos d ON p.IdDepartamento = d.Id
            WHERE 
                p.Nombre LIKE @busqueda OR 
                p.Codigo LIKE @busqueda OR
                d.Nombre LIKE @busqueda";

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
                                    IdDepartamento = reader["IdDepartamento"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["IdDepartamento"]),
                                    NombreDepartamento = reader["NombreDepartamento"].ToString()
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

        #region Departamentos
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