namespace QuoridorEngine.src.ui.gui.board
{
#if !CONSOLE222
    public class WallPartCell : BoardCell
    {
        private Orientation orientation;
        private bool isUsed;
        private int expand;
        private int offset;

        public bool IsUsed { get => isUsed; }

        public WallPartCell(int row, int column, int minSize, int maxSize) : base(row, column)
        {
            isUsed = false;

            MouseEnter += new EventHandler(OnMouseEnter);
            MouseLeave += new EventHandler(OnMouseLeave);
            Click += new EventHandler(OnClick);

            assignOrientation();
            setSizes(minSize, maxSize);
            BackColor = Color.White;
        }

        private void assignOrientation()
        {
            if (row % 2 == 1 && column % 2 == 0)
                orientation = Orientation.Horizontal;
            else if (column % 2 == 1 && row % 2 == 0)
                orientation = Orientation.Vertical;
            else
                throw new Exception("Not a wall part cell");
        }

        private void setSizes(int minSize, int maxSize)
        {
            if (orientation == Orientation.Horizontal)
            {
                Width = maxSize;
                Height = minSize;
            }
            else if (orientation == Orientation.Vertical)
            {
                Width = minSize;
                Height = maxSize;
            }

            expand = minSize;
            offset = expand / 2;
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            if (!isUsed) activeStyle();
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            if (!isUsed) inactiveStyle();
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (!isUsed)
            {
                isUsed = true;
            }
        }

        private void activeStyle()
        {
            Height += expand;
            Top -= offset;
            Width += expand;
            Left -= offset;
            BackColor = Color.Black;
            BringToFront();
        }

        private void inactiveStyle()
        {
            Height -= expand;
            Top += offset;
            Width -= expand;
            Left += offset;
            BackColor = Color.White;
            SendToBack();
        }
    }
#endif
}
