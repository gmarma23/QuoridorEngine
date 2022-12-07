using QuoridorEngine.Core;
using QuoridorEngine.src.ui.gui.board;
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
            game = new QuoridorGame();
            guiFrame = new GuiFrame();

            initializeGameComponents();
        }

        public void Play()
        {
            Application.Run(guiFrame);
        }

        public void OnPlaceWall(object sender, EventArgs e)
        {
            ((WallPartCell)sender).UsedStyle();
        }

        private void initializeGameComponents()
        {
            int gameWhitePawnRow = 0, gameWhitePawnColumn = 0, gameBlackPawnRow = 0, gameBlackPawnColumn = 0;
            game.GetWhiteCoordinates(ref gameWhitePawnRow, ref gameWhitePawnColumn);
            game.GetBlackCoordinates(ref gameBlackPawnRow, ref gameBlackPawnColumn);

            (int guiWhitePawnRow, int guiWhitePawnColumn) = TransformCoordinates.GameToGuiPlayer(gameWhitePawnRow, gameWhitePawnColumn);
            (int guiBlackPawnRow, int guiBlackPawnColumn) = TransformCoordinates.GameToGuiPlayer(gameBlackPawnRow, gameBlackPawnColumn);

            guiFrame.RenderBoard(
                this,
                TransformCoordinates.GameToGuiDimension(game.Dimension),
                guiWhitePawnRow, guiWhitePawnColumn,
                guiBlackPawnRow, guiBlackPawnColumn);

            guiFrame.RenderPlayerWallPanels();
            guiFrame.SetWhitePlayerWallCounter(game.GetPlayerWalls(true));
            guiFrame.SetBlackPlayerWallCounter(game.GetPlayerWalls(false));
        }

        private Orientation? determineWallOrientation(int guiRow, int guiColumn)
        {
            if (guiRow % 2 == 1 && guiColumn % 2 == 0)
                return Orientation.Horizontal;
            else if (guiRow % 2 == 0 && guiColumn % 2 == 1)
                return Orientation.Vertical;
            else
                return null;
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
