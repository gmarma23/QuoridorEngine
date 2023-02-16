using QuoridorEngine.Core;
using System.Diagnostics;

namespace QuoridorEngine.Solver
{
    public class MinimaxAgent : ISolver
    {
        public static Move GetBestMove(IGameState currentState, bool isWhitePlayerTurn, int depth)
        {
            Move bestMove = null;

            float maxEval = float.NegativeInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach(var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = minValue(currentState, isWhitePlayerTurn, !isWhitePlayerTurn, depth - 1);
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
            {
                float currentEval = currentState.EvaluateState(whiteIsMaximizingPlayer);
                debugUtil(currentState, currentEval, isWhitePlayerTurn, whiteIsMaximizingPlayer);
                return currentEval;
            }
               

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
            {
                float currentEval = currentState.EvaluateState(whiteIsMaximizingPlayer);
                debugUtil(currentState, currentEval,isWhitePlayerTurn, whiteIsMaximizingPlayer);
                return currentEval;
            }

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

        private static void debugUtil(IGameState currentState, float currentEval, bool isWhitePLayerTurn, bool whiteIsMaximizingPlayer)
        {
            int wrow = 0, wcol = 0, brow = 0, bcol = 0;
            QuoridorGame gameState = (Core.QuoridorGame)currentState;

            gameState.GetWhiteCoordinates(ref wrow, ref wcol);
            gameState.GetBlackCoordinates(ref brow, ref bcol);

            MessageBox.Show(
                $"White: (row = {wrow}, col = {wcol})\n" +
                $"Black: (row = {brow}, col = {bcol})\n" +
                $"WhiteDist: {gameState.Dimension-1 - wrow}\n" +
                $"BlackDist: {brow}\n" +
                $"IsWhiteTurn: {isWhitePLayerTurn}\n" +
                $"WhiteIsMax: {whiteIsMaximizingPlayer}\n" +
                $"Eval: {currentEval}"
                );
        }
    }
}