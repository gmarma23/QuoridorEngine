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
            // Set gradient background attributes
            TopColor = Color.FromArgb(255, 0, 0, 10);
            BottomColor = Color.FromArgb(255, 0, 38, 80);
            Angle = 45;

            renderBoard();
        }

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

        private void drawBoard()
        {
            boardCells = new Label[2 * game.Dimension - 1, 2 * game.Dimension - 1];
            int x = 0, y = 0;
            
            for (int row = 0; row < boardCells.GetLength(0); row++)
            {
                for (int column = 0; column < boardCells.GetLength(1); column++)
                {
                    drawBoardCell(row, column, x, y);
                    x += boardCells[row, column].Width;
                }    
                y += boardCells[row, 0].Height;
                x = 0;
            }    
        }

        private void drawBoardCell(int row, int column, int x, int y)
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

            boardCells[row, column].Location = new Point(x, y);
            Controls.Add(boardCells[row, column]);
            boardCells[row, column].Parent = board;
        }

        private void calculateCellSizes(double cellRatio = 0.1)
        {
            int combinedSize = board.Width / game.Dimension;
            wallCellSize = (int)(combinedSize * cellRatio);
            playerCellSize = combinedSize - wallCellSize;
        }

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