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

        public int Dimension { get => dimension; }

        public QuoridorBoard(int dimension)
        {
            Debug.Assert(dimension > 2);
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
        /// Check if given coordinates represent a square 
        /// a player can move within board limits
        /// </summary>
        /// <param name="row">Square row</param>
        /// <param name="col">Square col</param>
        /// <returns>True if square is valid</returns>
        public bool IsValidPlayerSquare(int row, int col)
        {
            return col > 0 && col < dimension && row > 0 && row < dimension;
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
            Debug.Assert(row - 1 >= 0);
            Debug.Assert(col >= 0);
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
            Debug.Assert(row >= 0);
            Debug.Assert(col >= 0);
            Debug.Assert(row <= horizontalWallParts.GetLength(0));
            Debug.Assert(col <= verticalWallParts.GetLength(1));
            return verticalWallParts[row, col];
        }

        /// <summary>
        /// Consider horizontal wall part used by making the 
        /// coresponding field true in horizontalWallParts 
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void AddWallPartHorizontal(int row, int col)
        {
            Debug.Assert(row - 1 >= 0);
            Debug.Assert(col >= 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col <= verticalWallParts.GetLength(1));
            horizontalWallParts[row - 1, col] = true;
        }

        /// <summary>
        /// Consider vertical wall part used by making the 
        /// coresponding field true in verticalWallParts 
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void AddWallPartVertical(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(col >= 0);
            Debug.Assert(row <= horizontalWallParts.GetLength(0));
            Debug.Assert(col <= verticalWallParts.GetLength(1));
            verticalWallParts[row, col] = true;
        }

        /// <summary>
        /// Consider horizontal wall part unused by making the 
        /// coresponding field false in horizontalWallParts 
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void RemoveWallPartHorizontal(int row, int col)
        {
            Debug.Assert(row - 1 >= 0);
            Debug.Assert(col >= 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col <= verticalWallParts.GetLength(1));
            horizontalWallParts[row - 1, col] = false;
        }

        /// <summary>
        /// Consider vertical wall part unused by making the 
        /// coresponding field false in verticalWallParts 
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Wall part row</param>
        /// <param name="col">Wall part column</param>
        public void RemoveWallPartVertical(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(col >= 0);
            Debug.Assert(row <= horizontalWallParts.GetLength(0));
            Debug.Assert(col <= verticalWallParts.GetLength(1));
            verticalWallParts[row, col] = false;
        }

        /// <summary>
        /// Check if grid corner is already in use
        /// after necessary coords transformation.
        /// </summary>
        /// <param name="row">Corner row</param>
        /// <param name="col">Corner column</param>
        public bool CheckCorner(int row, int col)
        {
            Debug.Assert(row - 1 >= 0);
            Debug.Assert(col - 1 >= 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col - 1 <= verticalWallParts.GetLength(1));
            return corners[row - 1, col - 1];
        }

        /// <summary>
        /// Consider grid corner used after 
        /// necessary coords transformation.
        /// </summary>
        /// <param name="row">Corner row</param>
        /// <param name="col">Corner column</param>
        public void AddCorner(int row, int col)
        {
            Debug.Assert(row - 1 >= 0);
            Debug.Assert(col - 1 >= 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col - 1 <= verticalWallParts.GetLength(1));
            corners[row - 1, col - 1] = true;
        }

        /// <summary>
        /// Consider grid corner unused after 
        /// necessary coords transformation.
        /// </summary>
        /// <param name="row">Corner row</param>
        /// <param name="col">Corner column</param>
        public void RemoveCorner(int row, int col)
        {
            Debug.Assert(row - 1 >= 0);
            Debug.Assert(col - 1 >= 0);
            Debug.Assert(row - 1 <= horizontalWallParts.GetLength(0));
            Debug.Assert(col - 1 <= verticalWallParts.GetLength(1));
            corners[row - 1, col - 1] = false;
        }
    }
}