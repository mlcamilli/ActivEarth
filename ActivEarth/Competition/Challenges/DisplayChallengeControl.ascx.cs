using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service.Competition;

namespace ActivEarth.Competition.Challenges
{
    /// <summary>
    /// This class represents a control that is used to display a
    /// single Challenge.
    /// </summary>
    public partial class DisplayChallengeControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Loads a Challenge's information into the display so it can be seen by the user.
        /// </summary>
        /// /// <param name="challenge">The challenge to load into the display.</param>
        public void LoadChallengeIntoDisplay(Challenge challenge)
        {
            var user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }

            _challengeName.Text = challenge.Name;
            _challengeImage.ImageUrl = challenge.ImagePath;
            _activityPointsValue.Text = challenge.Reward.ToString();
            _challengeDescription.Text = challenge.Description;

            _challengeProgressBar.Value = ChallengeManager.GetProgress(challenge.ID, user.UserID);
            _challengeProgressNumerical.Text = ChallengeManager.GetFormattedProgress(challenge.ID, user.UserID); 

            
            if (ChallengeManager.IsComplete(challenge.ID, user.UserID))
            {
                _challengeProgressNumerical.Text = "Completed";
            }
        }
    }
}