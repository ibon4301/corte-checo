using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CorteCheco.Datos;
using CorteCheco.Modelos;

namespace CorteCheco.Vistas
{
    public partial class frmProductos : Form
    {
        // Variable a nivel de clase para "recordar" la imagen original durante la edición.
        private readonly ConexionDB _conexionDB = new ConexionDB();
        private byte[] imagenOriginal = null;

        public frmProductos()
        {
            InitializeComponent();
        }

        #region Eventos Principales del Formulario

        private void frmProductos_Load(object sender, EventArgs e)
        {
            // Configuramos el estado inicial de la interfaz.
            pnlDetalles.Visible = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            CargarProductos();
            CargarComboDepartamentos();
        }

        #endregion

        #region Métodos de Lógica de la Interfaz

        private void CargarComboDepartamentos()
        {
            cmbDepartamento.DataSource = _conexionDB.GetDepartamentos();
            cmbDepartamento.DisplayMember = "Nombre"; // Lo que el usuario ve
            cmbDepartamento.ValueMember = "Id";       // El valor que usamos internamente
            cmbDepartamento.SelectedIndex = -1;       // Empieza sin nada seleccionado
        }

        private void CargarProductos()
        {
            // 1. Obtenemos los datos y los asignamos a la tabla.
            dgvProductos.DataSource = new BindingList<Producto>(_conexionDB.GetProductos());

            // 2. Llamamos a nuestro método central para que aplique todo el formato.
            ConfigurarColumnasDGV();
        }

        private void CargarDetallesProducto()
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvProductos.SelectedRows[0];

                txtCodigo.Text = row.Cells["Codigo"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtDescripcion.Text = row.Cells["Descripcion"].Value.ToString();
                txtPrecio.Text = row.Cells["Precio"].Value.ToString();
                txtExistencias.Text = row.Cells["Existencias"].Value.ToString();

                // --- AQUÍ ESTÁ LA CORRECCIÓN ---

                // 1. Obtenemos el valor de la celda en una variable temporal.
                object idDeptoValue = row.Cells["IdDepartamento"].Value;

                // 2. Comprobamos si el valor es nulo o DBNull.Value.
                if (idDeptoValue != DBNull.Value && idDeptoValue != null)
                {
                    // Si SÍ tiene un valor, lo asignamos.
                    cmbDepartamento.SelectedValue = Convert.ToInt32(idDeptoValue);
                }
                else
                {
                    // Si es NULO, le decimos al ComboBox que no seleccione nada.
                    cmbDepartamento.SelectedIndex = -1;
                }


                int idProducto = Convert.ToInt32(row.Cells["Id"].Value);
                pnlDetalles.Tag = idProducto;

                // Cargamos la imagen y la guardamos en nuestra variable de respaldo.
                byte[] imagenBytes = _conexionDB.GetImagenProductoPorId(idProducto); // Usando _conexionDB
                imagenOriginal = imagenBytes;

                if (imagenBytes != null)
                {
                    using (MemoryStream ms = new MemoryStream(imagenBytes))
                    {
                        picProducto.Image = new Bitmap(Image.FromStream(ms));
                    }
                }
                else
                {
                    picProducto.Image = null;
                }
            }
        }

        private void LimpiarYResetearPanel()
        {
            pnlDetalles.Visible = false;
            pnlDetalles.Tag = null;
            imagenOriginal = null;

            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtExistencias.Clear();
            picProducto.Image = null;
            cmbDepartamento.SelectedIndex = -1; 

            dgvProductos.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnGuardar.Text = "Guardar";
        }

        #endregion

        #region Eventos de Botones y Controles

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarYResetearPanel(); // Usamos el método centralizado.
            pnlDetalles.Visible = true;
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                CargarDetallesProducto();
                pnlDetalles.Visible = true;
                btnGuardar.Text = "Guardar Cambios";
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto de la lista para editar.", "Sin Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarYResetearPanel();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones de datos de entrada.
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCodigo.Text))
                {
                    MessageBox.Show("El código y el nombre son campos obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || !int.TryParse(txtExistencias.Text, out int existencias))
                {
                    MessageBox.Show("El precio y las existencias deben ser números válidos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // --- NUEVA VALIDACIÓN PARA EL DEPARTAMENTO ---
                if (cmbDepartamento.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un departamento.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lógica de imagen inteligente (ya la tienes y es perfecta).
                byte[] imagenParaGuardar = null;
                if (picProducto.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        picProducto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imagenParaGuardar = ms.ToArray();
                    }
                }
                else if (pnlDetalles.Tag != null && imagenOriginal != null)
                {
                    imagenParaGuardar = imagenOriginal;
                }

                // Creamos el objeto Producto con todos los datos.
                Producto producto = new Producto
                {
                    Codigo = txtCodigo.Text,
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = precio,
                    Existencias = existencias,
                    Imagen = imagenParaGuardar,

                    // --- LÍNEA MODIFICADA PARA OBTENER EL ID DEL COMBOBOX ---
                    IdDepartamento = Convert.ToInt32(cmbDepartamento.SelectedValue)
                };

                bool exito;

                if (pnlDetalles.Tag == null) // Es un INSERT
                {
                    exito = _conexionDB.InsertarProducto(producto);
                }
                else // Es un UPDATE
                {
                    producto.Id = Convert.ToInt32(pnlDetalles.Tag);
                    exito = _conexionDB.ActualizarProducto(producto);
                }

                if (exito)
                {
                    MessageBox.Show("Operación realizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    LimpiarYResetearPanel();
                }
                else
                {
                    MessageBox.Show("La operación no se pudo completar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                int idProducto = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["Id"].Value);
                string nombreProducto = dgvProductos.SelectedRows[0].Cells["Nombre"].Value.ToString();
                DialogResult confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar el producto '{nombreProducto}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmacion == DialogResult.Yes)
                {
                    ConexionDB conexionDB = new ConexionDB();
                    if (conexionDB.EliminarProducto(idProducto))
                    {
                        MessageBox.Show("Producto eliminado con éxito.");
                        CargarProductos();
                        LimpiarYResetearPanel();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el producto.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // 1. Obtenemos el término de búsqueda de forma segura, eliminando espacios.
            string terminoBusqueda = txtBuscar.Text.Trim();
            List<Producto> resultados;

            // 2. Decidimos si buscar o si el usuario quiere ver todos los productos de nuevo.
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                // Si la barra de búsqueda está vacía, obtenemos la lista completa de productos.
                resultados = _conexionDB.GetProductos();
            }
            else
            {
                // Si hay texto, llamamos al método de búsqueda específico.
                resultados = _conexionDB.BuscarProductos(terminoBusqueda);
            }

            // 3. Asignamos la lista de resultados (ya sea completa o filtrada) al DataGridView.
            dgvProductos.DataSource = new BindingList<Producto>(resultados);

            //    Esto asegura que la tabla se vea igual tanto en la carga inicial como después de una búsqueda.
            ConfigurarColumnasDGV();
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBuscar_Click(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }

        private void picProducto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    picProducto.Image = Image.FromStream(stream);
                }
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                CargarDetallesProducto();
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
                pnlDetalles.Visible = true;
                btnGuardar.Text = "Guardar Cambios";
            }
        }


        private void ConfigurarColumnasDGV()
        {
            // Ocultamos las columnas que el usuario no necesita ver.
            if (dgvProductos.Columns.Contains("Id")) { dgvProductos.Columns["Id"].Visible = false; }
            if (dgvProductos.Columns.Contains("Imagen")) { dgvProductos.Columns["Imagen"].Visible = false; }
            if (dgvProductos.Columns.Contains("IdDepartamento")) { dgvProductos.Columns["IdDepartamento"].Visible = false; }

            // Nos aseguramos de que la columna con el nombre del departamento sea visible y tenga un buen encabezado.
            if (dgvProductos.Columns.Contains("NombreDepartamento"))
            {
                dgvProductos.Columns["NombreDepartamento"].HeaderText = "Departamento";
            }
        }
        #endregion
    }
}