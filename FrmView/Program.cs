namespace FrmView
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Inicializa la configuraci�n de la aplicaci�n, como la configuraci�n de DPI o la fuente predeterminada.
            // Consulta https://aka.ms/applicationconfiguration para personalizar la configuraci�n de la aplicaci�n.
            ApplicationConfiguration.Initialize();

            // Ejecuta la aplicaci�n, mostrando el formulario principal (FrmView).
            Application.Run(new FrmView());
        }
    }
}
