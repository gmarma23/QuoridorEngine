using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorEngine.src.ui.gui
{
    public class BoardCell : Label
    {
        private BoardCellType type;
        private int row;
        private int column;

        public BoardCellType Type { get => type; }
        public int Row { get => row; }
        public int Column { get => column; }

        public BoardCell()
        {

        }
    }

    public enum BoardCellType
    {
        Player,
        Wall
    }
}
