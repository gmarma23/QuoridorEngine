using QuoridorEngine.Core;
using QuoridorEngine.UI;

using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.src.ui.gui
{
    public class GuiClient
    {
        private GuiFrame guiFrame;
        private QuoridorGame game;

        public GuiClient() 
        {
            game = new QuoridorGame(9);
            guiFrame = new GuiFrame();

            renderGameGuiComponents();
            Application.Run(guiFrame);
        }

        private void renderGameGuiComponents()
        {
            int gameWhitePawnRow = 0, gameWhitePawnColumn = 0, gameBlackPawnRow = 0, gameBlackPawnColumn = 0;
            game.GetWhiteCoordinates(ref gameWhitePawnRow, ref gameWhitePawnColumn);
            game.GetBlackCoordinates(ref gameBlackPawnRow, ref gameBlackPawnColumn);

            (int guiWhitePawnRow, int guiWhitePawnColumn) = TransformCoordinates.GameToGuiPlayer(gameWhitePawnRow, gameWhitePawnColumn);
            (int guiBlackPawnRow, int guiBlackPawnColumn) = TransformCoordinates.GameToGuiPlayer(gameBlackPawnRow, gameBlackPawnColumn);

            guiFrame.RenderBoard(
                TransformCoordinates.GameToGuiDimension(game.Dimension),
                guiWhitePawnRow, guiWhitePawnColumn,
                guiBlackPawnRow, guiBlackPawnColumn);
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
                return (orientation == Orientation.Horizontal) ? (2 * (gameRow - 1), 2 * gameColumn) : (2 * gameRow, 2 * (gameColumn - 1));
            }
        }
    }
}
