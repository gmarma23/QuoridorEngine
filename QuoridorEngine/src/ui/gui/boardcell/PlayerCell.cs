
namespace QuoridorEngine.src.ui.gui.board
{
    public class PlayerCell : BoardCell
    {
        private Color normalColor;
        private Color possibleMoveColor;

        public PlayerCell(GuiClient guiClient, int row, int column, int size) : base(row, column) 
        {
            sizes(size);
            defaultStyle();

            Click += guiClient.MovePlayer;
        }

        public void Normal()
        {
            BackColor = normalColor;
        }

        public void PossibleMove()
        {
            BackColor = possibleMoveColor;
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
