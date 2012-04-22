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
        public void CreateRowDisplay(Color backColor, Color textColor)
        {
            _position.ForeColor = textColor;
            _teamName.ForeColor = textColor;
            _currentScore.ForeColor = textColor;
            _row.BackColor = backColor;
            hideRowText();
        }

        public void setRowText(int position, string team, float score, string format)
        {
            _position.Text = position + ".";
            _teamName.Text = team;
            _currentScore.Text = score.ToString(format);
        }

        public void hideRowText()
        {
            _position.Visible = false;
            _teamName.Visible = false;
            _currentScore.Visible = false;
        }

        public void displayRowText()
        {
            _position.Visible = true;
            _teamName.Visible = true;
            _currentScore.Visible = true;
        }
    }
}