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

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["userDetails"] == null)
            {
                Response.Redirect("Login.aspx");

            }
            else
            {
                var userDetails = (User)Session["userDetails"];
                this.userID = userDetails.UserID;

                lblUserName.Text = userDetails.UserName;


                List<ActivEarth.Objects.Groups.Group> userGroups = GroupDAO.GetGroupsByUser(this.userID);

                Color[] backColors = { Color.FromArgb(75, 108, 158), Color.White };
                Color[] textColors = { Color.White, Color.Black };
                GroupsDisplayTable1.PopulateGroupsTable(userGroups, backColors, textColors); 


            }

        }
    }
}