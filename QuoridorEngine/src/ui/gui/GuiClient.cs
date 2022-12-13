using QuoridorEngine.Core;
using QuoridorEngine.Utils;
using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.UI
{
#if !CONSOLE
    public class GuiClient
    {
        private GuiFrame guiFrame;
        private QuoridorGame gameState;
        private GameMode gameMode;

        private readonly Dictionary<string, EventHandler> boardEventHandlers;
        
        private bool isWhitePlayerTurn;
        private bool initPlayerMove;

        public GuiClient(int boardDimension, int playerWallsCount, GameMode gameMode) 
        {
            gameState = new QuoridorGame(boardDimension);
            guiFrame = new GuiFrame();

            gameState.SetPlayerWalls(true, playerWallsCount);
            gameState.SetPlayerWalls(false, playerWallsCount);

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
                //guiFrame.BoardEventsSubscribe();

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
                gameState.ExecuteMove(newMove);
            }
            catch (InvalidMoveException)
            {
                // Ignore if invalid
                return;
            }

            // Update gui wall cells based on new state
            guiFrame.UpdateUsedBoardWallCells(gameState);

            // Update player's wall counter in gui
            guiFrame.SetPlayerWallCounter(gameState, isWhitePlayerTurn);
        }

        // Event handler for a wall placing move
        private void PlaceWall(object sender, EventArgs e)
        {
            GuiWallPartCell wallPartCell = (GuiWallPartCell)sender;

            // Ignore if wall part is already placed
            if (wallPartCell.IsPlaced) return;

            // Make wall preview permanent in gui
            guiFrame.UpdatePlacedBoardWallCells(gameState);

            // Cancel interrupted player pawn move 
            if (initPlayerMove) endPlayerPawnMove();

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
            gameState.UndoMoves(1);

            // Update gui wall cells based on new state
            guiFrame.UpdateFreeBoardWallCells(gameState);

            // Update player's wall counter in gui
            guiFrame.SetPlayerWallCounter(gameState, isWhitePlayerTurn);
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
                gameState.GetWhiteCoordinates(ref currentGameRow, ref currentGameColumn);
            else
                gameState.GetBlackCoordinates(ref currentGameRow, ref currentGameColumn);

            // Construct new quoridor move
            (int newGameRow, int newGameColumn) = TransformCoordinates.GuiToGamePlayer(playerCell.Row, playerCell.Column);
            QuoridorMove newMove = new QuoridorMove(currentGameRow, currentGameColumn, newGameRow, newGameColumn, isWhitePlayerTurn);

            try
            {
                // Attempt new move execution
                gameState.ExecuteMove(newMove);
            }
            catch (InvalidMoveException)
            {
                // Ignore if invalid
                return;
            }

            // Move completed
            endPlayerPawnMove();

            // Update player pawn location in gui based on last move
            guiFrame.MovePlayerPawn(gameState, isWhitePlayerTurn);

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
                endPlayerPawnMove();
            else
                beginPayerPawnMove(isWhitePlayer); 
        }  
        
        private void beginPayerPawnMove(bool isWhitePlayer)
        {
            // Player pawn move initiated
            initPlayerMove = true;
            // Initiate player move 
            guiFrame.ShowPossiblePlayerMoves(gameState, isWhitePlayer);
        }

        private void endPlayerPawnMove()
        {
            // Player pawn move completed, canceled or interrupted
            initPlayerMove = false;
            // Cancel initiated player move
            guiFrame.HidePossiblePlayerMoves(gameState);
        }

        // Render gui elements based on quoridor game core
        private void renderGameComponents()
        {
            // Render board
            int guiBoardDimension = TransformCoordinates.GameToGuiDimension(gameState.Dimension);
            guiFrame.RenderBoard(guiBoardDimension, boardEventHandlers);
            
            // Render player pawns
            int gameWhitePawnRow = 0, gameWhitePawnColumn = 0, gameBlackPawnRow = 0, gameBlackPawnColumn = 0;
            
            gameState.GetWhiteCoordinates(ref gameWhitePawnRow, ref gameWhitePawnColumn);
            (int guiWhitePawnRow, int guiWhitePawnColumn) = TransformCoordinates.GameToGuiPlayer(gameWhitePawnRow, gameWhitePawnColumn);
            guiFrame.MovePlayerPawn(gameState, true);

            gameState.GetBlackCoordinates(ref gameBlackPawnRow, ref gameBlackPawnColumn);
            (int guiBlackPawnRow, int guiBlackPawnColumn) = TransformCoordinates.GameToGuiPlayer(gameBlackPawnRow, gameBlackPawnColumn);
            guiFrame.MovePlayerPawn(gameState, false);

            // Render player wall counter panels 
            guiFrame.RenderPlayerWallsPanel(true);
            guiFrame.RenderPlayerWallsPanel(false);
            guiFrame.SetPlayerWallCounter(gameState, true);
            guiFrame.SetPlayerWallCounter(gameState, false);
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
            if (!gameState.IsTerminalState()) return false;

            // Freeze state 
            //guiFrame.BoardEventsUnsubscribe();
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
                //guiFrame.BoardEventsUnsubscribe();
            // Player has the move
            else if ((!isWhitePlayerTurn && gameMode == GameMode.WhiteIsAI) ||
                    (isWhitePlayerTurn && gameMode == GameMode.BlackIsAI))
                //guiFrame.BoardEventsSubscribe();
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
