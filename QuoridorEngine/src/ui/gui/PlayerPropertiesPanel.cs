using QuoridorEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuoridorEngine.src.ui.gui
{
    public class PlayerPropertiesPanel : Panel
    {
        private Label wins;
        private Label winsCounter;
        private Label availableWalls;
        private Label availableWallsCounter;

        public double PanelFrameWidthRatio { get; set; }
        public double PanelFrameYLocationRatio { get; set; }

        public PlayerPropertiesPanel(GuiFrame guiFrame, double panelFrameYLocRatio)
        {
            PanelFrameWidthRatio = 0.75;
            PanelFrameYLocationRatio = panelFrameYLocRatio;
            Width = (int)(guiFrame.ClientRectangle.Width * PanelFrameWidthRatio);
            Height = 50;
            BackColor = Color.Transparent;
            Location = new Point(
                (guiFrame.ClientRectangle.Width / 2) - (Width / 2),
                (int)(guiFrame.ClientRectangle.Height * PanelFrameYLocationRatio));

            setAvailableWalls();
        }

        private void setAvailableWalls()
        {
            availableWalls = new Label();
            Controls.Add(availableWalls);
            availableWalls.BringToFront();
            availableWalls.Text = "Available walls:";
            availableWalls.Font = new Font("Arial", 16.0f, FontStyle.Bold);
            availableWalls.ForeColor = Color.White;
            availableWalls.AutoSize = true;
        }
    }
}
