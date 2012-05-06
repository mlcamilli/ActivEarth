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
                Response.Redirect("~/Account/Login.aspx");

            }
            else if (Request.QueryString["Term"] == null)
            {
                Response.Redirect("~/Groups/Groups.aspx");
            }
            else
            {
                var userDetails = (User)Session["userDetails"];
                this.userID = userDetails.UserID;

                String[] terms = Request.QueryString["Term"].Split();
                List<String> searchTerms = new List<String>(terms);

                List<Group> results = new List<Group>();

                foreach (String searchTerm in searchTerms)
                {
                    List<Group> searchGroups = GroupDAO.GetAllGroupsByName(searchTerm);
                    List<Group> taggedGroups = GroupDAO.GetAllGroupsByHashTag(searchTerm);
                    ListUnion_NoRepeats(searchGroups, taggedGroups);
                    ListUnion_NoRepeats(results, searchGroups);
                }

                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };
               
                SearchGroupsDisplayTable1.PopulateGroupsTable(results, backColors, textColors); 


            }

        }

        protected void SearchGroups(object sender, EventArgs e)
        {
            if (searchBox.Text.Length > 0)
                Response.Redirect("~/Groups/GroupsSearch.aspx?Term=" + searchBox.Text);
        }

        protected List<Group> ListUnion_NoRepeats(List<Group> list1, List<Group> list2)
        {
            foreach (Group group in list2)
            {
                Boolean found = false;

                foreach (Group group2 in list1)
                {
                    if (group.ID == group2.ID)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    list1.Add(group);
                }
            }

            return list1;
        }
    }
}