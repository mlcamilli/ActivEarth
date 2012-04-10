using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class ChallengeDAO
    {
        /// <summary>
        /// Retrieves a Challenge from the DB based on its ID.
        /// </summary>
        /// <param name="challengeId">Identifier of the challenge to retrieve.</param>
        /// <returns>Challenge specified by the provided ID.</returns>
        public static Challenge GetChallengeFromChallengeId(int challengeId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                Challenge toReturn = (from c in data.ChallengeDataProviders
                        where c.id == challengeId
                        select
                            new Challenge
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Reward = c.points,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active
                            }).FirstOrDefault();

                toReturn.FormatString = StatisticInfoDAO.GetStatisticFormatString(toReturn.StatisticBinding);

                return toReturn;
            }
        }

        /// <summary>
        /// Retrieves all currently active challenges.
        /// </summary>
        /// <returns>All challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveChallenges()
        {
            List<Challenge> toReturn;

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                toReturn = (from c in data.ChallengeDataProviders
                        where c.active
                        select
                            new Challenge
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Reward = c.points,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active
                            }).ToList();

                foreach (Challenge challenge in toReturn)
                {
                    challenge.FormatString = StatisticInfoDAO.GetStatisticFormatString(challenge.StatisticBinding);
                }

                return toReturn;
            }
        }

        /// <summary>
        /// Retrieves all currently active daily challenges.
        /// </summary>
        /// <returns>All daily challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveDailyChallenges()
        {
            return GetActiveChallengesByDuration(1);
        }

        /// <summary>
        /// Retrieves all currently active weekly challenges.
        /// </summary>
        /// <returns>All weekly challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveWeeklyChallenges()
        {
            return GetActiveChallengesByDuration(7);
        }

        /// <summary>
        /// Retrieves all currently active monthly challenges.
        /// </summary>
        /// <returns>All monthly challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveMonthlyChallenges()
        {
            return GetActiveChallengesByDuration(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        /// <summary>
        /// Retrieves all currently active persistent challenges.
        /// </summary>
        /// <returns>All persistent challenges currently marked as active.</returns>
        public static List<Challenge> GetActivePersistentChallenges()
        {
            List<Challenge> toReturn;
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                toReturn = (from c in data.ChallengeDataProviders
                        where c.active && c.persistent
                        select
                            new Challenge
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Reward = c.points,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active
                            }).ToList();

                foreach (Challenge challenge in toReturn)
                {
                    challenge.FormatString = StatisticInfoDAO.GetStatisticFormatString(challenge.StatisticBinding);
                }

                return toReturn;
            }
        }

        /// <summary>
        /// Retrieves all challenges in the archive.
        /// </summary>
        /// <returns>All challenges in the archive.</returns>
        public static List<Challenge> GetAllChallenges()
        {
            List<Challenge> toReturn;

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                toReturn = (from c in data.ChallengeDataProviders
                        where c.id >= 0
                        select
                            new Challenge
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Reward = c.points,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active
                            }).ToList();

                foreach (Challenge challenge in toReturn)
                {
                    challenge.FormatString = StatisticInfoDAO.GetStatisticFormatString(challenge.StatisticBinding);
                }

                return toReturn;
            }
        }

        /// <summary>
        /// Saves a challenge as a new entry in the DB.
        /// </summary>
        /// <param name="challenge">Challenge object to add to the DB.</param>
        /// <returns>ID of the created challenge on success, 0 on failure.</returns>
        public static int CreateNewChallenge(Challenge challenge)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var challengeData = new ChallengeDataProvider 
                        {
                            name = challenge.Name,
                            description = challenge.Description,
                            points = challenge.Reward,
                            requirement = challenge.Requirement,
                            persistent = challenge.IsPersistent,
                            end_time = challenge.EndTime,
                            duration_days = challenge.Duration.Days,
                            statistic = (byte)challenge.StatisticBinding,
                            active = challenge.IsActive
                        };
                    data.ChallengeDataProviders.InsertOnSubmit(challengeData);
                    data.SubmitChanges();
                    return challengeData.id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Updates an existing Challenge in the DB.
        /// </summary>
        /// <param name="challenge">Challenge whose record needs updating.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdateChallenge(Challenge challenge)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    ChallengeDataProvider dbChallenge =
                        (from c in data.ChallengeDataProviders where c.id == challenge.ID select c).FirstOrDefault();
                    if (dbChallenge != null)
                    {
                        dbChallenge.name = challenge.Name;
                        dbChallenge.description = challenge.Description;
                        dbChallenge.points = challenge.Reward;
                        dbChallenge.requirement = challenge.Requirement;
                        dbChallenge.persistent = challenge.IsPersistent;
                        dbChallenge.end_time = challenge.EndTime;
                        dbChallenge.duration_days = challenge.Duration.Days;
                        dbChallenge.statistic = (byte)challenge.StatisticBinding;
                        dbChallenge.active = challenge.IsActive;

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

        #region Private Methods

        /// <summary>
        /// Retrieves all currently active challenges of a given length.
        /// </summary>
        /// <returns>Duration of the challenge in days.</returns>
        private static List<Challenge> GetActiveChallengesByDuration(int days)
        {
            List<Challenge> toReturn;

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                toReturn = (from c in data.ChallengeDataProviders
                        where c.active && c.duration_days == days
                        select
                            new Challenge
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Reward = c.points,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active
                            }).ToList();

                foreach (Challenge challenge in toReturn)
                {
                    challenge.FormatString = StatisticInfoDAO.GetStatisticFormatString(challenge.StatisticBinding);
                }

                return toReturn;
            }
        }

        #endregion Private Methods

    }
}