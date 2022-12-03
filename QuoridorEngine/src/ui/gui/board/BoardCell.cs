namespace QuoridorEngine.src.ui.gui.board
{
#if !CONSOLE
    public abstract class BoardCell : Label
    {
        protected int row;
        protected int column;

        public int Row { get => row; }
        public int Column { get => column; }

        public BoardCell(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
#endif
}
