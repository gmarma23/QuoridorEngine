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
        private int dimension;
        private bool[,] corners;
        private bool[,] verticalWallParts;
        private bool[,] horizontalWallParts;

        public QuoridorBoard(int dimension)
        {
            corners = new bool[dimension - 1, dimension - 1];
            verticalWallParts = new bool[dimension, dimension - 1];
            horizontalWallParts = new bool[dimension - 1, dimension];
        }

        public bool ChechWallPartHorizontal(int row, int col)
        {

        }

        public bool ChechWallPartVertical(int row, int col)
        {

        }

        public void AddWallPartHorizontal(int row, int col)
        {

        }

        public void AddWallPartVertical(int row, int col)
        {

        }

        public void RemoveWallPartHorizontal(int row, int col)
        {

        }

        public void RemoveWallPartVertical(int row, int col)
        {

        }

        public bool CheckCorner(int row, int col)
        {

        }

        public void AddCorner(int row, int col)
        {

        }

        public void RemoveCorner(int row, int col)
        {

        }
    }
}
