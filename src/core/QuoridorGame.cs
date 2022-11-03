using QuoridorEngine.Solver;
using QuoridorEngine.Utils;

namespace QuoridorEngine.Core
{
    internal class QuoridorGame : IGameState
    {
        private readonly int dimension = 9;
        private readonly QuoridorBoard board;
        private readonly QuoridorPlayer white;
        private readonly QuoridorPlayer black;

        public QuoridorGame(int dimension)
        {
            this.dimension = dimension;
            board = new QuoridorBoard();

            white = new QuoridorPlayer(true, 10);
            black = new QuoridorPlayer(false, 10);
        }

        /// <summary>
        /// Returns true if the state is terminal (i.e. game is over)
        /// </summary>
        /// <returns>True if the state is terminal (i.e. game is over)</returns>
        public bool IsTerminalState()
        {
            return false;
        }

        /// <summary>
        /// Returns a list of all the possible moves from this state for a given player
        /// </summary>
		/// <param name="playerIsWhite">True if we want moves for white player, false otherwise</param>
        /// <returns>A list of all the possible moves from this state for given player</returns>
        public List<Move> GetPossibleMoves(bool playerIsWhite)
        {
            List<Move> possibleMoves = new List<Move>();
            return possibleMoves;
        }

        /// <summary>
        /// Executes a given move in this state
        /// </summary>
        /// <param name="move">The move to be executed. Only accepts PlayerMovement or WallPlacement
        /// type of moves. If this is not the case throws an ArgumentException</param>
        public void ExecuteMove(Move move)
        {
            if(move is not QuoridorMove) throw new ArgumentException("Unsupported move type provided in ExecuteMove");

            QuoridorMove newMove = (QuoridorMove)(move);
            if (newMove.Type == MoveType.WallPlacement) placeWall(newMove);
            else if (newMove.Type == MoveType.PlayerMovement) movePlayer(newMove);
            else throw new InvalidMoveException();
        }

        /// <summary>
        /// Undoes a given move returning the state to its previous configuration. Assumes the move to be
        /// undone was legal at the moment it was executed.
        /// </summary>
        /// <param name="move">The move to be undone</param>
        public void UndoMove(Move move)
        {

        }

        /// <summary>
        /// Returns an evaluation of the likelyhood of selected player winning the game from this state
        /// </summary>
        /// <param name="playerIsWhite">True if player we are asking for is white, false otherwise</param>
        /// <returns>An evaluation of the likelyhood of selected player winning the game from this state</returns>
        public float EvaluateState(bool playerIsWhite)
        {
            return 0;
        }

        /// <summary>
        /// Executes a given player movement in this state. If the move has invalid parameters, throws an
        /// InvalidMoveException.
        /// </summary>
        /// <param name="move">The move to be executed</param>
        private void movePlayer(QuoridorMove move)
        {
            // Checking if values are inside bounds
            if (move.Column < 0 || move.Column >= dimension) throw new InvalidMoveException("Coordinates out of bounds");
            if (move.Row < 0 || move.Row >= dimension) throw new InvalidMoveException("Coordinates out of bounds");

            // Check if any walls make the move impossible
            int currentRow = 0, currentColumn = 0;
            getPlayerCoordinates(move.IsWhitePlayer, ref currentRow, ref currentColumn);

            int deltaRow = move.Row - currentRow;
            int deltaColumn = move.Column - currentColumn;

            if (deltaRow == 0 && board.CheckWallHorizontal(currentRow, currentColumn, move.Column)) 
                throw new InvalidMoveException("Wall is blocking player move");
            if (deltaColumn == 0 && board.CheckWallVertical(currentColumn, currentRow, move.Row))
                throw new InvalidMoveException("Wall is blocking player move");

            // Checking if another player is already located on destination coordinates
            if (white.Row == move.Row && white.Column == move.Column) throw new InvalidMoveException("This position is occupied");
            if (black.Row == move.Row && black.Column == move.Column) throw new InvalidMoveException("This position is occupied");


        }

        private void changePlayerCoordinates(bool whitePlayer, int newRow, int newColumn)
        {
            if (whitePlayer)
            {
                white.Row = newRow;
                white.Column = newColumn;
            }
            else
            {
                black.Row = newRow;
                black.Column = newColumn;
            }
        }

        

        /// <summary>
        /// Executes a given move in this state
        /// </summary>
        /// <param name="move">The move to be executed</param>
        private void placeWall(QuoridorMove move)
        {

        }
        
        private void getPlayerCoordinates(bool isWhite, ref int row, ref int column)
        {
            if (isWhite)
            {
                row = white.Row;
                column = white.Column;
            }
            else
            {
                row = black.Row;
                column = black.Column;
            }
        }
    }
}
