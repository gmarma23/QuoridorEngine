using QuoridorEngine.Core;
using QuoridorEngine.resources;
using QuoridorEngine.Solver;
using QuoridorEngine.Utils;
using System.Media;
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

        private bool activeBoardEvents;
        private bool isWhitePlayerTurn;
        private bool initPlayerMove;

        private SoundPlayer pawnMoveSound;
        private SoundPlayer wallPlacementSound;

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

            pawnMoveSound = new SoundPlayer(Resources.pawn_move);
            wallPlacementSound = new SoundPlayer(Resources.wall_placement);

            isWhitePlayerTurn = true;
            initPlayerMove = false;

            if (gameMode == GameMode.BlackIsAI)
                activeBoardEvents = true;

            if (gameMode == GameMode.WhiteIsAI || gameMode == GameMode.SoloAI)
            {
                activeBoardEvents = false;
                computerPlayMove();
            }
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
            // Ignore if board events are not active
            if (!activeBoardEvents) return;

            GuiWallPartCell wallPartCell = (GuiWallPartCell)sender;

            // Ignore if wall part is already placed
            if (wallPartCell.IsPlaced) return;

            // Construct new quoridor move
            Orientation orientation = (wallPartCell.Row % 2 == 1 && wallPartCell.Column % 2 == 0) ? Orientation.Horizontal : Orientation.Vertical;
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
            // Ignore if board events are not active
            if (!activeBoardEvents) return;

            GuiWallPartCell wallPartCell = (GuiWallPartCell)sender;

            // Ignore if wall part has no active preview
            if (!wallPartCell.IsActive) return;

            // Ignore if wall part is already placed
            if (wallPartCell.IsPlaced) return;

            // Make wall preview permanent in gui
            guiFrame.UpdatePlacedBoardWallCells(gameState);

            // Play wall placement sound
            wallPlacementSound.Play();

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
            // Ignore if board events are not active
            if (!activeBoardEvents) return;

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
            // Ignore if board events are not active
            if (!activeBoardEvents) return;

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
            
            pawnMoveSound.Play();

            // Player's turn has finished
            switchPlayerTurn();

            // Check if game has ended 
            if (gameMode != GameMode.TwoPlayers)
                computerPlayMove();
        }

        /// <summary>
        /// Event handler for player pawn click 
        /// </summary>
        /// <param name="sender">Clicked player pawn</param>
        /// <param name="e">Event arguments</param>
        private void PlayerPawnClicked(object sender, EventArgs e)
        {
            // Ignore if board events are not active
            if (!activeBoardEvents) return;

            bool isWhitePlayer = ((GuiPlayerPawn)sender).IsWhite;

            // Ignore if not player's turn
            if (isWhitePlayer != isWhitePlayerTurn) return;

            if (initPlayerMove)
                endPlayerPawnMove();
            else
                beginPayerPawnMove(isWhitePlayer); 
        }

        // Utility to determine which player has the next move
        private void switchPlayerTurn()
        {
            if (gameState.IsTerminalState())
            {
                // Check if game has ended 
                gameOverActions();
                return;
            }

            // Switch turns
            isWhitePlayerTurn = !isWhitePlayerTurn;

            // AI has the move 
            if ((isWhitePlayerTurn && gameMode == GameMode.WhiteIsAI) ||
               (!isWhitePlayerTurn && gameMode == GameMode.BlackIsAI))
                activeBoardEvents = false;
            // Player has the move
            else if ((!isWhitePlayerTurn && gameMode == GameMode.WhiteIsAI) ||
                    (isWhitePlayerTurn && gameMode == GameMode.BlackIsAI))
                activeBoardEvents = true;
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

        private void renderGameComponents()
        {
            // Render board
            int guiBoardDimension = TransformCoordinates.GameToGuiDimension(gameState.Dimension);
            guiFrame.RenderBoard(guiBoardDimension, boardEventHandlers);
            
            // Render player pawns
            guiFrame.MovePlayerPawn(gameState, true);
            guiFrame.MovePlayerPawn(gameState, false);

            // Render player wall counter panels 
            guiFrame.RenderPlayerWallsPanel(true);
            guiFrame.RenderPlayerWallsPanel(false);
            guiFrame.SetPlayerWallCounter(gameState, true);
            guiFrame.SetPlayerWallCounter(gameState, false);
        }

        // Game over handler
        private void gameOverActions()
        {
            if (!gameState.IsTerminalState())
                // Game is not over
                return;

            // Show winner message box
            string winner = gameState.WinnerIsWhite() ? "Red" : "Purple";
            MessageBox.Show($"{winner} is the winner", "Game Over!");

            // Freeze state 
            activeBoardEvents = false;
        }

        private async void computerPlayMove()
        {
            if (gameState.IsTerminalState()) 
                return;

            await Task.Delay(1000);

            QuoridorMove bestMove = (Core.QuoridorMove)MinimaxAgent.GetBestMove(gameState, isWhitePlayerTurn, 3);
            gameState.ExecuteMove(bestMove);
            // Update player pawn location in gui based on last move
            guiFrame.MovePlayerPawn(gameState, isWhitePlayerTurn);
            pawnMoveSound.Play();

            // Player's turn has finished
            switchPlayerTurn();

            if (gameMode == GameMode.SoloAI)
                computerPlayMove();
        }
    }

    public enum GameMode
    {
        TwoPlayers,
        WhiteIsAI,
        BlackIsAI,
        SoloAI
    }
#endif
}
