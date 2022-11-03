namespace QuoridorEngine.solver
{
    /// <summary>
    /// Common interface to be abided by any Solver class
    /// </summary>
    public interface ISolver
	{
        /// <summary>
        /// Calculates and returns the best move for the maximizing player given a specific game configuration
        /// </summary>
        /// <param name="currentState">The current game configuration</param>
        /// <param name="whiteIsMaximizingPlayer">True if white is the player asking for a move, false otherwise</param>
        /// <returns>A move that according to the solving algorithm is the best for the maximizing player</returns>
        public Move GetBestMove(IGameState currentState, bool whiteIsMaximizingPlayer = true);
	}
}