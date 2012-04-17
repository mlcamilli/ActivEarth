using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Server.Service.Competition;

namespace ActivEarth.Competition.Challenges
{
    public partial class DisplayChallengeControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Loads a Challenge's information into the display so it can be seen by the user.
        /// </summary>
        /// /// <param name="challenge">The challenge to load into the display.</param>
        public void LoadChallengeIntoDisplay(Challenge challenge)
        {
            _challengeName.Text = challenge.Name;
            _challengeImage.ImageUrl = challenge.ImagePath;
            _activityPointsValue.Text = challenge.Reward.ToString();
            _challengeDescription.Text = challenge.Description;

            _challengeProgressBar.Value = 50; // ChallengeManager.GetProgress(challenge.ID, user.UserID);
            _challengeProgressNumerical.Text = "0 / 5"; //ChallengeManager.GetFormattedProgress(challenge.ID, user.UserID); 

            /*
            if (ChallengeManager.IsComplete(challenge.ID, user.UserID))
            {
                _challengeProgressNumerical.Text = "Completed";
            }
            */
        }
    }
}