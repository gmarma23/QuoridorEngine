using QuoridorEngine.src.ui.gui;
using System.Drawing.Drawing2D;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public partial class GuiFrame : Form
    {
        private Board board;
        private Panel score;
        private Panel availableWalls;

        private Color BackgroundTopColor { get; set; }
        private Color BackgroundBottomColor { get; set; }
        private float BackgroundAngle { get; set; }

        public GuiFrame()
        {
            InitializeComponent();
            defaultStyle();
        }

        public void UpdateWhitePawnLocation(int newRow, int newColumn)
        {
            board.UpdatePawnLocation(Player.White, newRow, newColumn);
        }

        public void UpdateBlackPawnLocation(int newRow, int newColumn)
        {
            board.UpdatePawnLocation(Player.Black, newRow, newColumn);
        }

        /// <summary>
        /// Generate and place board panel to frame
        /// </summary>
        public void RenderBoard(int boardDimension)
        {
            // Initialize and include board panel to form
            board = new Board(this, boardDimension);
            Controls.Add(board);
            board.BringToFront();
        }

        private void defaultStyle()
        {
            // Set form's gradient background properties
            BackgroundTopColor = Color.FromArgb(255, 0, 0, 10);
            BackgroundBottomColor = Color.FromArgb(255, 0, 38, 80);
            BackgroundAngle = 45;  
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