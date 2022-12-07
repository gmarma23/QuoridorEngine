namespace QuoridorEngine.src.ui.gui.board
{
#if !CONSOLE222
    public class WallPartCell : BoardCell
    {
        private bool isUsed;
        private readonly int expand;
        private readonly int offset;

        public Orientation Orientation { get; init; }

        public Color FreeColor { get; set; }
        public Color UsedColor { get; set; } 

        public WallPartCell(Board board, int row, int column) : base (row, column)
        {
            isUsed = false;

            if (Row % 2 == 1 && Column % 2 == 0)
            {
                Width = board.PlayerCellSize;
                Height = board.WallCellSize;
                Orientation = Orientation.Horizontal;
            }
            else if (row % 2 == 0 && column % 2 == 1)
            {
                Width = board.WallCellSize;
                Height = board.PlayerCellSize;
                Orientation = Orientation.Vertical;
            }

            expand = board.WallCellSize;
            offset = expand / 2;

            FreeColor = board.WallCellFreeColor;
            UsedColor = board.WallCellUsedColor;

            MouseEnter += new EventHandler(OnMouseEnter);
            MouseLeave += new EventHandler(OnMouseLeave);
            Click += new EventHandler(OnClick);
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            UsedStyle();
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            FreeStyle();
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (!isUsed)
            {
                isUsed = true;
            }
        }

        public void UsedStyle()
        {
            if (isUsed) return;
            Height += expand;
            Top -= offset;
            Width += expand;
            Left -= offset;
            BackColor = UsedColor;
            BringToFront();
        }

        public void FreeStyle()
        {
            if (isUsed) return;
            Height -= expand;
            Top += offset;
            Width -= expand;
            Left += offset;
            BackColor = FreeColor;
            SendToBack();
        }
    }
#endif
}
