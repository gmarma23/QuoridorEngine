using System.Security.Permissions;

namespace QuoridorEngine.src.game
{
    /// <summary>
    /// Common class to represent a board node in any given board game.
    /// Any implementation of a particular game
    /// needs to extend this class accordingly
    /// </summary>
    internal abstract class BoardNode
    {   
        // List containing all neighbours of particular node
        protected List<BoardNode> neighbours;

        protected bool isOccupied;
        protected int x;
        protected int y;

        public abstract int X { set; get; }
        public abstract int Y { set; get; }
        public abstract bool IsOccupied { set; get; }

        BoardNode()
        {
            neighbours = new List<BoardNode>();
        }

        /// <summary>
        /// Append new neighbour node to current node based on 
        /// specific conditions according to a particular game
        /// </summary>
        /// <param name="node">Neighbour node</param>
        public abstract void AppendNeighbour(BoardNode node);

        /// <summary>
        /// Remove neighbour node from current node based on 
        /// specific conditions according to a particular game
        /// </summary>
        /// <param name="node">Neighbour node</param>
        public abstract void RemoveNeighbour(BoardNode node);

        // Get all neighbour nodes of current node
        public abstract void GetNeighbours();
    }

    /// <summary>
    /// Common class to represent a board in any given board game.
    /// Any implementation of a particular game
    /// needs to extend this class accordingly
    /// </summary>
    internal abstract class Board
    {
        // List containing all board nodes.
        // Behaves like a psudo graph that maps the entire board.
        protected List<BoardNode> boardNodes;

        protected int width;
        protected int height;
        
        public abstract int Width { set; get; }
        public abstract int Height { set; get; }

        Board()
        {
            boardNodes = new List<BoardNode>();
        }

        /// <summary>
        /// Update board according to newly played moves
        /// </summary>
        /// <param name="move">Last played move</param>
        public abstract void Update(Move move);

        // Initialize board at the begining of aech round
        public abstract void Initialize();

        // Clear board
        public abstract void Clear();
    }
}
