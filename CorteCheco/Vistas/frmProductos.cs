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
        // Usamos una única instancia de ConexionDB para todo el formulario.
        private readonly ConexionDB _conexionDB = new ConexionDB();

        // Esta variable "recuerda" qué producto estamos editando. Si es null, estamos creando uno nuevo.
        private Producto _productoSeleccionado = null;

        public frmProductos()
        {
            InitializeComponent();
        }

        #region Eventos Principales y de Carga

        private void frmProductos_Load(object sender, EventArgs e)
        {
            // Estado inicial de la interfaz.
            pnlDetalles.Visible = false;
            CargarProductos();
            CargarComboDepartamentos();
            // Los botones se deshabilitan a través del evento SelectionChanged que se dispara al cargar.
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            // Este es el ÚNICO lugar que controla la habilitación de los botones de edición/eliminación.
            if (dgvProductos.SelectedRows.Count > 0)
            {
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
            }
            else
            {
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
            }
        }

        #endregion

        #region Métodos de Lógica de la Interfaz

        private void CargarComboDepartamentos()
        {
            cmbDepartamento.DataSource = _conexionDB.GetDepartamentos();
            cmbDepartamento.DisplayMember = "Nombre";
            cmbDepartamento.ValueMember = "Id";
            cmbDepartamento.SelectedIndex = -1;
        }

        private void CargarProductos()
        {
            dgvProductos.DataSource = null; // Reseteo para asegurar la recarga correcta.
            dgvProductos.DataSource = new BindingList<Producto>(_conexionDB.GetProductos());
            ConfigurarColumnasDGV();
        }

        private void ConfigurarColumnasDGV()
        {
            if (dgvProductos.Columns.Count > 0)
            {
                if (dgvProductos.Columns.Contains("Id")) { dgvProductos.Columns["Id"].Visible = false; }
                if (dgvProductos.Columns.Contains("Imagen")) { dgvProductos.Columns["Imagen"].Visible = false; }
                if (dgvProductos.Columns.Contains("IdDepartamento")) { dgvProductos.Columns["IdDepartamento"].Visible = false; }

                if (dgvProductos.Columns.Contains("NombreDepartamento"))
                {
                    dgvProductos.Columns["NombreDepartamento"].HeaderText = "Departamento";
                }
            }
        }

        private void LimpiarYResetearPanel()
        {
            pnlDetalles.Visible = false;
            _productoSeleccionado = null; // ¡Crucial! Reseteamos el estado a "Modo Creación".

            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtExistencias.Clear();
            picProducto.Image = null;
            cmbDepartamento.SelectedIndex = -1;

            dgvProductos.ClearSelection(); // Esto dispara SelectionChanged y deshabilita los botones.
            btnGuardar.Text = "Guardar";
        }

        #endregion

        #region Eventos de Botones y Controles

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarYResetearPanel();
            pnlDetalles.Visible = true;
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                // Obtenemos el ID de la fila seleccionada.
                int idProducto = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["Id"].Value);

                // Obtenemos el objeto completo desde la base de datos.
                _productoSeleccionado = _conexionDB.GetProductoPorId(idProducto);

                if (_productoSeleccionado != null)
                {
                    // Llenamos el panel con los datos del objeto.
                    txtCodigo.Text = _productoSeleccionado.Codigo;
                    txtNombre.Text = _productoSeleccionado.Nombre;
                    txtDescripcion.Text = _productoSeleccionado.Descripcion;
                    txtPrecio.Text = _productoSeleccionado.Precio.ToString("F2"); // Formato con 2 decimales.
                    txtExistencias.Text = _productoSeleccionado.Existencias.ToString();

                    if (_productoSeleccionado.IdDepartamento.HasValue)
                    {
                        cmbDepartamento.SelectedValue = _productoSeleccionado.IdDepartamento.Value;
                    }
                    else
                    {
                        cmbDepartamento.SelectedIndex = -1;
                    }

                    if (_productoSeleccionado.Imagen != null)
                    {
                        using (MemoryStream ms = new MemoryStream(_productoSeleccionado.Imagen))
                        {
                            picProducto.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        picProducto.Image = null;
                    }

                    pnlDetalles.Visible = true;
                    btnGuardar.Text = "Guardar Cambios";
                }
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
                // 1. VALIDACIÓN
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
                if (cmbDepartamento.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un departamento.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // =========================================================================
                // === ¡NUEVA VALIDACIÓN DE IMAGEN OBLIGATORIA AL CREAR! ===
                // =========================================================================
                // Solo aplicamos esta regla si estamos en modo de creación (_productoSeleccionado es null)
                if (_productoSeleccionado == null && picProducto.Image == null)
                {
                    MessageBox.Show("Para crear un nuevo producto, es obligatorio seleccionar una imagen.",
                                    "Imagen Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Detenemos el proceso de guardado aquí.
                }
                // =========================================================================


                // 2. PREPARACIÓN DE IMAGEN
                byte[] imagenParaGuardar = null;
                if (picProducto.Image != null)
                {
                    using (var ms = new MemoryStream())
                    using (var bmp = new Bitmap(picProducto.Image))
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imagenParaGuardar = ms.ToArray();
                    }
                }


                bool exito;

                // 3. DECISIÓN: CREAR O EDITAR
                if (_productoSeleccionado == null) // MODO CREACIÓN
                {
                    Producto productoNuevo = new Producto
                    {
                        Codigo = txtCodigo.Text.Trim(),
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        Precio = precio,
                        Existencias = existencias,
                        IdDepartamento = Convert.ToInt32(cmbDepartamento.SelectedValue),
                        Imagen = imagenParaGuardar // Gracias a la validación, sabemos que aquí no será null
                    };
                    exito = _conexionDB.InsertarProducto(productoNuevo);
                }
                else // MODO EDICIÓN
                {
                    _productoSeleccionado.Codigo = txtCodigo.Text.Trim();
                    _productoSeleccionado.Nombre = txtNombre.Text.Trim();
                    _productoSeleccionado.Descripcion = txtDescripcion.Text.Trim();
                    _productoSeleccionado.Precio = precio;
                    _productoSeleccionado.Existencias = existencias;
                    _productoSeleccionado.IdDepartamento = Convert.ToInt32(cmbDepartamento.SelectedValue);

                    // Tu lógica para actualizar la imagen al editar es correcta.
                    // Si el PictureBox tiene una imagen, la actualizamos.
                    if (picProducto.Image != null)
                    {
                        _productoSeleccionado.Imagen = imagenParaGuardar;
                    }
                    else // Si el PictureBox está vacío, borramos la imagen.
                    {
                        _productoSeleccionado.Imagen = null;
                    }

                    exito = _conexionDB.ActualizarProducto(_productoSeleccionado);
                }

                // 4. RESULTADO Y LIMPIEZA
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
                MessageBox.Show("Ocurrió un error crítico: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (_conexionDB.EliminarProducto(idProducto))
                    {
                        MessageBox.Show("Producto eliminado con éxito.");
                        CargarProductos();
                        // LimpiarYResetearPanel se llama implícitamente al limpiar la selección.
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el producto.");
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string terminoBusqueda = txtBuscar.Text.Trim();
            List<Producto> resultados;

            if (string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                resultados = _conexionDB.GetProductos();
            }
            else
            {
                resultados = _conexionDB.BuscarProductos(terminoBusqueda);
            }

            dgvProductos.DataSource = null; // Reseteo.
            dgvProductos.DataSource = new BindingList<Producto>(resultados);
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
                try
                {
                    // Creamos una copia de la imagen en memoria para evitar bloqueos de archivo.
                    // Esto es mucho más seguro que cargar directamente desde el FileStream.
                    using (var bmpTemp = new Bitmap(openFileDialog.FileName))
                    {
                        picProducto.Image = new Bitmap(bmpTemp);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error de Archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}