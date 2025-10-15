using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CorteCheco.Modelos;

namespace CorteCheco.Vistas
{
    public partial class ucProductoCard : UserControl
    {
        public ucProductoCard()
        {
            InitializeComponent();
        }

        public void SetData(Producto producto)
        {
            lblNombre.Text = producto.Nombre;
            lblPrecio.Text = $"{producto.Precio:C}"; // ":C" le da formato de moneda automáticamente.
            lblStock.Text = $"Existencias: {producto.Existencias}";

            // Cargamos la imagen de forma segura desde el array de bytes.
            if (producto.Imagen != null && producto.Imagen.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(producto.Imagen))
                {
                    picImagen.Image = Image.FromStream(ms);
                }
            }
            else
            {
                // Si no hay imagen, puedes poner una imagen por defecto o dejarlo en blanco.
                picImagen.Image = null;
            }
        }
    }
}
    
