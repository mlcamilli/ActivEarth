using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Objects.Profile;

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
                List<Contest> contests = new List<Contest>();
                contests.Add(new Contest("Super Awesome Happy Funtime Contest", "No", 500, ContestEndMode.GoalBased,
                    ContestType.Group, DateTime.Now, new EndCondition(9001), Objects.Profile.Statistic.Steps));
                contests.Add(new Contest("Space Eyes", "No", 500, ContestEndMode.GoalBased,
                    ContestType.Group, DateTime.Now, new EndCondition(9001), Objects.Profile.Statistic.Steps));
                contests.Add(new Contest("No I needed that.", "No", 500, ContestEndMode.GoalBased,
                    ContestType.Group, DateTime.Now, new EndCondition(9001), Objects.Profile.Statistic.Steps));
                contests.Add(new Contest("The Doctor", "No", 500, ContestEndMode.GoalBased,
                    ContestType.Individual, DateTime.Now, new EndCondition(9001), Objects.Profile.Statistic.Steps));

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