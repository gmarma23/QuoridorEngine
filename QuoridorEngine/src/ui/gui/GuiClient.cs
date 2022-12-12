using QuoridorEngine.Core;
using QuoridorEngine.Utils;
using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.UI
{
#if !CONSOLE
    public class GuiClient
    {
        private GuiFrame guiFrame;
        private QuoridorGame game;
        private GameMode gameMode;

        private readonly Dictionary<string, EventHandler> boardEventHandlers;

        private delegate void BoardCellAction(int row, int column);
        
        private bool isWhitePlayerTurn;
        private bool initPlayerMove;

        public GuiClient(int boardDimension, int playerWallsCount, GameMode gameMode) 
        {
            game = new QuoridorGame(boardDimension);
            guiFrame = new GuiFrame();

            game.SetPlayerWalls(true, playerWallsCount);
            game.SetPlayerWalls(false, playerWallsCount);

            this.gameMode = gameMode;

            boardEventHandlers = new Dictionary<string, EventHandler>
            {
                { "OnPlayerCellClick", MovePlayer },
                { "OnWallCellClick", PlaceWall },
                { "OnWallCellEnter", PreviewWall },
                { "OnWallCellLeave", RemoveWallPreview },
                { "OnPlayerPawnClick", PlayerPawnClicked }
            };

            renderGameComponents();
            
            if (gameMode != GameMode.WhiteIsAI) 
                guiFrame.BoardEventsSubscribe();

            isWhitePlayerTurn = true;
            initPlayerMove = false;
        }

        public void RunGui()
        {
            Application.Run(guiFrame);
        }

        /// <summary>
        /// Event handler to preview a wall placing move
        /// </summary>
        /// <param name="sender">Wall part cell with invoked event</param>
        /// <param name="e">Event arguments</param>
        private void PreviewWall(object sender, EventArgs e)
        {
            GuiWallPartCell wallPartCell = (GuiWallPartCell)sender;

            // Ignore if wall part is already placed
            if (wallPartCell.IsPlaced) return;

            // Construct new quoridor move
            Orientation orientation = getWallOrientation(wallPartCell);
            (int gameRow, int gameColumn) = TransformCoordinates.GuiToGameWall(wallPartCell.Row, wallPartCell.Column, orientation);
            QuoridorMove newMove = new QuoridorMove(gameRow, gameColumn, isWhitePlayerTurn, orientation);

            try
            {
                // Attempt new move execution
                game.ExecuteMove(newMove);
            }
            catch (InvalidMoveException)
            {
                // Ignore if invalid
                return;
            }

            // Update gui wall cells based on new state
            refreshWallCells(guiFrame.UseWallCell, true);

            // Update player's wall counter in gui
            guiFrame.SetPlayerWallCounter(isWhitePlayerTurn, game.GetPlayerWalls(isWhitePlayerTurn));
        }

        // Event handler for a wall placing move
        private void PlaceWall(object sender, EventArgs e)
        {
            GuiWallPartCell wallPartCell = (GuiWallPartCell)sender;

            // Ignore if wall part is already placed
            if (wallPartCell.IsPlaced) return;

            // Make wall preview permanent in gui
            refreshWallCells(guiFrame.PlaceWallCell, true);

            // Cancel interrupted player pawn move 
            if (initPlayerMove) hidePossiblePlayerMoves();

            // Player's turn has finished
            switchPlayerTurn();
        }

        /// <summary>
        /// Event handler for removing wall placing move preview
        /// </summary>
        /// <param name="sender">Wall part cell with invoked event</param>
        /// <param name="e">Event arguments</param>
        private void RemoveWallPreview(object sender, EventArgs e)
        {
            GuiWallPartCell wallPartCell = (GuiWallPartCell)sender;

            // Ignore if wall part has no active preview
            if (!wallPartCell.IsActive) return;

            // Ignore if wall part is already placed
            if (wallPartCell.IsPlaced) return;

            // Undo last move (wall preview) in core
            game.UndoMoves(1);

            // Update gui wall cells based on new state
            refreshWallCells(guiFrame.FreeWallCell, false);

            // Update player's wall counter in gui
            guiFrame.SetPlayerWallCounter(isWhitePlayerTurn, game.GetPlayerWalls(isWhitePlayerTurn));
        }

        /// <summary>
        /// Event handler for player pawn movement
        /// </summary>
        /// <param name="sender">Player cell to move player pawn</param>
        /// <param name="e">Event arguments</param>
        private void MovePlayer(object sender, EventArgs e)
        {
            // Ignore if player move is not initiated
            if (!initPlayerMove) return;

            GuiPlayerCell playerCell = (GuiPlayerCell)sender;

            // Get current player coordinates
            int currentGameRow = 0, currentGameColumn = 0;
            if(isWhitePlayerTurn)
                game.GetWhiteCoordinates(ref currentGameRow, ref currentGameColumn);
            else
                game.GetBlackCoordinates(ref currentGameRow, ref currentGameColumn);

            // Construct new quoridor move
            (int newGameRow, int newGameColumn) = TransformCoordinates.GuiToGamePlayer(playerCell.Row, playerCell.Column);
            QuoridorMove newMove = new QuoridorMove(currentGameRow, currentGameColumn, newGameRow, newGameColumn, isWhitePlayerTurn);

            try
            {
                // Attempt new move execution
                game.ExecuteMove(newMove);
            }
            catch (InvalidMoveException)
            {
                // Ignore if invalid
                return;
            }

            // Move completed
            hidePossiblePlayerMoves();

            // Update player pawn location in gui based on last move
            guiFrame.MovePlayerPawn(isWhitePlayerTurn, playerCell.Row, playerCell.Column);

            // Player's turn has finished
            switchPlayerTurn();
        }

        /// <summary>
        /// Event handler for player pawn click 
        /// </summary>
        /// <param name="sender">Clicked player pawn</param>
        /// <param name="e">Event arguments</param>
        private void PlayerPawnClicked(object sender, EventArgs e)
        {
            bool isWhitePlayer = ((GuiPlayerPawn)sender).IsWhite;

            // Ignore if not player's turn
            if (isWhitePlayer != isWhitePlayerTurn) return;

            if (initPlayerMove)
                // Cancel initiated player move
                hidePossiblePlayerMoves();
            else
                // Initiate player move 
                showPossiblePlayerMoves(isWhitePlayer);
        }

        /// <summary>
        /// Update gui board player cells to show player's pawn possible moves
        /// </summary>
        /// <param name="isWhitePlayer">Player to show possible moves</param>
        private void showPossiblePlayerMoves(bool isWhitePlayer)
        {
            List<QuoridorMove> possiblePlayerMoves = (List<QuoridorMove>)game.GetPossiblePlayerMoves(isWhitePlayer);

            foreach (QuoridorMove move in possiblePlayerMoves)
            {
                (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiPlayer(move.Row, move.Column);
                guiFrame.PossibleMovePlayerCell(guiRow, guiColumn);
            }

            // Player pawn move initiated
            initPlayerMove = true;
        }

        // Reset all possible player move cells to normal state 
        private void hidePossiblePlayerMoves()
        {
            for (int gameRow = game.Dimension - 1; gameRow >= 0; gameRow--)
                for (int gameColumn = 0; gameColumn < game.Dimension; gameColumn++)
                {
                    (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiPlayer(gameRow, gameColumn);
                    guiFrame.NormalPlayerCell(guiRow, guiColumn);
                }

            // Player pawn move completed, canceled or interrupted
            initPlayerMove = false;
        }

        /// <summary>
        /// Utility to refresh/update wall cells on gui based on changes made in quoridor core
        /// </summary>
        /// <param name="function"> An action to perform on gui wall cells</param>
        /// <param name="hasWallPiece">Utility parameter to determine whether a wall piece 
        /// is placed or not in quoridor core according to performed action</param>
        private void refreshWallCells(BoardCellAction function, bool hasWallPiece)
        {
            for (int gameRow = game.Dimension - 1; gameRow >= 0; gameRow--)
                for (int gameColumn = 0; gameColumn < game.Dimension; gameColumn++)
                {
                    // Check horizontal wall pieces
                    if (gameRow > 0)
                        if (game.HasWallPiece(gameRow, gameColumn, Orientation.Horizontal) == hasWallPiece)
                        {
                            (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiWall(gameRow, gameColumn, Orientation.Horizontal);
                            function(guiRow, guiColumn);
                        }

                    // Check vertical wall pieces
                    if (gameColumn < game.Dimension - 1)
                        if (game.HasWallPiece(gameRow, gameColumn, Orientation.Vertical) == hasWallPiece)
                        {
                            (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiWall(gameRow, gameColumn, Orientation.Vertical);
                            function(guiRow, guiColumn);
                        }
                }
        }

        // Render gui elements based on quoridor game core
        private void renderGameComponents()
        {
            // Render board
            int guiBoardDimension = TransformCoordinates.GameToGuiDimension(game.Dimension);
            guiFrame.RenderBoard(guiBoardDimension, boardEventHandlers);
            
            // Render player pawns
            int gameWhitePawnRow = 0, gameWhitePawnColumn = 0, gameBlackPawnRow = 0, gameBlackPawnColumn = 0;
            
            game.GetWhiteCoordinates(ref gameWhitePawnRow, ref gameWhitePawnColumn);
            (int guiWhitePawnRow, int guiWhitePawnColumn) = TransformCoordinates.GameToGuiPlayer(gameWhitePawnRow, gameWhitePawnColumn);
            guiFrame.MovePlayerPawn(true, guiWhitePawnRow, guiWhitePawnColumn);

            game.GetBlackCoordinates(ref gameBlackPawnRow, ref gameBlackPawnColumn);
            (int guiBlackPawnRow, int guiBlackPawnColumn) = TransformCoordinates.GameToGuiPlayer(gameBlackPawnRow, gameBlackPawnColumn);
            guiFrame.MovePlayerPawn(false, guiBlackPawnRow, guiBlackPawnColumn);

            // Render player wall counter panels 
            guiFrame.RenderPlayerWallsPanel(true);
            guiFrame.RenderPlayerWallsPanel(false);
            guiFrame.SetPlayerWallCounter(true, game.GetPlayerWalls(true));
            guiFrame.SetPlayerWallCounter(false, game.GetPlayerWalls(false));
        }

        // Group of methods for coordinate transformations to ensure proper game/gui communication
        private static class TransformCoordinates
        {
            public static int GameToGuiDimension(int gameDimension)
            {
                return 2 * gameDimension - 1;
            }

            public static (int, int) GuiToGamePlayer(int guiRow, int guiColumn)
            {
                return (guiRow / 2, guiColumn / 2);
            }

            public static (int, int) GameToGuiPlayer(int gameRow, int gameColumn)
            {
                return (2 * gameRow, 2 * gameColumn);
            }

            public static (int, int) GuiToGameWall(int guiRow, int guiColumn, Orientation orientation)
            {
                return (orientation == Orientation.Horizontal) ? ((guiRow + 1) / 2, guiColumn / 2) : (guiRow / 2, (guiColumn - 1) / 2);
            }

            public static (int, int) GameToGuiWall(int gameRow, int gameColumn, Orientation orientation)
            {
                return (orientation == Orientation.Horizontal) ? (2 * gameRow - 1, 2 * gameColumn) : (2 * gameRow, 2 * gameColumn + 1);
            }
        }

        /// <summary>
        /// Determine orientation of provided wall cell
        /// </summary>
        /// <param name="wallcell">Provided wall cell</param>
        /// <returns>Wall cell orientation</returns>
        private Orientation getWallOrientation(GuiWallPartCell wallcell)
        {
            return (wallcell.Row % 2 == 1 && wallcell.Column % 2 == 0) ? Orientation.Horizontal : Orientation.Vertical;
        }

        // Game over handler
        private bool gameOver()
        {
            // Game is not over
            if (!game.IsTerminalState()) return false;

            // Freeze state 
            guiFrame.BoardEventsUnsubscribe();
            return true;
        }

        // Utility to determine which player has the next move
        private void switchPlayerTurn()
        {
            // Check if game has ended 
            if(gameOver()) return;

            // Switch turns
            isWhitePlayerTurn = !isWhitePlayerTurn;
            
            // AI has the move 
            if ((isWhitePlayerTurn && gameMode == GameMode.WhiteIsAI) ||
               (!isWhitePlayerTurn && gameMode == GameMode.BlackIsAI))
                guiFrame.BoardEventsUnsubscribe();
            // Player has the move
            else if ((!isWhitePlayerTurn && gameMode == GameMode.WhiteIsAI) ||
                    (isWhitePlayerTurn && gameMode == GameMode.BlackIsAI))
                guiFrame.BoardEventsSubscribe();
        }
    }

    public enum GameMode
    {
        TwoPlayers,
        WhiteIsAI,
        BlackIsAI
    }
#endif
}
