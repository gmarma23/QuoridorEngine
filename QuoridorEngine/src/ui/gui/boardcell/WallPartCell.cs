﻿using Orientation = QuoridorEngine.Core.Orientation;

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

        public WallPartCell(Board board, int row, int column, GuiClient guiClient) : base (row, column)
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

            MouseEnter += guiClient.OnPlaceWall;
            MouseLeave += guiClient.OnRemoveWall;
            MouseClick += OnClick;
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

        private void OnClick(object sender, EventArgs e)
        {
            IsPlaced = true;
        }
    }
#endif
}
