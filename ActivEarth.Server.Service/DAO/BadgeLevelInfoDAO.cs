using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class BadgeLevelInfoDAO
    {
        /// <summary>
        /// Retrieves the statistic requirement for a particular badge.
        /// </summary>
        /// <param name="stat">Statistic for which the badge is awarded.</param>
        /// <param name="badgeLevel">Desired level of the badge.</param>
        /// <returns>Statistic requirement for the given badge.</returns>
        public static float GetBadgeRequirement(Statistic stat, int badgeLevel)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                float foundEntry = (from c in data.BadgeConstantsDataProviders
                                      where c.statistic == (byte)stat && c.level == badgeLevel
                                      select (float)c.requirement).FirstOrDefault();

                return (foundEntry >= 0 ? (float)foundEntry : float.PositiveInfinity);
            }
        }

        /// <summary>
        /// Retrieves the statistic requirements for all levels of a particular badge.
        /// </summary>
        /// <param name="stat">Statistic for which the badge is awarded.</param>
        /// <returns>Statistic requirement for the given badge.</returns>
        public static float[] GetBadgeRequirementArray(Statistic stat)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.BadgeConstantsDataProviders
                                    where c.statistic == (byte)stat
                                    select (float)c.requirement).ToArray();
            }
        }

        /// <summary>
        /// Retrieves the reward for a particular badge.
        /// </summary>
        /// <param name="stat">Statistic for which the badge is awarded.</param>
        /// <param name="badgeLevel">Desired level of the badge.</param>
        /// <returns>Reward for the given badge.</returns>
        public static float GetBadgeReward(Statistic stat, int badgeLevel)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.BadgeConstantsDataProviders
                                    where c.statistic == (byte)stat && c.level == badgeLevel
                                    select (float)c.reward).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves the Activity Score rewards for all levels of a particular badge.
        /// </summary>
        /// <param name="stat">Statistic for which the badge is awarded.</param>
        /// <returns>Rewards for the given badge.</returns>
        public static int[] GetBadgeRewardArray(Statistic stat)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.BadgeConstantsDataProviders
                        where c.statistic == (byte)stat
                        select c.reward).ToArray();
            }
        }

        /// <summary>
        /// Retrieves the image path for a particular badge.
        /// </summary>
        /// <param name="stat">Statistic for which the badge is awarded.</param>
        /// <param name="badgeLevel">Desired level of the badge.</param>
        /// <returns>Reward for the given badge.</returns>
        public static string GetBadgeImagePath(Statistic stat, int badgeLevel)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.BadgeConstantsDataProviders
                                    where c.statistic == (byte)stat && c.level == badgeLevel
                                    select c.image_path).FirstOrDefault();
            }
        }
    }
}