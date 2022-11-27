using QuoridorEngine.src.ui.console;
using QuoridorEngine.UI;

namespace QuoridorEngine
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            #if !CONSOLE
                ApplicationConfiguration.Initialize();
                Application.Run(new QuoridorUI());
            #else
                ConsoleClient.Play();
            #endif
        }
    }
}