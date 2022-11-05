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
        private bool[,] verticalWalls;
        private bool[,] horizontalWalls;
        
        public QuoridorBoard(int dimension)
        {
            corners = new bool[dimension, dimension];
            verticalWalls = new bool[dimension, dimension];
            horizontalWalls = new bool[dimension, dimension];
        }

        public bool ChechWallPart()
        {

        }

        public bool AddWallPart()
        {

        }

        public bool RemoveWallPart()
        {

        }

        public bool CheckCorner()
        {

        }

        public bool AddCorner()
        {

        }

        public bool RemoveCorner()
        {

        }
    }
}
