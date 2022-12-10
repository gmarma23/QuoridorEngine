using QuoridorEngine.src.ui.gui.board;
using QuoridorEngine.UI;
using System.Diagnostics;
using System.Windows.Forms;

namespace QuoridorEngine.src.ui.gui
{
    public class Board : Panel
    {
        private BoardCell[,] boardCells;
        private PlayerPawn blackPawn;
        private PlayerPawn whitePawn;

        private int Dimension { get; init; }

        public double BoardFrameRatio { get; set; }
        public double WallPlayerCellRatio { get; set; }
        public int PlayerCellSize { get; private set; }
        public int WallCellSize { get; private set; }

        public Color PlayerCellColor { get; set; }
        public Color WallCellFreeColor { get; set; }
        public Color WallCellUsedColor { get; set; }

        private Color WhitePlayerPawnColor { get; set; }
        private Color BlackPlayerPawnColor { get; set; }


  
        public Board(GuiFrame guiFrame, GuiClient guiClient, int dimension)
        {
            Dimension = dimension;
            Parent = guiFrame;

            // Set default property values
            BoardFrameRatio = GuiFrame.boardFrameRatio;
            WallPlayerCellRatio = 0.11;
      
            PlayerCellColor = Color.RoyalBlue;
            WallCellFreeColor = Color.Transparent;
            WallCellUsedColor = Color.LightGray;
            BackColor = Color.Transparent;

            WhitePlayerPawnColor = Color.Red;
            BlackPlayerPawnColor = Color.Purple;

            // Set initial board size
            Width = (int)(guiFrame.ClientRectangle.Width * BoardFrameRatio);
            Height = Width;

            calculateCellSizes();

            // Fix board size after cell size calculation
            Width -= WallCellSize + 3;
            Height = Width;

            // Center board to frame
            Location = new Point(
                (guiFrame.ClientRectangle.Width / 2) - (Width / 2),
                (guiFrame.ClientRectangle.Height / 2) - (Height / 2));

            drawBoard(guiClient);
            drawPlayerPawn(guiClient, true);
            drawPlayerPawn(guiClient, false);
        }

        public void MovePlayerPawn(bool isWhitePlayer, int newRow, int newColumn)
        {
            getPlayerPawn(isWhitePlayer).Parent = boardCells[newRow, newColumn];
        }

        public void UseWallCell(int row, int column)
        {
            WallPartCell wallPartCell = (WallPartCell)boardCells[row, column];
            if (!wallPartCell.IsActive) wallPartCell.Use();
        }

        public void FreeWallCell(int row, int column)
        {
            WallPartCell wallPartCell = (WallPartCell)boardCells[row, column];
            if (wallPartCell.IsActive) wallPartCell.Free();
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
            boardCells = new BoardCell[Dimension, Dimension];
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
                boardCells[row, column] = new PlayerCell(this, guiClient, row, column);
            else if (row % 2 == 1 && column % 2 == 1)
                boardCells[row, column] = new WallCornerCell(this, row, column);
            else
                boardCells[row, column] = new WallPartCell(this, guiClient, row, column);

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
            playerPawn = new PlayerPawn(guiClient, PlayerCellSize);
            Controls.Add(playerPawn);
            playerPawn.BringToFront();
            playerPawn.MainColor = isWhitePlayer ? WhitePlayerPawnColor : BlackPlayerPawnColor;
            playerPawn.BackColor = PlayerCellColor;
        }

        /// <summary>
        /// Calculate player cell and wall cell sizes based on initial board size 
        /// Board size gets fixed after this action to remove space allocated for an extra wallcell
        /// </summary>
        private void calculateCellSizes()
        {
            // Calculate player cell and wall cell sizes combined
            int combinedSize = this.Width / ((Dimension + 1) / 2);

            WallCellSize = (int)(combinedSize * WallPlayerCellRatio);
            PlayerCellSize = combinedSize - WallCellSize;
        }

        private ref PlayerPawn getPlayerPawn(bool isWhitePlayer)
        {
            return ref isWhitePlayer ? ref whitePawn : ref blackPawn;
        }
    }
}
