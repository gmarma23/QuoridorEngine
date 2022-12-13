namespace QuoridorEngine.UI
{
#if !CONSOLE
    public class GuiWallPartCell : GuiBoardCell
    {
        private int expand;
        private int offset;
        private Color freeColor;
        private Color usedColor;

        public bool IsPlaced { get; set; }
        public bool IsActive { get; private set; }

        public GuiWallPartCell(int row, int column, int minSize, int maxSize) : base (row, column)
        {
            IsPlaced = false;

            setSizes(minSize, maxSize);
            applyDefaultStyle();
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

        public override void AddEventHandlers(Dictionary<string, EventHandler> eventHandlers)
        {
            MouseEnter += eventHandlers["OnWallCellEnter"];
            MouseLeave += eventHandlers["OnWallCellLeave"];
            Click += eventHandlers["OnWallCellClick"];
        }

        protected override void setSizes(int minSize, int maxSize)
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

        protected override void applyDefaultStyle()
        {
            freeColor = Color.Transparent;
            usedColor = Color.LightGray;
        }
    }
#endif
}
