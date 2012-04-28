using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Competition.Contests
{
    /// <summary>
    /// This class represents a table in which Teams in a contest are
    /// displayed.
    /// </summary>
    public partial class TeamDisplayTable : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Populates the Team table with the team names that are given.
        /// </summary>
        /// <param name="teams">The teams to display.</param>
        /// <param name="backColors">The backcolors to use for rows.</param>
        /// <param name="textColors">The text colors used for rows.</param>
        public void PopulateTeamTable(List<Team> teams, Color[] backColors, Color[] textColors)
        {
            int colorIndex = 0;
            int textIndex = 0;
            int idIndex = 0;

            foreach (Team team in teams)
            {
                TeamTable.Rows.Add(MakeRowForTable(team.Name, backColors[colorIndex], textColors[textIndex]));

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

                idIndex++;
            }
        }

        /// <summary>
        /// Creates a row for the team table.
        /// </summary>
        /// <param name="teamName">The name of team to display.</param>
        /// <param name="backColor">The backcolor of the row.</param>
        /// <param name="textColor">The text color of the row.</param>
        /// <returns>A table row for the team table.</returns>
        private TableRow MakeRowForTable(string teamName, Color backColor, Color textColor)
        {
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeTextCellForRow(teamName, textColor));
            return newRow;
        }

        /// <summary>
        /// Creates a new cell for a table with the name of the team.
        /// </summary>
        /// <param name="text">The team name to display.</param>
        /// <param name="textColor">The color of the text for this cell.</param>
        /// <returns>A table cell for the table.</returns>
        private TableCell MakeTextCellForRow(string name, Color textColor)
        {
            TableCell newCell = new TableCell();
            Label textLink = new Label();
            textLink.Text = name;
            textLink.ForeColor = textColor;
            newCell.Controls.Add(textLink);
            return newCell;
        }
    }
}