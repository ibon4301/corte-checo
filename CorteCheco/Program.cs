
using CorteCheco.Vistas;
using CorteCheco.Logica; // ◀️ ¡IMPORTANTE! Añade esta línea para poder acceder a 'SesionUsuario'.

namespace CorteCheco
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // La inicialización puede variar según la versión de .NET, tu línea está perfecta.
            ApplicationConfiguration.Initialize();

            // 1. Creamos una "instancia" de nuestro formulario de login.
            frmLogin loginForm = new frmLogin();

            // 2. Mostramos el formulario de login como un "diálogo".
            //    El código se detiene aquí, esperando a que el login se cierre.
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // 3. Si el login fue exitoso (resultado "OK"), ahora viene la parte nueva:
                //    LEEMOS EL ROL que guardamos en nuestra clase estática SesionUsuario.

                if (SesionUsuario.Rol == "Administrador")
                {
                    // 4a. Si el rol es "Administrador", ejecutamos el formulario principal con todos los controles.
                    Application.Run(new frmPrincipal());
                }
                else
                {
                    // 4b. Si el rol es cualquier otra cosa (ej. "Usuario"), ejecutamos el nuevo Dashboard de solo consulta.
                    Application.Run(new frmPrincipal());
                }
            }

            // Si el login NO fue exitoso (el usuario cerró la ventana o las credenciales eran incorrectas),
            // el `if` no se cumple, el método Main termina, y la aplicación se cierra limpiamente.
        }
    }
}