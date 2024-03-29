﻿using QuoridorEngine.Solver;
using QuoridorEngine.Utils;
using Priority_Queue;
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
        private readonly int dimension;

        private QuoridorBoard board;
        private QuoridorPlayer white;
        private QuoridorPlayer black;
        private Stack<QuoridorMove> gameHistory;

        private long[,] whitePlayerCellHashes;
        private long[,] blackPlayerCellHashes;
        private long[,] horizontalWallCellHashes;
        private long[,] verticalWallCellHashes;
        private long whiteTurnHash;

        private Stack<long> boardZobristHashes;

        /// <summary>
        /// Initializes a new Quoridor Game with the specified parameters
        /// </summary>
        /// <param name="dimension">The dimension of the board.
        /// Needs to be odd and greater than 2. Throws an ArgumentException if not</param>
        public QuoridorGame(int dimension = 9)
        {
            if (dimension < 2 || dimension % 2 == 0) throw new ArgumentException("Invalid Board Size");

            this.dimension = dimension;

            ResetGame();
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
        public IEnumerable<Move> GetPossibleMoves(bool playerIsWhite)
        {
            List<QuoridorMove> possibleMoves = new();
            QuoridorPlayer currentPlayer = getTargetPlayer(playerIsWhite);

            IEnumerable<QuoridorMove> possiblePlayerMoves = (IEnumerable<QuoridorMove>)GetPossiblePlayerMoves(playerIsWhite);
            possibleMoves.AddRange(possiblePlayerMoves);

            if (currentPlayer.AvailableWalls <= 0)
                return possibleMoves;
            
            for (int row = 1; row < dimension; row++)
                for (int col = 0; col < dimension - 1; col++)
                {
                    QuoridorMove hWallMove = new(row, col, playerIsWhite, Orientation.Horizontal);
                    if (canPlaceWall(hWallMove))
                        possibleMoves.Add(hWallMove);

                    QuoridorMove vWallMove = new(row, col, playerIsWhite, Orientation.Vertical);
                    if (canPlaceWall(vWallMove))
                        possibleMoves.Add(vWallMove);
                }

            return possibleMoves;
        }

        public IEnumerable<Move> GetPossiblePlayerMoves(bool playerIsWhite)
        {
            List<QuoridorMove> possiblePlayerMoves = new();
            QuoridorPlayer currentPlayer = getTargetPlayer(playerIsWhite);

            foreach ((int row, int col) in getLegalNeighbourSquares(currentPlayer.Row, currentPlayer.Column, playerIsWhite))
                possiblePlayerMoves.Add(new QuoridorMove(currentPlayer.Row, currentPlayer.Column, row, col, playerIsWhite));

            return possiblePlayerMoves;
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

            generateBoardZobristHash();
        }

        /// <summary>
        /// Undoes last given move returning the state to its previous configuration. 
        /// Assumes the move to be undone was legal at the moment it was executed.
        /// </summary>
        public void UndoMove(Move move)
        {
            QuoridorMove lastMove = (QuoridorMove)(move);
            if (lastMove.Type == MoveType.PlayerMovement)
            {
                QuoridorPlayer targetPlayer = getTargetPlayer(lastMove.IsWhitePlayer);
                Debug.Assert(board.IsValidPlayerSquare(lastMove.PrevRow, lastMove.PrevCol));
                targetPlayer.Row = lastMove.PrevRow;
                targetPlayer.Column = lastMove.PrevCol;
            }
            else if(lastMove.Type == MoveType.WallPlacement)
            {
                if (lastMove.Orientation == Orientation.Horizontal)
                {
                    board.RemoveWallPartHorizontal(lastMove.Row, lastMove.Column);
                    board.RemoveWallPartHorizontal(lastMove.Row, lastMove.Column + 1);
                    board.RemoveCorner(lastMove.Row, lastMove.Column + 1);
                }
                else if (lastMove.Orientation == Orientation.Vertical)
                {
                    board.RemoveWallPartVertical(lastMove.Row, lastMove.Column);
                    board.RemoveWallPartVertical(lastMove.Row - 1, lastMove.Column);
                    board.RemoveCorner(lastMove.Row, lastMove.Column + 1);
                }

                getTargetPlayer(lastMove.IsWhitePlayer).IncreaseAvailableWalls();
            }

            gameHistory.Pop();
            boardZobristHashes.Pop();
        }

        /// <summary>
        /// Returns an evaluation of the likelyhood of selected player winning the game from this state
        /// </summary>
        /// <param name="playerIsWhite">True if player we are asking for is white, false otherwise</param>
        /// <returns>An evaluation of the likelyhood of selected player winning the game from this state</returns>
        public float EvaluateState(bool isWhitePlayerTurn)
        {
            QuoridorPlayer whitePlayer = getTargetPlayer(true);
            QuoridorPlayer blackPlayer = getTargetPlayer(false);

            int whitePlayerDistance = distanceToGoal(true);
            int blackPlayerDistance = distanceToGoal(false);

            int deltaDistance = blackPlayerDistance - whitePlayerDistance;
            int deltaWallsCount = whitePlayer.AvailableWalls - blackPlayer.AvailableWalls;

            float eval = 0;
            eval += deltaDistance;
            eval += deltaWallsCount / 2;

            if (isWhitePlayerTurn && blackPlayerDistance < 3)
                eval -= 3 - blackPlayerDistance;

            if (!isWhitePlayerTurn && whitePlayerDistance < 3)
                eval += 3 - whitePlayerDistance;

            if (whitePlayerDistance == 0)
                eval += 1000;

            if (blackPlayerDistance == 0)
                eval -= 1000;

            return eval;

            /*
            if (whitePlayerDistance == 0)
                return 10000; 
            else if (blackPlayerDistance == 0)
                return -10000;

            int deltaDistance = blackPlayerDistance - whitePlayerDistance;
            int deltaWallsCount = whitePlayer.AvailableWalls - blackPlayer.AvailableWalls;

            float eval = 0;
            eval += 10 * deltaDistance;
            eval += 7 * deltaWallsCount;

            if (isWhitePlayerTurn && blackPlayerDistance < 3)
                eval -= 8 * (3 - blackPlayerDistance);
            else if (!isWhitePlayerTurn && whitePlayerDistance < 3)
                eval += 8 * (3 - whitePlayerDistance);

            return eval;
            */
        }

        /// <summary>
        /// Checks if the board has a wall of the specified orientation (both wall parts) in
        /// a given row and column. Returns true only if both wall parts are in place
        /// </summary>
        /// <param name="row">The row of the first part of the wall</param>
        /// <param name="column">The column of the first part of the wall</param>
        /// <param name="orientation">The orientation of the wall</param>
        /// <exception cref="ArgumentException"></exception>
        public bool HasWall(int row, int column, Orientation orientation)
        {
            //TODO: make the necessary assertions
            if (orientation == Orientation.Horizontal)
                return board.CheckWallPartHorizontal(row, column) && board.CheckWallPartHorizontal(row, column + 1)
                    && board.CheckCorner(row, column + 1);
            else if (orientation == Orientation.Vertical)
                return board.CheckWallPartVertical(row, column) && board.CheckWallPartVertical(row - 1, column)
                    && board.CheckCorner(row, column + 1);
            throw new ArgumentException("Unknown orientation type");
        }

        public bool HasWallPiece(int row, int column, Orientation orientation)
        {
            //TODO: make the necessary assertions
            if (orientation == Orientation.Horizontal)
                return board.CheckWallPartHorizontal(row, column);
            else if (orientation == Orientation.Vertical)
                return board.CheckWallPartVertical(row, column);
                    
            throw new ArgumentException("Unknown orientation type");
        }

        /// <summary>
        /// Moves the specified player to the requested coordinates
        /// 
        /// CAUTION: this function bypasses almost all of the move
        /// legality checks. Therefore it should only be used for
        /// testing and/or debugging purposes. DO NOT USE IT ANYWHERE
        /// ELSE!!!
        /// </summary>
        /// <param name="row">The new row for the player</param>
        /// <param name="column">The new column for the player</param>
        /// <param name="isWhite">True if this is a move for white, false for black</param>
        public void ForcePlayerMovement(int row, int column, bool isWhite)
        {
            Debug.Assert(board.IsValidPlayerSquare(row, column));
            
            QuoridorPlayer targetPlayer = getTargetPlayer(isWhite);
            Debug.Assert(targetPlayer != null);

            targetPlayer.Row = row;
            targetPlayer.Column = column;
        }

        /// <summary>
        /// The board is reset and cleared, the pawns are set to their starting positions,
        /// the number of walls is reset to default value and the move history is cleared
        /// </summary>
        public void ResetGame()
        {
            board = new QuoridorBoard(dimension);

            int startingColumn = dimension / 2;
            white = new QuoridorPlayer(true, 0, startingColumn, 10, dimension - 1);
            black = new QuoridorPlayer(false, dimension - 1, startingColumn, 10, 0);

            gameHistory = new Stack<QuoridorMove>();
            boardZobristHashes = new Stack<long>();

            setHashes();
            generateBoardZobristHash();
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

        public int GetPlayerWalls(bool isWhite)
        {
            return getTargetPlayer(isWhite).AvailableWalls;
        }

        public void SetPlayerWalls(bool isWhite, int numOfWalls)
        {
            if (numOfWalls < 0) throw new ArgumentException("Negative number of walls given to player");
            getTargetPlayer(isWhite).AvailableWalls = numOfWalls;
        }

        /// <summary>
        /// Returns true if white won the game, or false if black won
        /// Always assumes that the game is over. DO NOT CALL if 
        /// IsTerminalState() is not true.
        /// </summary>
        public bool WinnerIsWhite()
        {
            Debug.Assert(IsTerminalState());
            return white.IsInTargetBaseline();
        }

        /// <summary>
        /// Undoes the last x moves of the game history.
        /// </summary>
        /// <param name="x">The amount of moves to undo</param>
        /// <exception cref="ArgumentException">Thrown if x is negative/zero or greater 
        /// than the number of moves played</exception>
        public void UndoMoves(int x)
        {
            if(x <= 0 || x > gameHistory.Count) throw new ArgumentException();

            for(int i = 0; i < x; i++)
                UndoMove(gameHistory.Peek()); 
        }

        /// <summary>
        /// Get state's hash. Takes into account the player whose turn it is.
        /// </summary>
        /// <param name="isWhitePlayer">Player whose turn it is</param>
        /// <returns>State's hash</returns>
        public long GetHash(bool isWhitePlayerTurn)
        {
            long hash = boardZobristHashes.Peek();
            if (isWhitePlayerTurn)
                hash ^= whiteTurnHash;
            return hash;    
        }

        public int Dimension { get => dimension; }

        /// <summary>
        /// Executes a given player movement in this state. If the move has invalid parameters, throws an
        /// InvalidMoveException.
        /// </summary>
        /// <param name="move">The move desribing the player movement</param>
        private void movePlayer(QuoridorMove move)
        {
            QuoridorPlayer targetPlayer = getTargetPlayer(move.IsWhitePlayer);
            Debug.Assert(targetPlayer is not null);

            List<(int, int)> legalNeighbourSquares = getLegalNeighbourSquares(targetPlayer.Row, targetPlayer.Column, move.IsWhitePlayer);
            Debug.Assert(legalNeighbourSquares.Count > 0);
            
            // Check player move's validity
            if (!legalNeighbourSquares.Contains((move.Row, move.Column)))
                throw new InvalidMoveException();
;
            // Finally execute the move
            targetPlayer.Row = move.Row;
            targetPlayer.Column = move.Column;

            gameHistory.Push(move);
        } 

        /// <summary>
        /// Places a wall according to the parameters desribed by the given move
        /// If the wall placement is invalid throws an InvalidMoveException
        /// </summary>
        /// <param name="move">The move describing the wall placement</param>
        private void placeWall(QuoridorMove move)
        {
            if (!canPlaceWall(move)) throw new InvalidMoveException();

            QuoridorPlayer targetPlayer = getTargetPlayer(move.IsWhitePlayer);
            Debug.Assert(targetPlayer != null);

            if (targetPlayer.AvailableWalls <= 0) throw new InvalidMoveException("Player has no walls left");

            if (move.Orientation == Orientation.Horizontal)
            {
                board.AddWallPartHorizontal(move.Row, move.Column);
                board.AddWallPartHorizontal(move.Row, move.Column + 1);
                board.AddCorner(move.Row, move.Column + 1);
            }
            else if (move.Orientation == Orientation.Vertical)
            {                    
                board.AddWallPartVertical(move.Row, move.Column);
                board.AddWallPartVertical(move.Row - 1, move.Column);
                board.AddCorner(move.Row, move.Column + 1);
            }

            targetPlayer.DecreaseAvailableWalls();
            gameHistory.Push(move);
        }

        /// <summary>
        /// Check whether provided wall placement move is valid
        /// </summary>
        /// <param name="move">Wall placement move</param>
        /// <returns>True if valid</returns>
        private bool canPlaceWall(QuoridorMove move)
        {
            // Check if coordinates are inside bounds
            if ((move.Column < 0 || move.Column >= dimension - 1) ||
                (move.Row < 1 || move.Row >= dimension))
                return false;

            // Check if player has enough walls left
            if (getTargetPlayer(move.IsWhitePlayer).AvailableWalls <= 0)
                return false;

            if (move.Orientation == Orientation.Horizontal)
            {
                // Check if any walls occupy the space needed by the new
                // wall or new call forms illegal cross when placed
                if (board.CheckWallPartHorizontal(move.Row, move.Column) ||
                    board.CheckWallPartHorizontal(move.Row, move.Column + 1) ||
                    board.CheckCorner(move.Row, move.Column + 1))
                    return false;
            }
            else if (move.Orientation == Orientation.Vertical)
            {
                // Check if any walls occupy the space needed by the new
                // wall or new call forms illegal cross when placed
                if (board.CheckWallPartVertical(move.Row, move.Column) ||
                    board.CheckWallPartVertical(move.Row - 1, move.Column) ||
                    board.CheckCorner(move.Row, move.Column + 1))
                    return false;
            }
            else
                // Unknown orientation
                return false;

            // Temporarly place requested wall
            if (move.Orientation == Orientation.Horizontal)
            {
                board.AddWallPartHorizontal(move.Row, move.Column);
                board.AddWallPartHorizontal(move.Row, move.Column + 1);
                board.AddCorner(move.Row, move.Column + 1);
            }
            else if (move.Orientation == Orientation.Vertical)
            {
                board.AddWallPartVertical(move.Row, move.Column);
                board.AddWallPartVertical(move.Row - 1, move.Column);
                board.AddCorner(move.Row, move.Column + 1);
            }

            // Check if players have clear paths to their baselines
            bool whitePath = playerCanReachBaseline(isWhite: true);
            bool blackPath = playerCanReachBaseline(isWhite: false);

            // Remove temporarly placed wall
            if (move.Orientation == Orientation.Horizontal)
            {
                board.RemoveWallPartHorizontal(move.Row, move.Column);
                board.RemoveWallPartHorizontal(move.Row, move.Column + 1);
                board.RemoveCorner(move.Row, move.Column + 1);
            }
            else if (move.Orientation == Orientation.Vertical)
            {
                board.RemoveWallPartVertical(move.Row, move.Column);
                board.RemoveWallPartVertical(move.Row - 1, move.Column);
                board.RemoveCorner(move.Row, move.Column + 1);
            }

            if (!whitePath || !blackPath)
                return false;

            return true;

        }

        /// <summary>
        /// Check whether a path connecting current player's square
        /// and his target baseline exists using DFS algorithm.
        /// </summary>
        /// <param name="isWhite">Current player is White</param>
        /// <returns>True if path exists</returns>
        private bool playerCanReachBaseline(bool isWhite)
        {
            // Pending squares to be explored
            Stack<(int, int)> frontierSquares = new();

            // Already visited squares
            HashSet<(int, int)> visitedSquares = new();

            // Get current player object
            QuoridorPlayer currentPlayer = getTargetPlayer(isWhite);
            Debug.Assert(currentPlayer != null);

            // Add player's current square in frontier as first square to be explored
            frontierSquares.Push((currentPlayer.Row, currentPlayer.Column));

            while (frontierSquares.Count != 0)
            {
                // Get a square (and remove it) from frontier
                (int currentSquareRow, int currentSquareCol) = frontierSquares.Pop();

                // Skip this square if it has already been visited
                if (visitedSquares.Contains((currentSquareRow, currentSquareCol))) 
                    continue;

                // Store current square as visited
                visitedSquares.Add((currentSquareRow, currentSquareCol));

                // Check if player has reached goal
                if (currentPlayer.RowIsTargetBaseline(currentSquareRow))
                    return true;

                // Get current square's legal neighbours
                List<(int, int)> unblockedNeighbors = getUnblockedNeighborSquares(currentSquareRow, currentSquareCol);

                // Sort neighbours by descending row if current
                // player is black to reach his baseline faster 
                if (!isWhite)
                    unblockedNeighbors = unblockedNeighbors.OrderByDescending(x => x.Item1).ToList();

                // Store current square's legal neighbours in frontier to be explored later
                foreach ((int, int) neighbourSquare in unblockedNeighbors)
                    if (!visitedSquares.Contains(neighbourSquare)) 
                        frontierSquares.Push(neighbourSquare);
            }

            // No path found
            return false;
        }

#if DEBUG
        public int distanceToGoal(bool isWhite)
#else
        /// <summary>
        /// Finds the minimum distance in squares the given player needs
        /// to travel to reach their target baseline.
        /// </summary>
        /// <param name="isWhite">Whether current player is White</param>
        /// <returns>The minimum distance of given player to goal</returns>
        private int distanceToGoal(bool isWhite)
#endif
        {
            QuoridorPlayer currentPlayer = getTargetPlayer(isWhite);
            Debug.Assert(currentPlayer != null);

            // Performing a simple A* search to find the distance to the goal row

            // Frontier is a priority queue holding the current row, column and distance travelled
            SimplePriorityQueue<(int, int, int)> pq = new();

            // Used to store already visited nodes
            HashSet<(int, int)> visitedSquares = new();

            // Search starts from current position with 0 distance travelled so far
            pq.Enqueue((currentPlayer.Row, currentPlayer.Column, 0), 0);

            // Run until there are no more squares to explore
            while(pq.Count > 0)
            {
                (int currentRow, int currentCol, int distanceSoFar) = pq.Dequeue();
                // Goal reached so return the distance
                if(currentPlayer.RowIsTargetBaseline(currentRow)) return distanceSoFar;

                // Skip already visited nodes
                if (visitedSquares.Contains((currentRow, currentCol))) continue;

                visitedSquares.Add((currentRow, currentCol));

                // Expand neighbors
                List<(int, int)> legalNeighbours = getLegalNeighbourSquares(currentRow, currentCol, isWhite);

                foreach(var neighborSquare in legalNeighbours)
                {
                    (int newRow, int newCol) = neighborSquare;

                    // The heuristic is the manhttan distance of the player to the target base
                    int heuristic = currentPlayer.ManhattanDistanceToTargetBaseline(newRow);
                    int priority = heuristic + distanceSoFar + 1;

                    pq.Enqueue((newRow, newCol,  distanceSoFar + 1), priority);
                }
            }

            return -1;
        }

        /// <summary>
        /// Check for neighbour squares a player can move to 
        /// without encountering a wall or the other player.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>List of tuples with coordinates of legal neighbours</returns>
        /// 
        /// TODO: simplify this later using functions from the Vector2 class
        private List<(int, int)> getLegalNeighbourSquares(int row, int col, bool currentPlayerIsWhite)
        {
            Debug.Assert(board.IsValidPlayerSquare(row, col));

            List<(int, int)> legalNeighbours = new();

            // Square 'DOWN' is valid and not blocked by wall
            if (board.IsValidPlayerSquare(row - 1, col) && !board.CheckWallPartHorizontal(row, col))
                // Opponent is in square 'DOWN'
                if (opponentOccupiesSquare(row - 1, col, !currentPlayerIsWhite))
                {
                    // Square 'DOWN-DOWN' is valid and not blocked by wall
                    if (board.IsValidPlayerSquare(row - 2, col) && !board.CheckWallPartHorizontal(row - 1, col))
                        legalNeighbours.Add((row - 2, col));
                    else
                    {
                        // Square 'DOWN-LEFT' is valid and not blocked by wall
                        if (board.IsValidPlayerSquare(row - 1, col - 1) && !board.CheckWallPartVertical(row - 1, col - 1))
                            legalNeighbours.Add((row - 1, col - 1));

                        // Square 'DOWN-RIGHT' is valid and not blocked by wall
                        if (board.IsValidPlayerSquare(row - 1, col + 1) && !board.CheckWallPartVertical(row - 1, col))
                            legalNeighbours.Add((row - 1, col + 1));
                    }   
                }
                // Square 'DOWN' is not occupied
                else
                    legalNeighbours.Add((row - 1, col));


            // Square 'LEFT' is valid and not blocked by wall
            if (board.IsValidPlayerSquare(row, col - 1) && !board.CheckWallPartVertical(row, col - 1))
                // Opponent is in square 'LEFT'
                if (opponentOccupiesSquare(row, col - 1, !currentPlayerIsWhite))
                {
                    // Square 'LEFT-LEFT' is valid and not blocked by wall
                    if (board.IsValidPlayerSquare(row, col - 2) && !board.CheckWallPartVertical(row, col - 2))
                        legalNeighbours.Add((row, col - 2));
                    else
                    {
                        // Square 'LEFT-UP' is valid and not blocked by wall
                        if (board.IsValidPlayerSquare(row + 1, col - 1) && !board.CheckWallPartHorizontal(row + 1, col - 1))
                            legalNeighbours.Add((row + 1, col - 1));

                        // Square 'LEFT-DOWN' is valid and not blocked by wall
                        if (board.IsValidPlayerSquare(row - 1, col - 1) && !board.CheckWallPartHorizontal(row, col - 1))
                            legalNeighbours.Add((row - 1, col - 1));
                    }
                }
                // Square 'LEFT' is not occupied
                else
                    legalNeighbours.Add((row, col - 1));

            // Square 'RIGHT' is valid and not blocked by wall
            if (board.IsValidPlayerSquare(row, col + 1) && !board.CheckWallPartVertical(row, col))
                // Opponent is in square 'RIGHT'
                if (opponentOccupiesSquare(row, col + 1, !currentPlayerIsWhite))
                {
                    // Square 'RIGHT-RIGHT' is valid and not blocked by wall
                    if (board.IsValidPlayerSquare(row, col + 2) && !board.CheckWallPartVertical(row, col + 1))
                        legalNeighbours.Add((row, col + 2));
                    else
                    {
                        // Square 'RIGHT-UP' is valid and not blocked by wall
                        if (board.IsValidPlayerSquare(row + 1, col + 1) && !board.CheckWallPartHorizontal(row + 1, col + 1))
                            legalNeighbours.Add((row + 1, col + 1));

                        // Square 'RIGHT-DOWN' is valid and not blocked by wall
                        if (board.IsValidPlayerSquare(row - 1, col + 1) && !board.CheckWallPartHorizontal(row, col + 1))
                            legalNeighbours.Add((row - 1, col + 1));
                    }
                }
                // Square 'RIGHT' is not occupied
                else
                    legalNeighbours.Add((row, col + 1));

            // Square 'UP' is valid and not blocked by wall
            if (board.IsValidPlayerSquare(row + 1, col) && !board.CheckWallPartHorizontal(row + 1, col))
                // Opponent is in square 'UP'
                if (opponentOccupiesSquare(row + 1, col, !currentPlayerIsWhite))
                {
                    // Square 'UP-UP' is valid and not blocked by wall
                    if (board.IsValidPlayerSquare(row + 2, col) && !board.CheckWallPartHorizontal(row + 2, col))
                        legalNeighbours.Add((row + 2, col));
                    else
                    {
                        // Square 'UP-LEFT' is valid and not blocked by wall
                        if (board.IsValidPlayerSquare(row + 1, col - 1) && !board.CheckWallPartVertical(row + 1, col - 1))
                            legalNeighbours.Add((row + 1, col - 1));

                        // Square 'UP-RIGHT' is valid and not blocked by wall
                        if (board.IsValidPlayerSquare(row + 1, col + 1) && !board.CheckWallPartVertical(row + 1, col))
                            legalNeighbours.Add((row + 1, col + 1));
                    }
                }
                // Square 'UP' is not occupied
                else
                    legalNeighbours.Add((row + 1, col));

            return legalNeighbours;
        }

        private List<(int, int)> getUnblockedNeighborSquares(int row, int col)
        {
            Debug.Assert(board.IsValidPlayerSquare(row, col));

            List<(int, int)> unblockedNeighbors = new();

            // Square 'DOWN' is valid and not blocked by wall
            if (board.IsValidPlayerSquare(row - 1, col) && !board.CheckWallPartHorizontal(row, col))
                unblockedNeighbors.Add((row - 1, col));

            // Square 'LEFT' is valid and not blocked by wall
            if (board.IsValidPlayerSquare(row, col - 1) && !board.CheckWallPartVertical(row, col - 1))
                unblockedNeighbors.Add((row, col - 1));

            // Square 'RIGHT' is valid and not blocked by wall
            if (board.IsValidPlayerSquare(row, col + 1) && !board.CheckWallPartVertical(row, col))
                unblockedNeighbors.Add((row, col + 1));

            // Square 'UP' is valid and not blocked by wall
            if (board.IsValidPlayerSquare(row + 1, col) && !board.CheckWallPartHorizontal(row + 1, col))
                unblockedNeighbors.Add((row + 1, col));

            return unblockedNeighbors;
        }

        private bool opponentOccupiesSquare(int squareRow, int squareCol, bool opponentIsWhite)
        {
            QuoridorPlayer opponent = getTargetPlayer(opponentIsWhite);
            return squareRow == opponent.Row && squareCol == opponent.Column;
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

        /// <summary>
        /// Calculate and store (into the boardZobristHashes stack) the 
        /// new zobrist hash of the board after execution of new move
        /// </summary>
        private void generateBoardZobristHash()
        {
            long boardZobristHash;

            // Initial board state (game has just started)
            if (boardZobristHashes.Count() == 0)
            {
                QuoridorPlayer whitePlayer = getTargetPlayer(true);
                QuoridorPlayer blackPlayer = getTargetPlayer(false);

                long initWhitePawnHash = whitePlayerCellHashes[whitePlayer.Row, whitePlayer.Column];
                long initBlackPawnHash = blackPlayerCellHashes[blackPlayer.Row, blackPlayer.Column];

                long initPlayerPawnsHash = initWhitePawnHash ^ initBlackPawnHash;

                boardZobristHash = initPlayerPawnsHash;
                boardZobristHashes.Push(boardZobristHash);
                return;
            }

            QuoridorMove lastMove = gameHistory.Peek();
            long previousBoardZobristHash = boardZobristHashes.Peek();
            boardZobristHash = previousBoardZobristHash; 

            if (lastMove.Type == MoveType.PlayerMovement)
            {
                long lastPawnMovePreviousHash = lastMove.PrevRow ^ lastMove.PrevCol;
                long lastPawnMoveCurrentHash = lastMove.Row ^ lastMove.Column;

                // Removing pawn from initial location before last move
                // (XOR self-inverse property)
                boardZobristHash ^= lastPawnMovePreviousHash;

                // Adding pawn to it's new location after last move's execution 
                boardZobristHash ^= lastPawnMoveCurrentHash;
            }
            else if (lastMove.Type == MoveType.WallPlacement)
            {
                long lastWallPlacementHash = lastMove.Row ^ lastMove.Column;
                boardZobristHash ^= lastWallPlacementHash;
            }

            boardZobristHashes.Push(boardZobristHash);
        }

        private void setHashes()
        {
            Random rand = new();
            HashSet<long> usedRandNums= new();

            set2DRandArray(ref whitePlayerCellHashes, dimension, dimension, ref rand, ref usedRandNums);
            set2DRandArray(ref blackPlayerCellHashes, dimension, dimension, ref rand, ref usedRandNums);
            set2DRandArray(ref horizontalWallCellHashes, dimension, dimension - 1, ref rand, ref usedRandNums);
            set2DRandArray(ref verticalWallCellHashes, dimension - 1, dimension, ref rand, ref usedRandNums);

            whiteTurnHash = getUniqueRandInt64(ref rand, ref usedRandNums);
        }

        private long getUniqueRandInt64(ref Random rand, ref HashSet<long> usedRandNums)
        {
            long uniqueRandInt64;
            do uniqueRandInt64 = rand.NextInt64(); while (usedRandNums.Contains(uniqueRandInt64));
            usedRandNums.Add(uniqueRandInt64);
            return uniqueRandInt64;
        }

        private void set2DRandArray(ref long[,] randNums, int rows, int columns, ref Random rand, ref HashSet<long> usedRandNums)
        {
            randNums = new long[rows, columns];
            for (int row = 0; row < randNums.GetLength(0); row++)
                for (int column = 0; column < randNums.GetLength(1); column++)
                    randNums[row, column] = getUniqueRandInt64(ref rand, ref usedRandNums);
        }
    }
}