using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;

using ActivEarth.DAO;
using ActivEarth.Server.Service.Statistics;

namespace ActivEarth.Server.Service.Competition
{
    /// <summary>
    /// Serves as a wrapper class for the BadgeDAO class, further abstracting actions.
    /// </summary>
    public class BadgeManager
    {
        #region ---------- Static Methods ----------

        /// <summary>
        /// Creates a new Badge and adds it to the collection.
        /// </summary>
        /// <param name="user">User to whom the Badge belongs.</param>
        /// <param name="statistic">Statistic being tracked by the badge.</param>
        /// <returns>Database ID for the created badge.</returns>
        public static Badge CreateBadge(int userId, Statistic statistic)
        {
            Badge newBadge = new Badge(userId, statistic);
            return BadgeDAO.GetBadgeFromBadgeId(BadgeDAO.CreateNewBadge(newBadge));
        }

        /// <summary>
        /// Updates the badge to reflect a change in statistics.
        /// </summary>
        /// <param name="userId">User to update.</param>
        /// <param name="statistic">Statistic tracked by the badge to be updated.</param>
        public static int UpdateBadge(int userId, Statistic statistic)
        {
            int pointsEarned = 0;

            Badge badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(userId, statistic);

            int oldLevel = badge.Level;
            int newLevel = oldLevel;

            UserStatistic userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, statistic);

            if (userStat == null)
            {
                UserStatisticDAO.CreateNewStatisticForUser(userId, statistic, 0);
                userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, statistic);

                if (userStat == null) { return 0; }
            }

            float stat = userStat.value;

            while ((newLevel < BadgeLevels.Max) &&
                (stat >= badge.LevelRequirements[(int)newLevel + 1]))
            {
                newLevel++;
            }

            for (int i = oldLevel + 1; i <= newLevel; i++)
            {
                pointsEarned += badge.LevelRewards[i];
            }

            badge.Level = newLevel;

            if (badge.Level == BadgeLevels.Max)
            {
                badge.Progress = 100;
            }
            else
            {
                badge.Progress = (int)(100 * (stat - badge.LevelRequirements[newLevel]) /
                    (badge.LevelRequirements[newLevel + 1] - badge.LevelRequirements[newLevel]));
            }

            BadgeDAO.UpdateBadge(badge);

            return pointsEarned;
        }

        /// <summary>
        /// Returns the formatted text progress report for the Badge (e.g., "34.5 / 40.0").
        /// </summary>
        /// <returns>Formatted text progress report for the Badge.</returns>
        public static string GetFormattedProgress(int badgeId)
        {
            Badge badge = BadgeDAO.GetBadgeFromBadgeId(badgeId);

            if (badge == null) { return null; }

            UserStatistic userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(badge.UserID, badge.StatisticBinding);

            string numerator = String.Format(badge.FormatString, (userStat != null ? userStat.value : 0));

            if (badge.Level < BadgeLevels.Max)
            {
                string denominator = String.Format(badge.FormatString, badge.GetNextLevelRequirement());

                return String.Format("{0} / {1}", numerator, denominator);
            }
            else
            {
                return numerator;
            }
        }

        #endregion ---------- Public Methods ----------
    }
}