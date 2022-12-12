
namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public class GuiPlayerCell : GuiBoardCell
    {
        private Color normalColor;
        private Color possibleMoveColor;

        public GuiPlayerCell(int row, int column, int size) : base(row, column) 
        {
            sizes(size);
            defaultStyle();
        }

        public void Normal()
        {
            BackColor = normalColor;
        }

        public void PossibleMove()
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

        private void sizes(int size)
        {
            Width = size;
            Height = Width;
        }

        private void defaultStyle()
        {
            normalColor = Color.RoyalBlue;
            possibleMoveColor = Color.YellowGreen;
            Normal();
        }
    }
#endif
}
