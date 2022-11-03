namespace QuoridorEngine.Utils
{
    public class InvalidMoveException : Exception {
        public InvalidMoveException(string message) :base(message) { }
        public InvalidMoveException() : base() { }
    }
}