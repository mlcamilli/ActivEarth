using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ActivEarth.Account;
using ActivEarth.Groups;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects;
using ActivEarth.Server.Service;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service.Competition;
using ActivEarth.Competition.Contests;
using ActivEarth.DAO;

namespace ActivEarth.Groups
{
    public partial class WallPage : System.Web.UI.Page
    {
        /// <summary>
        /// Prepares the wall table to display when the page loads.  Redirects the user
        /// if they have not signed in or if a Group ID has not been provided.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["userDetails"] == null)
            {
                Response.Redirect("~/Groups/Login.aspx");

            }
            else if (Request.QueryString["ID"] == null)
            {
                Response.Redirect("~/Groups/Groups.aspx");
            }
            else
            {
                int groupID = Convert.ToInt32(Request.QueryString["ID"]);
                Group currentGroup = GroupDAO.GetGroupFromGroupId(groupID);

                lblGroupName.Text = currentGroup.Name;
                lblDescription.Text = currentGroup.Description;

                Color[] backColors = { Color.FromArgb(75, 108, 158), Color.White };
                Color[] textColors = { Color.White, Color.Black };

                List<ActivEarth.Objects.Groups.Message> messageList = currentGroup.Wall.Messages;
                WallDisplay1.PopulateMessageTable(messageList, backColors, textColors);


            }

        }
    }
}