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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void PopulateLeaderBoard(List<Team> teams, Color[] backColors, Color[] textColors, string scoreFormat)
        {
            int colorIndex = 0;
            int textIndex = 0;

            foreach (Team team in teams)
            {
                if (team != null)
                {
                    LeaderBoardTable.Rows.Add(MakeRowForTable(team, backColors[colorIndex], textColors[textIndex], scoreFormat));

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
            }
        }

        private TableRow MakeRowForTable(Team team, Color backColor, Color textColor, string scoreFormat)
        {
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.ForeColor = textColor;
            newRow.Cells.Add(MakeBracketCell(team));
            newRow.Cells.Add(MakeTeamCell(team));
            newRow.Cells.Add(MakeScoreCell(team, scoreFormat));
            return newRow;
        }

        private TableCell MakeBracketCell(Team team)
        {
            TableCell bracketCell = new TableCell();
            bracketCell.HorizontalAlign = HorizontalAlign.Center;

            System.Web.UI.WebControls.Image bracketImage = new System.Web.UI.WebControls.Image();
            bracketImage.Width = new Unit("25px");
            bracketImage.Height = new Unit("25px");
            bracketImage.ImageAlign = ImageAlign.Middle;

            if (team.Bracket == (int)ContestBracket.Bronze)
            {
                bracketImage.ImageUrl = "~/Images/Competition/Contests/BronzeBracket.png";
            }
            else if (team.Bracket == (int)ContestBracket.Silver)
            {
                bracketImage.ImageUrl = "~/Images/Competition/Contests/SilverBracket.png";
            }
            else if (team.Bracket == (int)ContestBracket.Gold)
            {
                bracketImage.ImageUrl = "~/Images/Competition/Contests/GoldBracket.png";
            }
            else if (team.Bracket == (int)ContestBracket.Platinum)
            {
                bracketImage.ImageUrl = "~/Images/Competition/Contests/PlatinumBracket.png";
            }
            else
            {
                bracketImage.ImageUrl = "~/Images/Competition/Contests/DiamondBracket.png";
            }

            bracketCell.Controls.Add(bracketImage);

            return bracketCell;
        }

        private TableCell MakeTeamCell(Team team)
        {
            TableCell teamCell = new TableCell();

            Label teamNameLabel = new Label();
            teamNameLabel.Text = team.Name;

            teamCell.Controls.Add(teamNameLabel);

            return teamCell;
        }

        private TableCell MakeScoreCell(Team team, string scoreFormat)
        {
            TableCell scoreCell = new TableCell();
            scoreCell.HorizontalAlign = HorizontalAlign.Right;

            Label scoreLabel = new Label();
            scoreLabel.Text = team.Score.ToString(scoreFormat);

            scoreCell.Controls.Add(scoreLabel);

            return scoreCell;
        }
    }
}