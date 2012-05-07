using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Server.Service;
using ActivEarth.Server.Service.Competition;

namespace ActivEarth.DAO
{
    public class ChallengeDAO
    {
        /// <summary>
        /// Retrieves a Challenge from the DB based on its ID.
        /// </summary>
        /// <param name="challengeId">Identifier of the challenge to retrieve.</param>
        /// <param name="userId">Optional. UserID to load challenge progress from.</param>
        /// <returns>Challenge specified by the provided ID.</returns>
        public static Challenge GetChallengeFromChallengeId(int challengeId, int userId = 0)
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
                                Reward = c.reward,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active,
                                ImagePath = c.image_path
                            }).FirstOrDefault();

                toReturn.Progress = (userId > 0 ? ChallengeManager.GetProgress(toReturn.ID, userId) : 0);
                toReturn.FormatString = StatisticInfoDAO.GetStatisticFormatString(toReturn.StatisticBinding);

                return toReturn;
            }
        }

        /// <summary>
        /// Retrieves all currently active challenges.
        /// </summary>
        /// <param name="userId">Optional. UserID to load challenge progress from.</param>
        /// <returns>All challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveChallenges(int userId = 0)
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
                                Reward = c.reward,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active,
                                ImagePath = c.image_path
                            }).ToList();

                foreach (Challenge challenge in toReturn)
                {
                    challenge.Progress = (userId > 0 ? ChallengeManager.GetProgress(challenge.ID, userId) : 0);
                    challenge.FormatString = StatisticInfoDAO.GetStatisticFormatString(challenge.StatisticBinding);
                }

                return toReturn;
            }
        }

        /// <summary>
        /// Retrieves all currently active daily challenges.
        /// </summary>
        /// <param name="userId">Optional. UserID to load challenge progress from.</param>
        /// <returns>All daily challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveDailyChallenges(int userId = 0)
        {
            return GetActiveChallengesByDuration(1, userId);
        }

        /// <summary>
        /// Retrieves all currently active weekly challenges.
        /// </summary>
        /// <param name="userId">Optional. UserID to load challenge progress from.</param>
        /// <returns>All weekly challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveWeeklyChallenges(int userId = 0)
        {
            return GetActiveChallengesByDuration(7, userId);
        }

        /// <summary>
        /// Retrieves all currently active monthly challenges.
        /// </summary>
        /// <param name="userId">Optional. UserID to load challenge progress from.</param>
        /// <returns>All monthly challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveMonthlyChallenges(int userId = 0)
        {
            return GetActiveChallengesByDuration(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), userId);
        }

        /// <summary>
        /// Retrieves all currently active persistent challenges.
        /// </summary>
        /// <returns>All persistent challenges currently marked as active.</returns>
        public static List<Challenge> GetActivePersistentChallenges(int userId = 0)
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
                                Reward = c.reward,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active,
                                ImagePath = c.image_path
                            }).ToList();

                foreach (Challenge challenge in toReturn)
                {
                    challenge.Progress = (userId > 0 ? ChallengeManager.GetProgress(challenge.ID, userId) : 0);
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
                                Reward = c.reward,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active,
                                ImagePath = c.image_path
                            }).ToList();

                foreach (Challenge challenge in toReturn)
                {
                    challenge.FormatString = StatisticInfoDAO.GetStatisticFormatString(challenge.StatisticBinding);
                }

                return toReturn;
            }
        }

        /// <summary>
        /// Lookup for the statistic being tracked by a specific challenge.
        /// </summary>
        /// <param name="challengeId">ID of the challenge to query.</param>
        /// <returns>Statistic being watched by the challenge.</returns>
        public static Statistic GetStatisticFromChallengeId(int challengeId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (Statistic)(from c in data.ChallengeDataProviders
                                      where c.id == challengeId
                                      select c.statistic).FirstOrDefault();
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
                            reward = challenge.Reward,
                            requirement = challenge.Requirement,
                            persistent = challenge.IsPersistent,
                            end_time = challenge.EndTime,
                            duration_days = challenge.Duration.Days,
                            statistic = (byte)challenge.StatisticBinding,
                            active = challenge.IsActive,
                            image_path = challenge.ImagePath
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
                        dbChallenge.reward = challenge.Reward;
                        dbChallenge.requirement = challenge.Requirement;
                        dbChallenge.persistent = challenge.IsPersistent;
                        dbChallenge.end_time = challenge.EndTime;
                        dbChallenge.duration_days = challenge.Duration.Days;
                        dbChallenge.statistic = (byte)challenge.StatisticBinding;
                        dbChallenge.active = challenge.IsActive;
                        dbChallenge.image_path = challenge.ImagePath;

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
        /// Creates a new user initialization entry for a challenge.
        /// </summary>
        /// <param name="challengeId">ID of the challenge the user is participating in.</param>
        /// <param name="userId">ID of the user being initialized.</param>
        /// <param name="value">Current value of the relevant statistic for the challenge.</param>
        /// <returns></returns>
        public static bool CreateInitializationEntry(int challengeId, int userId, float value)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var challengeData = new ChallengeInitializationDataProvider
                    {
                        challenge_id = challengeId,
                        user_id = userId,
                        value = value
                    };
                    data.ChallengeInitializationDataProviders.InsertOnSubmit(challengeData);
                    data.SubmitChanges();
                    return (challengeData.id > 0);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes all initialization entries for a challenge (to clean up DB upon challenge expiration).
        /// </summary>
        /// <param name="challengeId">ID of the challenge whose data shall be removed.</param>
        /// <returns></returns>
        public static bool RemoveInitializationValues(int challengeId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var toDelete = (from c in data.ChallengeInitializationDataProviders
                                where c.challenge_id == challengeId
                                select c).ToList();

                    foreach (ChallengeInitializationDataProvider row in toDelete)
                    {
                        data.ChallengeInitializationDataProviders.DeleteOnSubmit(row);
                    }

                    data.SubmitChanges();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the stored initial value for a user in a particular challenge.
        /// </summary>
        /// <param name="challengeId">ID of the challenge to look up.</param>
        /// <param name="userId">ID of the user whose value shall be returned.</param>
        /// <returns></returns>
        public static float GetInitializationValue(int challengeId, int userId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var initval = (from c in data.ChallengeInitializationDataProviders
                                    where (c.challenge_id == challengeId) && (c.user_id == userId)
                                    select c).FirstOrDefault();

                    return (initval != null ? (float)initval.value : -1);
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Creates a number of random challenges by reading in from the Challenge_Definitions SQL table.
        /// </summary>
        /// <param name="type">Type of challenges desired (Daily/Weekly/Monthly).</param>
        /// <param name="count">Number of challenges needing to be created.</param>
        /// <returns></returns>
        public static List<Challenge> GenerateRandomChallenges(ChallengeType type, int count)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    List<Challenge> challenges = new List<Challenge>();

                    var data = new ActivEarthDataProvidersDataContext(connection);
                    List<Challenge> templates = (from c in data.ChallengeDefinitionDataProviders
                                               where (c.challenge_type == (byte)type && c.persistent == false)
                                               select new Challenge()
                                               {
                                                   IsPersistent = c.persistent,
                                                   StatisticBinding = (Statistic)c.statistic,
                                                   Requirement = (float)c.requirement,
                                                   Reward = c.reward,
                                                   Description = c.description,
                                                   Name = (c.name == null ? String.Empty : c.name),
                                                   ImagePath = c.image_path
                                               }).ToList();

                    Random rand = new Random();

                    while (challenges.Count < count)
                    {
                        int i = rand.Next(templates.Count);

                        if (!challenges.Contains(templates[i]))
                        {
                            string formatString = StatisticInfoDAO.GetStatisticFormatString(templates[i].StatisticBinding);
                            string requirementString = templates[i].Requirement.ToString(formatString);

                            templates[i].Description = String.Format(templates[i].Description, requirementString);

                            challenges.Add(templates[i]);
                        }
                    }

                    return challenges;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region Private Methods

        /// <summary>
        /// Retrieves all currently active challenges of a given length.
        /// </summary>
        /// <param name="days">Duration of the challenge in days.</param>
        /// <param name="userId">Optional. UserID to load progress from.</param>
        /// <returns>List of active challenges with the specified duration.</returns>
        private static List<Challenge> GetActiveChallengesByDuration(int days, int userId = 0)
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
                                Reward = c.reward,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active,
                                ImagePath = c.image_path
                            }).ToList();

                foreach (Challenge challenge in toReturn)
                {
                    challenge.Progress = (userId > 0 ? ChallengeManager.GetProgress(challenge.ID, userId) : 0);
                    challenge.FormatString = StatisticInfoDAO.GetStatisticFormatString(challenge.StatisticBinding);
                }

                return toReturn;
            }
        }

        #endregion Private Methods

    }
}