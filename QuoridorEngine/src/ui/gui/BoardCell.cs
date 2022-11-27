
namespace QuoridorEngine.src.ui.gui
{
#if !CONSOLE
    public abstract class BoardCell : Label
    {
        protected int guiRow;
        protected int guiColumn;
        protected int internalRow;
        protected int internalColumn;

        public int GuiRow { get => guiRow; }
        public int GuiColumn { get => guiColumn; }
        public int InternalRow { get => internalRow; } 
        public int InternalColumn { get => internalColumn; }

        public BoardCell(int row, int column)
        {
            guiRow = row;
            guiColumn = column;
        }

        protected abstract void transformCellCoordinates();
    }
#endif
}
