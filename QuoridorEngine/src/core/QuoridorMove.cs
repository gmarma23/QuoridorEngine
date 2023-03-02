using QuoridorEngine.Solver;
using System.Diagnostics;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// This class holds all information necessary to describe a
    /// Quoridor Move. Extends the Move class so it can be used as
    /// a result of the solver. CAUTION: this class is not meant
    /// to contain any functionality. Its purpose is to carry all
    /// the move-related data around.
    /// </summary>
    public class QuoridorMove : Move
    {
        private readonly int prevRow, prevColumn, row, column;
        private readonly bool isWhitePlayer;
        private readonly Orientation orientation;
        private readonly MoveType type;

        /// <summary>
        /// Initializes a wall placement QuoridorMove with the data provided
        /// </summary>
        /// <param name="row">The target row for the wall</param>
        /// <param name="column">The target column for the wall</param>
        /// <param name="isWhitePlayer">Whether this move is executed by the white player or not</param>
        /// <param name="orientation">The orientation of wall (Horizontal/Vertical)</param>
        public QuoridorMove(int row, int column, bool isWhitePlayer, Orientation orientation)
        {
            Debug.Assert(orientation == Orientation.Horizontal || orientation == Orientation.Vertical);

            this.row = row;
            this.column = column;
            this.isWhitePlayer = isWhitePlayer;
            this.orientation = orientation;
            type = MoveType.WallPlacement;
        }

        /// <summary>
        /// Initializes a Player Movement move with the data provided
        /// </summary>
        /// <param name="prevRow">The previous row of the player</param>
        /// <param name="prevColumn">The previous column of the player</param>
        /// <param name="row">The new row of the player</param>
        /// <param name="column">The new column of the player</param>
        /// <param name="isWhitePlayer">Whether this move is executed by the white player or not</param>
        public QuoridorMove(int prevRow, int prevColumn, int row, int column, bool isWhitePlayer)
        {
            this.row = row;
            this.column = column;
            this.prevRow = prevRow;
            this.prevColumn = prevColumn;
            this.isWhitePlayer = isWhitePlayer;
            type = MoveType.PlayerMovement;
        }

        public int Row { get => row; }
        public int Column { get => column; }
        public int PrevRow { get => prevRow; }
        public int PrevCol { get => prevColumn; }
        public bool IsWhitePlayer { get => isWhitePlayer; } 
        public Orientation Orientation { get => orientation; } 
        public MoveType Type { get => type; }

        public bool IsEqual(QuoridorMove move)
        {
            Debug.Assert(this != null);
            if (move == null)
                return false;

            if (Type != move.Type || 
                IsWhitePlayer != move.IsWhitePlayer ||
                Row != move.Row ||
                Column != move.Column)
                return false;

            if (Type == MoveType.PlayerMovement)
                return PrevRow == move.PrevRow &&
                       PrevCol == move.PrevCol;
            else if (Type == MoveType.WallPlacement)
                return Orientation == move.Orientation;

            return false;
        }
    }

    /// <summary>
    /// The orientation of a wall. Can be Horizontal or vertical
    /// </summary>
    public enum Orientation
    {
        Horizontal, Vertical
    };

    /// <summary>
    /// The type of a move. Can either be a placement of the wall
    /// or a movement of the player
    /// </summary>
    public enum MoveType
    {
        WallPlacement, PlayerMovement
    }
}
