using System.Collections;

/// <summary>
/// Defines the interface to be implemented by any class 
/// that is used to define a Game solvable by the solvers
/// included in this project.
/// </summary>
public interface IGameState
{
	// Returns true if the state is terminal (i.e. game is over)
	public bool IsTerminalState();

	// Returns a list of all the possible moves from this state
	public List<Move> GetPossibleMoves();

	// Executes a given move in this state
	public void ExecuteMove(Move move);

	// Undoes a given move returning the state to its previous configuration
	public void UndoMove(Move move);

	// Returns an evaluation of the likelyhood of selected player winning the game from this state
	public float EvaluateState(bool playerIsWhite);
}