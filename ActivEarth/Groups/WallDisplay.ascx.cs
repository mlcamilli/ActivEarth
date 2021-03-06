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
using ActivEarth.Objects.Profile;

namespace ActivEarth.Groups
{
    public partial class WallDisplay : System.Web.UI.UserControl
    {
        /// <summary>
        /// This is a component, so no work is required when it loads.  The population of the table
        /// is handled by the page ASP.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Formats the table of Messages based on the given list of Messages and colors.
        /// </summary>
        /// <param name="messages">The list of messages to display in the table</param>
        /// <param name="backColors">The list of alternating background colors to display in the table</param>
        /// <param name="textColors">The list of alternating text colors to display in the table</param>
        public void PopulateMessageTable(List<ActivEarth.Objects.Groups.Message> messages, Color[] backColors, Color[] textColors)
        {
            int colorIndex = 0;
            int textIndex = 0;

            messages.Reverse();
            foreach (ActivEarth.Objects.Groups.Message message in messages)
            {
                _wall.Rows.Add(MakeRowForTable(message, backColors[colorIndex], textColors[textIndex]));

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
            messages.Reverse();
        }

        /// <summary>
        /// Formats a single row to the table using the given message.
        /// </summary>
        /// <param name="message">The message to display in the row</param>
        /// <param name="backColor">The background color to display in the row</param>
        /// <param name="textColor">The text color to display in the row</param>
        private TableRow MakeRowForTable(ActivEarth.Objects.Groups.Message message, Color backColor, Color textColor)
        {        
            TableRow newRow = new TableRow();
            newRow.BackColor = backColor;
            newRow.Cells.Add(MakeImageCellForRow(message.Poster));
            newRow.Cells.Add(MakeTextCellForRow(message.Poster.UserName, textColor));
            newRow.Cells.Add(MakeTextCellForRow("<b><u>" + message.Title + "</u></b><br/><br/>" +
                message.Text + "<br/><br/><br/>" + message.Time + "&nbsp;&nbsp;&nbsp;" + message.Date + "</div>", textColor));
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