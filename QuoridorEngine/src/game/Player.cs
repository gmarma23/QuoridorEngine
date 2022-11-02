namespace QuoridorEngine.src.game
{
    internal abstract class Player
    {
        protected bool id;
        protected int x;
        protected int y;

        public abstract int X { set; get; }
        public abstract int Y { set; get; }
        public abstract bool Id { set; get; }
    }
}
