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
        public void LoadBadgeIntoDisplay(Badge badge)
        {
            _badgeName.Text = badge.Name;
            _badgeImage.ImageUrl = badge.GetImagePath();
            _activityPointsValue.Text = badge.GetNextLevelReward().ToString();

            if (badge.Level != BadgeLevels.Max)
            {
                _badgeProgressNumerical.Text = badge.GetCurrentProgressFormated() + " / " + badge.GetNextLevelRequirementFormated();
                _badgeProgressBar.Value = (int)((badge.GetCurrentProgress() / badge.GetNextLevelRequirement()) * 100);
            }
            else
            {
                _activityPointsValue.Visible = false;
                _activityScoreImage.Visible = false;
                _badgeProgressNumerical.Text = badge.GetCurrentProgressFormated();
                _badgeProgressBar.Value = 100;
            }
        }
    }
}