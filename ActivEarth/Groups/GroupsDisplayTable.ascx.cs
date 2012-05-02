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

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
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


        private TableRow MakeRowForTable(ActivEarth.Objects.Groups.Group group, Color backColor, Color textColor)
        {        
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeLinkCellForRow(group.Name, group.ID, textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.Description, textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.Owner.UserName, textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.ActivityScore.TotalScore.ToString(), textColor));
            newRow.Cells.Add(MakeTextCellForRow(group.GreenScore.ToString(), textColor));
            if (MembersContains(group.Members, userDetails))
            {  
                newRow.Cells.Add(MakeButtonCellForRow(group.ID, 0));
            }
            else
            {
                newRow.Cells.Add(MakeButtonCellForRow(group.ID, 1));  
            }
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


        private void joinClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            Group currentGroup = GroupDAO.GetGroupFromGroupId(Convert.ToInt32(clickedButton.ID));
            currentGroup.Join(userDetails);
            GroupDAO.UpdateGroup(currentGroup);

            Response.Redirect("Groups.aspx");
        }

        private void quitClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            Group currentGroup = GroupDAO.GetGroupFromGroupId(Convert.ToInt32(clickedButton.ID));
            currentGroup.Quit(userDetails);
            GroupDAO.UpdateGroup(currentGroup);
    
            Response.Redirect("Groups.aspx");
        }

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