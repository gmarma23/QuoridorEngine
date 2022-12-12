
namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public class GuiBoard : Panel
    {
        private const double wallPlayerCellRatio = 0.11;

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

            sizesAndArrangement(parrentWidth, parrentHeight);
            defaultStyle();

            drawBoard();
            drawPlayerPawn(true);
            drawPlayerPawn(false);
        }

        public void MovePlayerPawn(bool isWhitePlayer, int newRow, int newColumn)
        {
            getPlayerPawn(isWhitePlayer).Parent = boardCells[newRow, newColumn];
        }

        public void NormalPlayerCell(int row, int column)
        {
            ((GuiPlayerCell)boardCells[row, column]).Normal();
        }

        public void PossibleMovePlayerCell(int row, int column)
        {
            ((GuiPlayerCell)boardCells[row, column]).PossibleMove();
        }

        public void UseWallCell(int row, int column)
        {
            ((GuiWallPartCell)boardCells[row, column]).Use();
        }

        public void FreeWallCell(int row, int column)
        {
            ((GuiWallPartCell)boardCells[row, column]).Free();
        }

        public void PlaceWallCell(int row, int column, bool isPlaced)
        {
            ((GuiWallPartCell)boardCells[row, column]).IsPlaced = isPlaced;
        }

        // Add event handlers to all board events
        public void AddEventHandlers()
        {
            for (int row = boardCells.GetLength(0) - 1; row >= 0; row--)
                for (int column = 0; column < boardCells.GetLength(1); column++)
                    if (boardCells[row, column] is GuiPlayerCell)
                        ((GuiPlayerCell)boardCells[row, column]).AddEventHandlers(eventHandlers["OnPlayerCellClick"]);
                    else if (boardCells[row, column] is GuiWallPartCell)
                        ((GuiWallPartCell)boardCells[row, column]).AddEventHandlers(
                            eventHandlers["OnWallCellEnter"], eventHandlers["OnWallCellLeave"], eventHandlers["OnWallCellClick"]);

            whitePawn.AddEventHandlers(eventHandlers["OnPlayerPawnClick"]);
            blackPawn.AddEventHandlers(eventHandlers["OnPlayerPawnClick"]);
        }

        // Remove event handlers from all board events
        public void RemoveEventHandlers()
        {
            for (int row = boardCells.GetLength(0) - 1; row >= 0; row--)
                for (int column = 0; column < boardCells.GetLength(1); column++)
                    if (boardCells[row, column] is GuiPlayerCell)
                        ((GuiPlayerCell)boardCells[row, column]).RemoveEventHandlers(eventHandlers["OnPlayerCellClick"]);
                    else if (boardCells[row, column] is GuiWallPartCell)
                        ((GuiWallPartCell)boardCells[row, column]).RemoveEventHandlers(
                            eventHandlers["OnWallCellEnter"], eventHandlers["OnWallCellLeave"], eventHandlers["OnWallCellClick"]);

            whitePawn.RemoveEventHandlers(eventHandlers["OnPlayerPawnClick"]);
            blackPawn.RemoveEventHandlers(eventHandlers["OnPlayerPawnClick"]);
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
            ref GuiPlayerPawn playerPawn = ref getPlayerPawn(isWhitePlayer);
            playerPawn = new GuiPlayerPawn(isWhitePlayer, maxCellSize);
            Controls.Add(playerPawn);
            playerPawn.BringToFront();
        }

        private ref GuiPlayerPawn getPlayerPawn(bool isWhitePlayer)
        {
            return ref isWhitePlayer ? ref whitePawn : ref blackPawn;
        }

        private void sizesAndArrangement(int parrentWidth, int parrentHeight)
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

        private void defaultStyle()
        {
            BackColor = Color.Transparent;
        }
    }
#endif
}
