using QuoridorEngine.Solver;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// Class representing quoridor board
    /// </summary>
    internal class QuoridorBoard
    {
        List<(int, int)> usedWalls;
        private int width;
        private int height;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        QuoridorBoard(int width, int height)
        {
            usedWalls = new List<(int, int)>();
            this.width = width;
            this.height = height;
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
