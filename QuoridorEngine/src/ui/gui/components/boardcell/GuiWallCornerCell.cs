namespace QuoridorEngine.UI
{
#if !CONSOLE
    public class GuiWallCornerCell : GuiBoardCell
    {
        public GuiWallCornerCell(int row, int column, int size) : base(row, column)
        {
            setSizes(size, size);
            applyDefaultStyle();
        }

        public override void AddEventHandlers(Dictionary<string, EventHandler> eventHandlers)
        {
            return;
        }

        protected override void setSizes(int minSize, int maxSize)
        {
            Width = minSize;
            Height = Width;
        }

        protected override void applyDefaultStyle()
        {
            BackColor = Color.Transparent;
        }
    }
#endif
}
