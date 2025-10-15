namespace CorteCheco.Logica
{
    public static class SesionUsuario
    {
        // Propiedades para almacenar los datos del usuario logueado.
        // 'private set' significa que solo podemos asignarles valor desde dentro de esta clase.
        public static string NombreUsuario { get; private set; }
        public static string Rol { get; private set; }

        /// <summary>
        /// Guarda los datos del usuario cuando el login es exitoso.
        /// </summary>
        /// <param name="nombreUsuario">El nombre del usuario que inició sesión.</param>
        /// <param name="rol">El rol del usuario que inició sesión.</param>
        public static void IniciarSesion(string nombreUsuario, string rol)
        {
            NombreUsuario = nombreUsuario;
            Rol = rol;
        }

        /// <summary>
        /// Limpia los datos de la sesión, por ejemplo, al cerrar la aplicación o la sesión.
        /// </summary>
        public static void CerrarSesion()
        {
            NombreUsuario = null;
            Rol = null;
        }

    }
}