using QuoridorEngine.src.ui.gui;
using QuoridorEngine.src.ui.gui.board;
using System.Drawing.Drawing2D;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public partial class GuiFrame : Form
    {
        private Panel board;
        private Panel score;
        private Panel availableWalls;
        private BoardCell[,] boardCells;
        private PlayerPawn blackPawn;
        private PlayerPawn whitePawn;

        private int BoardDimension { get; set; }
        
        private double BoardFrameRatio { get; set; }
        private double WallPlayerCellRatio { get; set; }
        private int PlayerCellSize { get; set; }
        private int WallCellSize { get; set; }

        private Color BackgroundTopColor { get; set; }
        private Color BackgroundBottomColor { get; set; }
        private float BackgroundAngle { get; set; }

        private Color PlayerBoardCellColor { get; set; }
        private Color WallBoardCellFreeColor { get; set; }
        private Color WallBoardCellUsedColor { get; set; }

        private Color WhitePlayerPawnColor { get; set; }
        private Color BlackPlayerPawnColor { get; set; }

        public GuiFrame()
        {
            InitializeComponent();
            defaultRatios();
            defaultStyle();
        }

        /// <summary>
        /// Render player pawn to board 
        /// </summary>
        /// <param name="player">Player enum option</param>
        /// <param name="initRow">Initial row</param>
        /// <param name="initColumn">Initial column</param>
        public void renderPlayerPawn(Player player, int initRow, int initColumn)
        {
            PlayerPawn playerPawn = (player == Player.White) ? ref whitePawn : ref blackPawn;
            playerPawn = new PlayerPawn(PlayerCellSize);
            Controls.Add(playerPawn);
            playerPawn.UpdateLocation((PlayerCell)boardCells[initRow, initColumn]);
            playerPawn.BringToFront();
            playerPawn.MainColor = (player == Player.White) ? WhitePlayerPawnColor : BlackPlayerPawnColor;
        }

        /// <summary>
        /// Generate and place board panel to frame
        /// </summary>
        public void renderBoard(int boardDimension)
        {
            BoardDimension = boardDimension;

            // Initialize and include board panel to form
            board = new Panel();
            Controls.Add(board);
            board.BringToFront();

            // Set initial board size
            board.Width = (int)(ClientRectangle.Width * BoardFrameRatio);
            board.Height = board.Width;

            calculateCellSizes();

            // Fix board size after cell size calculation
            board.Width -= WallCellSize + 3;
            board.Height = board.Width;

            // Center board to frame
            board.Location = new Point(
                (ClientRectangle.Width / 2) - (board.Width / 2),
                (ClientRectangle.Height / 2) - (board.Height / 2));

            drawBoard();
        }

        /// <summary>
        /// Coordinate board cells drawing on board panel
        /// </summary>
        private void drawBoard()
        {
            boardCells = new BoardCell[BoardDimension, BoardDimension];
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
                boardCells[row, column] = new PlayerCell(row, column, PlayerCellSize);
            else if (row % 2 == 1 && column % 2 == 1)
                boardCells[row, column] = new WallCornerCell(row, column, WallCellSize);
            else
                boardCells[row, column] = new WallPartCell(row, column, WallCellSize, PlayerCellSize);

            boardCells[row, column].Location = new Point(xLoc, yLoc);
            Controls.Add(boardCells[row, column]);
            boardCells[row, column].Parent = board;
        }

        /// <summary>
        /// Calculate player cell and wall cell sizes based on initial board size 
        /// Board size gets fixed after this action to remove space allocated for an extra wallcell
        /// </summary>
        /// <param name="cellRatio"> (wall cell size / player cell size) ratio</param>
        private void calculateCellSizes()
        {
            // Calculate player cell and wall cell sizes combined
            int combinedSize = board.Width / ((BoardDimension + 1) / 2);

            WallCellSize = (int)(combinedSize * WallPlayerCellRatio);
            PlayerCellSize = combinedSize - WallCellSize;
        }

        private void defaultRatios()
        {
            BoardFrameRatio = 0.75;
            WallPlayerCellRatio = 0.1;
        }

        private void defaultStyle()
        {
            // Set gradient background attributes for frame
            BackgroundTopColor = Color.FromArgb(255, 0, 0, 10);
            BackgroundBottomColor = Color.FromArgb(255, 0, 38, 80);
            BackgroundAngle = 45;

            // Set board cell colors
            PlayerBoardCellColor = Color.RoyalBlue;
            WallBoardCellFreeColor = Color.White;
            WallBoardCellUsedColor = Color.Black;

            // Set player pawn colors
            WhitePlayerPawnColor = Color.Red;
            BlackPlayerPawnColor = Color.Purple;           
        }

        /// <summary>
        /// Overriding OnPaint method for frame to create gradient background effect
        /// </summary>
        /// <param name="e">Paint event Arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = new(ClientRectangle, BackgroundTopColor, BackgroundBottomColor, BackgroundAngle);
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(brush, ClientRectangle);
            base.OnPaint(e);
        }
    }

    public enum Player
    {
        White,
        Black
    }
#endif
}