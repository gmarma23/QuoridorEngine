
namespace QuoridorEngine.src.ui.gui.board
{
    public class WallCornerCell : BoardCell
    {
        public WallCornerCell(Board board, int row, int column) : base(row, column)
        {
            Width = board.WallCellSize;
            Height = Width;
            defaultStyle();
        }

        private void defaultStyle()
        {
            BackColor = Color.Transparent;
        }
    }
}
