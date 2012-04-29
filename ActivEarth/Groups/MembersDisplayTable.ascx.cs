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

namespace ActivEarth.Groups
{
    public partial class MembersDisplayTable : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        public void PopulateMembersTable(List<User> users, Color[] backColors, Color[] textColors)
        {
            TableRow imageRow = new TableRow();
            imageRow.BackColor = backColors[0];
            TableRow nameRow = new TableRow();
            nameRow.BackColor = backColors[1];

            foreach (User user in users)
            {
                imageRow.Cells.Add(MakeImageCellForRow(user));
                nameRow.Cells.Add(MakeTextCellForRow(user.UserName, textColors[1]));
            }


            _usersTable.Rows.Add(imageRow);
            _usersTable.Rows.Add(nameRow);


        }


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