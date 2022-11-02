namespace QuoridorEngine.src.game
{
    /// <summary>
    /// Common class to represent a piece in any given game.
    /// Any implementation of a particular game
    /// needs to extend this class accordingly
    /// </summary>
    internal abstract class Piece
    {
        protected BoardNode? location;

        public abstract BoardNode? Location { set; get; }

    }

    /// <summary>
    /// Common class to represent a player in any given game.
    /// Any implementation of a particular game
    /// needs to extend this class accordingly
    /// </summary>
    internal abstract class Player
    {
        protected List<Piece>? pieces;
        protected int victories = 0;
        protected int defeats = 0;
        protected int draws = 0;
        protected char id;
        
        public abstract char Id { set; get; }

        public int Victories { 
            set { victories++; }
            get { return victories; }
        }

        public int Defeats
        {
            set { defeats++; }
            get { return victories; }
        }

        public int Draws
        {
            set { draws++; }
            get { return victories; }
        }

        Player()
        {
            pieces = new List<Piece>();
        }

        /// <summary>
        /// Add new piece to a player
        /// </summary>
        /// <param name="piece">Piece to be added</param>
        public abstract void AddPiece(Piece piece);

        /// <summary>
        /// Remove piece from a player
        /// </summary>
        /// <param name="piece">Piece to be removed</param>
        public abstract void RemovePiece(Piece piece);
    }
}
