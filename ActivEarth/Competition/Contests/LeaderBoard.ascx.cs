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
    /// <summary>
    /// This class represents the leaderboard of a contest which
    /// displays bracket information to the user.
    /// </summary>
    public partial class LeaderBoard : System.Web.UI.UserControl
    {
        /// <summary>
        /// Fills the leaderboard with the teams that are passed in.
        /// </summary>
        /// <param name="teams">The teams corressponding to the brackets realative to the user.</param>
        /// <param name="backColors">The backcolors the leaderboard should use.</param>
        /// <param name="textColors">The text colors the leaderboard should use.</param>
        /// <param name="scoreFormat">The format of each teams score.</param>
        /// <param name="rewards">The reward levels of each bracket.</param>
        public void PopulateLeaderBoard(List<Team> teams, Color[] backColors, Color[] textColors, string scoreFormat, List<int> rewards)
        {
            int colorIndex = 0;
            int textIndex = 0;

            foreach (Team team in teams)
            {
                if (team != null)
                {
                    LeaderBoardTable.Rows.Add(MakeRowForTable(team, backColors[colorIndex], textColors[textIndex], scoreFormat, rewards));

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

        /// <summary>
        /// Creates a new row for the leaderboard table.
        /// </summary>
        /// <param name="team">The team corressponding to the row.</param>
        /// <param name="backColor">The backcolor of the row.</param>
        /// <param name="textColor">The text color of the row.</param>
        /// <param name="scoreFormat">The score format for the row.</param>
        /// <param name="rewards">The rewards of each bracket.</param>
        /// <returns>A new row for the leaderboard table.</returns>
        private TableRow MakeRowForTable(Team team, Color backColor, Color textColor, string scoreFormat, List<int> rewards)
        {
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.ForeColor = textColor;
            newRow.Cells.Add(MakeBracketCell(team));
            newRow.Cells.Add(MakeTeamCell(team));
            newRow.Cells.Add(MakeScoreCell(team, scoreFormat));
            newRow.Cells.Add(MakeRewardCell(team, rewards));
            return newRow;
        }

        /// <summary>
        /// Makes a bracket cell for a row.
        /// </summary>
        /// <param name="team">The team corressponding to the row the cell is being
        /// placed in.</param>
        /// <returns>The Bracket cell for the row.</returns>
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

            bracketCell.Controls.Add(bracketImage);;

            return bracketCell;
        }

        /// <summary>
        /// Makes a Team cell for the row.
        /// </summary>
        /// <param name="team">The name of the team to display.</param>
        /// <returns>A Table Cell containing the name of the team.</returns>
        private TableCell MakeTeamCell(Team team)
        {
            TableCell teamCell = new TableCell();

            Label teamNameLabel = new Label();
            teamNameLabel.Text = team.Name;

            teamCell.Controls.Add(teamNameLabel);

            return teamCell;
        }

        /// <summary>
        /// Makes a Table Cell containing the team's score.
        /// </summary>
        /// <param name="team">The team to get the score from.</param>
        /// <param name="scoreFormat">The format of the score.</param>
        /// <returns>A Table Cell containing the score.</returns>
        private TableCell MakeScoreCell(Team team, string scoreFormat)
        {
            TableCell scoreCell = new TableCell();
            scoreCell.HorizontalAlign = HorizontalAlign.Right;

            Label scoreLabel = new Label();
            scoreLabel.Text = team.Score.ToString(scoreFormat);

            scoreCell.Controls.Add(scoreLabel);

            return scoreCell;
        }

        /// <summary>
        /// Makes a table cell containing the reward the team
        /// will receive.
        /// </summary>
        /// <param name="team">The Team to retrieve the bracket from.</param>
        /// <param name="rewards">The rewards for each bracket.</param>
        /// <returns>A Table Cell containing the reward.</returns>
        private TableCell MakeRewardCell(Team team, List<int> rewards)
        {
            TableCell rewardCell = new TableCell();
            rewardCell.HorizontalAlign = HorizontalAlign.Right;
            
            Label rewardLabel = new Label();
            rewardLabel.Style.Add("margin-right", "5px");

            if (team.Bracket == (int)ContestBracket.Bronze)
            {
                rewardLabel.Text = rewards[0].ToString();
            }
            else if (team.Bracket == (int)ContestBracket.Silver)
            {
                rewardLabel.Text = rewards[1].ToString();
            }
            else if (team.Bracket == (int)ContestBracket.Gold)
            {
                rewardLabel.Text = rewards[2].ToString();
            }
            else if (team.Bracket == (int)ContestBracket.Platinum)
            {
                rewardLabel.Text = rewards[3].ToString();
            }
            else
            {
                rewardLabel.Text = rewards[4].ToString();
            }

            System.Web.UI.WebControls.Image activityScoreImage = new System.Web.UI.WebControls.Image();
            activityScoreImage.Width = new Unit("20px");
            activityScoreImage.Height = new Unit("20px");
            activityScoreImage.ImageAlign = ImageAlign.Middle;
            activityScoreImage.ImageUrl = "~/Images/Competition/Activity_Score.png";

            rewardCell.Controls.Add(rewardLabel);
            rewardCell.Controls.Add(activityScoreImage);

            return rewardCell;
        }
    }
}