namespace QuoridorEngine.Core
{
    internal class WallPlacement : QuoridorMove
    {
        private readonly Orientation orientation;

        public WallPlacement(int row, int column, Orientation orientation) : base(row, column)
        {
            this.orientation = orientation;
        }

        public Orientation Orientation { get; }
    }

    public enum Orientation
    {
        Horizontal, Vertical
    };
}
