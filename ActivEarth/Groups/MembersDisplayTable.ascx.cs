using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth;
using ActivEarth.Account;
using ActivEarth.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Groups;
using ActivEarth.DAO;

namespace ActivEarth.Groups
{
    public partial class MembersDisplayTable : System.Web.UI.UserControl
    {
        /// <summary>
        /// This is a component, so no work is required when it loads.  The population of the table
        /// is handled by the page ASP.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Formats the preview table of Users on the Group Page based on the given list of Users and colors.
        /// </summary>
        /// <param name="users">The list of users to display in the table</param>
        /// <param name="backColors">The list of alternating background colors to display in the table</param>
        /// <param name="textColors">The list of alternating text colors to display in the table</param>
        public void PopulateMembersTable_Display(List<User> users, Color[] backColors, Color[] textColors)
        {
            TableRow imageRow = new TableRow();
            imageRow.BackColor = backColors[0];
            TableRow nameRow = new TableRow();
            nameRow.BackColor = backColors[1];

            for (int i = 0; i < Math.Min(6, users.Count); i++)
            {

                imageRow.Cells.Add(MakeImageCellForRow(users.ElementAt(i)));
                nameRow.Cells.Add(MakeTextCellForRow(users.ElementAt(i).UserName, textColors[1]));

            }


            _usersTable.Rows.Add(imageRow);
            _usersTable.Rows.Add(nameRow);

        }

        /// <summary>
        /// Formats the table of all Users in the Group based on the given list of Users and colors.
        /// </summary>
        /// <param name="users">The list of users to display in the table</param>
        /// <param name="backColors">The list of alternating background colors to display in the table</param>
        /// <param name="textColors">The list of alternating text colors to display in the table</param>
        public void PopulateMembersTable_SeeAll(List<User> users, Color[] backColors, Color[] textColors)
        {
            int colorIndex = 0;
            int textIndex = 0;
          
            foreach (User user in users)
            {
                _usersTable.Rows.Add(MakeRowForSeeAllTable(user, backColors[colorIndex], textColors[textIndex]));

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
        /// Formats the table of all Users in the Group with the option to remove users for the Owner of the group.
        /// </summary>
        /// <param name="users">The list of users to display in the table</param>
        /// <param name="backColors">The list of alternating background colors to display in the table</param>
        /// <param name="textColors">The list of alternating text colors to display in the table</param>
        public void PopulateMembersTable_Owner(List<User> users, Color[] backColors, Color[] textColors)
        {
            int colorIndex = 0;
            int textIndex = 0;

            foreach (User user in users)
            {
                _usersTable.Rows.Add(MakeRowForOwnerTable(user, backColors[colorIndex], textColors[textIndex]));

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
        /// Formats a single row for the Owner's table containing the given User.
        /// </summary>
        /// <param name="user">The User to display in the row</param>
        /// <param name="backColor">The background color to display in the row</param>
        /// <param name="textColor">The text color to display in the row</param>
        private TableRow MakeRowForOwnerTable(User user, Color backColor, Color textColor)
        {
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeImageCellForRow(user));
            newRow.Cells.Add(MakeTextCellForRow(user.UserName, textColor));

            User owner = (User)Session["userDetails"];
            if (owner.UserID == user.UserID)
            {
                newRow.Cells.Add(MakeTextCellForRow("Group Owner", textColor));
            }
            else
            {
                newRow.Cells.Add(MakeBootCellForRow(user.UserID));
            }
            return newRow;
        }

        /// <summary>
        /// Formats a single row for the table of all Users containing the given User.
        /// </summary>
        /// <param name="user">The User to display in the row</param>
        /// <param name="backColor">The background color to display in the row</param>
        /// <param name="textColor">The text color to display in the row</param>
        private TableRow MakeRowForSeeAllTable(User user, Color backColor, Color textColor)
        {
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeImageCellForRow(user));
            newRow.Cells.Add(MakeTextCellForRow(user.UserName, textColor));
            return newRow;
        }

        /// <summary>
        /// Formats a cell to add to the row containing the given User's picture.
        /// </summary>
        /// <param name="User">The User whose image should be displayed in the cell</param>
        private TableCell MakeImageCellForRow(User user)
        {
            
            TableCell newCell = new TableCell();
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = getUserImageUrl(user, "icon");
            img.Height = new Unit(75, UnitType.Pixel);
            
            img.Width = new Unit(75, UnitType.Pixel);
            newCell.Controls.Add(img);
            return newCell;

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
        /// Formats a cell to add to the row with a button allowing the Owner to remove the User from the Group.
        /// </summary>
        /// <param name="userId">The User ID of the User to remove from the Group</param>
        private TableCell MakeBootCellForRow(int userId)
        {
            TableCell newCell = new TableCell();

            Button b = new Button();
            b.ID = userId.ToString();
            b.CssClass = "Button";
            b.Text = "Remove User From Group";
            b.Click += new EventHandler(bootClick);

            newCell.Controls.Add(b);

            return newCell;
        }

        /// <summary>
        /// Method called when a Remove User Button is clicked allowing the Owner to remove the User from the Group.
        /// </summary>
        private void bootClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            Group currentGroup = GroupDAO.GetGroupFromGroupId(Convert.ToInt32(Request.QueryString["ID"]));
            currentGroup.Quit(UserDAO.GetUserFromUserId(Convert.ToInt32(clickedButton.ID)));
            GroupDAO.UpdateGroup(currentGroup);

            Response.Redirect("~/Groups/EditGroup.aspx?ID=" + Request.QueryString["ID"]);
        }

        /// <summary>
        /// Returns the relative url for an image.
        /// 
        /// Current image sizes are:
        ///     - icon: a 150x150 image for the user's profile
        /// </summary>
        /// <param name="user">The user to retrieve the image for.</param>
        /// <param name="imageSizeName">The name of the image size to retrieve.</param>
        /// <returns></returns>
        private string getUserImageUrl(User user, string imageSizeName)
        {
            string path = Server.MapPath("~") + "\\Images\\Account\\UserProfile\\" + imageSizeName + "\\";
            int userImageDir = (user.UserID / 1000);
            string uploadPath = String.Format("{0}\\{1}\\{2}.png", path, userImageDir, user.UserID);

            if (System.IO.File.Exists(uploadPath))
            {
                return String.Format("/Images/Account/UserProfile/{0}/{1}/{2}.png", imageSizeName, userImageDir, user.UserID);
            }
            else
            {
                return String.Format("/Images/Account/UserProfileDefaults/default_{0}.png", imageSizeName);
            }
        }


   /*     private TableCell MakeControlCellForRow(int groupID)
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
        } */

    }
}