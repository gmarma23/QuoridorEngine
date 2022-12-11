
namespace QuoridorEngine.src.ui.gui.board
{
#if !CONSOLE
    public class WallCornerCell : BoardCell
    {
        public WallCornerCell(int row, int column, int size) : base(row, column)
        {
            sizes(size);
            defaultStyle();
        }

        private void sizes(int size)
        {
            Width = size;
            Height = Width;
        }

        private void defaultStyle()
        {
            BackColor = Color.Transparent;
        }
    }
#endif
}
