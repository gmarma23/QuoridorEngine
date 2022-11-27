
namespace QuoridorEngine.src.ui.gui
{
#if !CONSOLE222
    public class WallPartCell : BoardCell
    {
        private const int expand = 6;
        private const int offset = expand / 2;
        private Orientation orientation;
        private bool isUsed;

        public bool IsUsed { get => isUsed; }

        public WallPartCell(int row, int column) : base(row, column)
        {
            isUsed = false;
            assignOrientation();
            transformCellCoordinates();
            MouseEnter += new EventHandler(OnMouseEnter);
            MouseLeave += new EventHandler(OnMouseLeave);
            Click += new EventHandler(OnClick);

            BackColor = Color.White;
        }

        protected override void transformCellCoordinates()
        {
            if (orientation == Orientation.Horizontal)
            {
                internalRow = (guiRow + 1) / 2;
                internalColumn = guiColumn / 2;
            }
            else if (orientation == Orientation.Vertical)
            {
                internalRow = guiRow / 2;
                internalColumn = (guiColumn - 1) / 2;
            }
        }

        private void assignOrientation()
        {
            if (guiRow % 2 == 1 && guiColumn % 2 == 0)
                orientation = Orientation.Horizontal;
            else if (guiColumn % 2 == 1 && guiRow % 2 == 0)
                orientation = Orientation.Vertical;
            else
                throw new Exception("Not a wall part cell");
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            if(!isUsed) activeStyle();
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            if(!isUsed) inactiveStyle();
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (!isUsed) 
            {
                isUsed = true;
            }  
        }

        private void activeStyle()
        {
            Height += expand;
            Top -= offset;
            Width += expand;
            Left -= offset;
            BackColor = Color.Black;
            BringToFront();
        }

        private void inactiveStyle()
        {
            Height -= expand;
            Top += offset;
            Width -= expand;
            Left += offset;
            BackColor = Color.White;
            SendToBack();
        }
    }
#endif
}
