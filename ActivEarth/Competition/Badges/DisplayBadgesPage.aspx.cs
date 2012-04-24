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
            var user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                BadgeManager.UpdateBadge(user.UserID, Statistic.Steps);
                BadgeManager.UpdateBadge(user.UserID, Statistic.AggregateDistance);
                BadgeManager.UpdateBadge(user.UserID, Statistic.WalkDistance);
                BadgeManager.UpdateBadge(user.UserID, Statistic.RunDistance);
                BadgeManager.UpdateBadge(user.UserID, Statistic.BikeDistance);
                BadgeManager.UpdateBadge(user.UserID, Statistic.AggregateTime);
                BadgeManager.UpdateBadge(user.UserID, Statistic.WalkTime);
                BadgeManager.UpdateBadge(user.UserID, Statistic.RunTime);
                BadgeManager.UpdateBadge(user.UserID, Statistic.BikeTime);
                BadgeManager.UpdateBadge(user.UserID, Statistic.ChallengesCompleted);
                BadgeManager.UpdateBadge(user.UserID, Statistic.GasSavings);

                List<Badge> badges = BadgeDAO.GetBadgesFromUserId(user.UserID);
                if (badges == null || badges.Count == 0)
                {
                    BadgeManager.CreateBadge(user.UserID, Statistic.Steps);
                    BadgeManager.CreateBadge(user.UserID, Statistic.AggregateDistance);
                    BadgeManager.CreateBadge(user.UserID, Statistic.WalkDistance);
                    BadgeManager.CreateBadge(user.UserID, Statistic.RunDistance);
                    BadgeManager.CreateBadge(user.UserID, Statistic.BikeDistance);
                    BadgeManager.CreateBadge(user.UserID, Statistic.AggregateTime);
                    BadgeManager.CreateBadge(user.UserID, Statistic.WalkTime);
                    BadgeManager.CreateBadge(user.UserID, Statistic.RunTime);
                    BadgeManager.CreateBadge(user.UserID, Statistic.BikeTime);
                    BadgeManager.CreateBadge(user.UserID, Statistic.ChallengesCompleted);
                    BadgeManager.CreateBadge(user.UserID, Statistic.GasSavings);
                    badges = BadgeDAO.GetBadgesFromUserId(user.UserID);
                }

                foreach (Badge badge in badges)
                {
                    _displayBadgesControl.AddBadgeToDisplay(badge);
                }
            }
        }
    }
}