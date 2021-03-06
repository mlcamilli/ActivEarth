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
using ActivEarth.Objects;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service.Competition;
using ActivEarth.DAO;

namespace ActivEarth.Groups
{
    public partial class Groups : System.Web.UI.Page
    {
        int userID;

        /// <summary>
        /// Prepares the Groups table and Owned Groups table when the page loads.  Redirects the user
        /// if they have not signed in.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["userDetails"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");

            }
            else
            {
                var userDetails = (User)Session["userDetails"];
                this.userID = userDetails.UserID;
                
                List<Group> userGroups = GroupDAO.GetGroupsByUser(this.userID);
                List<Group> ownedGroups = GroupDAO.GetAllGroupsByOwner(userDetails);

                if (userGroups.Count > 0)
                {
                    Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                    Color[] textColors = { Color.White, Color.Black };
                    GroupsDisplayTable1.PopulateGroupsTable(userGroups, backColors, textColors);
                    OwnedGroupsDisplayTable1.PopulateGroupsTable(ownedGroups, backColors, textColors);

                    GroupsDisplayTable1.Visible = true;
                    OwnedGroupsDisplayTable1.Visible = true;
                    EmptyGroup.Visible = false;
                }
                else
                {
                    GroupsDisplayTable1.Visible = false;
                    OwnedGroupsDisplayTable1.Visible = false;
                    EmptyGroup.Visible = true;
                }
                  

            }

        }

        /// <summary>
        /// Method called when the Search Button is clicked.  Redirects the User to the Search Groups page using the 
        /// Text in the searchBox as the search terms.
        /// </summary>
        protected void SearchGroups(object sender, EventArgs e)
        {
            if (searchBox.Text.Length > 0)
                Response.Redirect("~/Groups/GroupsSearch.aspx?Term=" + searchBox.Text);
        }

        /// <summary>
        /// Method called when the Create Group Button is clicked.  Redirects the User to the Create Group page.
        /// </summary>
        protected void CreateGroup(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups/CreateGroup.aspx");
        }
    }
}