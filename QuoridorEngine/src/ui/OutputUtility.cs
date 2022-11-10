using QuoridorEngine.Core;
using System.Diagnostics;
using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.UI
{
    public static class OutputUtility
    {
        private const int cellSize = 3;

        /// <summary>
        /// Prints a message with the success format defined in the Quoridor
        /// Text Protocol.
        /// </summary>
        /// <param name="message">The message to print</param>
        public static void PrintSuccessMessage(String message)
        {
            Console.Write("=" + message + "\n\n");
        }

        /// <summary>
        /// Prints a message with the fail format defined in the Quoridor
        /// Text Protocol.
        /// </summary>
        /// <param name="message">The message to print</param>
        public static void PrintFailureMessage(String message)
        {
            Console.Write("? " + message + "\n\n");
        }

        /// <summary>
        /// Prints a quoridor state to the console with
        /// a nice formatting
        /// </summary>
        /// <param name="state">The state to be printed</param>
        public static void PrintState(QuoridorGame state)
        {
            Debug.Assert(state != null);
            Debug.Assert(state.Dimension >= 3);

            Console.WriteLine();
            printLetterRow(state.Dimension);

            for(int row = state.Dimension-1; row >= 0; row--)
            {
                printBorderRow(state, row);
                printCellRow(state, row);
            }

            printBorderRow(state, -1);
            printLetterRow(state.Dimension);

            Console.WriteLine();
            Console.WriteLine("White walls: " + state.GetPlayerWalls(true));
            Console.WriteLine("Black walls: " + state.GetPlayerWalls(false));
        }

        private static void printLetterRow(int dimension)
        {
            printSpaces(cellSize + 2);
            for(int i = 0; i < dimension; i++)
            {
                Console.Write("" + Convert.ToChar('A' + i));
                printSpaces(cellSize);
            }
            Console.WriteLine();
        }

        private static void printBorderRow(QuoridorGame state, int row)
        {
            printSpaces(3);
            Console.Write("+");

            for(int i = 0; i < state.Dimension; i++)
            {
                if(row >= state.Dimension-1 || row < 0)
                {
                    printHorizontalBorder();
                    Console.Write('+');
                    continue;
                }

                if (state.HasWallPiece(row+1, i, Orientation.Horizontal))
                    printHorizontalWall();
                else
                    printHorizontalBorder();

                Console.Write('+');
                continue;

                if (row == state.Dimension - 1)
                {
                    Console.Write("+");
                    continue;
                }
            }

            Console.WriteLine();           
        }

        private static void printCellRow(QuoridorGame state, int row)
        {
            Console.Write(row + 1);
            printSpaces(2);

            for(int i = 0; i < state.Dimension; i++)
            {
                if(i < state.Dimension - 1 && state.HasWallPiece(row, i, Orientation.Vertical))
                    Console.Write('H');
                else
                    Console.Write("|");

                int whiteRow = 0, whiteCol = 0, blackRow = 0, blackCol = 0;
                state.GetWhiteCoordinates(ref whiteRow, ref whiteCol);
                state.GetBlackCoordinates(ref blackRow, ref blackCol);

                // There's a white player in this cell
                if (whiteRow == row && whiteCol == i)
                    Console.Write(" W ");
                else if (blackRow == row && blackCol == i)
                    Console.Write(" B ");
                else
                    printSpaces(cellSize);
            }

            Console.WriteLine("| " + row);
        }

        private static void printSpaces(int amountOfSpaces)
        {
            for (int i = 0; i < amountOfSpaces; i++)
                Console.Write(" ");
        }

        private static void printHorizontalBorder()
        {
            for(int i = 0; i < cellSize; i++)
                Console.Write('-');
        }

        private static void printHorizontalWall()
        {
            for (int i = 0; i < cellSize; i++)
                Console.Write('=');
        }
    }
}
