using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
