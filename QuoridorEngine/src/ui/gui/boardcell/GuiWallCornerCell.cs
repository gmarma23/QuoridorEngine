
namespace QuoridorEngine.UI
{
#if !CONSOLE
    public class GuiWallCornerCell : GuiBoardCell
    {
        public GuiWallCornerCell(int row, int column, int size) : base(row, column)
        {
            sizes(size);
            defaultStyle();
        }

        private void sizes(int size)
        {
            Width = size;
            Height = Width;
        }

        private void defaultStyle()
        {
            BackColor = Color.Transparent;
        }
    }
#endif
}
