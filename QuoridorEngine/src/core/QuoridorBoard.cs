using System.Diagnostics;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// Class representing quoridor board
    /// </summary>
    public class QuoridorBoard
    {
        List<(int, int)> usedWalls;

        // The complete number of squares per row/column
        // including the ones destined for walls
        private readonly int dimension;

        public int Dimension { get { return dimension; } }

        public QuoridorBoard(int dimension)
        {
            usedWalls = new List<(int, int)>();
            this.dimension = dimension;
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
            return row >= 2 && col >= 2 && row < dimension && col < dimension && row != col;
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
            return row % 2 == 0 && col % 2 == 0;
        }

        /// <summary>
        /// Function to get the second part of the wall
        /// </summary>
        /// <param name="row">First part row</param>
        /// <param name="col">First part column</param>
        /// <returns>Tuple of second part coords</returns>
        private (int, int) ExpandWall(int row, int col)
        {
            Debug.Assert(IsGridSquare(row, col));
            Debug.Assert(!IsPlayerSquare(row, col));

            int expRow = row;
            int expCol = col;

            // Wall is horizontal
            if (row % 2 != 0 && col % 2 == 0)
                expCol = col + 2;
            // Wall is vertical
            else if (row % 2 == 0 && col % 2 != 0)
                expRow = row - 2;

            Debug.Assert(IsGridSquare(expRow, expCol));
            return (expRow, expCol);
        }

        /// <summary>
        /// Check if horizontal wall exists between two player squares 
        /// </summary>
        /// <param name="row">Player squares row</param>
        /// <param name="col">Player square one column</param>
        /// <param name="newCol">Player square two column</param>
        /// <returns>True if wall exists</returns>
        public bool CheckWallHorizontal(int row, int col, int newCol)
        {
            Debug.Assert(col != newCol);
            int dx = Math.Sign(newCol - col);
            return usedWalls.Contains((row, col+dx));
        }

        /// <summary>
        /// Check if vertical wall exists between two player squares 
        /// </summary>
        /// <param name="row">Player squares one row</param>
        /// <param name="col">Player squares column</param>
        /// <param name="newRow">Player square two row</param>
        /// <returns>True if wall exists</returns>
        public bool CheckWallVertical(int row, int col, int newRow)
        {
            Debug.Assert(row != newRow);
            int dy = Math.Sign(newRow - row);
            return usedWalls.Contains((row+dy, col));
        }

        /// <summary>
        /// Check if provided wall forms a 
        /// cross with an already existing one 
        /// </summary>
        /// <param name="row">First Wall Part Row</param>
        /// <param name="col">First Wall Part Column</param>
        /// <param name="expRow">Second Wall Part Row</param>
        /// <param name="expCol">Second Wall Part Column</param>
        /// <returns></returns>
        public bool FormsWallCross(int row, int col, int expRow, int expCol)
        {
            Debug.Assert(IsGridSquare(row, col));
            Debug.Assert(!IsPlayerSquare(row, col));
            Debug.Assert(IsGridSquare(expRow, expCol));
            Debug.Assert(!IsPlayerSquare(expRow, expCol));

            return usedWalls.Contains((col, row)) && usedWalls.Contains((expCol, expRow));
        }

        /// <summary>
        /// Add wall part to used walls
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void AddWallPart(int row, int col)
        {
            Debug.Assert(IsGridSquare(row, col));
            Debug.Assert(!IsPlayerSquare(row, col));
            Debug.Assert(!usedWalls.Contains((row, col)));

            usedWalls.Add((row, col));
        }

        /// <summary>
        /// Remove wall part to used walls
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void RemoveWallPart(int row, int col)
        {
            Debug.Assert(IsGridSquare(row, col));
            Debug.Assert(!IsPlayerSquare(row, col));
            Debug.Assert(usedWalls.Contains((row, col)));

            usedWalls.Remove((row, col));
        }

        /// <summary>
        /// Function to get all neighbour squares that 
        /// a player could move from current square 
        /// </summary>
        /// <param name="row">Current player square row</param>
        /// <param name="col">Current player square col</param>
        /// <returns>List of all linked neighbour squares</returns>
        public List<(int, int)> GetLinkedPlayerSquareNeighbours(int row, int col)
        {
            Debug.Assert(IsGridSquare(row, col));
            Debug.Assert(IsPlayerSquare(row, col));

            // Add all possible neighbours
            List<(int, int)> neighbours = new()
            {
                (row + 2, col),
                (row, col + 2),
                (row - 2, col),
                (row, col - 2)
            };

            foreach((int r, int c) in neighbours)
            {
                // Neighbour square out of range 
                if (!IsGridSquare(r, c))    
                    neighbours.Remove((r, c));

                // Neighbour square blocked by wall
                if (row == r && CheckWallVertical(row, col, r))
                    neighbours.Remove((r, c));
                else if (col == c && CheckWallHorizontal(row, col, r))
                    neighbours.Remove((r, c));
            }

            return neighbours;
        }
    }
}
