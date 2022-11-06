using System.Diagnostics;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// Is responsible for storing information concerning 
    /// the quoridor board and providing public methods, 
    /// in order for the QuoridorGame class to interact
    /// with stored information.
    /// </summary>
    public class QuoridorBoard
    {
        private readonly int dimension;

        // Using bool arrays to represent used walls
        // and grid corners for faster search times.
        private bool[,] corners;
        private bool[,] verticalWallParts;
        private bool[,] horizontalWallParts;

        public int Dimension { get; }

        public QuoridorBoard(int dimension)
        {
            this.dimension = dimension;

            corners = new bool[dimension - 1, dimension - 1];
            InitializeArray(corners);

            verticalWallParts = new bool[dimension, dimension - 1];
            InitializeArray(verticalWallParts);

            horizontalWallParts = new bool[dimension - 1, dimension];
            InitializeArray(horizontalWallParts);
        }

        /// <summary>
        /// Utility method to initialize all board arrays
        /// </summary>
        /// <param name="array">Array to be initialized</param>
        private static void InitializeArray(bool[,] array)
        {
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    array[row, col] = false;
                }
            }
        }

        /// <summary>
        /// Check if specified horizontal wall part exists
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        /// <returns>True if wall part exists</returns>
        public bool CheckWallPartHorizontal(int row, int col)
        {
            Debug.Assert(row - 1 > 0);
            Debug.Assert(col > 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col <= verticalWallParts.GetLength(1));
            return horizontalWallParts[row - 1, col];
        }

        /// <summary>
        /// Check if specified vertical wall part exists
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        /// <returns>True if wall part exists</returns>
        public bool CheckWallPartVertical(int row, int col)
        {
            Debug.Assert(row > 0);
            Debug.Assert(col - 1 > 0);
            Debug.Assert(row <= horizontalWallParts.GetLength(0));
            Debug.Assert(col - 1 <= verticalWallParts.GetLength(1));
            return verticalWallParts[row, col - 1];

            // Wall is horizontal
            if (row % 2 != 0 && col % 2 == 0)
                expCol = col + 2;
            // Wall is vertical
            else if (row % 2 == 0 && col % 2 != 0)
                expRow = row - 2;

            Debug.Assert(IsGridSquare(expRow, expCol));
            return (expRow, expCol);
        }
            Debug.Assert(row - 1 > 0);
            Debug.Assert(col > 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col <= verticalWallParts.GetLength(1));
            horizontalWallParts[row - 1, col] = true;
        /// coresponding field true in horizontalWallParts 
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void AddWallPartHorizontal(int row, int col)
        {
            Debug.Assert(col != newCol);
            int dx = Math.Sign(newCol - col);
            return usedWalls.Contains((row, col+dx));
        }
            Debug.Assert(row > 0);
            Debug.Assert(col - 1 > 0);
            Debug.Assert(row <= horizontalWallParts.GetLength(0));
            Debug.Assert(col - 1 <= verticalWallParts.GetLength(1));
            verticalWallParts[row, col-1] = true;
        /// coresponding field true in verticalWallParts 
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void AddWallPartVertical(int row, int col)
        {
            Debug.Assert(row != newRow);
            int dy = Math.Sign(newRow - row);
            return usedWalls.Contains((row+dy, col));
        }
            Debug.Assert(row - 1 > 0);
            Debug.Assert(col > 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col <= verticalWallParts.GetLength(1));
            horizontalWallParts[row - 1, col] = false;
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void RemoveWallPartHorizontal(int row, int col)
        {
            Debug.Assert(IsGridSquare(row, col));
            Debug.Assert(!IsPlayerSquare(row, col));
            Debug.Assert(IsGridSquare(expRow, expCol));
            Debug.Assert(!IsPlayerSquare(expRow, expCol));

            return usedWalls.Contains((col, row)) && usedWalls.Contains((expCol, expRow));
            Debug.Assert(row > 0);
            Debug.Assert(col - 1 > 0);
            Debug.Assert(row <= horizontalWallParts.GetLength(0));
            Debug.Assert(col - 1 <= verticalWallParts.GetLength(1));
            verticalWallParts[row, col - 1] = false;
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void RemoveWallPartVertical(int row, int col)
        {
            Debug.Assert(IsGridSquare(row, col));
            Debug.Assert(!IsPlayerSquare(row, col));
            Debug.Assert(!usedWalls.Contains((row, col)));

            Debug.Assert(row - 1 > 0);
            Debug.Assert(col - 1 > 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col -1 <= verticalWallParts.GetLength(1));
            return corners[row - 1, col - 1];
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Corner row</param>
        /// <param name="col">Corner column</param>
        public bool CheckCorner(int row, int col)
        {
        /// <param name="row">Corner row</param>
        /// <param name="col">Corner column</param>
        public void AddCorner(int row, int col)
        {
            Debug.Assert(row - 1 > 0);
            Debug.Assert(col - 1 > 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col - 1 <= verticalWallParts.GetLength(1));
            corners[row - 1, col - 1] = true;
        }
        /// </summary>
        /// <summary>
        /// Consider grid corner unused after 
        /// necessary coords transformation.
        /// </summary>
        /// <param name="row">Corner row</param>
        /// <param name="col">Corner column</param>
        public void RemoveCorner(int row, int col)
        {
            Debug.Assert(row - 1 > 0);
            Debug.Assert(col - 1 > 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col - 1 <= verticalWallParts.GetLength(1));
            corners[row - 1, col - 1] = false;
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
