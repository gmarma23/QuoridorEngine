using QuoridorEngine.Solver;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// Quoridor Move class extending abstract class Move 
    /// </summary>
    internal class QuoridorMove : Move
    {
        private int row, column;

        public QuoridorMove(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public int Row { get; }
        public int Column { get; }
    }
}
