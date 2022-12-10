
namespace QuoridorEngine.src.ui.gui.board
{
    public class WallCornerCell : BoardCell
    {
        public WallCornerCell(Board board, int row, int column) : base(row, column)
        {
            BackColor = board.WallCellFreeColor;
            Width = board.WallCellSize;
            Height = Width;
        }
    }
}
