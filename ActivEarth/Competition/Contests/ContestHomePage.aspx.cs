using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.DAO;
using ActivEarth.Server.Service.Competition;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Competition.Contests
{
    public partial class ContestHomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                //Test code
                List<Contest> contests = ContestDAO.GetActiveContests();

                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };
                displayCurrentContests.PopulateContestTable(contests, backColors, textColors);
            }
        }

        protected void OpenCreateContestPage(object sender, EventArgs e)
        {
            Response.Redirect("~/Competition/Contests/ContestCreationPage.aspx");
        }
    }
}