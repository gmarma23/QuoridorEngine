using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorEngine.src.ui.gui.board
{
    public class PlayerCell : BoardCell
    {
        public PlayerCell(Board board, int row, int column) : base(row, column) 
        {
            BackColor = board.PlayerCellColor;
            Width = board.PlayerCellSize;
            Height = Width;
        }
    }
}
