using QuoridorEngine.src.ui.gui;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public partial class GuiFrame : Form
    {
        public const double boardFrameRatio = 0.75;

        private Board board;
        private PlayerWallsPanel whitePlayerWalls;
        private PlayerWallsPanel blackPlayerWalls;

        private Color BackgroundTopColor { get; set; }
        private Color BackgroundBottomColor { get; set; }
        private float BackgroundAngle { get; set; }

        public GuiFrame()
        {
            InitializeComponent();

            // Set form's gradient background properties
            BackgroundTopColor = Color.FromArgb(255, 0, 0, 10);
            BackgroundBottomColor = Color.FromArgb(255, 0, 38, 80);
            BackgroundAngle = 45;
        }

        public void UseWallCell(int row, int column)
        {
            board.UseWallCell(row, column);
        }

        public void FreeWallCell(int row, int column)
        {
            board.FreeWallCell(row, column);
        }

        public void SetWhitePlayerWallCounter(int numOfWalls)
        {
            whitePlayerWalls.SetWallNum(numOfWalls);
        }

        public void SetBlackPlayerWallCounter(int numOfWalls)
        {
            blackPlayerWalls.SetWallNum(numOfWalls);
        }

        /// <summary>
        /// Initialize and include board panel to frame
        /// </summary>
        public void RenderBoard(GuiClient guiClient, int boardDimension, int initWhiteRow, int initWhiteColumn, int initBlackRow, int initBlackColumn)
        {
            board = new Board(this, guiClient, boardDimension, initWhiteRow, initWhiteColumn, initBlackRow, initBlackColumn);
            Controls.Add(board);
            board.BringToFront();
        }

        public void RenderPlayerWallPanels()
        {
            whitePlayerWalls = new PlayerWallsPanel(this, Player.White);
            Controls.Add(whitePlayerWalls);
            whitePlayerWalls.BringToFront();

            blackPlayerWalls = new PlayerWallsPanel(this, Player.Black);
            Controls.Add(blackPlayerWalls);
            blackPlayerWalls.BringToFront();
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
#endif
}