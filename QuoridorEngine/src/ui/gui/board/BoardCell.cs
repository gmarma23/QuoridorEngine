using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorEngine.src.ui.gui.board
{
    public class BoardCell : Label
    {
        public int Row { get; init; }
        public int Column { get; init; }

        public BoardCell(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
