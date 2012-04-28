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
    /// This class represents a table that is used to display contest names
    /// as hyperlinks to contest pages.
    /// </summary>
    public partial class ContestDisplayTable : System.Web.UI.UserControl
    {
        /// <summary>
        /// Loads the page.
        /// </summary>
        /// <param name="sender">Object that requested the page load.</param>
        /// <param name="e">The event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Adds a list of contests to the table.
        /// </summary>
        /// <param name="contests">The contest names to display.</param>
        /// <param name="ids">The contest ids of the contests.</param>
        /// <param name="backColors">The colors used for the background of rows in the table.</param>
        /// <param name="textColors">The color of the text used in the table.</param>
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

        /// <summary>
        /// Creates a new row for the table and poulates it.
        /// </summary>
        /// <param name="contestName">The name of the contest</param>
        /// <param name="contestId">The contest id.</param>
        /// <param name="backColor">The backcolor of the row.</param>
        /// <param name="textColor">The text color of the row.</param>
        /// <returns>A table row with the settings applied.</returns>
        private TableRow MakeRowForTable(string contestName, int contestId, Color backColor, Color textColor)
        {
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeTextCellForRow(contestName, contestId, textColor));
            return newRow;
        }

        /// <summary>
        /// Makes a table cell which contains the hyperlink to a contest page.
        /// </summary>
        /// <param name="name">The name of the contest.</param>
        /// <param name="contestId">The contest id.</param>
        /// <param name="textColor">The color of the text in the cell.</param>
        /// <returns>A table cell to be put in the table.</returns>
        private TableCell MakeTextCellForRow(string name, int contestId, Color textColor)
        {
            TableCell newCell = new TableCell();
            HyperLink textLink = new HyperLink();
            textLink.Text = name;
            textLink.ForeColor = textColor;
            textLink.NavigateUrl = "~/Competition/Contests/DisplayContestPage.aspx?id=" + contestId;
            newCell.Controls.Add(textLink);
            return newCell;
        }   
    }
}