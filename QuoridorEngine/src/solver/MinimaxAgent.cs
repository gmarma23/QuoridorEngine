using System.Diagnostics;

namespace QuoridorEngine.Solver
{
    public class MinimaxAgent : ISolver
    {
        private const int DEPTH = 3;
        
        public static Move GetBestMove(IGameState currentState, bool isWhitePlayerTurn)
        {
            Move bestMove = null;

            float bestEval = isWhitePlayerTurn ? float.NegativeInfinity : float.PositiveInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach(var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = isWhitePlayerTurn ? minValue(currentState, !isWhitePlayerTurn, DEPTH - 1) : 
                                                        maxValue(currentState, !isWhitePlayerTurn, DEPTH - 1);
                currentState.UndoMove(nextMove);

                bool isCurrentEvalBetter = isWhitePlayerTurn ? currentEval > bestEval : currentEval < bestEval;
                if (isCurrentEvalBetter) 
                { 
                    bestEval = currentEval;
                    bestMove = nextMove;
                }
            }
            Debug.Assert(bestMove != null);
            return bestMove;
        }

        private static float maxValue(IGameState currentState, bool isWhitePlayerTurn, int depthRemaining)
        {
            if (depthRemaining == 0 || currentState.IsTerminalState())
                return currentState.EvaluateState(isWhitePlayerTurn);

            float maxEval = float.NegativeInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach(var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = minValue(currentState, !isWhitePlayerTurn, depthRemaining - 1);
                currentState.UndoMove(nextMove);

                maxEval = Math.Max(maxEval, currentEval);
            }

            return maxEval;
        }

        private static float minValue(IGameState currentState, bool isWhitePlayerTurn, int depthRemaining)
        {
            if (depthRemaining == 0 || currentState.IsTerminalState())
                return currentState.EvaluateState(isWhitePlayerTurn);

            float minEval = float.PositiveInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach (var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = maxValue(currentState, !isWhitePlayerTurn, depthRemaining - 1);
                currentState.UndoMove(nextMove);

                minEval = Math.Min(minEval, currentEval);
            }

            return minEval;
        }
    }
}