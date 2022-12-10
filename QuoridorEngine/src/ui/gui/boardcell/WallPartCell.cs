
namespace QuoridorEngine.src.ui.gui.board
{
#if !CONSOLE
    public class WallPartCell : BoardCell
    {
        private readonly int expand;
        private readonly int offset;

        public bool IsPlaced { get; set; }
        public bool IsActive { get; private set; }

        public Color FreeColor { get; set; }
        public Color UsedColor { get; set; } 

        public WallPartCell(Board board, GuiClient guiClient, int row, int column) : base (row, column)
        {
            IsPlaced = false;

            if (Row % 2 == 1 && Column % 2 == 0)
            {
                Width = board.PlayerCellSize;
                Height = board.WallCellSize;
            }
            else if (row % 2 == 0 && column % 2 == 1)
            {
                Width = board.WallCellSize;
                Height = board.PlayerCellSize;
            }

            expand = board.WallCellSize;
            offset = expand / 2;

            FreeColor = board.WallCellFreeColor;
            UsedColor = board.WallCellUsedColor;

            MouseEnter += guiClient.PreviewWall;
            MouseLeave += guiClient.RemoveWallPreview;
            MouseClick += guiClient.PlaceWall;
        }

        public void Use()
        {
            IsActive = true;
            Height += expand;
            Top -= offset;
            Width += expand;
            Left -= offset;
            BackColor = UsedColor;
            BringToFront();
        }

        public void Free()
        {
            IsActive = false;
            Height -= expand;
            Top += offset;
            Width -= expand;
            Left += offset;
            BackColor = FreeColor;
            SendToBack();
        }
    }
#endif
}
