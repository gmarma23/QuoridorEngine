using QuoridorEngine.Core;
using QuoridorEngine.src.ui.gui.board;
using QuoridorEngine.UI;
using QuoridorEngine.Utils;
using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.src.ui.gui
{
    public class GuiClient
    {
        public delegate void Function<in T1, in T2>(T1 arg1, T2 arg2);

        private GuiFrame guiFrame;
        private QuoridorGame game;

        public GuiClient() 
        {
            game = new QuoridorGame();
            guiFrame = new GuiFrame();

            renderGameComponents();
        }

        public void Play()
        {
            Application.Run(guiFrame);
        }

        public void OnPlaceWall(object sender, EventArgs e)
        {
            if (((WallPartCell)sender).IsPlaced) return;

            Orientation orientation = getWallOrientation((WallPartCell)sender);
            (int gameRow, int gameColumn) = TransformCoordinates.GuiToGameWall(((WallPartCell)sender).Row, ((WallPartCell)sender).Column, orientation);
            QuoridorMove newMove = new QuoridorMove(gameRow, gameColumn, true, orientation);

            try
            {
                game.ExecuteMove(newMove);
            }
            catch (InvalidMoveException) 
            { 
                return; 
            }

            iterateGameBoard(guiFrame.UseWallCell, true);
            guiFrame.SetPlayerWallCounter(true, game.GetPlayerWalls(true));
        }

        public void OnRemoveWall(object sender, EventArgs e)
        {
            if (!((WallPartCell)sender).IsActive) return;

            if (((WallPartCell)sender).IsPlaced)
            {
                iterateGameBoard(guiFrame.PlaceWallCell, true);
                return;
            }

            game.UndoMoves(1);

            iterateGameBoard(guiFrame.FreeWallCell, false);
            guiFrame.SetPlayerWallCounter(true, game.GetPlayerWalls(true));
        }

        private void iterateGameBoard(Function<int, int> function, bool hasWallPiece)
        {
            for (int gameRow = game.Dimension - 1; gameRow >= 0; gameRow--)
                for (int gameColumn = 0; gameColumn < game.Dimension; gameColumn++)
                {
                    if (gameRow > 0)
                        if (game.HasWallPiece(gameRow, gameColumn, Orientation.Horizontal) == hasWallPiece)
                        {
                            (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiWall(gameRow, gameColumn, Orientation.Horizontal);
                            function(guiRow, guiColumn);
                        }

                    if (gameColumn < game.Dimension - 1)
                        if (game.HasWallPiece(gameRow, gameColumn, Orientation.Vertical) == hasWallPiece)
                        {
                            (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiWall(gameRow, gameColumn, Orientation.Vertical);
                            function(guiRow, guiColumn);
                        }
                }
        }

        private void renderGameComponents()
        {
            int guiBoardDimension = TransformCoordinates.GameToGuiDimension(game.Dimension);
            guiFrame.RenderBoard(this, guiBoardDimension);
            
            int gameWhitePawnRow = 0, gameWhitePawnColumn = 0, gameBlackPawnRow = 0, gameBlackPawnColumn = 0;
            
            game.GetWhiteCoordinates(ref gameWhitePawnRow, ref gameWhitePawnColumn);
            (int guiWhitePawnRow, int guiWhitePawnColumn) = TransformCoordinates.GameToGuiPlayer(gameWhitePawnRow, gameWhitePawnColumn);
            guiFrame.MovePlayerPawn(true, guiWhitePawnRow, guiWhitePawnColumn);

            game.GetBlackCoordinates(ref gameBlackPawnRow, ref gameBlackPawnColumn);
            (int guiBlackPawnRow, int guiBlackPawnColumn) = TransformCoordinates.GameToGuiPlayer(gameBlackPawnRow, gameBlackPawnColumn);
            guiFrame.MovePlayerPawn(false, guiBlackPawnRow, guiBlackPawnColumn);

            guiFrame.RenderPlayerWallsPanel(true);
            guiFrame.RenderPlayerWallsPanel(false);
            guiFrame.SetPlayerWallCounter(true, game.GetPlayerWalls(true));
            guiFrame.SetPlayerWallCounter(false, game.GetPlayerWalls(false));
        }

        private Orientation getWallOrientation(WallPartCell wallcell)
        {
            return (wallcell.Row % 2 == 1 && wallcell.Column % 2 == 0) ? Orientation.Horizontal : Orientation.Vertical;
        }

        private static class TransformCoordinates
        {
            public static int GameToGuiDimension(int gameDimension)
            {
                return 2 * gameDimension - 1;
            }

            public static (int, int) GuiToGamePlayer(int guiRow, int guiColumn)
            {
                return (guiRow / 2, guiColumn / 2);
            }

            public static (int, int) GameToGuiPlayer(int gameRow, int gameColumn)
            {
                return (2 * gameRow, 2 * gameColumn);
            }

            public static (int, int) GuiToGameWall(int guiRow, int guiColumn, Orientation orientation)
            {
                return (orientation == Orientation.Horizontal) ? ((guiRow + 1) / 2, guiColumn / 2) : (guiRow / 2, (guiColumn - 1) / 2);
            }

            public static (int, int) GameToGuiWall(int gameRow, int gameColumn, Orientation orientation)
            {
                return (orientation == Orientation.Horizontal) ? (2 * gameRow - 1, 2 * gameColumn) : (2 * gameRow, 2 * gameColumn + 1);
            }
        }
    }
}
