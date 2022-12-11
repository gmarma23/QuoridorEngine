using QuoridorEngine.Core;
using QuoridorEngine.src.ui.gui.board;
using QuoridorEngine.UI;
using QuoridorEngine.Utils;
using Orientation = QuoridorEngine.Core.Orientation;

namespace QuoridorEngine.src.ui.gui
{
    public class GuiClient
    {
        public delegate void BoardCellAction(int row, int column);

        private GuiFrame guiFrame;
        private QuoridorGame game;

        private bool IsWhitePlayerTurn { get; set; } 
        private bool InitPlayerMove { get; set; }

        public GuiClient() 
        {
            game = new QuoridorGame();
            guiFrame = new GuiFrame();

            renderGameComponents();

            IsWhitePlayerTurn = true;
            InitPlayerMove = false;
        }

        public void Play()
        {
            Application.Run(guiFrame);
        }

        public void PreviewWall(object sender, EventArgs e)
        {
            WallPartCell wallPartCell = (WallPartCell)sender;

            if (wallPartCell.IsPlaced) return;

            Orientation orientation = getWallOrientation(wallPartCell);
            (int gameRow, int gameColumn) = TransformCoordinates.GuiToGameWall(wallPartCell.Row, wallPartCell.Column, orientation);
            QuoridorMove newMove = new QuoridorMove(gameRow, gameColumn, IsWhitePlayerTurn, orientation);

            try
            {
                game.ExecuteMove(newMove);
            }
            catch (InvalidMoveException) 
            { 
                return; 
            }

            refreshWallCells(guiFrame.UseWallCell, true);
            guiFrame.SetPlayerWallCounter(IsWhitePlayerTurn, game.GetPlayerWalls(IsWhitePlayerTurn));
        }

        public void PlaceWall(object sender, EventArgs e)
        {
            refreshWallCells(guiFrame.PlaceWallCell, true);
            if (InitPlayerMove) hidePossiblePlayerMoves();
            switchPlayerTurn();
        }

        public void RemoveWallPreview(object sender, EventArgs e)
        {
            WallPartCell wallPartCell = (WallPartCell)sender;

            if (!wallPartCell.IsActive) return;

            if (wallPartCell.IsPlaced) return;

            game.UndoMoves(1);

            refreshWallCells(guiFrame.FreeWallCell, false);
            guiFrame.SetPlayerWallCounter(IsWhitePlayerTurn, game.GetPlayerWalls(IsWhitePlayerTurn));
        }

        public void MovePlayer(object sender, EventArgs e)
        {

        }

        public void PlayerPawnClicked(object sender, EventArgs e)
        {
            if (InitPlayerMove)
                hidePossiblePlayerMoves();
            else
                showPossiblePlayerMoves(sender, e);
        }

        private void showPossiblePlayerMoves(object sender, EventArgs e)
        {
            bool isWhitePlayer = ((PlayerPawn)sender).IsWhite;
            List<QuoridorMove> possiblePlayerMoves = (List<QuoridorMove>)game.GetPossiblePlayerMoves(isWhitePlayer);

            foreach (QuoridorMove move in possiblePlayerMoves)
            {
                (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiPlayer(move.Row, move.Column);
                guiFrame.PossibleMovePlayerCell(guiRow, guiColumn);
            }

            InitPlayerMove = true;
        }

        private void hidePossiblePlayerMoves()
        {
            for (int gameRow = game.Dimension - 1; gameRow >= 0; gameRow--)
                for (int gameColumn = 0; gameColumn < game.Dimension; gameColumn++)
                {
                    (int guiRow, int guiColumn) = TransformCoordinates.GameToGuiPlayer(gameRow, gameColumn);
                    guiFrame.NormalPlayerCell(guiRow, guiColumn);
                }

            InitPlayerMove = false;
        }

        private void refreshWallCells(BoardCellAction function, bool hasWallPiece)
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

        private void switchPlayerTurn()
        {
            IsWhitePlayerTurn = !IsWhitePlayerTurn;
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

        private Orientation getWallOrientation(WallPartCell wallcell)
        {
            return (wallcell.Row % 2 == 1 && wallcell.Column % 2 == 0) ? Orientation.Horizontal : Orientation.Vertical;
        }
    }
}
