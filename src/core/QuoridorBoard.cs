using System.Diagnostics;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// Class representing quoridor board
    /// </summary>
    internal class QuoridorBoard
    {
        List<(int, int)> usedWalls;

        // The number of squares the player can move to per row/column
        private readonly int dimention;

        // The complete number of squares per row/column
        // including the ones destined for walls
        private readonly int extendedDimention;

        public int Dimention { get { return dimention; } }
        public int ExtendedDimention { get { return extendedDimention; } }

        public QuoridorBoard(int dimention)
        {
            usedWalls = new List<(int, int)>();
            this.dimention = dimention;
            extendedDimention = 2*dimention-1;
        }

        /// <summary>
        /// Utility function to check whether provided 
        /// square coords represent valid grid square. 
        /// </summary>
        /// <param name="row">Square row</param>
        /// <param name="col">Square column</param>
        /// <returns>True if square is valid else false</returns>
        public bool IsGridSquare(int row, int col)
        {
            if (row >= 0 && col >= 0 && row < extendedDimention && col < extendedDimention)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Utility function to check whether provided square coords 
        /// represent player square. If square row or column
        /// index is odd then it's not a player's square.
        /// </summary>
        /// <param name="row">Square row</param>
        /// <param name="col">Square column</param>
        /// <returns>True if player's square else false</returns>
        public bool IsPlayerSquare(int row, int col)
        {
            if (row % 2 == 0 && col % 2 == 0)
                return true;
            else
                return false;
        }

        private (int, int) ExpandWall(int row, int col)
        {
            Debug.Assert(IsGridSquare(row, col));    
            Debug.Assert(!IsPlayerSquare(row, col));

            // Wall is horizontal
            if (row % 2 != 0 && col % 2 == 0)
                return (row, col + 2);
            // Wall is vertical
            else if (row % 2 == 0 && col % 2 != 0)
                return (row + 2, col);
            else
                Debug.Assert(false);
                return (row, col); 
        }

        public bool CheckWallHorizontal(int row, int col, int newCol)
        {
            int dx = Math.Sign(newCol - col);
            return usedWalls.Contains((row, col+dx));
        }

        public bool CheckWallVertical(int row, int col, int newRow)
        {
            int dx = Math.Sign(newRow - row);
            return usedWalls.Contains((row+dx, col));
        }

        public void AddWall(int row, int col)
        {
            Debug.Assert(!IsPlayerSquare(row, col));

            (int expandedRow, int expandedCol) = ExpandWall(row, col);

            // Add wall to used
            usedWalls.Add((row, col));
            usedWalls.Add((expandedRow, expandedCol));
        }
        public void AddWall(int row, int col, bool isHorizontal)
        {
            //body
        }

        public void RemoveWall(int row, int col)
        {
            Debug.Assert(!IsPlayerSquare(row, col));

            (int expandedRow, int expandedCol) = ExpandWall(row, col);
            if (usedWalls.Contains((row, col)) && usedWalls.Contains((expandedRow, expandedCol)))
            {
                usedWalls.Remove((row, col));
                usedWalls.Remove((expandedRow, expandedCol));
            }
        }
        public void RemoveWall(int row, int col, bool isHorizontal)
        {
            //body
        }
    }
}
