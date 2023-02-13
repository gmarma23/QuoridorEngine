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
                if(currentEval > maxEval)
                {
                    maxEval = currentEval;
                    bestMove = nextMove;
                }

                currentState.UndoMove(nextMove);
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
                maxEval = Math.Max(maxEval, currentEval);

                currentState.UndoMove(nextMove);
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
                minEval = Math.Min(minEval, currentEval);

                currentState.UndoMove(nextMove);
            }

            return minEval;
        }
    }
}