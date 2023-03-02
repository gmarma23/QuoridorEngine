using System.Diagnostics;

namespace QuoridorEngine.Solver
{
    public class MinimaxΑΒAgent : ISolver
    {
        private const int DEPTH = 5;
        
        public static Move GetBestMove(IGameState currentState, bool isWhitePlayerTurn)
        {
            Move bestMove = null;
            float a = float.NegativeInfinity;
            float b = float.PositiveInfinity;

            if(isWhitePlayerTurn) // Maximizer
            {
                float bestEval = float.NegativeInfinity;
                var possibleNextMoves = currentState.GetPossibleAgentMoves(isWhitePlayerTurn);

                foreach (var nextMove in possibleNextMoves)
                {
                    currentState.ExecuteMove(nextMove);
                    float currentEval = minValue(currentState, !isWhitePlayerTurn, DEPTH - 1, a, b);
                    currentState.UndoMove(nextMove);

                    if(currentEval > bestEval)
                    {
                        bestMove = nextMove;
                        bestEval = currentEval;
                    }

                    Debug.Assert(bestMove != null);
                    if (currentEval > b) return bestMove;
                    a = Math.Max(a, currentEval);
                }
            }
            else
            {
                float bestEval = float.PositiveInfinity;
                var possibleNextMoves = currentState.GetPossibleAgentMoves(isWhitePlayerTurn);

                foreach (var nextMove in possibleNextMoves)
                {
                    currentState.ExecuteMove(nextMove);
                    float currentEval = maxValue(currentState, !isWhitePlayerTurn, DEPTH - 1, a, b);
                    currentState.UndoMove(nextMove);

                    if (currentEval < bestEval)
                    {
                        bestMove = nextMove;
                        bestEval = currentEval;
                    }

                    Debug.Assert(bestMove != null);
                    if (currentEval < a) return bestMove;
                    b = Math.Min(b, currentEval);
                }
            }

            Debug.Assert(bestMove != null);
            return bestMove;

            //float bestEval = isWhitePlayerTurn ? float.NegativeInfinity : float.PositiveInfinity;
            //var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            //foreach(var nextMove in possibleNextMoves)
            //{
            //    currentState.ExecuteMove(nextMove);
            //    float currentEval = isWhitePlayerTurn ? minValue(currentState, !isWhitePlayerTurn, DEPTH - 1, float.NegativeInfinity, float.PositiveInfinity) : 
            //                                            maxValue(currentState, !isWhitePlayerTurn, DEPTH - 1, float.NegativeInfinity, float.PositiveInfinity);
            //    currentState.UndoMove(nextMove);

            //    bool isCurrentEvalBetter = isWhitePlayerTurn ? currentEval > bestEval : currentEval < bestEval;
            //    if (isCurrentEvalBetter) 
            //    { 
            //        bestEval = currentEval;
            //        bestMove = nextMove;
            //    }
            //}

            //Debug.Assert(bestMove != null);
            //return bestMove;
        }

        private static float maxValue(IGameState currentState, bool isWhitePlayerTurn, int depthRemaining, float a, float b)
        {
            if (depthRemaining == 0 || currentState.IsTerminalState())
                return currentState.EvaluateState(isWhitePlayerTurn);

            float maxEval = float.NegativeInfinity;
            var possibleNextMoves = currentState.GetPossibleAgentMoves(isWhitePlayerTurn);

            foreach(var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = minValue(currentState, !isWhitePlayerTurn, depthRemaining - 1, a, b);         
                currentState.UndoMove(nextMove);

                maxEval = Math.Max(maxEval, currentEval);
                if (maxEval > b) return maxEval;
                a = Math.Max(a, maxEval);         
            }

            return maxEval;
        }

        private static float minValue(IGameState currentState, bool isWhitePlayerTurn, int depthRemaining, float a, float b)
        {
            if (depthRemaining == 0 || currentState.IsTerminalState())
                return currentState.EvaluateState(isWhitePlayerTurn);

            float minEval = float.PositiveInfinity;
            var possibleNextMoves = currentState.GetPossibleAgentMoves(isWhitePlayerTurn);

            foreach (var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = maxValue(currentState, !isWhitePlayerTurn, depthRemaining - 1, a, b);
                currentState.UndoMove(nextMove);

                minEval = Math.Min(minEval, currentEval);
                if(minEval < a) return minEval;
                b = Math.Min(b, minEval);
            }

            return minEval;
        }
    }
}