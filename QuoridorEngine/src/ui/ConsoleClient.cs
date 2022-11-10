using QuoridorEngine.Core;
using QuoridorEngine.Utils;

namespace QuoridorEngine.UI
{
    public class ConsoleClient
    {
        private const string engineName = "Quoridor Engine";
        private const int defaultBoardSize = 7;
        private static QuoridorGame game;
        private static List<String> knownCommands = new List<string>
        {
            "name", "known_command", "list_commands", "quit", "boardsize", "clear_board", "walls", "winner", "showboard"
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
                if (line == null) break;

                line.Trim();
                line.ToLower();

                // Ignore blank and comment lines
                if (String.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("#")) continue;

                // If the command was the exit one we exit the loop
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
                OutputUtility.PrintSuccessMessage("");
                return true;
            }
            else if (String.Equals(commandBody, "name"))
                Console.WriteLine("=" + engineName + "\n");
            else if (commandBody.Equals("known_command"))
            {
                if(tokens.Length < 2)
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }

                if (knownCommands.Contains(tokens[1]))
                    OutputUtility.PrintSuccessMessage("true");
                else
                    OutputUtility.PrintSuccessMessage("false");
            }
            else if (commandBody.Equals("list_commands"))
            {
                Console.Write("=");
                foreach (String command in knownCommands)
                    Console.WriteLine(command);
                Console.WriteLine();
            }
            else if (commandBody.Equals("boardsize"))
            {
                if (tokens.Length < 2)
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }

                try
                {
                    int boardSize = Int32.Parse(tokens[1]);
                    game = new QuoridorGame(boardSize);
                    OutputUtility.PrintSuccessMessage("");
                }
                catch (FormatException)
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }
                catch (ArgumentException)
                {
                    OutputUtility.PrintFailureMessage("unacceptable size");
                    return false;
                }
            }
            else if (commandBody.Equals("clear_board"))
            {
                game.ClearBoard();
                OutputUtility.PrintSuccessMessage("");
            }
            else if (commandBody.Equals("walls"))
            {
                if (tokens.Length < 2)
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }

                try
                {
                    int walls = Int32.Parse(tokens[1]);
                    game.SetPlayerWalls(true, walls);
                    game.SetPlayerWalls(false, walls);
                    OutputUtility.PrintSuccessMessage("");
                }
                catch (FormatException)
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }
                catch (ArgumentException)
                {
                    OutputUtility.PrintFailureMessage("unacceptable number of walls");
                    return false;
                }
            }
            else if (commandBody.Equals("winner"))
            {
                if (!game.IsTerminalState())
                    OutputUtility.PrintSuccessMessage("false");
                else
                {
                    String winnerName = game.WinnerIsWhite() ? "white" : "black";
                    OutputUtility.PrintSuccessMessage("true " + winnerName);
                }
            }
            else if (commandBody.Equals("showboard"))
            {
                Console.WriteLine("=");
                OutputUtility.PrintState(game);
                Console.WriteLine("\n");
            }
            else
                OutputUtility.PrintFailureMessage("unknown command");

            return false;
        }
    }
}