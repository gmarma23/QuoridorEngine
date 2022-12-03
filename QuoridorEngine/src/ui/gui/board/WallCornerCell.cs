using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorEngine.src.ui.gui.board
{
    public class WallCornerCell : BoardCell
    {
        public WallCornerCell(int row, int column, int size) : base(row, column)
        {
            normalStyle();
            Width = size;
            Height = size;
        }

        public void normalStyle()
        {
            BackColor = Color.White;
        }
    }
}
