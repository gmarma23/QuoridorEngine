namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public abstract class GuiBoardCell : Label
    {
        public int Row { get; init; }
        public int Column { get; init; }

        public GuiBoardCell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public abstract void AddEventHandlers(Dictionary<string, EventHandler> eventHandlers);

        protected abstract void setSizes(int minSize, int maxSize);

        protected abstract void applyDefaultStyle();
    }
#endif
}
