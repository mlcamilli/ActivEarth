using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ActivEarth.Competition.Contests
{
    public partial class LeaderBoardRow : System.Web.UI.UserControl
    {
        public void CreateRowDisplay(Color backColor, Color textColor, int bracket)
        {
            //Test
            if (bracket == 0)
            {
                BracketImage.ImageUrl = "~/Images/Competition/Contests/DiamondBracket.png";
            }
            else if (bracket == 1)
            {
                BracketImage.ImageUrl = "~/Images/Competition/Contests/PlatinumBracket.png";
            }
            else if (bracket == 2)
            {
                BracketImage.ImageUrl = "~/Images/Competition/Contests/GoldBracket.png";
            }
            else if (bracket == 3)
            {
                BracketImage.ImageUrl = "~/Images/Competition/Contests/SilverBracket.png";
            }
            else if (bracket == 4)
            {
                BracketImage.ImageUrl = "~/Images/Competition/Contests/BronzeBracket.png";
            }
            else
            {
                BracketImage.ImageUrl = "~/Images/Competition/Contests/NoneBracket.png";
            }

            Row.BackColor = backColor;
            TeamName.ForeColor = textColor;
            CurrentScore.ForeColor = textColor;
            hideRowText();
        }

        public void setRowText(string team, float score, string format)
        {
            TeamName.Text = team;
            CurrentScore.Text = score.ToString(format);
        }

        public void hideRowText()
        {
            TeamName.Visible = false;
            CurrentScore.Visible = false;
        }

        public void displayRowText()
        {
            TeamName.Visible = true;
            CurrentScore.Visible = true;
        }
    }
}