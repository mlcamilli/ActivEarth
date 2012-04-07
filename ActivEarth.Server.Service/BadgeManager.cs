using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;

using ActivEarth.DAO;

namespace ActivEarth.Server.Service.Competition
{
    /// <summary>
    /// Serves as a wrapper class for the BadgeDAO class, further abstracting actions.
    /// </summary>
    public class BadgeManager
    {
        #region ---------- Static Methods ----------

        /// <summary>
        /// Creates a new Challenge and adds it to the collection.
        /// </summary>
        /// <param name="user">User to whom the Badge belongs.</param>
        /// <param name="statistic">Statistic being tracked by the badge.</param>
        /// <returns>Database ID for the created badge.</returns>
        public static Badge CreateBadge(User user, Statistic statistic)
        {
            Badge newBadge = new Badge(user, statistic);
            return BadgeDAO.GetBadgeFromBadgeId(BadgeDAO.CreateNewBadge(newBadge));
        }

        #endregion ---------- Public Methods ----------
    }
}