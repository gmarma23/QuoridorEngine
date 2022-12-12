using System.Drawing.Drawing2D;

namespace QuoridorEngine.UI
{
#if !CONSOLE
    public class GuiPlayerPawn : Label
    {
        private const double cornerRadiusWidthRatio = 0.5;
        private const double pawnCellRatio = 0.66;

        private Color mainColor;
        private Color boarderColor;
        private float boarderSize;

        public bool IsWhite { get; init; }

        public GuiPlayerPawn(bool isWhite, int cellSize)
        {
            IsWhite = isWhite;

            sizesAndArrangement(cellSize);

            DoubleBuffered = true;
            defaultStyle();
        }

        public void AddEventHandlers(EventHandler onClick)
        {
            Click += onClick;
        }

        public void RemoveEventHandlers(EventHandler onClick)
        {
            Click -= onClick;
        }

        // Paint player pawn as rectangle with smooth round corners
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GraphicsPath graphicsPath = getRoundRectangle(ClientRectangle);
            SolidBrush brush = new SolidBrush(mainColor);
            Pen pen = new Pen(boarderColor, boarderSize);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, graphicsPath);
            e.Graphics.DrawPath(pen, graphicsPath);
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor);
        }

        private void sizesAndArrangement(int cellSize)
        {
            Width = (int)(cellSize * pawnCellRatio);
            Height = Width;
            Location = new Point(cellSize / 2 - Width / 2, cellSize / 2 - Height / 2);
        }

        private void defaultStyle()
        {
            mainColor = IsWhite ? Color.Red : Color.Purple;
            boarderColor = Color.White;
            BackColor = Color.Transparent;
            boarderSize = 3.0f;
        }

        private GraphicsPath getRoundRectangle(Rectangle rectangle)
        {
            int diminisher = 1;
            int cornerRadius = (int)(Width * cornerRadiusWidthRatio);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rectangle.X, rectangle.Y, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(rectangle.X + rectangle.Width - cornerRadius - diminisher, rectangle.Y, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(rectangle.X + rectangle.Width - cornerRadius - diminisher, rectangle.Y + rectangle.Height - cornerRadius - diminisher, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(rectangle.X, rectangle.Y + rectangle.Height - cornerRadius - diminisher, cornerRadius, cornerRadius, 90, 90);
            path.CloseAllFigures();
            return path;
        }
    }
#endif
}
