using QuoridorEngine.src.ui.gui.board;
using QuoridorEngine.UI;
using System.Windows.Forms.Design;

namespace QuoridorEngine.src.ui.gui
{
    public class Board : Panel
    {
        private const double wallPlayerCellRatio = 0.11;

        private BoardCell[,] boardCells;
        private PlayerPawn blackPawn;
        private PlayerPawn whitePawn;
        private readonly int dimension;
        private int maxCellSize;
        private int minCellSize;

        public Board(GuiFrame guiFrame, GuiClient guiClient, int dimension)
        {
            this.dimension = dimension;

            sizesAndArrangement(guiFrame.ClientRectangle.Width, guiFrame.ClientRectangle.Height);
            defaultStyle();

            drawBoard(guiClient);
            drawPlayerPawn(guiClient, true);
            drawPlayerPawn(guiClient, false);
        }

        public void MovePlayerPawn(bool isWhitePlayer, int newRow, int newColumn)
        {
            getPlayerPawn(isWhitePlayer).Parent = boardCells[newRow, newColumn];
        }

        public void NormalPlayerCell(int row, int column)
        {
            ((PlayerCell)boardCells[row, column]).Normal();
        }

        public void PossibleMovePlayerCell(int row, int column)
        {
            ((PlayerCell)boardCells[row, column]).PossibleMove();
        }

        public void UseWallCell(int row, int column)
        {
            ((WallPartCell)boardCells[row, column]).Use();
        }

        public void FreeWallCell(int row, int column)
        {
            ((WallPartCell)boardCells[row, column]).Free();
        }

        public void PlaceWallCell(int row, int column, bool isPlaced)
        {
            ((WallPartCell)boardCells[row, column]).IsPlaced = isPlaced;
        }

        /// <summary>
        /// Coordinate board cells drawing on board panel
        /// </summary>
        private void drawBoard(GuiClient guiClient)
        {
            boardCells = new BoardCell[dimension, dimension];
            int xLoc = 0, yLoc = 0;

            for (int row = boardCells.GetLength(0) - 1; row >= 0; row--)
            {
                for (int column = 0; column < boardCells.GetLength(1); column++)
                {
                    drawBoardCell(guiClient, row, column, xLoc, yLoc);
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
        private void drawBoardCell(GuiClient guiClient, int row, int column, int xLoc, int yLoc)
        {
            if (row % 2 == 0 && column % 2 == 0)
                boardCells[row, column] = new PlayerCell(guiClient, row, column, maxCellSize);
            else if (row % 2 == 1 && column % 2 == 1)
                boardCells[row, column] = new WallCornerCell(row, column, minCellSize);
            else
                boardCells[row, column] = new WallPartCell(guiClient, row, column, minCellSize, maxCellSize);

            boardCells[row, column].Location = new Point(xLoc, yLoc);
            Controls.Add(boardCells[row, column]);
            boardCells[row, column].Parent = this;
        }

        /// <summary>
        /// Render player pawn to board 
        /// </summary>
        /// <param name="player">Player enum option</param>
        public void drawPlayerPawn(GuiClient guiClient, bool isWhitePlayer)
        {
            ref PlayerPawn playerPawn = ref getPlayerPawn(isWhitePlayer);
            playerPawn = new PlayerPawn(guiClient, isWhitePlayer, maxCellSize);
            Controls.Add(playerPawn);
            playerPawn.BringToFront();
        }

        private ref PlayerPawn getPlayerPawn(bool isWhitePlayer)
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
}
