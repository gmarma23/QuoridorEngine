using System.Drawing.Drawing2D;
using QuoridorEngine.src.ui.gui.board;

namespace QuoridorEngine.src.ui.gui
{
#if !CONSOLE
    public class PlayerPawn : Label
    {
        public bool IsWhite { get; init; }

        public double CornerRadiusWidthRatio { get; set; }
        public double PawnCellRatio { get; set; }

        public Color MainColor { get; set; }
        public Color BoarderColor { get; set; }
        public float BoarderSize { get; set; }

        public PlayerPawn(GuiClient guiClient, bool isWhite, int cellSize)
        {
            IsWhite = isWhite;
            DoubleBuffered = true;
            defaultStyle();

            // Set default property values
            CornerRadiusWidthRatio = 0.5;
            PawnCellRatio = 0.66;
            
            Width = (int)(cellSize * PawnCellRatio);
            Height = Width;
            Location = new Point(cellSize / 2 - Width / 2, cellSize / 2 - Height / 2);
            
            Click += guiClient.PlayerPawnClicked;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GraphicsPath graphicsPath = getRoundRectangle(ClientRectangle);
            SolidBrush brush = new SolidBrush(MainColor);
            Pen pen = new Pen(BoarderColor, BoarderSize);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, graphicsPath);
            e.Graphics.DrawPath(pen, graphicsPath);
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor);
        }

        private GraphicsPath getRoundRectangle(Rectangle rectangle)
        {
            int diminisher = 1;
            int cornerRadius = (int)(Width * CornerRadiusWidthRatio);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rectangle.X, rectangle.Y, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(rectangle.X + rectangle.Width - cornerRadius - diminisher, rectangle.Y, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(rectangle.X + rectangle.Width - cornerRadius - diminisher, rectangle.Y + rectangle.Height - cornerRadius - diminisher, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(rectangle.X, rectangle.Y + rectangle.Height - cornerRadius - diminisher, cornerRadius, cornerRadius, 90, 90);
            path.CloseAllFigures();
            return path;
        }

        private void defaultStyle()
        {
            MainColor = IsWhite ? Color.Green : Color.Purple;
            BoarderColor = Color.White;
            BackColor = Color.Transparent;
            BoarderSize = 3.0f;
        }
    }
#endif
}
