using QuoridorEngine.Core;
using QuoridorEngine.Utils;
using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public class GuiBoard : Panel
    {
        private const double wallPlayerCellRatio = 0.11;

        private delegate void BoardCellAction(int row, int column);

        private readonly Dictionary<string, EventHandler> eventHandlers;
        private readonly int dimension;

        private GuiBoardCell[,] boardCells;
        private GuiPlayerPawn blackPawn;
        private GuiPlayerPawn whitePawn;

        private int maxCellSize;
        private int minCellSize;

        public GuiBoard(int parrentWidth, int parrentHeight, int dimension, Dictionary<string, EventHandler> eventHandlers)
        {
            this.dimension = dimension;
            this.eventHandlers = eventHandlers;

            setSizesAndArrangement(parrentWidth, parrentHeight);
            applyDefaultStyle();

            drawBoard();
            drawPlayerPawn(true);
            drawPlayerPawn(false);

            addEventHandlers();
        }

        public void UpdateUsedWallCells(QuoridorGame gameState) => updateWallCells(gameState, UseWallCell, true);

        public void UpdatePlacedWallCells(QuoridorGame gameState) => updateWallCells(gameState, PlaceWallCell, true);

        public void UpdateFreeWallCells(QuoridorGame gameState) => updateWallCells(gameState, FreeWallCell, false);
 
        /// <summary>
        /// Update gui board player cells to show player's pawn possible moves
        /// </summary>
        /// <param name="isWhitePlayer">Player to show possible moves</param>
        public void ShowPossiblePlayerMoveCells(QuoridorGame gameState, bool isWhitePlayer)
        {
            List<QuoridorMove> possiblePlayerMoves = (List<QuoridorMove>)gameState.GetPossiblePlayerMoves(isWhitePlayer);

            foreach (QuoridorMove move in possiblePlayerMoves)
            {
                (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiPlayer(move.Row, move.Column);
                PossiblePlayerMoveCell(guiRow, guiColumn);
            }
        }

        // Reset all possible player move cells to normal state 
        public void HidePossiblePlayerMoveCells(QuoridorGame gameState)
        {
            for (int gameRow = gameState.Dimension - 1; gameRow >= 0; gameRow--)
                for (int gameColumn = 0; gameColumn < gameState.Dimension; gameColumn++)
                {
                    (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiPlayer(gameRow, gameColumn);
                    NormalPlayerCell(guiRow, guiColumn);
                }
        }

        /// <summary>
        /// Utility to refresh/update wall cells on gui based on changes made in quoridor core
        /// </summary>
        /// <param name="function"> An action to perform on gui wall cells</param>
        /// <param name="hasWallPiece">Utility parameter to determine whether a wall piece 
        /// is placed or not in quoridor core according to performed action</param>
        private void updateWallCells(QuoridorGame gameState, BoardCellAction function, bool hasWallPiece)
        {
            // Check horizontal wall pieces
            for (int gameRow = gameState.Dimension - 1; gameRow > 0; gameRow--)
                for (int gameColumn = 0; gameColumn < gameState.Dimension; gameColumn++)
                    if (gameState.HasWallPiece(gameRow, gameColumn, Orientation.Horizontal) == hasWallPiece)
                    {
                        (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiWall(gameRow, gameColumn, Orientation.Horizontal);
                        function(guiRow, guiColumn);
                    }

            // Check vertical wall pieces
            for (int gameRow = gameState.Dimension - 1; gameRow >= 0; gameRow--)
                for (int gameColumn = 0; gameColumn < gameState.Dimension - 1; gameColumn++)
                    if (gameState.HasWallPiece(gameRow, gameColumn, Orientation.Vertical) == hasWallPiece)
                    {
                        (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiWall(gameRow, gameColumn, Orientation.Vertical);
                        function(guiRow, guiColumn);
                    }
        }

        public void MovePlayerPawn(QuoridorGame gameState, bool isWhitePlayer)
        {
            int gameRow = 0, gameColumn = 0;
            if (isWhitePlayer)
                gameState.GetWhiteCoordinates(ref gameRow, ref gameColumn);
            else
                gameState.GetBlackCoordinates(ref gameRow, ref gameColumn);

            (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiPlayer(gameRow, gameColumn);
            getPlayerPawn(isWhitePlayer).Parent = boardCells[guiRow, guiColumn];
        }

        private void NormalPlayerCell(int row, int column)
        {
            ((GuiPlayerCell)boardCells[row, column]).ToNormal();
        }

        private void PossiblePlayerMoveCell(int row, int column)
        {
            ((GuiPlayerCell)boardCells[row, column]).ToPossibleMove();
        }

        private void UseWallCell(int row, int column)
        {
            ((GuiWallPartCell)boardCells[row, column]).Use();
        }

        private void FreeWallCell(int row, int column)
        {
            ((GuiWallPartCell)boardCells[row, column]).Free();
        }

        private void PlaceWallCell(int row, int column)
        {
            ((GuiWallPartCell)boardCells[row, column]).IsPlaced = true;
        }

        // Add event handlers to all board events
        private void addEventHandlers()
        {
            for (int row = boardCells.GetLength(0) - 1; row >= 0; row--)
                for (int column = 0; column < boardCells.GetLength(1); column++)
                    boardCells[row, column].AddEventHandlers(eventHandlers);

            whitePawn.AddEventHandlers(eventHandlers);
            blackPawn.AddEventHandlers(eventHandlers);
        }

        // Coordinate board cells drawing on board panel
        private void drawBoard()
        {
            boardCells = new GuiBoardCell[dimension, dimension];
            int xLoc = 0, yLoc = 0;

            for (int row = boardCells.GetLength(0) - 1; row >= 0; row--)
            {
                for (int column = 0; column < boardCells.GetLength(1); column++)
                {
                    drawBoardCell(row, column, xLoc, yLoc);
                    xLoc += boardCells[row, column].Width;
                }
                yLoc += boardCells[row, 0].Height;
                xLoc = 0;
            }
        }

        /// <summary>
        /// Generate and draw board cells based on their type
        /// </summary>
        /// <param name="row">Board row</param>
        /// <param name="column">Board column</param>
        /// <param name="xLoc">Cell x location</param>
        /// <param name="yLoc">Cell y location</param>
        private void drawBoardCell(int row, int column, int xLoc, int yLoc)
        {
            if (row % 2 == 0 && column % 2 == 0)
                boardCells[row, column] = new GuiPlayerCell(row, column, maxCellSize);
            else if (row % 2 == 1 && column % 2 == 1)
                boardCells[row, column] = new GuiWallCornerCell(row, column, minCellSize);
            else
                boardCells[row, column] = new GuiWallPartCell(row, column, minCellSize, maxCellSize);

            boardCells[row, column].Location = new Point(xLoc, yLoc);
            Controls.Add(boardCells[row, column]);
        }

        /// <summary>
        /// Render player pawn to board 
        /// </summary>
        /// <param name="isWhitePlayer">Player option</param>
        public void drawPlayerPawn(bool isWhitePlayer)
        {
            ref GuiPlayerPawn playerPawn = ref getPlayerPawnRef(isWhitePlayer);
            playerPawn = new GuiPlayerPawn(isWhitePlayer, maxCellSize);
            Controls.Add(playerPawn);
            playerPawn.BringToFront();
        }

        private GuiPlayerPawn getPlayerPawn(bool isWhitePlayer)
        {
            return isWhitePlayer ? whitePawn : blackPawn;
        }

        private ref GuiPlayerPawn getPlayerPawnRef(bool isWhitePlayer)
        {
            return ref isWhitePlayer ? ref whitePawn : ref blackPawn;
        }

        private void setSizesAndArrangement(int parrentWidth, int parrentHeight)
        {
            // Set initial board size
            Width = (int)(parrentWidth * GuiFrame.boardFrameRatio);
            Height = Width;

            calculateCellSizes();

            // Fix board size after cell size calculation
            Width -= minCellSize + 3;
            Height = Width;

            // Center board to frame
            Location = new Point((parrentWidth / 2) - (Width / 2), (parrentHeight / 2) - (Height / 2));
        }

        /// <summary>
        /// Calculate player cell and wall cell sizes based on initial board size 
        /// Board size gets fixed after this action to remove space allocated for an extra wallcell
        /// </summary>
        private void calculateCellSizes()
        {
            int combinedSize = Width / ((dimension + 1) / 2);
            minCellSize = (int)(combinedSize * wallPlayerCellRatio);
            maxCellSize = combinedSize - minCellSize;
        }

        private void applyDefaultStyle()
        {
            BackColor = Color.Transparent;
        }
    }
#endif
}
