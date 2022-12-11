
namespace QuoridorEngine.src.ui.gui.board
{
#if !CONSOLE
    public class WallPartCell : BoardCell
    {
        private int expand;
        private int offset;
        private Color freeColor;
        private Color usedColor;

        public bool IsPlaced { get; set; }
        public bool IsActive { get; private set; }

        public WallPartCell(int row, int column, int minSize, int maxSize) : base (row, column)
        {
            IsPlaced = false;

            sizes(minSize, maxSize);
            defaultStyle();
        }

        public void Use()
        {
            if (IsActive) return;
            IsActive = true;
            Height += expand;
            Top -= offset;
            Width += expand;
            Left -= offset;
            BackColor = usedColor;
            BringToFront();
        }

        public void Free()
        {
            if (!IsActive) return;
            IsActive = false;
            Height -= expand;
            Top += offset;
            Width -= expand;
            Left += offset;
            BackColor = freeColor;
            SendToBack();
        }

        public void AddEventHandlers(EventHandler onMouseEnter, EventHandler onMouseLeave, EventHandler onClick)
        {
            MouseEnter += new EventHandler(onMouseEnter);
            MouseLeave += new EventHandler(onMouseLeave);
            Click += new EventHandler(onClick);
        }

        public void RemoveEventHandlers(EventHandler onMouseEnter, EventHandler onMouseLeave, EventHandler onClick)
        {
            MouseEnter -= new EventHandler(onMouseEnter);
            MouseLeave -= new EventHandler(onMouseLeave);
            Click -= new EventHandler(onClick);
        }

        private void sizes(int minSize, int maxSize)
        {
            if (Row % 2 == 1 && Column % 2 == 0)
            {
                Width = maxSize;
                Height = minSize;
            }
            else if (Row % 2 == 0 && Column % 2 == 1)
            {
                Width = minSize;
                Height = maxSize;
            }

            expand = minSize;
            offset = expand / 2;
        }

        private void defaultStyle()
        {
            freeColor = Color.Transparent;
            usedColor = Color.LightGray;
        }
    }
#endif
}
