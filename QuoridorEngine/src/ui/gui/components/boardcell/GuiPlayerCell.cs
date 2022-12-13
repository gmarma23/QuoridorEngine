
namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public class GuiPlayerCell : GuiBoardCell
    {
        private Color normalColor;
        private Color possibleMoveColor;

        public GuiPlayerCell(int row, int column, int size) : base(row, column) 
        {
            setSizes(size, size);
            applyDefaultStyle();
        }

        public void ToNormal()
        {
            BackColor = normalColor;
        }

        public void ToPossibleMove()
        {
            BackColor = possibleMoveColor;
        }

        public override void AddEventHandlers(Dictionary<string, EventHandler> eventHandlers)
        {
            Click += eventHandlers["OnPlayerCellClick"];
        }

        protected override void setSizes(int minSize, int maxSize)
        {
            Width = minSize;
            Height = Width;
        }

        protected override void applyDefaultStyle()
        {
            normalColor = Color.RoyalBlue;
            possibleMoveColor = Color.YellowGreen;
            ToNormal();
        }
    }
#endif
}
