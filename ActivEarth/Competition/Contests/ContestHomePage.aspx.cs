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
    /// <summary>
    /// This class represents the Contest Home page of the 
    /// ActivEarth website.
    /// </summary>
    public partial class ContestHomePage : System.Web.UI.Page
    {
        /// <summary>
        /// Loads the page.
        /// </summary>
        /// <param name="sender">Object that requested the page load.</param>
        /// <param name="e">The event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                List<int> contestIds = ContestDAO.GetContestIdsFromUserId(user.UserID);
                List<string> contestNames = new List<string>();

                #region ---------- Test Code ----------

                /*
                int cid = ContestManager.CreateContest(ContestType.Individual, "Super Awesome Happy Funtime Contest", "You want 500 dollar.", 500,
                    new DateTime(2012, 4, 18, 0, 0, 0), 25000, true, Statistic.Steps, user.UserID);
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "The Doctor", Score = 23454 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "Rory Williams", Score = 23300 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "Amy Pond", Score = 15432 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "Donna Noble", Score = 20034 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "Jack Harkness", Score = 15432 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "Rose Tyler", Score = 3045 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "River Song", Score = 23455 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "Martha Jones", Score = 234 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "Mickey Smith", Score = 22111 });
                ContestManager.AddTeam(new Team() { ContestId = cid, Name = "Dalek Caan", Score = 0 });
                
                contestNames.Add("Super Awesome Happy Funtime Contest");
                contestIds.Add(id);
                */

                #endregion ---------- Test Code ----------

                foreach (int id in contestIds)
                {
                    contestNames.Add(ContestDAO.GetContestNameFromContestId(id));
                }

                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };
                displayCurrentContests.PopulateContestTable(contestNames, contestIds, backColors, textColors);
            }
        }

        /// <summary>
        /// Event handler for the Create contest button. Opens the create contest
        /// page.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void OpenCreateContestPage(object sender, EventArgs e)
        {
            Response.Redirect("~/Competition/Contests/ContestCreationPage.aspx");
        }

        /// <summary>
        /// Event handler for the Find contest button. Opens the Find contest
        /// page.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void OpenFindContestsPage(object sender, EventArgs e)
        {
            Response.Redirect("~/Competition/Contests/FindContestsPage.aspx");
        }
    }
}