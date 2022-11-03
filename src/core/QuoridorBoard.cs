using QuoridorEngine.Solver;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// Class representing quoridor board
    /// </summary>
    internal class QuoridorBoard
    {
        List<(int, int)> usedWalls;
        private int dimention;

        public int Dimention { get { return dimention; } }

        QuoridorBoard(int dimention)
        {
            usedWalls = new List<(int, int)>();
            this.dimention = dimention;
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
