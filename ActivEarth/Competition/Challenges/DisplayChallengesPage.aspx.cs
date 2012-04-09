using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Objects.Profile;

namespace ActivEarth.Competition.Challenges
{
    public partial class DisplayChallengesPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Test code
            Challenge daily = new Challenge("Walk this way", "Walk 5 miles", 10, false, new System.DateTime(1,1,1,1,1,1), 1, Statistic.WalkDistance, 5.0f);
            Challenge weekly = new Challenge("Run this way", "Run 5 miles", 50, false, new System.DateTime(1, 1, 1, 1, 1, 1), 7, Statistic.RunDistance, 5.0f);
            Challenge monthly = new Challenge("Bike this way", "Bike 5 miles", 100, false, new System.DateTime(1, 1, 1, 1, 1, 1), 30, Statistic.BikeDistance, 5.0f);
            Challenge persistent = new Challenge("Over 9000", "Go 10000 steps", 10, false, new System.DateTime(1, 1, 1, 1, 1, 1), 365, Statistic.Steps, 10000.0f);

            daily.ImagePath = "~/Images/Competition/Challenges/Distance_Walked/Daily.png";
            weekly.ImagePath = "~/Images/Competition/Challenges/Distance_Ran/Weekly.png";
            monthly.ImagePath = "~/Images/Competition/Challenges/Distance_Biked/Monthly.png";
            persistent.ImagePath = "~/Images/Competition/Challenges/Challenge_Templates/Permaent.png";

            _displayDailyChallenges.AddChallengeToDisplay(daily);
            _displayDailyChallenges.AddChallengeToDisplay(daily);
            _displayDailyChallenges.AddChallengeToDisplay(daily);

            _displayWeeklyChallenges.AddChallengeToDisplay(weekly);
            _displayWeeklyChallenges.AddChallengeToDisplay(weekly);
            _displayWeeklyChallenges.AddChallengeToDisplay(weekly);

            _displayMonthlyChallenges.AddChallengeToDisplay(monthly);
            _displayMonthlyChallenges.AddChallengeToDisplay(monthly);
            _displayMonthlyChallenges.AddChallengeToDisplay(monthly);

            _displayPersistentChallenges.AddChallengeToDisplay(persistent);
            _displayPersistentChallenges.AddChallengeToDisplay(persistent);
            _displayPersistentChallenges.AddChallengeToDisplay(persistent);
            _displayPersistentChallenges.AddChallengeToDisplay(persistent);
            _displayPersistentChallenges.AddChallengeToDisplay(persistent);
            _displayPersistentChallenges.AddChallengeToDisplay(persistent);
            //End test code
        }
    }
}