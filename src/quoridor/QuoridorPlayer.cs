using QuoridorEngine.src.game;

namespace QuoridorEngine.src.quoridor
{
    /// <summary>
    /// Class representing quoridor pawn
    /// </summary>
    internal class QuoridorPawn //: Piece
    {

    }

    /// <summary>
    /// Class representing quoridor wall
    /// </summary>
    internal class QuoridorWall //: Piece
    {

    }

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
        
        public int AvailableWalls
        {
            get { return availableWalls; }
        }

        public bool IsWhite
        { 
            set { isWhite = value; }
            get { return isWhite; } 
        }

        public int Victories
        {
            set { victories++; }
            get { return victories; }
        }

        public int Defeats
        {
            set { defeats++; }
            get { return defeats; }
        }

        public int Draws
        {
            set { draws++; }
            get { return draws; }
        }

        QuoridorPlayer(bool isWhite)
        {
            availableWalls = totalWalls;
            IsWhite = isWhite;
        }

    }
}
