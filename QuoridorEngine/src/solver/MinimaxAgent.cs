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
                //debugUtil(currentState, currentEval, !isWhitePlayerTurn);
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
            {
                float currentEval = currentState.EvaluateState(isWhitePlayerTurn);
                //debugUtil(currentState, currentEval, isWhitePlayerTurn);
                return currentEval;
            }
               

            float maxEval = float.NegativeInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach(var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = minValue(currentState, !isWhitePlayerTurn, depthRemaining - 1);
                //debugUtil(currentState, currentEval, !isWhitePlayerTurn);
                currentState.UndoMove(nextMove);

                maxEval = Math.Max(maxEval, currentEval);
            }

            return maxEval;
        }

        private static float minValue(IGameState currentState, bool isWhitePlayerTurn, int depthRemaining)
        {
            if (depthRemaining == 0 || currentState.IsTerminalState())
            {
                float currentEval = currentState.EvaluateState(isWhitePlayerTurn);
                //debugUtil(currentState, currentEval,isWhitePlayerTurn);
                return currentEval;
            }

            float minEval = float.PositiveInfinity;
            var possibleNextMoves = currentState.GetPossibleMoves(isWhitePlayerTurn);

            foreach (var nextMove in possibleNextMoves)
            {
                currentState.ExecuteMove(nextMove);
                float currentEval = maxValue(currentState, !isWhitePlayerTurn, depthRemaining - 1);
                //debugUtil(currentState, currentEval, !isWhitePlayerTurn);
                currentState.UndoMove(nextMove);

                minEval = Math.Min(minEval, currentEval);
            }

            return minEval;
        }

        private static void debugUtil(IGameState currentState, float currentEval, bool isWhitePLayerTurn)
        {
#if !CONSOLE 
            int wrow = 0, wcol = 0, brow = 0, bcol = 0;
            Core.QuoridorGame gameState = (Core.QuoridorGame)currentState;

            gameState.GetWhiteCoordinates(ref wrow, ref wcol);
            gameState.GetBlackCoordinates(ref brow, ref bcol);

            MessageBox.Show(
                $"White: (row = {wrow}, col = {wcol})\n" +
                $"Black: (row = {brow}, col = {bcol})\n" +
                $"WhiteDist: {gameState.Dimension-1 - wrow}\n" +
                $"BlackDist: {brow}\n" +
                $"IsWhiteTurn: {isWhitePLayerTurn}\n" +
                $"Eval: {currentEval}"
                );
#endif
        }
    }
}