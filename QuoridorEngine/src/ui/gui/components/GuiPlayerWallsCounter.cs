using QuoridorEngine.UI;
using System.Diagnostics;

namespace QuoridorEngine.UI
{
#if !CONSOLE
    public class GuiPlayerWallsCounter : Panel
    {
        private const int panelBoardMargin = 40;
        private const int descriptionCounterMargin = 48;
        private const string descriptionTxt = "Available Walls:";

        private Label description;
        private Label availableWallsCounter;

        public GuiPlayerWallsCounter(GuiFrame guiFrame, bool isWhitePlayer)
        {
            initializeLabels();
            setSizesAndArrangement(guiFrame.ClientRectangle.Width, isWhitePlayer);

            applyDefaultStyle();
        }

        public void SetWallNum(int numOfWalls)
        {
            availableWallsCounter.Text = numOfWalls.ToString();
        }

        private void initializeLabels()
        {
            description = new Label();
            Controls.Add(description);
            description.Text = descriptionTxt;

            availableWallsCounter = new Label();
            Controls.Add(availableWallsCounter);
        }

        private void setSizesAndArrangement(int parrentWidth, bool isWhitePlayer)
        {
            int playerSide = isWhitePlayer ? 1 : -1;
            Height = 25;
            Width = (int)(parrentWidth * GuiFrame.boardFrameRatio);

            int xLoc = (parrentWidth / 2) - (Width / 2);
            int yLoc = (int)(parrentWidth / 2 + playerSide * parrentWidth * GuiFrame.boardFrameRatio / 2) +
                             playerSide * panelBoardMargin - (isWhitePlayer ? Height : 0);
            
            Location = new Point(xLoc, yLoc);

            description.AutoSize = true;
            availableWallsCounter.AutoSize = true;

            availableWallsCounter.Left = description.Width + descriptionCounterMargin;
        }

        private void applyDefaultStyle()
        {
            BackColor = Color.Transparent;

            Color fontColor  = Color.White;
            float fontSize = 12.0f;
            string font = "Arial";

            description.Font = new Font(font, fontSize, FontStyle.Bold);
            description.ForeColor = fontColor;

            availableWallsCounter.Font = new Font(font, fontSize);
            availableWallsCounter.ForeColor = fontColor;
        }
    }
#endif
}
