
namespace QuoridorEngine.src.ui.gui
{
#if !CONSOLE
    public class PlayerCell : BoardCell
    {
        public PlayerCell(int row, int column) : base (row, column)
        {
            transformCellCoordinates();
            normalStyle();
        }

        public void availableStyle()
        {
            BackColor = Color.Red;
        }

        public void normalStyle()
        {
            BackColor = Color.RoyalBlue;
        }

        protected override void transformCellCoordinates()
        {
            internalRow = guiRow / 2;
            internalColumn = guiColumn / 2;
        }
    }
# endif
}
