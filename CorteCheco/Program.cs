using CorteCheco.Vistas;

namespace CorteCheco
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        // Este es el punto de entrada principal de la aplicaci�n.
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. Creamos una "instancia" (una copia en memoria) de nuestro formulario de login.
            frmLogin loginForm = new frmLogin();

            // 2. Mostramos el formulario de login como un "di�logo".
            //    Esto es muy importante: el c�digo se detendr� aqu� mismo, esperando a que 
            //    el formulario de login se cierre antes de continuar.
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // 3. Si el login se cerr� devolviendo un resultado de "OK" (que significar� "�xito"),
                //    entonces, y solo entonces, procedemos a ejecutar el formulario principal.
                Application.Run(new CorteCheco.Vistas.frmPrincipal());
            }

            // Si el login NO fue exitoso (por ejemplo, el usuario cerr� la ventana con la 'X'),
            // el `if` no se cumple, el m�todo Main termina, y la aplicaci�n se cierra.
            // �Esto es exactamente lo que queremos!
        }
    }
}