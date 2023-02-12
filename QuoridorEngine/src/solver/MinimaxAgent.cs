using System.Diagnostics;

namespace QuoridorEngine.Solver
{
    public class MinimaxAgent : ISolver
    {
        public Move GetBestMove(IGameState currentState, bool whiteIsMaximizingPlayer)
        {
            Move bestMove = null;

            float eval = float.NegativeInfinity;
            var moves = currentState.GetPossibleMoves(whiteIsMaximizingPlayer);

            foreach(var move in moves)
            {
                currentState.ExecuteMove(move);

                float currentEval = minValue(currentState, whiteIsMaximizingPlayer, whiteIsMaximizingPlayer);
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

        private float maxValue(IGameState currentState, bool whiteIsMaximizingPlayer, bool isWhitePlayerTurn)
        {
            if (currentState.IsTerminalState()) return currentState.EvaluateState(isWhitePlayerTurn, whiteIsMaximizingPlayer);

            float eval = float.NegativeInfinity;
            var moves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach(var move in moves)
            {
                currentState.ExecuteMove(move);

                eval = Math.Max(eval, minValue(currentState, whiteIsMaximizingPlayer, !isWhitePlayerTurn));

                currentState.UndoMove(move);
            }

            return eval;
        }

        private float minValue(IGameState currentState, bool whiteIsMaximizingPlayer, bool isWhitePlayerTurn)
        {
            if (currentState.IsTerminalState()) return currentState.EvaluateState(isWhitePlayerTurn, whiteIsMaximizingPlayer);

            float eval = float.PositiveInfinity;
            var moves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach (var move in moves)
            {
                currentState.ExecuteMove(move);

                eval = Math.Min(eval, maxValue(currentState, whiteIsMaximizingPlayer, !isWhitePlayerTurn));

                currentState.UndoMove(move);
            }

            return eval;
        }
    }
}