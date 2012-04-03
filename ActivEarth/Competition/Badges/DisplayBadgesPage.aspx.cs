using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Profile;

namespace ActivEarth.Competition.Badges
{
    public partial class DisplayBadgesPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Test Code
            User _user = new User("Test", "Subject");

            _user.Badges[Statistic.BikeDistance] =
                new Badge(1, "Distance Biked", _user, Statistic.BikeDistance,
                BadgeConstants.BikeDistance.REQUIREMENTS, BadgeConstants.BikeDistance.REWARDS,
                BadgeConstants.BikeDistance.FORMAT, BadgeConstants.BikeDistance.IMAGES);

            _user.Badges[Statistic.WalkDistance] =
                new Badge(2, "Distance Walked", _user, Statistic.WalkDistance,
                BadgeConstants.WalkDistance.REQUIREMENTS, BadgeConstants.WalkDistance.REWARDS,
                BadgeConstants.WalkDistance.FORMAT, BadgeConstants.WalkDistance.IMAGES);

            _user.Badges[Statistic.RunDistance] =
                new Badge(3, "Distance Ran", _user, Statistic.RunDistance,
                BadgeConstants.RunDistance.REQUIREMENTS, BadgeConstants.RunDistance.REWARDS,
                BadgeConstants.RunDistance.FORMAT, BadgeConstants.RunDistance.IMAGES);

            _user.Badges[Statistic.Steps] =
                new Badge(4, "Steps", _user, Statistic.Steps,
                BadgeConstants.Steps.REQUIREMENTS, BadgeConstants.Steps.REWARDS,
                BadgeConstants.Steps.FORMAT, BadgeConstants.Steps.IMAGES);

            _user.Badges[Statistic.ChallengesCompleted] =
                new Badge(5, "Challenges Completed", _user, Statistic.ChallengesCompleted,
                BadgeConstants.Challenges.REQUIREMENTS, BadgeConstants.Challenges.REWARDS,
                BadgeConstants.Challenges.FORMAT, BadgeConstants.Challenges.IMAGES);

            _user.Badges[Statistic.GasSavings] =
                new Badge(6, "Gas Savings", _user, Statistic.GasSavings,
                BadgeConstants.GasSavings.REQUIREMENTS, BadgeConstants.GasSavings.REWARDS,
                BadgeConstants.GasSavings.FORMAT, BadgeConstants.GasSavings.IMAGES);

            _user.SetStatistic(Statistic.BikeDistance, 345);
            _user.SetStatistic(Statistic.WalkDistance, 150);
            _user.SetStatistic(Statistic.RunDistance, 2345);
            _user.SetStatistic(Statistic.Steps, 23456);
            _user.SetStatistic(Statistic.ChallengesCompleted, 145);
            _user.SetStatistic(Statistic.GasSavings, 55.76f);

            _user.Badges[Statistic.Steps].Update();
            _user.Badges[Statistic.BikeDistance].Update();
            _user.Badges[Statistic.ChallengesCompleted].Update();
            _user.Badges[Statistic.RunDistance].Update();
            _user.Badges[Statistic.WalkDistance].Update();
            _user.Badges[Statistic.GasSavings].Update();

            _displayBadgesControl.AddBadgeToDisplay(_user.Badges[Statistic.Steps]);
            _displayBadgesControl.AddBadgeToDisplay(_user.Badges[Statistic.WalkDistance]);
            _displayBadgesControl.AddBadgeToDisplay(_user.Badges[Statistic.RunDistance]);
            _displayBadgesControl.AddBadgeToDisplay(_user.Badges[Statistic.BikeDistance]);
            _displayBadgesControl.AddBadgeToDisplay(_user.Badges[Statistic.ChallengesCompleted]);
            _displayBadgesControl.AddBadgeToDisplay(_user.Badges[Statistic.GasSavings]);
        }
    }
}