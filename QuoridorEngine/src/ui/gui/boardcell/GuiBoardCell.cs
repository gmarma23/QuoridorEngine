
namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public class GuiBoardCell : Label
    {
        public int Row { get; init; }
        public int Column { get; init; }

        public GuiBoardCell(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
#endif
}
