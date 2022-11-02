using QuoridorEngine.src.solver;

namespace QuoridorEngine.src.quoridor
{
    /// <summary>
    /// Class representing quoridor board
    /// </summary>
    internal class QuoridorBoard
    {
        private int width;
        private int height;

        public int Width
        {
            set { if (value is int and > 0) { width = value; } }
            get { return width; }
        }

        public int Height
        {
            set { if (value is int and > 0) { height = value; } }
            get { return height; }
        }

        QuoridorBoard(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Update board according to newly played moves
        /// </summary>
        /// <param name="move">Last played move</param>
        public void Update(Move move)
        {

        }

        // Initialize board at the begining of aech round
        public void Initialize()
        {

        }

        // Clear board
        public void Clear()
        {

        }
    }
}
