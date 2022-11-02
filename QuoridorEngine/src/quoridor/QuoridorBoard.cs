using QuoridorEngine.src.solver;

namespace QuoridorEngine.src.quoridor
{
    /// <summary>
    /// Class representing quoridor board
    /// </summary>
    internal class QuoridorBoard
    {
        List<(int, int)> usedWalls;
        private int width;
        private int height;

        public int Width
        {
            set { if (value is int and > 0) { width = value; } }
            get { return width; }
        }

        public int Height
        {
            set { if (value is int and > 0) { height = value; } }
            get { return height; }
        }

        QuoridorBoard(int width, int height)
        {
            usedWalls = new List<(int, int)>();
            Width = width;
            Height = height;
        }

        public void AddWall(QuoridorPlayer player, int x, int y)
        {
            usedWalls.Add((x, y));
            player.DecreaseAvailableWalls();
        }

        public void RemoveWall(QuoridorPlayer player, int x, int y)
        {
            usedWalls.Remove((x, y));
            player.IncreaseAvailableWalls();
        }
    }
}
