using QuoridorEngine.Core;
using QuoridorEngine.src.ui.gui;
using System.Drawing.Drawing2D;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public partial class QuoridorUI : Form
    {
        private const int defaultBoardDimention = 9;
        private QuoridorGame game;

        private Panel board;
        private Label[,] boardCells;
        private Player blackPlayer;
        private Player whitePlayer;

        private Panel score;
        private Panel availableWalls;

        private int playerCellSize;
        private int wallCellSize;

        private Color TopColor { get; set; }
        private Color BottomColor { get; set; }
        private float Angle { get; set; }

        public QuoridorUI()
        {
            game = new QuoridorGame(defaultBoardDimention);

            InitializeComponent();
            renderGUIComponents();
        }

        private void renderGUIComponents()
        {
            // Set gradient background attributes for frame
            TopColor = Color.FromArgb(255, 0, 0, 10);
            BottomColor = Color.FromArgb(255, 0, 38, 80);
            Angle = 45;

            renderBoard();

            renderPlayer(ref whitePlayer, 2 * game.Dimension - 2, (2 * game.Dimension - 1) / 2, Color.Red);
            renderPlayer(ref blackPlayer, 0, (2 * game.Dimension - 1) / 2, Color.Purple);
        }

        /// <summary>
        /// Generate and place board panel to frame
        /// </summary>
        private void renderBoard()
        {
            // Initialize and include board panel to form
            board = new Panel();
            Controls.Add(board);
            board.BringToFront();

            // Set initial board size
            board.Width = 3 * this.ClientRectangle.Width / 4;
            board.Height = board.Width;

            calculateCellSizes();

            // Fix board size after cell size calculation
            board.Width -= wallCellSize + 3;
            board.Height = board.Width;

            // Center board to frame
            board.Location = new Point(
                (this.ClientRectangle.Width / 2) - (board.Width / 2),
                (this.ClientRectangle.Height / 2) - (board.Height / 2));

            drawBoard();
        }

        /// <summary>
        /// Coordinate board cells drawing on board panel
        /// </summary>
        private void drawBoard()
        {
            boardCells = new Label[2 * game.Dimension - 1, 2 * game.Dimension - 1];
            int xLoc = 0, yLoc = 0;
            
            for (int row = 0; row < boardCells.GetLength(0); row++)
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
            if(row % 2 == 0 && column % 2 == 0)
                boardCells[row, column] = new PlayerCell(row, column, playerCellSize);
            else if ((row % 2 == 0 && column % 2 == 1) || (row % 2 == 1 && column % 2 == 0))
                boardCells[row, column] = new WallPartCell(row, column, wallCellSize, playerCellSize);
            else
            {
                boardCells[row, column] = new Label()
                {
                    Width = wallCellSize,
                    Height = wallCellSize,
                    BackColor = Color.White
                };
            }

            boardCells[row, column].Location = new Point(xLoc, yLoc);
            Controls.Add(boardCells[row, column]);
            boardCells[row, column].Parent = board;
        }

        /// <summary>
        /// Calculate player cell and wall cell sizes based on initial board size 
        /// Board size gets fixed after this action to remove space allocated for an extra wallcell
        /// </summary>
        /// <param name="cellRatio"> (wall cell size / player cell size) ratio</param>
        private void calculateCellSizes(double cellRatio = 0.1)
        {
            // Calculate player cell and wall cell sizes combined
            int combinedSize = board.Width / game.Dimension;

            wallCellSize = (int)(combinedSize * cellRatio);
            playerCellSize = combinedSize - wallCellSize;
        }

        /// <summary>
        /// Render player pawn to board 
        /// </summary>
        /// <param name="player">Player object reference</param>
        /// <param name="initRow">Initial row</param>
        /// <param name="initColumn">Initial column</param>
        /// <param name="color">Pawn color</param>
        private void renderPlayer(ref Player player, int initRow, int initColumn, Color color)
        {
            player = new Player(playerCellSize);
            Controls.Add(player);
            player.Parent = boardCells[initRow, initColumn];
            player.BringToFront();
            player.MainColor = color;
        }

        /// <summary>
        /// Overriding OnPaint method for frame to create gradient background effect
        /// </summary>
        /// <param name="e">Paint event Arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = new(this.ClientRectangle, this.TopColor, this.BottomColor, this.Angle);
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(brush, this.ClientRectangle);
            base.OnPaint(e);
        }
    }
#endif
}