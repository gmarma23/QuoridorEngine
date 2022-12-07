using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.src.ui.gui.board
{
#if !CONSOLE
    public class WallPartCell : BoardCell
    {
        private bool isUsed;
        private readonly int expand;
        private readonly int offset;

        public event EventHandler<EventArgs> RaisePlaceWallEvent;

        public Color FreeColor { get; set; }
        public Color UsedColor { get; set; } 

        public WallPartCell(Board board, int row, int column) : base (row, column)
        {
            isUsed = false;

            if (Row % 2 == 1 && Column % 2 == 0)
            {
                Width = board.PlayerCellSize;
                Height = board.WallCellSize;
            }
            else if (row % 2 == 0 && column % 2 == 1)
            {
                Width = board.WallCellSize;
                Height = board.PlayerCellSize;
            }

            expand = board.WallCellSize;
            offset = expand / 2;

            FreeColor = board.WallCellFreeColor;
            UsedColor = board.WallCellUsedColor;

            MouseEnter += new EventHandler(OnMouseEnter);
            MouseLeave += new EventHandler(OnMouseLeave);
            Click += new EventHandler(OnClick);
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

        protected virtual void OnRaisePlaceWallEvent(EventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<EventArgs> raiseEvent = RaisePlaceWallEvent;

            // Event will be null if there are no subscribers
            if (raiseEvent != null)
                raiseEvent(this, e);
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            OnRaisePlaceWallEvent(new EventArgs());
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
    }
#endif
}
