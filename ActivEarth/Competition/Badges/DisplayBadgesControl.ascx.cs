using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ActivEarth.Objects.Competition.Badges;

namespace ActivEarth.Competition.Badges
{
    /// <summary>
    /// This class represents a control which is used to display 
    /// a group of Badges.
    /// </summary>
    public partial class DisplayBadgesControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Adds a Badge to the display so it can be seen by the user.
        /// </summary>
        /// 
        /// <param name="badge">The Badge to add to the display.</param>
        public void AddBadgeToDisplay(Badge badge)
        {
            DisplayBadgeControl displayBadgeControl = (DisplayBadgeControl)LoadControl("DisplayBadgeControl.ascx");
            displayBadgeControl.LoadBadgeIntoDisplay(badge);
            
            _displayBadgeControls.Controls.Add(displayBadgeControl);
        }
    }
}