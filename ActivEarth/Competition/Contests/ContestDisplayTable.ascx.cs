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
    public partial class ContestDisplayTable : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void PopulateContestTable(List<string> contests, List<int> ids, Color[] backColors, Color[] textColors)
        {
            int colorIndex = 0;
            int textIndex = 0;
            int idIndex = 0;

            foreach (string contest in contests)
            {
                _contestTable.Rows.Add(MakeRowForTable(contest, ids[idIndex], backColors[colorIndex], textColors[textIndex]));

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

        private TableRow MakeRowForTable(string contestName, int contestId, Color backColor, Color textColor)
        {
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeTextCellForRow(contestName, contestId, textColor));
            return newRow;
        }

        private TableCell MakeTextCellForRow(string text, int contestId, Color textColor)
        {
            TableCell newCell = new TableCell();
            HyperLink textLink = new HyperLink();
            textLink.Text = text;
            textLink.ForeColor = textColor;
            textLink.NavigateUrl = "~/Competition/Contests/DisplayContestPage.aspx?id=" + contestId;
            newCell.Controls.Add(textLink);
            return newCell;
        }   
    }
}