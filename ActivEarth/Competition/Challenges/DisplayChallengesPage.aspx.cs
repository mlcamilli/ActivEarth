using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service.Competition;
using ActivEarth.DAO;

namespace ActivEarth.Competition.Challenges
{
    /// <summary>
    /// This class represents the page that is used to display Challenges
    /// on the ActivEarth website.
    /// </summary>
    public partial class DisplayChallengesPage : System.Web.UI.Page
    {
        /// <summary>
        /// Loads the page.
        /// </summary>
        /// <param name="sender">Sender that request the page load.</param>
        /// <param name="e">The arguments of the event.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
             var user = (User)Session["userDetails"];
             if (user == null)
             {
                 Response.Redirect("~/Account/Login.aspx");
             }
             else
             {
                 foreach (Challenge challenge in ChallengeDAO.GetActiveDailyChallenges(user.UserID))
                 {
                     _displayDailyChallenges.AddChallengeToDisplay(challenge);
                 }

                 foreach (Challenge challenge in ChallengeDAO.GetActiveWeeklyChallenges(user.UserID))
                 {
                     _displayWeeklyChallenges.AddChallengeToDisplay(challenge);
                 }

                 foreach (Challenge challenge in ChallengeDAO.GetActiveMonthlyChallenges(user.UserID))
                 {
                     _displayMonthlyChallenges.AddChallengeToDisplay(challenge);
                 }

                 foreach (Challenge challenge in ChallengeDAO.GetActivePersistentChallenges(user.UserID))
                 {
                     _displayPersistentChallenges.AddChallengeToDisplay(challenge);
                 }
             }
        }
    }
}