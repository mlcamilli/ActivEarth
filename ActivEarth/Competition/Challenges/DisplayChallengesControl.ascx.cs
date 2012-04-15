using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Challenges;

namespace ActivEarth.Competition.Challenges
{
    public partial class DisplayChallengesControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Adds a Challenge to the display so it can be seen by the user.
        /// </summary>
        /// /// <param name="challenge">The challenge to add to the display.</param>
        public void AddChallengeToDisplay(Challenge challenge)
        {
            DisplayChallengeControl displayChallengeControl = (DisplayChallengeControl)LoadControl("DisplayChallengeControl.ascx");
            displayChallengeControl.LoadChallengeIntoDisplay(challenge);

            _displayChallengeControls.Controls.Add(displayChallengeControl);
        }
    }
}