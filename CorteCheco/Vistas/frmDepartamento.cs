using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CorteCheco.Datos;      
using CorteCheco.Modelos;    

namespace CorteCheco.Vistas
{
    public partial class frmDepartamento : Form
    {
        private readonly ConexionDB _conexionDB = new ConexionDB();
        private Departamento _deptoSeleccionado = null;

        public frmDepartamento()
        {
            InitializeComponent();
        }

        private void frmDepartamentos_Load(object sender, EventArgs e)
        {
            // Al cargar el formulario, lo primero es mostrar los datos.
            CargarDepartamentos();
            ConfigurarDataGridView();
            ActualizarEstadoBotones();
        }

        private void CargarDepartamentos()
        {
            dgvDepartamentos.DataSource = null;

            // Asignamos la lista de departamentos. Como AutoGenerateColumns es True,
            // se crearán las columnas 'Id' y 'Nombre' automáticamente.
            dgvDepartamentos.DataSource = _conexionDB.GetDepartamentos();

            // Ahora que las columnas existen, las configuramos.
            ConfigurarDataGridView();
        }

        private void ConfigurarDataGridView()
        {
            // Solo procedemos si la tabla tiene columnas.
            if (dgvDepartamentos.Columns.Count > 0)
            {
                // Nos aseguramos de que la columna 'Id' sea visible y tenga un buen encabezado.
                if (dgvDepartamentos.Columns.Contains("Id"))
                {
                    dgvDepartamentos.Columns["Id"].Visible = true;
                    dgvDepartamentos.Columns["Id"].HeaderText = "ID";
                    // Damos un tamaño fijo al ID para que no ocupe mucho espacio.
                    dgvDepartamentos.Columns["Id"].Width = 60;
                }

                // Configuramos la columna 'Nombre'.
                if (dgvDepartamentos.Columns.Contains("Nombre"))
                {
                    dgvDepartamentos.Columns["Nombre"].HeaderText = "Nombre del Departamento";
                    // Hacemos que la columna de nombre rellene el resto del espacio.
                    dgvDepartamentos.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // --- 1. Validación  ---
            if (string.IsNullOrWhiteSpace(txtNombreDepartamento.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del departamento.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreDepartamento.Focus();
                return;
            }

            // --- 2. Preparación del Objeto  ---
            Departamento depto = new Departamento
            {
                Nombre = txtNombreDepartamento.Text.Trim()
            };

            bool exito;

            // --- 3. Lógica de Decisión: ¿Crear o Actualizar?  ---
            if (_deptoSeleccionado == null)
            {
                // MODO CREACIÓN
                exito = _conexionDB.InsertarDepartamento(depto);
            }
            else
            {
                // MODO EDICIÓN
                depto.Id = _deptoSeleccionado.Id;
                exito = _conexionDB.ActualizarDepartamento(depto);
            }

            // --- 4. Resultado y Actualización de la UI ---
            if (exito)
            {
                MessageBox.Show("Operación completada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // --- 5. Limpieza y Reset del Formulario ---
                CargarDepartamentos();      // Refrescamos la lista para ver los cambios.

                // ¡Usamos nuestro método centralizado para limpiar y restablecer todo!
                ResetearFormulario();
            }
        }

        private void btnNuevoDepartamento_Click(object sender, EventArgs e)
        {
            CargarDepartamentos();

            pnlEdicion.Visible = true;

            txtNombreDepartamento.Clear();

            txtNombreDepartamento.Focus();
            dgvDepartamentos.ClearSelection();
            pnlEdicion.Visible = true;

            _deptoSeleccionado = null;
            dgvDepartamentos.ClearSelection();

            // 2. Llamamos a nuestro método central en "Modo Creación".
            ActivarPanelEdicion(esModoEdicion: false);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            dgvDepartamentos.ClearSelection(); // Deseleccionamos cualquier fila.
            pnlEdicion.Visible = false;

            // 1. Ocultamos el panel de edición.
            pnlEdicion.Visible = false;

            // 2. Limpiamos el campo de texto.
            txtNombreDepartamento.Clear();
            ResetearFormulario();
        }

        private void dgvDepartamentos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // 1. Obtenemos los datos de la fila seleccionada.
                int idDepto = Convert.ToInt32(dgvDepartamentos.Rows[e.RowIndex].Cells["Id"].Value);
                string nombreDepto = dgvDepartamentos.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();

                // 2. Creamos un objeto Departamento y lo asignamos a _deptoSeleccionado.
                // ¡Este es el paso que activa el "modo edición"!
                _deptoSeleccionado = new Departamento { Id = idDepto, Nombre = nombreDepto };

                // 3. Poblamos el formulario con los datos para que el usuario pueda editar.
                txtNombreDepartamento.Text = nombreDepto;

                // 4. Mostramos el panel de edición.
                pnlEdicion.Visible = true;
                txtNombreDepartamento.Focus();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Primero, verificamos que haya una fila seleccionada en el DataGridView.
            if (dgvDepartamentos.SelectedRows.Count > 0)
            {
                // Obtenemos el ID del departamento de la fila seleccionada.
                int idAEliminar = Convert.ToInt32(dgvDepartamentos.SelectedRows[0].Cells["Id"].Value);
                string nombreDepto = dgvDepartamentos.SelectedRows[0].Cells["Nombre"].Value.ToString();

                // Pedimos confirmación al usuario. ¡Esto es vital para una buena UX!
                var confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar el departamento '{nombreDepto}'?",
                                                   "Confirmar Eliminación",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    bool exito = _conexionDB.EliminarDepartamento(idAEliminar);

                    if (exito)
                    {
                        MessageBox.Show("Departamento eliminado con éxito.", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDepartamentos(); // Refrescamos la tabla.
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione el departamento que desea eliminar.", "Ninguna Selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarEstadoBotones()
        {
            // Comprobamos si hay al menos una fila seleccionada.
            // dgvDepartamentos.SelectedRows.Count es la forma más fiable de saberlo.
            if (dgvDepartamentos.SelectedRows.Count > 0)
            {
                // Si hay selección, habilitamos los botones.
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
            }
            else
            {
                // Si no hay selección, los deshabilitamos.
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
            }
        }

        private void frmDepartamento_Load(object sender, EventArgs e)
        {
            CargarDepartamentos();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void dgvDepartamentos_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarEstadoBotones();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Primero, nos aseguramos de que realmente hay una fila seleccionada.
            // Aunque el botón debería estar deshabilitado, esta es una buena práctica (defensive coding).
            if (dgvDepartamentos.SelectedRows.Count > 0)
            {
                // 1. Obtenemos los datos de la fila seleccionada.
                int idDepto = Convert.ToInt32(dgvDepartamentos.SelectedRows[0].Cells["Id"].Value);
                string nombreDepto = dgvDepartamentos.SelectedRows[0].Cells["Nombre"].Value.ToString();

                // 2. ¡EL PASO CLAVE! Asignamos los datos al objeto _deptoSeleccionado.
                // Ahora la aplicación "sabe" que estamos editando este departamento específico.
                _deptoSeleccionado = new Departamento { Id = idDepto, Nombre = nombreDepto };

                // 3. Preparamos y mostramos el panel de edición con los datos cargados.
                ActivarPanelEdicion(esModoEdicion: true);
            }
        }

        private void ActivarPanelEdicion(bool esModoEdicion)
        {
            if (esModoEdicion)
            {
                // --- Configuración para MODO EDICIÓN ---
                // Ponemos el nombre del departamento seleccionado en el TextBox.
                txtNombreDepartamento.Text = _deptoSeleccionado.Nombre;
                // Cambiamos el texto del botón para que sea más descriptivo.
                btnGuardar.Text = "Guardar Cambios";
            }
            else
            {
                // --- Configuración para MODO CREACIÓN ---
                txtNombreDepartamento.Clear();
                btnGuardar.Text = "Guardar";
            }

            // Estas líneas son comunes para ambos modos.
            pnlEdicion.Visible = true;
            txtNombreDepartamento.Focus();
        }

        private void ResetearFormulario()
        {
            txtNombreDepartamento.Clear();
            pnlEdicion.Visible = false;
            _deptoSeleccionado = null;
            btnGuardar.Text = "Guardar"; // Restablecemos el texto original del botón.
            dgvDepartamentos.ClearSelection(); // Esto a su vez llamará a ActualizarEstadoBotones().
        }

        private void btnBuscarDepto_Click(object sender, EventArgs e)
        {
            string terminoBusqueda = txtBuscarDepto.Text.Trim();
            List<Departamento> resultados;

            if (string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                resultados = _conexionDB.GetDepartamentos();
            }
            else
            {
                resultados = _conexionDB.BuscarDepartamentos(terminoBusqueda);
            }

            // El truco para evitar el error: resetea el DataSource ANTES de reasignarlo.
            dgvDepartamentos.DataSource = null;
            dgvDepartamentos.DataSource = resultados;

            // Reaplicamos el formato a las columnas recién generadas.
            ConfigurarDataGridView();
        }

        private void txtBuscarDepto_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                btnBuscarDepto_Click(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }
    }

}
