using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Server.Service.Competition;

namespace ActivEarth.Competition.Badges
{
    public partial class DisplayBadgeControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Loads a Badge's information into the display so it can be seen by the user.
        /// </summary>
        /// /// <param name="badge">The Badge to load into the display.</param>
        public void LoadBadgeIntoDisplay(Badge badge)
        {
            _badgeName.Text = badge.Name;
            _badgeImage.ImageUrl = badge.ImagePath;
            _activityPointsValue.Text = badge.GetNextLevelReward().ToString();
            _badgeProgressNumerical.Text = BadgeManager.GetFormattedProgress(badge.ID);
            _badgeProgressBar.Value = badge.Progress;

            if (badge.IsComplete())
            {
                _activityPointsValue.Visible = false;
                _activityScoreImage.Visible = false;
            }
        }
    }
}