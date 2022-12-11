
namespace QuoridorEngine.src.ui.gui.board
{
    public class PlayerCell : BoardCell
    {
        private Color normalColor;
        private Color possibleMoveColor;

        public PlayerCell(int row, int column, int size) : base(row, column) 
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
            Click += new EventHandler(onClick);
        }

        public void RemoveEventHandlers(EventHandler onClick)
        {
            Click -= new EventHandler(onClick);
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
}
