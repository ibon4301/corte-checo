using CorteCheco.Vistas;

namespace CorteCheco
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        // Este es el punto de entrada principal de la aplicación.
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. Creamos una "instancia" (una copia en memoria) de nuestro formulario de login.
            frmLogin loginForm = new frmLogin();

            // 2. Mostramos el formulario de login como un "diálogo".
            //    Esto es muy importante: el código se detendrá aquí mismo, esperando a que 
            //    el formulario de login se cierre antes de continuar.
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // 3. Si el login se cerró devolviendo un resultado de "OK" (que significará "éxito"),
                //    entonces, y solo entonces, procedemos a ejecutar el formulario principal.
                Application.Run(new CorteCheco.Vistas.frmPrincipal());
            }

            // Si el login NO fue exitoso (por ejemplo, el usuario cerró la ventana con la 'X'),
            // el `if` no se cumple, el método Main termina, y la aplicación se cierra.
            // ¡Esto es exactamente lo que queremos!
        }
    }
}