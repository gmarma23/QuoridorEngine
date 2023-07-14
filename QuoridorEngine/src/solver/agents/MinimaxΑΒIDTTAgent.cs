using System.Diagnostics;

namespace QuoridorEngine.Solver
{
    public class MinimaxΑΒIDTTAgent : ISolver
    {
        private const int moveTime = 4200; // milliseconds
        private const int absoluteDepthLimit = 20;
        private static Stopwatch timer = new Stopwatch();
        private static TranspositionTable transpositionTable = new (25000009);
        private static bool timeout = false;
        
        public static Move GetBestMove(IGameState currentState, bool isWhitePlayerTurn)
        {
            Move bestMove = null;
            timeout = false;
            Stopwatch iterationTimer = new Stopwatch();

            timer.Restart();

            for (int i = 1; i <= absoluteDepthLimit; i++)
            {
                iterationTimer.Restart();

                transpositionTable.Clear();
                Move result = bestMoveInDepth(currentState, isWhitePlayerTurn, i, bestMove);
                if (!timeout)
                    bestMove = result;
                else
                    break;

                iterationTimer.Stop();

                long remainingTime = moveTime - timer.ElapsedMilliseconds;
                if (2.5f * iterationTimer.ElapsedMilliseconds > remainingTime)
                    break;
            }

            timer.Stop();

            Debug.Assert(bestMove != null);
            return bestMove;
        }

        private static Move bestMoveInDepth(IGameState currentState, bool isWhitePlayerTurn, int depth, Move previousBestMove)
        {
            Move bestMove = null;

            float a = float.NegativeInfinity;
            float b = float.PositiveInfinity;

            List<Move> possibleNextMoves = currentState.GetPossibleAgentMoves(isWhitePlayerTurn).ToList();
            if (previousBestMove != null)
            {
                Debug.Assert(possibleNextMoves.Contains(previousBestMove));
                possibleNextMoves.Remove(previousBestMove);
                possibleNextMoves.Insert(0, previousBestMove);
            }

            if (isWhitePlayerTurn)
            {
                float bestEval = float.NegativeInfinity;

                foreach (var nextMove in possibleNextMoves)
                {
                    if (timer.ElapsedMilliseconds > moveTime)
                    {
                        timeout = true;
                        return bestMove;
                    }
                 
                    currentState.ExecuteMove(nextMove);
                    float currentEval = minValue(currentState, !isWhitePlayerTurn, depth - 1, a, b);
                    currentState.UndoMove(nextMove);

                    if (currentEval > bestEval)
                    {
                        bestMove = nextMove;
                        bestEval = currentEval;
                    }

                    Debug.Assert(bestMove != null);
                    if (currentEval > b)
                        return bestMove;
                    a = Math.Max(a, currentEval);
                }
            }
            else
            {
                float bestEval = float.PositiveInfinity;

                foreach (var nextMove in possibleNextMoves)
                {
                    if (timer.ElapsedMilliseconds > moveTime)
                    {
                        timeout = true;
                        return bestMove;
                    }

                    currentState.ExecuteMove(nextMove);
                    float currentEval = maxValue(currentState, !isWhitePlayerTurn, depth - 1, a, b);
                    currentState.UndoMove(nextMove);

                    if (currentEval < bestEval)
                    {
                        bestMove = nextMove;
                        bestEval = currentEval;
                    }

                    Debug.Assert(bestMove != null);
                    if (currentEval < a)
                        return bestMove;
                    b = Math.Min(b, currentEval);
                }
            }

            Debug.Assert(bestMove != null);
            return bestMove;
        }

        private static float maxValue(IGameState currentState, bool isWhitePlayerTurn, int depthRemaining, float a, float b)
        {
            long stateHash = currentState.GetHash(isWhitePlayerTurn);
            if (transpositionTable.HasKey(stateHash))
                return transpositionTable.Get(stateHash);

            if (depthRemaining == 0 || currentState.IsTerminalState())
            {
                float eval = currentState.EvaluateState(isWhitePlayerTurn);
                transpositionTable.Add(stateHash, eval);
                return eval;
            }

            float maxEval = float.NegativeInfinity;
            var possibleNextMoves = currentState.GetPossibleAgentMoves(isWhitePlayerTurn);

            foreach(var nextMove in possibleNextMoves)
            {
                if (timer.ElapsedMilliseconds > moveTime)
                {
                    timeout = true;
                    return 0;
                }

                currentState.ExecuteMove(nextMove);
                float currentEval = minValue(currentState, !isWhitePlayerTurn, depthRemaining - 1, a, b);         
                currentState.UndoMove(nextMove);

                maxEval = Math.Max(maxEval, currentEval);
                if (maxEval > b) 
                    return maxEval;
                a = Math.Max(a, maxEval);         
            }

            return maxEval;
        }

        private static float minValue(IGameState currentState, bool isWhitePlayerTurn, int depthRemaining, float a, float b)
        {
            long stateHash = currentState.GetHash(isWhitePlayerTurn);
            if (transpositionTable.HasKey(stateHash))
                return transpositionTable.Get(stateHash);

            if (depthRemaining == 0 || currentState.IsTerminalState())
            {
                float eval = currentState.EvaluateState(isWhitePlayerTurn);
                transpositionTable.Add(stateHash, eval);
                return eval;
            }

            float minEval = float.PositiveInfinity;
            var possibleNextMoves = currentState.GetPossibleAgentMoves(isWhitePlayerTurn);

            foreach (var nextMove in possibleNextMoves)
            {
                if (timer.ElapsedMilliseconds > moveTime)
                {
                    timeout = true;
                    return 0;
                }

                currentState.ExecuteMove(nextMove);
                float currentEval = maxValue(currentState, !isWhitePlayerTurn, depthRemaining - 1, a, b);
                currentState.UndoMove(nextMove);

                minEval = Math.Min(minEval, currentEval);
                if(minEval < a) 
                    return minEval;
                b = Math.Min(b, minEval);
            }

            return minEval;
        }
    }
}