namespace QuoridorEngine.src.game
{
    internal abstract class BoardNode
    {
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

        public void AppendNeighbour(BoardNode node)
        {
            neighbours.Add(node);
        }

        public void RemoveNeighbour(BoardNode node)
        {
            neighbours.Remove(node);
        }

        public abstract void GetNeighbours();
    }

    internal abstract class Board
    {
        protected List<(int, int)> playersPos;
        protected List<BoardNode> boardNodes;
        protected int width;
        protected int height;
        
        public abstract int Width { set; get; }
        public abstract int Height { set; get; }

        Board()
        {
            boardNodes = new List<BoardNode>();
            playersPos = new List<(int, int)>();
        }

        public abstract void Update(Move move);

        public abstract void Clear();
    }
}
