using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Server.Service;
using ActivEarth.DAO;

namespace ActivEarth.Server.Service.DAO
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
                                       StatisticBinding = (Statistic)c.statistic,
                                       Level = c.badge_level,
                                       User = UserDAO.GetUserFromUserId(c.user_id),
                                       Progress = c.progress
                                   }).ToList();

                if (badges != null)
                {
                    foreach (Badge badge in badges)
                    {
                        LoadBadgeConstants(badge);
                    }
                }

                return badges;
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
                        user_id = badge.User.UserID,
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
                        dbBadge.user_id = badge.User.UserID;
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
        /// Loads the requirements, rewards, and imagepaths arrays for a badge based on
        /// the bound statistic.
        /// </summary>
        /// <param name="badge">The badge to load information for.</param>
        private static void LoadBadgeConstants(Badge badge)
        {
            switch (badge.StatisticBinding)
            {
                case Statistic.BikeDistance:
                {
                    badge.ImagePaths = BadgeConstants.BikeDistance.IMAGES;
                    badge.LevelRequirements = BadgeConstants.BikeDistance.REQUIREMENTS;
                    badge.LevelRewards = BadgeConstants.BikeDistance.REWARDS;
                    break;
                }
                case Statistic.WalkDistance:
                {
                    badge.ImagePaths = BadgeConstants.WalkDistance.IMAGES;
                    badge.LevelRequirements = BadgeConstants.WalkDistance.REQUIREMENTS;
                    badge.LevelRewards = BadgeConstants.WalkDistance.REWARDS;
                    break;
                }
                case Statistic.RunDistance:
                {
                    badge.ImagePaths = BadgeConstants.RunDistance.IMAGES;
                    badge.LevelRequirements = BadgeConstants.RunDistance.REQUIREMENTS;
                    badge.LevelRewards = BadgeConstants.RunDistance.REWARDS;
                    break;
                }
                case Statistic.Steps:
                {
                    badge.ImagePaths = BadgeConstants.Steps.IMAGES;
                    badge.LevelRequirements = BadgeConstants.Steps.REQUIREMENTS;
                    badge.LevelRewards = BadgeConstants.Steps.REWARDS;
                    break;
                }
                case Statistic.ChallengesCompleted:
                {
                    badge.ImagePaths = BadgeConstants.Challenges.IMAGES;
                    badge.LevelRequirements = BadgeConstants.Challenges.REQUIREMENTS;
                    badge.LevelRewards = BadgeConstants.Challenges.REWARDS;
                    break;
                }
                case Statistic.GasSavings:
                {
                    badge.ImagePaths = BadgeConstants.GasSavings.IMAGES;
                    badge.LevelRequirements = BadgeConstants.GasSavings.REQUIREMENTS;
                    badge.LevelRewards = BadgeConstants.GasSavings.REWARDS;
                    break;
                }
            }
        }
    }
}