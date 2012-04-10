using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Profile;
using ActivEarth.DAO;
using ActivEarth.Server.Service.Competition;

namespace ActivEarth.Competition.Badges
{
    public partial class DisplayBadgesPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Test Code
            User user = UserDAO.GetUserFromUserId(UserDAO.GetUserIdFromUserName("badgetest1"));

            Badge Steps = user.Badges[Statistic.Steps] = BadgeManager.CreateBadge(user, Statistic.Steps);
            Badge WalkDistance = user.Badges[Statistic.WalkDistance] = BadgeManager.CreateBadge(user, Statistic.WalkDistance);
            Badge RunDistance  = user.Badges[Statistic.RunDistance] = BadgeManager.CreateBadge(user, Statistic.RunDistance);
            Badge BikeDistance = user.Badges[Statistic.BikeDistance] = BadgeManager.CreateBadge(user, Statistic.BikeDistance);
            Badge ChallengesCompleted = user.Badges[Statistic.ChallengesCompleted] = BadgeManager.CreateBadge(user, Statistic.ChallengesCompleted);
            Badge GasSavings = user.Badges[Statistic.GasSavings] = BadgeManager.CreateBadge(user, Statistic.GasSavings);

            user.SetStatistic(Statistic.BikeDistance, 345);
            user.SetStatistic(Statistic.WalkDistance, 150);
            user.SetStatistic(Statistic.RunDistance, 2345);
            user.SetStatistic(Statistic.Steps, 23456);
            user.SetStatistic(Statistic.ChallengesCompleted, 145);
            user.SetStatistic(Statistic.GasSavings, 55.76f);

            /*
            Steps.Update();
            WalkDistance.Update();
            RunDistance.Update();
            BikeDistance.Update();
            ChallengesCompleted.Update();
            GasSavings.Update();
            */

            user.Badges[Statistic.Steps].Update();
            user.Badges[Statistic.BikeDistance].Update();
            user.Badges[Statistic.ChallengesCompleted].Update();
            user.Badges[Statistic.RunDistance].Update();
            user.Badges[Statistic.WalkDistance].Update();
            user.Badges[Statistic.GasSavings].Update();

            _displayBadgesControl.AddBadgeToDisplay(Steps);
            _displayBadgesControl.AddBadgeToDisplay(WalkDistance);
            _displayBadgesControl.AddBadgeToDisplay(RunDistance);
            _displayBadgesControl.AddBadgeToDisplay(BikeDistance);
            _displayBadgesControl.AddBadgeToDisplay(ChallengesCompleted);
            _displayBadgesControl.AddBadgeToDisplay(GasSavings);
        }
    }
}