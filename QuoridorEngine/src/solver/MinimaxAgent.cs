using QuoridorEngine.UI;
using System.Diagnostics;

namespace QuoridorEngine.Solver
{
    public class MinimaxAgent : ISolver
    {
        private const int depth = 2;

        public static Move GetBestMove(IGameState currentState, bool whiteIsMaximizingPlayer)
        {
            Move bestMove = null;

            float eval = float.NegativeInfinity;
            var moves = currentState.GetPossibleMoves(whiteIsMaximizingPlayer);

            foreach(var move in moves)
            {
                currentState.ExecuteMove(move);

                float currentEval = minValue(currentState, whiteIsMaximizingPlayer, !whiteIsMaximizingPlayer, depth-1);
                if(currentEval > eval)
                {
                    eval = currentEval;
                    bestMove = move;
                }

                currentState.UndoMove(move);
            }

            Debug.Assert(bestMove != null);
            return bestMove;
        }

        private static float maxValue(IGameState currentState, bool whiteIsMaximizingPlayer, bool isWhitePlayerTurn, int depthRemaining)
        {
            if (currentState.IsTerminalState() || depthRemaining == 0)
            {
                float value = currentState.EvaluateState(isWhitePlayerTurn, whiteIsMaximizingPlayer);
                return value;
            }

            float eval = float.NegativeInfinity;
            var moves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach(var move in moves)
            {
                currentState.ExecuteMove(move);

                eval = Math.Max(eval, minValue(currentState, whiteIsMaximizingPlayer, !isWhitePlayerTurn, depthRemaining-1));

                currentState.UndoMove(move);
            }

            return eval;
        }

        private static float minValue(IGameState currentState, bool whiteIsMaximizingPlayer, bool isWhitePlayerTurn, int depthRemaining)
        {
            if (currentState.IsTerminalState() || depthRemaining == 0)
            {
                float value = currentState.EvaluateState(isWhitePlayerTurn, whiteIsMaximizingPlayer);
                return value;
            }

            float eval = float.PositiveInfinity;
            var moves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach (var move in moves)
            {
                currentState.ExecuteMove(move);

                eval = Math.Min(eval, maxValue(currentState, whiteIsMaximizingPlayer, !isWhitePlayerTurn, depthRemaining-1));

                currentState.UndoMove(move);
            }

            return eval;
        }
    }
}