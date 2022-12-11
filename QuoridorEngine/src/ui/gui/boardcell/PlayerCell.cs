
namespace QuoridorEngine.src.ui.gui.board
{
    public class PlayerCell : BoardCell
    {
        public Color NormalColor { get; set; }
        public Color PossibleMoveColor { get; set; }

        public PlayerCell(Board board, GuiClient guiClient, int row, int column) : base(row, column) 
        {
            NormalColor = board.PlayerCellNormalColor;
            PossibleMoveColor = board.PlayerCellPossibleMoveColor;
            Normal();

            Width = board.PlayerCellSize;
            Height = Width;

            Click += guiClient.MovePlayer;
        }

        public void Normal()
        {
            BackColor = NormalColor;
        }

        public void PossibleMove()
        {
            BackColor= PossibleMoveColor;
        }
    }
}
