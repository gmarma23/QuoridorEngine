using QuoridorEngine.UI;

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
            new GuiClient(9, 10, GameMode.BlackIsAI).RunGui();
#else
            ConsoleClient.Play();
#endif
        }
    }
}