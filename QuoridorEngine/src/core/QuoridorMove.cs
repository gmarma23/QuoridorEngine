using QuoridorEngine.Solver;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// Quoridor Move class extending abstract class Move 
    /// </summary>
    internal class QuoridorMove : Move
    {
        private readonly int row, column;
        private readonly bool isWhitePlayer;
        private readonly Orientation orientation;
        private readonly MoveType type;

        public QuoridorMove(int row, int column, bool isWhitePlayer, Orientation orientation)
        {
            this.row = row;
            this.column = column;
            this.isWhitePlayer = isWhitePlayer;
            this.orientation = orientation;
            type = MoveType.WallPlacement;
        }

        public QuoridorMove(int row, int column, bool isWhitePlayer)
        {
            this.row = row;
            this.column = column;
            this.isWhitePlayer = isWhitePlayer;
            type = MoveType.PlayerMovement;
        }

        public int Row { get; }
        public int Column { get; }

        public bool IsWhitePlayer { get; } 
        public Orientation Orientation { get; } 
        public MoveType Type { get; }
    }

    public enum Orientation
    {
        Horizontal, Vertical
    };

    public enum MoveType
    {
        WallPlacement, PlayerMovement
    }
}
