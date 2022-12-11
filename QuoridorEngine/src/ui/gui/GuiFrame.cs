using QuoridorEngine.src.ui.gui;
using QuoridorEngine.src.ui.gui.board;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace QuoridorEngine.UI
{
#if !CONSOLE 
    public partial class GuiFrame : Form
    {
        public const double boardFrameRatio = 0.75;

        private Board board;
        private PlayerWallsPanel whitePlayerWalls;
        private PlayerWallsPanel blackPlayerWalls;

        private Color BackgroundTopColor { get; set; }
        private Color BackgroundBottomColor { get; set; }
        private float BackgroundAngle { get; set; }

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

        public void SetPlayerWallCounter(bool isWhitePlayer, int numOfWalls) => getPlayerWallsPanel(isWhitePlayer).SetWallNum(numOfWalls);

        /// <summary>
        /// Initialize and include board panel to frame
        /// </summary>
        public void RenderBoard(GuiClient guiClient, int boardDimension)
        {
            board = new Board(this, guiClient, boardDimension);
            Controls.Add(board);
            board.BringToFront();
        }

        public void RenderPlayerWallsPanel(bool isWhitePlayer)
        {
            ref PlayerWallsPanel playerWallsPanel = ref getPlayerWallsPanel(isWhitePlayer);
            playerWallsPanel = new PlayerWallsPanel(this, isWhitePlayer);
            Controls.Add(playerWallsPanel);
            playerWallsPanel.BringToFront();
        }

        /// <summary>
        /// Overriding OnPaint method for frame to create gradient background effect
        /// </summary>
        /// <param name="e">Paint event Arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = new(ClientRectangle, BackgroundTopColor, BackgroundBottomColor, BackgroundAngle);
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(brush, ClientRectangle);
            base.OnPaint(e);
        }

        private ref PlayerWallsPanel getPlayerWallsPanel(bool isWhitePlayer)
        {
            return ref isWhitePlayer ? ref whitePlayerWalls : ref blackPlayerWalls;
        }

        private void defaultStyle()
        {
            // Set form's gradient background properties
            BackgroundTopColor = Color.FromArgb(255, 0, 0, 10);
            BackgroundBottomColor = Color.FromArgb(255, 0, 38, 80);
            BackgroundAngle = 45;
        }
    }
#endif
}