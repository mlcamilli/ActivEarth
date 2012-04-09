using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Badges;

namespace ActivEarth.Competition.Badges
{
    public partial class DisplayBadgeControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Loads a Badge's information into the display so it can be seen by the user.
        /// </summary>
        /// /// <param name="badge">The Badge to load into the display.</param>
        public void LoadBadgeIntoDisplay(Badge badge /*, string name */)
        {
            //_badgeName.Text = name;
            _badgeImage.ImageUrl = badge.GetImagePath();
            _activityPointsValue.Text = badge.GetNextLevelReward().ToString();
            _badgeProgressNumerical.Text = badge.GetFormattedProgress();
            _badgeProgressBar.Value = badge.Progress;

            if (badge.IsComplete())
            {
                _activityPointsValue.Visible = false;
                _activityScoreImage.Visible = false;
            }
        }
    }
}