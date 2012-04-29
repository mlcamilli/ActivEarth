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
    public partial class GroupsDisplayTable : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        public void PopulateGroupsTable(List<ActivEarth.Objects.Groups.Group> groups, Color[] backColors, Color[] textColors)
        {
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