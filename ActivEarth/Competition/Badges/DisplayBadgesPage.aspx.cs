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
                List<Badge> badges = BadgeDAO.GetBadgesFromUserId(user.UserID);
                if (badges == null || badges.Count == 0)
                {
                    BadgeManager.CreateBadge(user, Statistic.Steps);
                    BadgeManager.CreateBadge(user, Statistic.WalkDistance);
                    BadgeManager.CreateBadge(user, Statistic.RunDistance);
                    BadgeManager.CreateBadge(user, Statistic.BikeDistance);
                    BadgeManager.CreateBadge(user, Statistic.ChallengesCompleted);
                    BadgeManager.CreateBadge(user, Statistic.GasSavings);
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