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
            if (bracket == 0)
            {
                _row.BackImageUrl = "~/Images/Competition/Leaderboard/Diamond.png";
            }
            else if (bracket == 1)
            {
                _row.BackImageUrl = "~/Images/Competition/Leaderboard/Platinum.png";
            }
            else if (bracket == 2)
            {
                _row.BackImageUrl = "~/Images/Competition/Leaderboard/Gold.png";
            }
            else if (bracket == 3)
            {
                _row.BackImageUrl = "~/Images/Competition/Leaderboard/Silver.png";
            }
            else 
            {
                _row.BackImageUrl = "~/Images/Competition/Leaderboard/Bronze.png";
            }

            _teamName.ForeColor = textColor;
            _currentScore.ForeColor = textColor;
            hideRowText();
        }

        public void setRowText(string team, float score, string format)
        {
            _teamName.Text = team;
            _currentScore.Text = score.ToString(format);
        }

        public void hideRowText()
        {
            _teamName.Visible = false;
            _currentScore.Visible = false;
        }

        public void displayRowText()
        {
            _teamName.Visible = true;
            _currentScore.Visible = true;
        }
    }
}