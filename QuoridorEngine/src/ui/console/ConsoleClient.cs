using QuoridorEngine.Core;
using QuoridorEngine.Utils;
using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.UI
{
    public class ConsoleClient
    {
        private const string engineName = "Quoridor Engine";
        private static QuoridorGame game;
        private static List<string> knownCommands = new List<string>
        {
            "name", "known_command", "list_commands", "quit", "boardsize",
            "clear_board", "walls", "playmove", "playwall", "undo", "winner",
            "showboard"
        };

        /// <summary>
        /// Runs the game according to the terminal implementation of
        /// the Quoridor Text Protocol (QTP).
        /// </summary>
        public static void Play()
        {
            game = new QuoridorGame();

            Console.WriteLine("#Welcome to Quoridor Engine's Terminal Implementation. Please type any commands\n");
            do
            {
                string line = Console.ReadLine();
                if (line == null) break;

                line.Trim();
                line.ToLower();

                // Ignore blank and comment lines
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("#")) continue;

                // If the command was the exit one we exit the loop
                if (handleCommand(line)) break;
            } while (true);
        }

        /// <summary>
        /// Handles a given quoridor command
        /// </summary>
        /// <param name="rawCommand">The command to handle</param>
        /// <returns>Returns true if the exit command was given</returns>
        private static bool handleCommand(string rawCommand)
        {
            string[] tokens = rawCommand.Split(' ');
            if (tokens.Length == 0) return false;

            string commandBody = tokens[0];
            if (string.Equals(commandBody, "quit"))
            {
                OutputUtility.PrintSuccessMessage("");
                return true;
            }
            else if (string.Equals(commandBody, "name"))
                Console.WriteLine("=" + engineName + "\n");
            else if (commandBody.Equals("known_command"))
            {
                if (tokens.Length < 2)
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
                foreach (string command in knownCommands)
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
                    int boardSize = int.Parse(tokens[1]);
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
                game.ResetGame();
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
                    int walls = int.Parse(tokens[1]);
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
            else if (commandBody.Equals("playmove"))
            {
                int row = 0, col = 0;
                bool isWhite = false;

                if (!parsePosition(ref row, ref col, tokens, 2) || !parsePlayer(ref isWhite, tokens, 1))
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }

                int prevRow = 0;
                int prevColumn = 0;
                if (isWhite)
                    game.GetWhiteCoordinates(ref prevRow, ref prevColumn);
                else
                    game.GetBlackCoordinates(ref prevRow, ref prevColumn);

                QuoridorMove move = new QuoridorMove(prevRow, prevColumn, row, col, isWhite);
                try
                {
                    game.ExecuteMove(move);
                }
                catch (InvalidMoveException)
                {
                    OutputUtility.PrintFailureMessage("illegal move");
                    return false;
                }

                OutputUtility.PrintSuccessMessage("");
            }
            else if (commandBody.Equals("playwall"))
            {
                int row = 0, col = 0;
                bool isWhite = false;
                Orientation orientation = Orientation.Horizontal;

                if (!parsePosition(ref row, ref col, tokens, 2) || !parsePlayer(ref isWhite, tokens, 1) ||
                    !parseOrientation(ref orientation, tokens, 3))
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }

                QuoridorMove move = new QuoridorMove(row, col, isWhite, orientation);
                try
                {
                    game.ExecuteMove(move);
                }
                catch (InvalidMoveException)
                {
                    OutputUtility.PrintFailureMessage("illegal move");
                    return false;
                }

                OutputUtility.PrintSuccessMessage("");
            }
            else if (commandBody.Equals("undo"))
            {
                if (tokens.Length < 2)
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }

                try
                {
                    int moves = int.Parse(tokens[1]);
                    game.UndoMoves(moves);
                    OutputUtility.PrintSuccessMessage("");
                }
                catch (FormatException)
                {
                    OutputUtility.PrintFailureMessage("syntax error");
                    return false;
                }
                catch (ArgumentException)
                {
                    OutputUtility.PrintFailureMessage("cannot undo");
                    return false;
                }
            }
            else if (commandBody.Equals("winner"))
            {
                if (!game.IsTerminalState())
                    OutputUtility.PrintSuccessMessage("false");
                else
                {
                    string winnerName = game.WinnerIsWhite() ? "white" : "black";
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

        private static bool parsePlayer(ref bool isWhite, string[] arguments, int indexOfArgument)
        {
            string player = arguments[indexOfArgument];
            player.Trim();

            if (player.Equals("w") || player.Equals("white"))
            {
                isWhite = true;
                return true;
            }
            else if (player.Equals("b") || player.Equals("black"))
            {
                isWhite = false;
                return true;
            }

            return false;
        }

        private static bool parsePosition(ref int row, ref int column, string[] arguments, int indexOfArgument)
        {
            if (indexOfArgument >= arguments.Length)
                return false;

            char columnLetter = arguments[indexOfArgument][0];

            if (!char.IsLetter(columnLetter))
                return false;

            column = columnLetter - 'a';
            string rawRow = arguments[indexOfArgument].Substring(1);

            try
            {
                row = int.Parse(rawRow) - 1;
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        private static bool parseOrientation(ref Orientation orientation, string[] arguments, int indexOfArgument)
        {
            if (indexOfArgument >= arguments.Length)
                return false;

            string rawOrientation = arguments[indexOfArgument];
            rawOrientation.Trim();

            if (rawOrientation.Equals("h") || rawOrientation.Equals("horizontal"))
            {
                orientation = Orientation.Horizontal;
                return true;
            }
            else if (rawOrientation.Equals("v") || rawOrientation.Equals("vertical"))
            {
                orientation = Orientation.Vertical;
                return true;
            }

            return false;
        }
    }
}