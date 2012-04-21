using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Competition.Contests
{
    public partial class LeaderBoard : System.Web.UI.UserControl
    {
        private int _numSlots;
        private int _displayIndex;
        private List<Team> _contestTeams;
        private List<LeaderBoardRow> _rows;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void MakeLeaderBoard(int slots, List<Team> teams, Color[] backColors, Color[] textColors)
        {
            _numSlots = slots;
            _contestTeams = teams;
            _rows = new List<LeaderBoardRow>(slots);

            int colorIndex = 0;
            int textIndex = 0;

            for (int i = 0; i < slots; i++)
            {
                AddRowToLeaderBoard(backColors[colorIndex], textColors[textIndex]);
                
                colorIndex++;
                if (colorIndex == backColors.Length)
                {
                    colorIndex = 0;
                }

                textIndex++;
                if (textIndex == textColors.Length)
                {
                    textIndex = 0;
                }
            }

            populateLeaderBoard();
        }

        private void AddRowToLeaderBoard(Color backColor, Color textColor)
        {
            LeaderBoardRow leaderBoardRow = (LeaderBoardRow)LoadControl("LeaderBoardRow.ascx");
            leaderBoardRow.CreateRowDisplay(backColor, textColor);
            _rows.Add(leaderBoardRow);
            _displayLeaderBoardRows.Controls.Add(leaderBoardRow);
        }

        private void populateLeaderBoard()
        {
            for (int i = 0; i < _numSlots; i++)
            {
                LeaderBoardRow leaderBoardRow = _rows[i];

                if (i + _displayIndex < _contestTeams.Count)
                {
                    int position = i + _displayIndex;
                    leaderBoardRow.setRowText(position + 1, _contestTeams[position].Name, _contestTeams[position].Score.ToString());
                    leaderBoardRow.displayRowText();
                }
                else
                {
                    leaderBoardRow.hideRowText();
                }
            }
        }
    }
}