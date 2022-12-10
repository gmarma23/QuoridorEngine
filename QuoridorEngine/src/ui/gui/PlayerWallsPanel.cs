using QuoridorEngine.UI;

namespace QuoridorEngine.src.ui.gui
{
    public class PlayerWallsPanel : Panel
    {
        private Label description;
        private Label availableWallsCounter;

        public double PanelFrameWidthRatio { get; set; }
        public int PanelBoardMargin { get; set; }
        public int DescriptionCounterMargin { get; set; }

        public PlayerWallsPanel(GuiFrame guiFrame, bool isWhitePlayer)
        {
            int playerSide = isWhitePlayer ? 1 : -1;
            PanelFrameWidthRatio = GuiFrame.boardFrameRatio;
            PanelBoardMargin = 40;
            DescriptionCounterMargin = 6;

            Width = (int)(guiFrame.ClientRectangle.Width * PanelFrameWidthRatio);
            Height = 25;
            BackColor = Color.Transparent;

            Location = new Point(
                (guiFrame.ClientRectangle.Width / 2) - (Width / 2),
                (int)(guiFrame.ClientRectangle.Width / 2 + playerSide * guiFrame.ClientRectangle.Width * GuiFrame.boardFrameRatio / 2) + 
                playerSide * PanelBoardMargin - (isWhitePlayer ? Height : 0));

            description= new Label();
            Controls.Add(description);
            description.BringToFront();
            description.Font = new Font("Arial", 12.0f, FontStyle.Bold);
            description.ForeColor = Color.White;
            description.AutoSize = true;
            description.Text = "Available walls:";

            availableWallsCounter = new Label();
            Controls.Add(availableWallsCounter);
            availableWallsCounter.BringToFront();
            availableWallsCounter.Font = new Font("Arial", 12.0f);
            availableWallsCounter.ForeColor = Color.White;
            availableWallsCounter.AutoSize = true;
            availableWallsCounter.Left = description.Width + DescriptionCounterMargin;
        }

        public void SetWallNum(int numOfWalls)
        {
            availableWallsCounter.Text = numOfWalls.ToString();
        }
    }
}
