
namespace QuoridorEngine.UI
{
#if !CONSOLE
    public class GuiWallCornerCell : GuiBoardCell
    {
        public GuiWallCornerCell(int row, int column, int size) : base(row, column)
        {
            setSizes(size);
            applyDefaultStyle();
        }

        private void setSizes(int size)
        {
            Width = size;
            Height = Width;
        }

        private void applyDefaultStyle()
        {
            BackColor = Color.Transparent;
        }
    }
#endif
}
