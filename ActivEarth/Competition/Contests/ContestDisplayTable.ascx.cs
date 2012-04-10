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

        public void PopulateContestTable(List<Contest> contests, Color[] backColors, Color[] textColors)
        {
            int colorIndex = 0;
            int textIndex = 0;

            foreach (Contest contest in contests)
            {
                _contestTable.Rows.Add(MakeRowForTable(contest, backColors[colorIndex], textColors[textIndex]));

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

        private TableRow MakeRowForTable(Contest contest, Color backColor, Color textColor)
        {
            string typeString = "";
            if (contest.Type == ContestType.Group)
            {
                typeString = "Group";
            }
            else
            {
                typeString = "Individual";
            }

            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
           // newRow.ForeColor = Color.Black;
            newRow.Cells.Add(MakeTextCellForRow(contest.Name, textColor));
            newRow.Cells.Add(MakeTextCellForRow(typeString, textColor));
            return newRow;
        }

        private TableCell MakeTextCellForRow(string text, Color textColor)
        {
            TableCell newCell = new TableCell();
            Label textLabel = new Label();
            textLabel.Text = text;
            textLabel.ForeColor = textColor;
            newCell.Controls.Add(textLabel);
            return newCell;
        }   
    }
}