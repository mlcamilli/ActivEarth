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

        public void MakeLeaderBoard(List<Team> teams, Color[] backColors, Color[] textColors, string format)
        {
            _numSlots = 0;
            _contestTeams = teams;
            _rows = new List<LeaderBoardRow>();

            int colorIndex = 0;
            int textIndex = 0;

            foreach (Team team in teams)
            {
                if (team != null)
                {
                    AddRowToLeaderBoard(backColors[colorIndex], textColors[textIndex], _numSlots);

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

                    _numSlots++;
                }
            }

            PopulateLeaderBoard(format);
        }

        private void AddRowToLeaderBoard(Color backColor, Color textColor, int i)
        {
            LeaderBoardRow leaderBoardRow = (LeaderBoardRow)LoadControl("LeaderBoardRow.ascx");
            leaderBoardRow.CreateRowDisplay(backColor, textColor, i);
            _rows.Add(leaderBoardRow);
            _displayLeaderBoardRows.Controls.Add(leaderBoardRow);
        }

        private void PopulateLeaderBoard(string format)
        {
            for (int i = 0; i < _numSlots; i++)
            {
                LeaderBoardRow leaderBoardRow = _rows[i];

                if (i + _displayIndex < _contestTeams.Count)
                {
                    int position = i + _displayIndex;
                    leaderBoardRow.setRowText(_contestTeams[position].Name, _contestTeams[position].Score, format);
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