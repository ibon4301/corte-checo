using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CorteCheco.Datos;
using CorteCheco.Modelos;

namespace CorteCheco.Vistas
{
    public partial class frmDashboard : Form
    {
        private readonly ConexionDB _conexionDB;
        private List<Producto> _listaCompletaProductos;


        public frmDashboard()
        {
            InitializeComponent();
            _conexionDB = new ConexionDB();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            CargarDatosCompletos();
            CargarDepartamentosEnCombo();

            // Mostramos todos los productos la primera vez.
            MostrarProductosEnTarjetas(_listaCompletaProductos);
        }
        private void CargarDatosCompletos()
        {
            // Llenamos nuestra lista "caché" con todos los productos y sus imágenes.
            _listaCompletaProductos = _conexionDB.GetProductos();
            foreach (var producto in _listaCompletaProductos)
            {
                producto.Imagen = _conexionDB.GetImagenProductoPorId(producto.Id);
            }
        }

        // Nuevo método para poblar el ComboBox de departamentos.
        private void CargarDepartamentosEnCombo()
        {
            var departamentos = _conexionDB.GetDepartamentos();

            // ¡Truco de UX! Añadimos un item "Todos" al principio de la lista.
            // Esto permitirá al usuario limpiar el filtro y ver todos los productos de nuevo.
            departamentos.Insert(0, new Departamento { Id = 0, Nombre = "Todas las categorías" });

            cmbDepartamentos.DataSource = departamentos;
            cmbDepartamentos.DisplayMember = "Nombre"; // La parte que ve el usuario.
            cmbDepartamentos.ValueMember = "Id";       // El valor interno que usaremos (el ID).
        }

        // Este método ahora solo se encarga de MOSTRAR, no de cargar.
        private void MostrarProductosEnTarjetas(List<Producto> productosAMostrar)
        {
            flpDashboard.Controls.Clear();
            foreach (var producto in productosAMostrar)
            {
                var card = new ucProductoCard();
                card.SetData(producto);
                flpDashboard.Controls.Add(card);
            }
        }

        private void AplicarFiltros() // Si el tuyo tiene (object sender, EventArgs e), déjalo así.
        {
            var productosFiltrados = _listaCompletaProductos;

            // Obtenemos el item seleccionado y lo convertimos de forma segura a un objeto Departamento.
            var departamentoSeleccionado = cmbDepartamentos.SelectedItem as Departamento;

            // Comprobamos que no sea nulo y que su ID no sea 0 (el de "Todas las categorías").
            if (departamentoSeleccionado != null && departamentoSeleccionado.Id != 0)
            {
                int idDepartamentoSeleccionado = departamentoSeleccionado.Id; // Ahora sí obtenemos el ID.
                productosFiltrados = productosFiltrados
                                     .Where(p => p.IdDepartamento == idDepartamentoSeleccionado)
                                     .ToList();
            }

            // (Asegúrate de tener un txtBuscar y un btnBuscar si sigues el plan anterior)
            string textoBusqueda = txtBuscar.Text.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                productosFiltrados = productosFiltrados
                                     .Where(p => p.Nombre.ToLower().Contains(textoBusqueda) ||
                                                 p.Codigo.ToLower().Contains(textoBusqueda))
                                     .ToList();
            }

            MostrarProductosEnTarjetas(productosFiltrados);
        }

        private void cmbDepartamentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AplicarFiltros();
                e.SuppressKeyPress = true;
            }
        }
    }
}