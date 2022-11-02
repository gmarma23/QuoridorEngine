using System;

/// <summary>
/// Common interface to be abided by any Solver class
/// </summary>
public interface ISolver
{
	// Returns the best move for the maximizing player given a specific game configuration
	public Move GetBestMove(IGameState state, bool whiteIsMaximizingPlayer);
}
