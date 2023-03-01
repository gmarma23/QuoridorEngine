using System.Diagnostics;
using System.Collections.Generic;

namespace QuoridorEngine.Solver
{
    public class AlphaBetaIDTranspositionAgent : ISolver
    {
        private const float moveTime = 2000; // milliseconds
        private static Stopwatch timer = new Stopwatch();
        private static TranspositionTable transpositionTable = new (24000000);
        private static bool timeout = false;
        
        public static Move GetBestMove(IGameState currentState, bool isWhitePlayerTurn)
        {
            Move bestMove = null;
            timeout = false;
            timer.Restart();

            for (int i = 2; ; i++)
            {
                transpositionTable.Clear();
                Move result = bestMoveInDepth(currentState, isWhitePlayerTurn, i);
                if (!timeout)
                    bestMove = result;
                else
                    break;     
            }

            timer.Stop();

            Debug.Assert(bestMove != null);
            return bestMove;
        }

        private static Move bestMoveInDepth(IGameState currentState, bool isWhitePlayerTurn, int depth)
        {
            Move bestMove = null;
            float a = float.NegativeInfinity;
            float b = float.PositiveInfinity;

            if (isWhitePlayerTurn)
            {
                float bestEval = float.NegativeInfinity;
                var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

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
                    if (currentEval > b) return bestMove;
                    a = Math.Max(a, currentEval);
                }
            }
            else
            {
                float bestEval = float.PositiveInfinity;
                var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

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
            long stateHash = currentState.GetHash(isWhitePlayerTurn);
            if (transpositionTable.HasKey(stateHash))
                return transpositionTable.Get(stateHash).evaluation;

            if (depthRemaining == 0 || currentState.IsTerminalState())
            {
                float eval = currentState.EvaluateState(isWhitePlayerTurn);

                EntryType returnType;
                returnType.evaluation = eval;

                transpositionTable.Add(stateHash, returnType);
                return eval;
            }

            float maxEval = float.NegativeInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

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
                if (maxEval > b) return maxEval;
                a = Math.Max(a, maxEval);         
            }

            return maxEval;
        }

        private static float minValue(IGameState currentState, bool isWhitePlayerTurn, int depthRemaining, float a, float b)
        {
            long stateHash = currentState.GetHash(isWhitePlayerTurn);
            if (transpositionTable.HasKey(stateHash))
                return transpositionTable.Get(stateHash).evaluation;

            if (depthRemaining == 0 || currentState.IsTerminalState())
            {
                float eval = currentState.EvaluateState(isWhitePlayerTurn);

                EntryType returnType;
                returnType.evaluation = eval;

                transpositionTable.Add(stateHash, returnType);
                return eval;
            }

            float minEval = float.PositiveInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

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
                if(minEval < a) return minEval;
                b = Math.Min(b, minEval);
            }

            return minEval;
        }
    }

    public class TranspositionTable
    {
      
        private Dictionary<long, EntryType> table;
        private readonly int capacity;

        public TranspositionTable(int capacity)
        {
            table = new Dictionary<long, EntryType>();
            table.EnsureCapacity(capacity);
            this.capacity = capacity;
        }

        public void Add(long key, EntryType data)
        {
            //if (table.Count == capacity) return;

            table.Add(key, data);
        }

        public EntryType Get(long key) { 
            Debug.Assert(HasKey(key));
            return table[key];      
        }

        public bool HasKey(long key)
        {
            return table.ContainsKey(key);
        }

        public void Clear()
        {
            table.Clear();
        }
    }

    public struct EntryType
    {
        public float evaluation;
    }
}