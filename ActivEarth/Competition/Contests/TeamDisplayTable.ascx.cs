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
    public partial class TeamDisplayTable : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

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

        private TableRow MakeRowForTable(string contestName, Color backColor, Color textColor)
        {
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeTextCellForRow(contestName, textColor));
            return newRow;
        }

        private TableCell MakeTextCellForRow(string text, Color textColor)
        {
            TableCell newCell = new TableCell();
            Label textLink = new Label();
            textLink.Text = text;
            textLink.ForeColor = textColor;
            newCell.Controls.Add(textLink);
            return newCell;
        }
    }
}