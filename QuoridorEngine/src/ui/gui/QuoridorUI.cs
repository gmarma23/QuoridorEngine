using QuoridorEngine.Core;
using System.Drawing.Drawing2D;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public partial class QuoridorUI : Form
    {
        private const int defaultBoardSize = 9;
        private QuoridorGame game;

        private Panel board;
        private Label[,] boardCells;
        private Panel score;
        private Panel availableWalls;

        private Color TopColor { get; set; }
        private Color BottomColor { get; set; }
        private float Angle { get; set; }

        public QuoridorUI()
        {
            game = new QuoridorGame(defaultBoardSize);

            InitializeComponent();
            renderGUIComponents();
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

            drawBoard();
            
            board.Width = 3 * this.ClientRectangle.Width / 4;
            board.Height = 3 * this.ClientRectangle.Width / 4;
            board.Location = new Point(this.ClientRectangle.Width / 8, this.ClientRectangle.Width / 8);
            board.BackColor = Color.Transparent; 
        }

        private void drawBoard()
        {
            boardCells = new Label[2 * game.Dimension - 1, 2 * game.Dimension - 1];
            int x = 0, y = 0;
            
            for (int i = 0; i < boardCells.GetLength(0); i++)
            {
                for (int j = 0; j < boardCells.GetLength(1); j++)
                {
                    drawBoardCell(i, j, x, y);
                    x += boardCells[i, j].Width;
                }    
                y += boardCells[i, 0].Height;
                x = 0;
            }
                
        }

        private void drawBoardCell(int i, int j, int x, int y)
        {
            boardCells[i, j] = new Label()
            {
                Width = (j % 2 == 0) ? 55 : 5,
                Height = (i % 2 == 0) ? 55 : 5,
                BackColor = (i % 2 == 0 && j % 2 == 0) ? Color.RoyalBlue : Color.White,
            };

            boardCells[i, j].Location = new Point(x, y);

            if (!(i % 2 == 0 && j % 2 == 0))
            {
                boardCells[i, j].MouseEnter += new EventHandler(Wall_MouseEnter);
                boardCells[i, j].MouseLeave += new EventHandler(Wall_MouseLeave);
            }

            Controls.Add(boardCells[i, j]);
            boardCells[i, j].BringToFront();
            boardCells[i, j].Parent = board;
        }

        private void Wall_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Height += 6;
            lbl.Top -= 3;
            lbl.Width += 6;
            lbl.Left -= 3;
            lbl.BackColor = Color.Black;
            lbl.BringToFront();
        }

        private void Wall_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Height -= 6;
            lbl.Top += 3;
            lbl.Width -= 6;
            lbl.Left += 3;
            lbl.BackColor = Color.White;
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