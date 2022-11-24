using QuoridorEngine.Core;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public partial class QuoridorUI : Form
    {
        private const int defaultBoardSize = 9;
        private QuoridorGame game;

        private Panel board;
        private Panel score;
        private Panel availableWalls;

        private Color TopColor { get; set; }
        private Color BottomColor { get; set; }
        private float Angle { get; set; }

        public QuoridorUI()
        {
            InitializeComponent();
            renderGUIComponents();

            game = new QuoridorGame(defaultBoardSize);
        }

        private void renderGUIComponents()
        {
            // Set gradient background attributes
            TopColor = Color.FromArgb(255, 0, 0, 10);
            BottomColor = Color.FromArgb(255, 0, 38, 80);
            Angle = 45;

            // Initialize and include Board panel to form
            board = new Panel();
            Controls.Add(board);
            board.BringToFront();
            board.Width = 3 * this.ClientRectangle.Width / 4;
            board.Height = 3 * this.ClientRectangle.Width / 4;
            board.Location = new Point(this.ClientRectangle.Width / 8, this.ClientRectangle.Width / 8);
            board.BackColor = Color.Transparent;
            board.Paint += new PaintEventHandler(drawBoard);
            
        }

        private void drawBoard(object sender, PaintEventArgs e)
        {
            for(int i = 0; i < game.Dimension; i++)
                for(int j = 0; j < game.Dimension; j++)
                    drawBoardCell(e, i, j);
        }

        private void drawBoardCell(PaintEventArgs e, int i, int j)
        {
            int dim = (int)((float)board.Width / (float)game.Dimension);
            Pen pen = new(Color.FromArgb(100, 255, 255, 255), 3);

            int x = i * dim;
            int y = j * dim;
            int width = dim;
            int height = dim;

            // Draw rectangle to screen.
            e.Graphics.DrawRectangle(pen, x, y, width, height);
            e.Graphics.FillRectangle(new SolidBrush(Color.RoyalBlue), x, y, width, height);
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