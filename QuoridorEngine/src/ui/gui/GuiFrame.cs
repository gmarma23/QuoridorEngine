using QuoridorEngine.Core;
using System.Drawing.Drawing2D;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public partial class GuiFrame : Form
    {
        public const double boardFrameRatio = 0.75;

        private GuiBoard board;
        private GuiPlayerWallsCounter whitePlayerWallsCounter;
        private GuiPlayerWallsCounter blackPlayerWallsCounter;

        private Color backgroundTopColor;
        private Color backgroundBottomColor;
        private float backgroundAngle;

        public GuiFrame()
        {
            InitializeComponent();
            applyDefaultStyle();
        }

        public void MovePlayerPawn(QuoridorGame gameState, bool isWhitePlayer) => board.MovePlayerPawn(gameState, isWhitePlayer);

        public void HidePossiblePlayerMoves(QuoridorGame gameState) => board.HidePossiblePlayerMoveCells(gameState);

        public void ShowPossiblePlayerMoves(QuoridorGame gameState, bool isWhitePlayer) => board.ShowPossiblePlayerMoveCells(gameState, isWhitePlayer);

        public void UpdateUsedBoardWallCells(QuoridorGame gameState) => board.UpdateUsedWallCells(gameState);

        public void UpdateFreeBoardWallCells(QuoridorGame gameState) => board.UpdateFreeWallCells(gameState);

        public void UpdatePlacedBoardWallCells(QuoridorGame gameState) => board.UpdatePlacedWallCells(gameState);

        public void SetPlayerWallCounter(QuoridorGame gameState, bool isWhitePlayer)
        {
            int numOfWalls = gameState.GetPlayerWalls(isWhitePlayer);
            getPlayerWallsCounter(isWhitePlayer).SetWallNum(numOfWalls);
        }

        /// <summary>
        /// Initialize and include board panel to frame
        /// </summary>
        public void RenderBoard(int boardDimension, Dictionary<string, EventHandler> boardEventHandlers)
        {
            board = new GuiBoard(ClientRectangle.Width, ClientRectangle.Height, boardDimension, boardEventHandlers);
            Controls.Add(board);
        }

        public void RenderPlayerWallsPanel(bool isWhitePlayer)
        {
            ref GuiPlayerWallsCounter playerWallsCounter = ref getPlayerWallsCounterRef(isWhitePlayer);
            playerWallsCounter = new GuiPlayerWallsCounter(this, isWhitePlayer);
            Controls.Add(playerWallsCounter);
        }

        /// <summary>
        /// Overriding OnPaint method for frame to create gradient background effect
        /// </summary>
        /// <param name="e">Paint event arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = new(ClientRectangle, backgroundTopColor, backgroundBottomColor, backgroundAngle);
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(brush, ClientRectangle);
            base.OnPaint(e);
        }

        private GuiPlayerWallsCounter getPlayerWallsCounter(bool isWhitePlayer)
        {
            return isWhitePlayer ? whitePlayerWallsCounter : blackPlayerWallsCounter;
        }

        private ref GuiPlayerWallsCounter getPlayerWallsCounterRef(bool isWhitePlayer)
        {
            return ref isWhitePlayer ? ref whitePlayerWallsCounter : ref blackPlayerWallsCounter;
        }

        private void applyDefaultStyle()
        {
            // Set form's gradient background properties
            backgroundTopColor = Color.FromArgb(255, 0, 0, 10);
            backgroundBottomColor = Color.FromArgb(255, 0, 38, 80);
            backgroundAngle = 45;
        }
    }
#endif
}