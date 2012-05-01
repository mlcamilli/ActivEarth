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
    public partial class GroupsSearch : System.Web.UI.Page
    {
        int userID;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["userDetails"] == null)
            {
                Response.Redirect("Login.aspx");

            }
            else if (Request.QueryString["Term"] == null)
            {
                Response.Redirect("~/Groups/Groups.aspx");
            }
            else
            {
                var userDetails = (User)Session["userDetails"];
                this.userID = userDetails.UserID;

                String searchTerm = Request.QueryString["Term"];

                List<ActivEarth.Objects.Groups.Group> searchGroups = GroupDAO.GetAllGroupsByName(searchTerm);
                searchGroups.Union(GroupDAO.GetAllGroupsByHashTag(searchTerm));

                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };
               
                SearchGroupsDisplayTable1.PopulateGroupsTable(searchGroups, backColors, textColors); 


            }

        }

        protected void SearchGroups(object sender, EventArgs e)
        {
            if (searchBox.Text.Length > 0)
                Response.Redirect("GroupsSearch.aspx?Term=" + searchBox.Text);
        }
    }
}