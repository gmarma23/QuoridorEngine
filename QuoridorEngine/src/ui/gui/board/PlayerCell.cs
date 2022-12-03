namespace QuoridorEngine.src.ui.gui.board
{
#if !CONSOLE
    public class PlayerCell : BoardCell
    {
        public PlayerCell(int row, int column, int size) : base(row, column)
        {
            normalStyle();
            Width = size;
            Height = size;
        }

        public void availableStyle()
        {
            BackColor = Color.Red;
        }

        public void normalStyle()
        {
            BackColor = Color.RoyalBlue;
        }
    }
# endif
}
