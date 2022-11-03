namespace QuoridorEngine.Core
{
    /// <summary>
    /// Class representing quoridor player
    /// </summary>
    internal class QuoridorPlayer
    {
        private int availableWalls;

        private readonly bool isWhite;
        private int row;
        private int column;

        public int AvailableWalls { get; }
        public bool IsWhite { get; }
        public int Row { set; get; }
        public int Column { set; get; }

        public QuoridorPlayer(bool isWhite, int startingAmountOfWalls)
        {
            availableWalls = startingAmountOfWalls;
            this.isWhite = isWhite;
        }

        public void IncreaseAvailableWalls() { availableWalls++; }

        public void DecreaseAvailableWalls() { availableWalls--; }
    }
}
