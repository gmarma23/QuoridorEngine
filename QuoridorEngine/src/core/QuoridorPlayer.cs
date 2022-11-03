using System.Diagnostics;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// This class holds all the information and operations related
    /// to a Quoridor Player.
    /// </summary>
    internal class QuoridorPlayer
    {
        private int availableWalls, row, column;

        private readonly bool isWhite;
        private readonly int targetBaseline;

        /// <summary>
        /// Initializes a new player with the specified parameters
        /// </summary>
        /// <param name="isWhite">True if player is white, false if it is black</param>
        /// <param name="row">The starting row of the player</param>
        /// <param name="column">The starting column of the player</param>
        /// <param name="startingWalls">The starting amount of walls</param>
        /// <param name="targetBaseline">The row the player needs to reach in order to win</param>
        public QuoridorPlayer(bool isWhite, int row, int column, int startingWalls, int targetBaseline)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(column >= 0);
            Debug.Assert(startingWalls >= 0);
            Debug.Assert(targetBaseline >= 0);
           
            this.isWhite = isWhite;
            this.row = row;
            this.column = column;
            this.targetBaseline = targetBaseline;
            availableWalls = startingWalls;
        }

        /// <summary>
        /// Increases the available walls of the player by one
        /// </summary>
        public void IncreaseAvailableWalls() { availableWalls++; }

        /// <summary>
        /// Decreases the available walls of the player by one.
        /// Assumes the player has at least 1 wall before decreasing
        /// </summary>
        public void DecreaseAvailableWalls() { 
            Debug.Assert(availableWalls >= 1);
            availableWalls--; 
        }

        /// <summary>
        /// Returns whether the player has reached the target row
        /// required to win.
        /// </summary>
        public bool IsInTargetBaseline() { return row == targetBaseline; }

        public int AvailableWalls { get; }
        public bool IsWhite { get; }
        public int Row { set; get; }
        public int Column { set; get; }
    }
}
