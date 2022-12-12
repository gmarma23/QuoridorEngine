
namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public class GuiPlayerCell : GuiBoardCell
    {
        private Color normalColor;
        private Color possibleMoveColor;

        public GuiPlayerCell(int row, int column, int size) : base(row, column) 
        {
            setSizes(size);
            applyDefaultStyle();
        }

        public void ToNormal()
        {
            BackColor = normalColor;
        }

        public void ToPossibleMove()
        {
            BackColor = possibleMoveColor;
        }

        public void AddEventHandlers(EventHandler onClick)
        {
            Click += onClick;
        }

        public void RemoveEventHandlers(EventHandler onClick)
        {
            Click -= onClick;
        }

        private void setSizes(int size)
        {
            Width = size;
            Height = Width;
        }

        private void applyDefaultStyle()
        {
            normalColor = Color.RoyalBlue;
            possibleMoveColor = Color.YellowGreen;
            ToNormal();
        }
    }
#endif
}
