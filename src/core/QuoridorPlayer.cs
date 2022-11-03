namespace QuoridorEngine.Core
{
    /// <summary>
    /// Class representing quoridor player
    /// </summary>
    internal class QuoridorPlayer
    {
        private readonly int totalWalls = 10;
        private int availableWalls;
        private int victories = 0;
        private int defeats = 0;
        private int draws = 0;
        private bool isWhite;
        private int x;
        private int y;

        public int AvailableWalls { get; }
        public bool IsWhite { get; }
        public int Victories { get; }
        public int Defeats { get; }
        public int Draws { get; }
        public int X { set; get; }
        public int Y { set; get; }

        QuoridorPlayer(bool isWhite)
        {
            availableWalls = totalWalls;
            this.isWhite = isWhite;
        }

        public void IncreaseAvailableWalls() { availableWalls++; }

        public void DecreaseAvailableWalls() { availableWalls--; }
    }
}
