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


  
        public Board(GuiFrame guiFrame, GuiClient guiClient, int dimension, int initWhiteRow, int initWhiteColumn, int initBlackRow, int initBlackColumn)
        {
            Dimension = dimension;
            Parent = guiFrame;

            // Set default property values
            BoardFrameRatio = GuiFrame.boardFrameRatio;
            WallPlayerCellRatio = 0.11;
      
            PlayerCellColor = Color.RoyalBlue;
            WallCellFreeColor = Color.Transparent;
            WallCellUsedColor = Color.White;
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
            drawPlayerPawn(Player.White);
            drawPlayerPawn(Player.Black);

            UpdatePawnLocation(Player.White, initWhiteRow, initWhiteColumn);
            UpdatePawnLocation(Player.Black, initBlackRow, initBlackColumn);
        }

        public void ClickedWallCell(int row, int column, bool isClicked)
        {
            Debug.Assert(boardCells[row, column] is WallPartCell);
            ((WallPartCell)boardCells[row, column]).IsClicked = isClicked;
        }

        public void UseWallCell(int row, int column)
        {
            Debug.Assert(boardCells[row, column] is WallPartCell);
            if(!((WallPartCell)boardCells[row, column]).IsActive)
                ((WallPartCell)boardCells[row, column]).UsedStyle();
        }

        public void FreeWallCell(int row, int column)
        {
            Debug.Assert(boardCells[row, column] is WallPartCell);
            if (((WallPartCell)boardCells[row, column]).IsActive)
                ((WallPartCell)boardCells[row, column]).FreeStyle();
        }

        private void UpdatePawnLocation(Player player, int newRow, int newColumn)
        {
            getPlayerPawn(player).Parent = boardCells[newRow, newColumn];
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
                    drawBoardCell(row, column, xLoc, yLoc, guiClient);
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
        private void drawBoardCell(int row, int column, int xLoc, int yLoc, GuiClient guiClient)
        {
            if (row % 2 == 0 && column % 2 == 0)
                boardCells[row, column] = new PlayerCell(this, row, column);
            else if (row % 2 == 1 && column % 2 == 1)
                boardCells[row, column] = new WallCornerCell(this, row, column);
            else
                boardCells[row, column] = new WallPartCell(this, row, column, guiClient);

            boardCells[row, column].Location = new Point(xLoc, yLoc);
            Controls.Add(boardCells[row, column]);
            boardCells[row, column].Parent = this;
        }

        /// <summary>
        /// Render player pawn to board 
        /// </summary>
        /// <param name="player">Player enum option</param>
        public void drawPlayerPawn(Player player)
        {
            ref PlayerPawn playerPawn = ref getPlayerPawn(player);
            playerPawn = new PlayerPawn(PlayerCellSize);
            Controls.Add(playerPawn);
            playerPawn.BringToFront();
            playerPawn.MainColor = (player == Player.White) ? WhitePlayerPawnColor : BlackPlayerPawnColor;
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

        private ref PlayerPawn getPlayerPawn(Player player)
        {
            return ref (player == Player.White) ? ref whitePawn : ref blackPawn;
        }
    }

    public enum Player
    {
        White,
        Black
    }
}
