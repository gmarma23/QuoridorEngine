
namespace QuoridorEngine.src.ui.gui.board
{
    public class PlayerCell : BoardCell
    {
        public PlayerCell(Board board, GuiClient guiClient, int row, int column) : base(row, column) 
        {
            BackColor = board.PlayerCellColor;
            Width = board.PlayerCellSize;
            Height = Width;
        }
    }
}
