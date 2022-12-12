using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.Utls
{
    // Group of methods for coordinate transformations to ensure proper game/gui communication
    public static class TransformCoordinates
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
