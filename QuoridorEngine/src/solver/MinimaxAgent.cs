using System.Diagnostics;

namespace QuoridorEngine.Solver
{
    public class MinimaxAgent : ISolver
    {
        private const int depth = 2;

        public static Move GetBestMove(IGameState currentState, bool whiteIsMaximizingPlayer)
        {
            Move bestMove = null;

            float maxEval = float.NegativeInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(whiteIsMaximizingPlayer);

            foreach(var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = minValue(currentState, whiteIsMaximizingPlayer, !whiteIsMaximizingPlayer, depth - 1);
                currentState.UndoMove(nextMove);

                if (currentEval > maxEval)
                {
                    maxEval = currentEval;
                    bestMove = nextMove;
                }
            }

            Debug.Assert(bestMove != null);
            return bestMove;
        }

        private static float maxValue(IGameState currentState, bool whiteIsMaximizingPlayer, bool isWhitePlayerTurn, int depthRemaining)
        {
            if (depthRemaining == 0 || currentState.IsTerminalState())
                return currentState.EvaluateState(isWhitePlayerTurn, whiteIsMaximizingPlayer);

            float maxEval = float.NegativeInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach(var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = minValue(currentState, whiteIsMaximizingPlayer, !isWhitePlayerTurn, depthRemaining - 1);
                currentState.UndoMove(nextMove);

                maxEval = Math.Max(maxEval, currentEval);
            }

            return maxEval;
        }

        private static float minValue(IGameState currentState, bool whiteIsMaximizingPlayer, bool isWhitePlayerTurn, int depthRemaining)
        {
            if (depthRemaining == 0 || currentState.IsTerminalState())
                return currentState.EvaluateState(isWhitePlayerTurn, whiteIsMaximizingPlayer);

            float minEval = float.PositiveInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach (var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = maxValue(currentState, whiteIsMaximizingPlayer, !isWhitePlayerTurn, depthRemaining - 1);
                currentState.UndoMove(nextMove);

                minEval = Math.Min(minEval, currentEval);
                
            }

            return minEval;
        }
    }
}