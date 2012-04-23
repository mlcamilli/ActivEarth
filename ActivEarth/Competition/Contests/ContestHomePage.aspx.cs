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
                /*
                int id = ContestManager.CreateContest(ContestType.Individual, "Super Awesome Happy Funtime Contest", "You want 500 dollar.", 500,
                    new DateTime(2012, 4, 18, 0, 0, 0), 25000, true, Statistic.Steps, user.UserID);
                ContestManager.AddTeam(new Team() { ContestId=id, Name = "The Doctor", Score = 23454 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "Rory Williams", Score = 23300 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "Amy Pond", Score = 15432 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "Donna Noble", Score = 20034 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "Jack Harkness", Score = 15432 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "Rose Tyler", Score = 3045 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "River Song", Score = 23455 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "Martha Jones", Score = 234 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "Mickey Smith", Score = 22111 });
                ContestManager.AddTeam(new Team() { ContestId = id, Name = "Dalek Caan", Score = 0 });

                List<string> contestNames = new List<string>();
                contestNames.Add("Super Awesome Happy Funtime Contest");
                List<int> contestIds = new List<int>();
                contestIds.Add(id);

                List<Contest> contests = ContestDAO.GetActiveContests();

                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };
                displayCurrentContests.PopulateContestTable(contestNames, contestIds, backColors, textColors);
                */
            }
        }

        protected void OpenCreateContestPage(object sender, EventArgs e)
        {
            Response.Redirect("~/Competition/Contests/ContestCreationPage.aspx");
        }
    }
}