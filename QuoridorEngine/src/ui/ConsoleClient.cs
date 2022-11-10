using QuoridorEngine.Core;

namespace QuoridorEngine.UI
{
    public class ConsoleClient
    {
        private const string engineName = "Quoridor Engine";
        private const int defaultBoardSize = 7;
        private static QuoridorGame game;
        private static List<String> knownCommands = new List<string>
        {
            "name", "known_command", "list_commands", "quit"
        };

        /// <summary>
        /// Runs the game according to the terminal implementation of
        /// the Quoridor Text Protocol (QTP).
        /// </summary>
        public static void Play()
        {
            game = new QuoridorGame(defaultBoardSize);

            Console.WriteLine("#Welcome to Quoridor Engine's Terminal Implementation. Please type any commands\n");
            do
            {
                String line = Console.ReadLine();
                if (line == null) continue;

                line.Trim();
                line.ToLower();
                if(handleCommand(line)) break;
            } while (true);
        }

        /// <summary>
        /// Handles a given quoridor command
        /// </summary>
        /// <param name="cmd">The command to handle</param>
        /// <returns>Returns true if the exit command was given</returns>
        private static bool handleCommand(String cmd)
        {
            String[] tokens = cmd.Split(' ');
            if (tokens.Length == 0) return false;

            String commandBody = tokens[0];
            if (String.Equals(commandBody, "quit"))
            {
                Console.WriteLine("=\n");
                return true;
            }
            else if (String.Equals(commandBody, "name"))
                Console.WriteLine("=" + engineName + "\n");
            else if (commandBody.Equals("known_command"))
            {
                if (knownCommands.Contains(tokens[1]))
                    Console.WriteLine("=true\n");
                else
                    Console.WriteLine("=false\n");
            }
            else if (commandBody.Equals("list_commands"))
            {
                Console.Write("=");
                foreach (String command in knownCommands)
                    Console.WriteLine(command);
                Console.WriteLine();
            }
            else
                Console.WriteLine("? unknown command\n");

            return false;
        }
    }
}