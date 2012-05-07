using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.Account;
using ActivEarth.Objects.Profile;
using ActivEarth.Groups;
using ActivEarth.Objects.Groups;
using ActivEarth.DAO;

namespace ActivEarth.Groups
{
    public partial class GroupsDisplayTable : System.Web.UI.UserControl
    {
        User userDetails;

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
        /// <param name="groups">The list of messages to display in the table</param>
        /// <param name="backColors">The list of alternating background colors to display in the table</param>
        /// <param name="textColors">The list of alternating text colors to display in the table</param>
        public void PopulateGroupsTable(List<ActivEarth.Objects.Groups.Group> groups, Color[] backColors, Color[] textColors)
        {
            this.userDetails = (User)Session["userDetails"];

            int colorIndex = 0;
            int textIndex = 0;

            foreach (ActivEarth.Objects.Groups.Group group in groups)
            {
                _groupsTable.Rows.Add(MakeRowForTable(group, backColors[colorIndex], textColors[textIndex]));

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
        /// Formats a single row to the table using the given Groups.
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
            newRow.Cells.Add(MakeTextCellForRow(group.Owner.UserName, textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.ActivityScore.TotalScore.ToString(), textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.GreenScore.ToString(), textColor));

            if (userDetails.UserID != group.Owner.UserID)
            {
                if (MembersContains(group.Members, userDetails))
                {
                    newRow.Cells.Add(MakeButtonCellForRow(group.ID, 0));
                }
                else
                {
                    newRow.Cells.Add(MakeButtonCellForRow(group.ID, 1));
                }
            }
            else
            {
                newRow.Cells.Add(MakeTextCellForRow("Group Owner", textColor));
            }
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
            textLink.NavigateUrl = "~/Groups/GroupDisplay.aspx?ID=" + groupId;
            newCell.Controls.Add(textLink);
            return newCell;
        }

        /// <summary>
        /// Formats a cell to add to the row with a button allowing the User to join the Group if they are not
        /// a Member and to leave the Group if the User is not a Member of the Group.
        /// </summary>
        /// <param name="groupId">The ID of the Group to join or leave</param>
        /// <param name="leaveOrJoin">0 if the User is a Member, 1 if the User is not a Member</param> 
        private TableCell MakeButtonCellForRow(int groupID, int leaveOrJoin)
        {
            TableCell newCell = new TableCell();

            Button b = new Button();
            b.ID = groupID.ToString();
            b.CssClass = "Button";
            
            if(leaveOrJoin == 0)
            {
                b.Text = "Leave the Group";
                b.Click += new EventHandler(quitClick);
            }
            else
            {
                b.Text = "Join the Group!";
                b.Click += new EventHandler(joinClick);
            }

            newCell.Controls.Add(b);

            return newCell;
        }

        /// <summary>
        /// Method called when a Join Button is clicked allowing the User to join the Group.
        /// </summary>
        private void joinClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            Group currentGroup = GroupDAO.GetGroupFromGroupId(Convert.ToInt32(clickedButton.ID));
            currentGroup.Join(userDetails);
            GroupDAO.UpdateGroup(currentGroup);

            Response.Redirect("~/Groups/Groups.aspx");
        }

        /// <summary>
        /// Method called when a Leave Button is clicked allowing the User to leave the Group.
        /// </summary>
        private void quitClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            Group currentGroup = GroupDAO.GetGroupFromGroupId(Convert.ToInt32(clickedButton.ID));
            currentGroup.Quit(userDetails);
            GroupDAO.UpdateGroup(currentGroup);
    
            Response.Redirect("~/Groups/Groups.aspx");
        }

        /// <summary>
        /// Determines whether or not the given user is in the list of Members.
        /// </summary>
        /// <param name="members">The list of Members for a Group</param>
        /// <param name="user">The User to search for</param>
        /// <returns>True if the User is in the List, False if the User is not</returns>
        private Boolean MembersContains(List<User> members, User user)
        {
            foreach (User member in members)
            {
                if (member.UserID == userDetails.UserID)
                    return true;
            }
            return false;
        }

    }
}