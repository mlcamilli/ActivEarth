using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Server.Service;
using ActivEarth.DAO;

namespace ActivEarth.DAO
{
    public class BadgeDAO
    {
        /// <summary>
        /// Retrieves the collection of badges belonging to a given user.
        /// </summary>
        /// <param name="badgeId">Identifier of the user.</param>
        /// <returns>Badges belonging to the user specified by the provided ID.</returns>
        public static List<Badge> GetBadgesFromUserId(int userId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                List<Badge> badges = (from c in data.BadgeDataProviders
                               where c.user_id == userId
                               select
                                   new Badge
                                   {
                                       ID = c.id,
                                       UserID = c.user_id,
                                       StatisticBinding = (Statistic)c.statistic,
                                       Level = c.badge_level,
                                       Progress = c.progress
                                   }).ToList();

                foreach (Badge badge in badges)
                {
                    LoadExternalBadgeData(badge);
                }

                return badges;
            }
        }

        /// <summary>
        /// Retrieves a badge matching a provided ID.
        /// </summary>
        /// <param name="badgeId">Identifier of the badge.</param>
        /// <returns>Badge matching the provided ID.</returns>
        public static Badge GetBadgeFromBadgeId(int badgeId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                Badge badge = (from c in data.BadgeDataProviders
                        where c.id == badgeId
                        select
                            new Badge
                            {
                                ID = c.id,
                                UserID = c.user_id,
                                StatisticBinding = (Statistic)c.statistic,
                                Level = c.badge_level,
                                Progress = c.progress
                            }).FirstOrDefault();

                LoadExternalBadgeData(badge);
                
                return badge;
            }
        }

        /// <summary>
        /// Retrieves a badge of a particular statistic for a specific user.
        /// </summary>
        /// <param name="userId">Identifier of the badge owner.</param>
        /// <param name="statistic">Statistic tracked by the badge.</param>
        /// <returns>Badge matching the provided ID.</returns>
        public static Badge GetBadgeFromUserIdAndStatistic(int userId, Statistic statistic)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                Badge badge = (from c in data.BadgeDataProviders
                               where c.user_id == userId && c.statistic == (byte)statistic
                               select
                                   new Badge
                                   {
                                       ID = c.id,
                                       UserID = c.user_id,
                                       StatisticBinding = (Statistic)c.statistic,
                                       Level = c.badge_level,
                                       Progress = c.progress
                                   }).FirstOrDefault();

                LoadExternalBadgeData(badge);

                return badge;
            }
        }

        /// <summary>
        /// Saves a badge as a new entry in the DB.
        /// </summary>
        /// <param name="badge">Badge object to add to the DB.</param>
        /// <returns>ID of the created badge on success, 0 on failure.</returns>
        public static int CreateNewBadge(Badge badge)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var badgeData = new BadgeDataProvider
                    {
                        user_id = badge.UserID,
                        badge_level = (byte)badge.Level,
                        progress = (byte)badge.Progress,
                        statistic = (byte)badge.StatisticBinding
                    };
                    data.BadgeDataProviders.InsertOnSubmit(badgeData);
                    data.SubmitChanges();
                    return badgeData.id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Updates an existing Badge in the DB.
        /// </summary>
        /// <param name="badge">Badge whose record needs updating.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdateBadge(Badge badge)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    BadgeDataProvider dbBadge =
                        (from c in data.BadgeDataProviders where c.id == badge.ID select c).FirstOrDefault();
                    if (dbBadge != null)
                    {
                        dbBadge.user_id = badge.UserID;
                        dbBadge.badge_level = (byte)badge.Level;
                        dbBadge.progress = (byte)badge.Progress;
                        dbBadge.statistic = (byte)badge.StatisticBinding;

                        data.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Loads the information stored in external tables for the badge (LevelRequirements, LevelRewards, etc.)
        /// </summary>
        /// <param name="badge">The badge to finish loading.</param>
        private static void LoadExternalBadgeData(Badge badge)
        {
            badge.LevelRequirements = BadgeLevelInfoDAO.GetBadgeRequirementArray(badge.StatisticBinding);
            badge.LevelRewards = BadgeLevelInfoDAO.GetBadgeRewardArray(badge.StatisticBinding);
            badge.ImagePath = BadgeLevelInfoDAO.GetBadgeImagePath(badge.StatisticBinding, badge.Level);
            badge.FormatString = StatisticInfoDAO.GetStatisticFormatString(badge.StatisticBinding);
            badge.Name = StatisticInfoDAO.GetStatisticName(badge.StatisticBinding);
        }
    }
}