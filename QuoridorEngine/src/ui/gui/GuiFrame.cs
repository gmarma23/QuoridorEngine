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
            defaultStyle();
        }

        public void MovePlayerPawn(bool isWhitePlayer, int row, int column) => board.MovePlayerPawn(isWhitePlayer, row, column);

        public void NormalPlayerCell(int row, int column) => board.NormalPlayerCell(row, column);

        public void PossibleMovePlayerCell(int row, int column) => board.PossibleMovePlayerCell(row, column);

        public void UseWallCell(int row, int column) => board.UseWallCell(row, column);

        public void FreeWallCell(int row, int column) => board.FreeWallCell(row, column);

        public void PlaceWallCell(int row, int column) => board.PlaceWallCell(row, column, true);

        public void RemoveWallCell(int row, int column) => board.PlaceWallCell(row, column, false);

        public void SetPlayerWallCounter(bool isWhitePlayer, int numOfWalls) => getPlayerWallsCounter(isWhitePlayer).SetWallNum(numOfWalls);

        public void BoardEventsSubscribe() => board.AddEventHandlers();

        public void BoardEventsUnsubscribe() => board.RemoveEventHandlers();


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
            ref GuiPlayerWallsCounter playerWallsCounter = ref getPlayerWallsCounter(isWhitePlayer);
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

        private ref GuiPlayerWallsCounter getPlayerWallsCounter(bool isWhitePlayer)
        {
            return ref isWhitePlayer ? ref whitePlayerWallsCounter : ref blackPlayerWallsCounter;
        }

        private void defaultStyle()
        {
            // Set form's gradient background properties
            backgroundTopColor = Color.FromArgb(255, 0, 0, 10);
            backgroundBottomColor = Color.FromArgb(255, 0, 38, 80);
            backgroundAngle = 45;
        }
    }
#endif
}