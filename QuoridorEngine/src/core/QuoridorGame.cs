using QuoridorEngine.Solver;
using QuoridorEngine.Utils;
using System.Diagnostics;

namespace QuoridorEngine.Core
{
    /// <summary>
    /// Implementation of the IGameState interface specific to
    /// a Quoridor state. Is responsible for managing all the 
    /// information and operations related to a Quoridor Game
    /// including but not limited to the board, the players,
    /// the walls, the moves etc. It's only possible to interact
    /// with a Quoridor Game State using this class' public interface
    /// </summary>
    public class QuoridorGame : IGameState
    {
        // TODO: maybe this variable should only be handled by Board
        // class in the future
        private readonly int dimension = 9;
        private readonly QuoridorBoard board;
        private readonly QuoridorPlayer white;
        private readonly QuoridorPlayer black;
        private readonly List<QuoridorMove> gameHistory;

        /// <summary>
        /// Initializes a new Quoridor Game with the specified parameters
        /// </summary>
        /// <param name="dimension">The dimension of the board.
        /// Needs to be odd and greater than 2. Throws an ArgumentException if not</param>
        public QuoridorGame(int dimension)
        {
            if (dimension < 2 || dimension % 2 == 1) throw new ArgumentException("Invalid Board Size");

            this.dimension = dimension;
            board = new QuoridorBoard(dimension);

            int startingColumn = dimension / 2 + 1;
            white = new QuoridorPlayer(true, 0, startingColumn, 10, dimension - 1);
            black = new QuoridorPlayer(false, dimension - 1, startingColumn, 10, 0);

            gameHistory = new List<QuoridorMove>();
        }

        /// <summary>
        /// Returns true if the state is terminal (i.e. game is over)
        /// </summary>
        /// <returns>True if the state is terminal (i.e. game is over)</returns>
        public bool IsTerminalState()
        {
            Debug.Assert(white is not null);
            Debug.Assert(black is not null);

            return white.IsInTargetBaseline() || black.IsInTargetBaseline();
        }

        /// <summary>
        /// Returns a list of all the possible moves from this state for a given player
        /// </summary>
		/// <param name="playerIsWhite">True if we want moves for white player, false otherwise</param>
        /// <returns>A list of all the possible moves from this state for given player</returns>
        public List<Move> GetPossibleMoves(bool playerIsWhite)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes a given move in this state
        /// </summary>
        /// <param name="move">The move to be executed. Only accepts PlayerMovement or WallPlacement
        /// type of moves. If this is not the case throws an ArgumentException</param>
        public void ExecuteMove(Move move)
        {
            if (move is not QuoridorMove) throw new ArgumentException("Unsupported move type provided in ExecuteMove");

            QuoridorMove newMove = (QuoridorMove)(move);
            if (newMove.Type == MoveType.WallPlacement) placeWall(newMove);
            else if (newMove.Type == MoveType.PlayerMovement) movePlayer(newMove);
            else throw new InvalidMoveException("Unknown move type");
        }

        /// <summary>
        /// Undoes a given move returning the state to its previous configuration. Assumes the move to be
        /// undone was legal at the moment it was executed.
        /// </summary>
        /// <param name="move">The move to be undone</param>
        public void UndoMove(Move move)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an evaluation of the likelyhood of selected player winning the game from this state
        /// </summary>
        /// <param name="playerIsWhite">True if player we are asking for is white, false otherwise</param>
        /// <returns>An evaluation of the likelyhood of selected player winning the game from this state</returns>
        public float EvaluateState(bool playerIsWhite)
        {
            throw new NotImplementedException();
        }

        public void GetWhiteCoordinates(ref int row, ref int column)
        {
            row = white.Row;
            column = white.Column;
        }

        public void GetBlackCoordinates(ref int row, ref int column)
        {
            row = black.Row;
            column= black.Column;
        }

        public int Dimension { get; }

        /// <summary>
        /// Executes a given player movement in this state. If the move has invalid parameters, throws an
        /// InvalidMoveException.
        /// </summary>
        /// <param name="move">The move desribing the player movement</param>
        private void movePlayer(QuoridorMove move)
        {
            // Checking if values are inside bounds
            if (!board.IsValidPlayerSquare(move.Row, move.Column)) 
                throw new InvalidMoveException("Coordinates out of bounds");

            // Check if any walls make the move impossible
            QuoridorPlayer targetPlayer = getTargetPlayer(move.IsWhitePlayer);
            Debug.Assert(targetPlayer is not null);

            int deltaRow = move.Row - targetPlayer.Row;
            int deltaColumn = move.Column - targetPlayer.Column;

            if (deltaRow == 0 && board.CheckWallPartHorizontal(targetPlayer.Row, targetPlayer.Column)) 
                throw new InvalidMoveException("Wall is blocking player move");
            if (deltaColumn == 0 && board.CheckWallPartVertical(targetPlayer.Row, targetPlayer.Column))
                throw new InvalidMoveException("Wall is blocking player move");

            // Checking if another player is already located on destination coordinates
            if (white.Row == move.Row && white.Column == move.Column) throw new InvalidMoveException("This position is occupied");
            if (black.Row == move.Row && black.Column == move.Column) throw new InvalidMoveException("This position is occupied");

            // Verify that player is not moving more than one square at a time
            // TODO: Player can jump his opponent when they are side by side
            if (deltaRow + deltaColumn > 1) throw new InvalidMoveException("Player tried to move more than 2 cells at once");

            // Finally execute the move
            targetPlayer.Row = move.Row;
            targetPlayer.Column = move.Column;

            gameHistory.Add(move);
        } 

        /// <summary>
        /// Places a wall according to the parameters desribed by the given move
        /// If the wall placement is invalid throws an InvalidMoveException
        /// </summary>
        /// <param name="move">The move describing the wall placement</param>
        private void placeWall(QuoridorMove move)
        {
            // Checking if values are inside bounds
            if (move.Column < 0 || move.Column >= dimension - 1) throw new InvalidMoveException("Coordinates out of bounds");
            if (move.Row < 1 || move.Row >= dimension) throw new InvalidMoveException("Coordinates out of bounds");

            // Check if player has enough walls left
            QuoridorPlayer targetPlayer = getTargetPlayer(move.IsWhitePlayer);
            Debug.Assert(targetPlayer != null);

            if (targetPlayer.AvailableWalls <= 0) throw new InvalidMoveException("Player has no walls left");

            if (move.Orientation == Orientation.Horizontal)
            {
                // Check if any walls occupy the space needed by the new wall
                if (board.CheckWallPartHorizontal(move.Row, move.Column) ||
                    board.CheckWallPartHorizontal(move.Row, move.Column + 1))
                    throw new InvalidMoveException("Wall position occupied");

                board.AddWallPartHorizontal(move.Row, move.Column);
                board.AddWallPartHorizontal(move.Row, move.Column+1);
            }
            else if (move.Orientation == Orientation.Vertical)
            {              
                // Check if any walls occupy the space needed by the new wall
                if (board.CheckWallPartVertical(move.Row, move.Column) ||
                    board.CheckWallPartVertical(move.Row - 1, move.Column))
                    throw new InvalidMoveException("Wall position occupied");

                board.AddWallPartVertical(move.Row, move.Column);
                board.AddWallPartVertical(move.Row - 1, move.Column);
            }
            else throw new InvalidMoveException("Unknown orientation specification");

            // TODO: Check whether new wall blocks and player's path to the goal
            // TODO: Check whether new wall forms a cross with any of the existing walls

            targetPlayer.DecreaseAvailableWalls();
            gameHistory.Add(move);
        }

        

        private List<(int, int)> getLegalNeighbourSquares(int row, int col)
        {
            if (!board.IsValidPlayerSquare(row, col))
                throw new ArgumentException("Current square coordinates out of bounds");

            List<(int, int)> legalNeighbours = new();

            for (int i = -1; i < 2; i++)
            {
                if (i == 0) continue;

                if (board.IsValidPlayerSquare(row + i, col) && 
                    board.CheckWallPartHorizontal(row + i, col))
                    legalNeighbours.Add((row + i, col));

                if (board.IsValidPlayerSquare(row, col + i) &&
                    board.CheckWallPartVertical(row, col + i))
                    legalNeighbours.Add((row, col + i));
            }

            return legalNeighbours;
        }

        /// <summary>
        /// Returns the reference to the requested player object
        /// </summary>
        /// <param name="isWhite">True if we need the white player, false if we need the black</param>
        /// <returns>The reference to the white or black player</returns>
        private QuoridorPlayer getTargetPlayer(bool isWhite)
        {
            return isWhite ? white : black;
        }
    }
}
