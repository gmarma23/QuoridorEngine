using QuoridorEngine.src.ui.console;
using QuoridorEngine.src.ui.gui;

namespace QuoridorEngine
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if !CONSOLE
            ApplicationConfiguration.Initialize();
            new GuiClient().RunGui();
#else
            ConsoleClient.Play();
#endif
        }
    }
}