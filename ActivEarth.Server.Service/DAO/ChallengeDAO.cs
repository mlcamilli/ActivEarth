using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        public static Challenge GetChallengeFromChallengeId(uint challengeId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.ChallengeDataProviders
                        where c.id == challengeId
                        select
                            new Challenge
                            {
                                ID = (uint)c.id,
                                Name = c.name,
                                Description = c.description,
                                Points = c.points,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Placeholder.Statistic)c.statistic,
                                IsActive = c.active
                            }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves all currently active challenges.
        /// </summary>
        /// <returns>All challenges currently marked as active.</returns>
        public static List<Challenge> GetActiveChallenges()
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.ChallengeDataProviders
                        where c.active == true
                        select
                            new Challenge
                            {
                                ID = (uint)c.id,
                                Name = c.name,
                                Description = c.description,
                                Points = c.points,
                                Requirement = (float)c.requirement,
                                IsPersistent = c.persistent,
                                EndTime = c.end_time,
                                Duration = new TimeSpan(c.duration_days, 0, 0, 0),
                                StatisticBinding = (Placeholder.Statistic)c.statistic,
                                IsActive = c.active
                            }).ToList();
            }
        }

        /// <summary>
        /// Saves a challenge as a new entry in the DB.
        /// </summary>
        /// <param name="challenge">Challenge object to add to the DB.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool CreateNewChallenge(Challenge challenge)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var challengeData = new ChallengeDataProvider 
                        {
                            id = (int)challenge.ID,
                            name = challenge.Name,
                            description = challenge.Description,
                            points = challenge.Points,
                            requirement = challenge.Requirement,
                            persistent = challenge.IsPersistent,
                            end_time = challenge.EndTime,
                            duration_days = challenge.Duration.Days,
                            statistic = (byte)challenge.StatisticBinding,
                            active = challenge.IsActive
                        };
                    data.ChallengeDataProviders.InsertOnSubmit(challengeData);
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
                        dbChallenge.points = challenge.Points;
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

    }
}