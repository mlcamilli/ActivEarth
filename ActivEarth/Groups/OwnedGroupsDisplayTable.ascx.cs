using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.Account;
using ActivEarth.Groups;
using ActivEarth.Objects.Groups;

namespace ActivEarth.Groups
{
    public partial class OwnedGroupsDisplayTable : System.Web.UI.UserControl
    {
        /// <summary>
        /// This is a component, so no work is required when it loads.  The population of the table
        /// is handled by the page ASP.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Formats the table of Groups based on the given list of Groups and colors.
        /// </summary>
        /// <param name="messages">The list of Groups to display in the table</param>
        /// <param name="backColors">The list of alternating background colors to display in the table</param>
        /// <param name="textColors">The list of alternating text colors to display in the table</param>
        public void PopulateGroupsTable(List<ActivEarth.Objects.Groups.Group> groups, Color[] backColors, Color[] textColors)
        {
            int colorIndex = 0;
            int textIndex = 0;

            foreach (ActivEarth.Objects.Groups.Group group in groups)
            {
                _ownedGroupsTable.Rows.Add(MakeRowForTable(group, backColors[colorIndex], textColors[textIndex]));

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

        /// <summary>
        /// Formats a single row to the table with the given Group's information.
        /// </summary>
        /// <param name="group">The group to display in the row</param>
        /// <param name="backColor">The background color to display in the row</param>
        /// <param name="textColor">The text color to display in the row</param>
        private TableRow MakeRowForTable(ActivEarth.Objects.Groups.Group group, Color backColor, Color textColor)
        {        
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeLinkCellForRow(group.Name, group.ID, textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.Description, textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.ActivityScore.TotalScore.ToString(), textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.GreenScore.ToString(), textColor));  
            return newRow;
        }

        /// <summary>
        /// Formats a cell to add to the row containing the given text.
        /// </summary>
        /// <param name="text">The messages to display in the row</param>
        /// <param name="textColors">The text color to display in the cell</param>
        private TableCell MakeTextCellForRow(string text, Color textColor)
        {
            TableCell newCell = new TableCell();
            Label textLabel = new Label();
            textLabel.Text = text;
            textLabel.ForeColor = textColor;
            newCell.Controls.Add(textLabel);
            return newCell;
        }

        /// <summary>
        /// Formats a cell to add to the row containing the given text linking to the given Group ID.
        /// </summary>
        /// <param name="text">The messages to display in the link</param>
        /// <param name="groupId">The ID of the Group this cell should link to the edit page of</param>
        /// <param name="textColors">The text color to display in the cell</param>
        private TableCell MakeLinkCellForRow(string text, int groupId, Color textColor)
        {
            TableCell newCell = new TableCell();
            HyperLink textLink = new HyperLink();
            textLink.Text = text;
            textLink.ForeColor = textColor;
            textLink.NavigateUrl = "~/Groups/EditGroup.aspx?ID=" + groupId;
            newCell.Controls.Add(textLink);
            return newCell;
        }   

/*        private TableCell MakeControlCellForRow(int groupID)
        {
            TableCell newCell = new TableCell();

            Button b = new Button();
            b.Text = "To the Group!";
            b.ID = groupID.ToString();
            b.OnClientClick = "buttonClick";
            b.Controls.Add(new LiteralControl("buttonClick"));
            b.Click += new EventHandler(buttonClick);

            newCell.Controls.Add(b);

            return newCell;
        }

        private void buttonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Response.Redirect("~/Groups/GroupDisplay.aspx?ID=" + clickedButton.ID);
        }*/

    }
}