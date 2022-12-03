using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using QuoridorEngine.src.ui.gui.board;

namespace QuoridorEngine.src.ui.gui
{
#if !CONSOLE
    public class Player : Label
    {
        private int guiRow;
        private int guiColumn;
        private int internalRow;
        private int internalColumn;
        private bool moveInit;

        public int GuiRow { get => guiRow; }
        public int GuiColumn { get => guiColumn; }
        public int InternalRow { get => internalRow; }
        public int InternalColumn { get => internalColumn; }

        public Color MainColor { get; set; }
        public Color BoarderColor { get; set; }
        public int CornerRadius { get; set; }

        public Player(int cellSize)
        {
            moveInit = false;
            DoubleBuffered = true;
            style(cellSize);
            Click += new EventHandler(OnClick);
        }

        public void UpdateLocation(PlayerCell cell)
        {
            Parent = cell;
            guiRow = cell.GuiRow;
            guiColumn = cell.GuiColumn;
            internalRow = cell.InternalRow;
            internalColumn = cell.InternalColumn;
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (!moveInit)
            {
                moveInit = true;
                
            }
            else
            {
                moveInit = false;
            }      
        }

        private void style(int cellSize)
        {
            CornerRadius = 17;
            Width = 2 * cellSize / 3;
            Height = Width;
            Location = new Point(cellSize / 2 - Width / 2, cellSize / 2 - Height / 2);
            BackColor = Color.RoyalBlue;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var graphicsPath = getRoundRectangle(this.ClientRectangle))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(MainColor))
                    e.Graphics.FillPath(brush, graphicsPath);
                using (var pen = new Pen(Color.White, 3.0f))
                    e.Graphics.DrawPath(pen, graphicsPath);
                TextRenderer.DrawText(e.Graphics, Text, this.Font, this.ClientRectangle, this.ForeColor);
            }
        }

        private GraphicsPath getRoundRectangle(Rectangle rectangle)
        {
            int diminisher = 1;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rectangle.X, rectangle.Y, CornerRadius, CornerRadius, 180, 90);
            path.AddArc(rectangle.X + rectangle.Width - CornerRadius - diminisher, rectangle.Y, CornerRadius, CornerRadius, 270, 90);
            path.AddArc(rectangle.X + rectangle.Width - CornerRadius - diminisher, rectangle.Y + rectangle.Height - CornerRadius - diminisher, CornerRadius, CornerRadius, 0, 90);
            path.AddArc(rectangle.X, rectangle.Y + rectangle.Height - CornerRadius - diminisher, CornerRadius, CornerRadius, 90, 90);
            path.CloseAllFigures();
            return path;
        }
    }
#endif
}
