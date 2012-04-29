using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Challenges;

using ActivEarth.DAO;

namespace ActivEarth.Server.Service.Competition
{
    /// <summary>
    /// Serves as a wrapper class for the ChallengeDAO class, further abstracting actions.
    /// </summary>
    public class ChallengeManager
    {
        /// <summary>
        /// Private constructor, can't instantiate class.
        /// </summary>
        private ChallengeManager()
        {
        }

        #region ---------- Static Methods ----------

        /// <summary>
        /// Creates a new Challenge and adds it to the collection.
        /// </summary>
        /// <param name="name">Challenge Name.</param>
        /// <param name="description">Challenge Description.</param>
        /// <param name="points">Points to be awarded upon completion of the Challenge.</param>
        /// <param name="start">Time at which the challenge should begin.</param>
        /// <param name="durationInDays">Number of days that the challenge should be active.</param>
        /// <param name="persistent">True if the Challenge is persistent, false otherwise.</param>
        /// <param name="statistic">Statistic on which the Challenge is based.</param>
        /// <param name="requirement">Statistic value required to complete the challenge.</param>
        /// <returns></returns>
        public static int CreateChallenge(string name, string description, int points, bool persistent,
            DateTime start, int durationInDays, Statistic statistic, float requirement)
        {
            Challenge newChallenge = new Challenge(name, description, points, persistent,
                    start, durationInDays, statistic, requirement);

            int id = ChallengeDAO.CreateNewChallenge(newChallenge);

            return id;
        }

        /// <summary>
        /// Retrieves an active challenge based on its ID.
        /// </summary>
        /// <param name="id">ID of the Challenge to be retrieved.</param>
        /// false to search only active challenges.</param>
        /// <returns>Challenge with ID matching the provided ID, null if no match is found.</returns>
        public static Challenge GetChallenge(int id)
        {
            return ChallengeDAO.GetChallengeFromChallengeId(id);
        }

        /// <summary>
        /// Cleans up the challenge list, moving expired transient challenges to the archive
        /// and refreshing persistent challenges. Should be called daily at the challenge cutoff time.
        /// </summary>
        public static void CleanUp()
        {
            foreach (Challenge challenge in ChallengeDAO.GetActiveChallenges())
            {
                if (challenge.EndTime <= DateTime.Now)
                {
                    ChallengeDAO.RemoveInitializationValues(challenge.ID);

                    if (challenge.IsPersistent)
                    {
                        challenge.EndTime = challenge.EndTime.Add(challenge.Duration);
                    }
                    else
                    {
                        challenge.IsActive = false;
                    }

                    ChallengeDAO.UpdateChallenge(challenge);
                }
            }
        }

        /// <summary>
        /// Sets a user's initial value for a challenge (to allow calculation of the
        /// user's delta from the beginning of the challenge).
        /// </summary>
        public static void InitializeUser(int challengeId, int userId)
        {
            Statistic statistic = ChallengeDAO.GetStatisticFromChallengeId(challengeId);

            if (ChallengeDAO.GetInitializationValue(challengeId, userId) < 0)
            {

                UserStatistic userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, statistic);

                if (userStat == null)
                {
                    UserStatisticDAO.CreateNewStatisticForUser(userId, statistic, 0);
                    userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, statistic);
                };

                ChallengeDAO.CreateInitializationEntry(challengeId, userId, userStat.Value);
            }
        }


        /// <summary>
        /// Gets the progress made by a user in the challenge as a percentage (0-100).
        /// </summary>
        /// <param name="challengeId">The challenge to evaluate progress for.</param>
        /// <param name="userId">The user to evaluate.</param>
        /// <returns>User's progress in the challenge (for use in a progress bar).</returns>
        public static int GetProgress(int challengeId, int userId)
        {
            Challenge challenge = ChallengeDAO.GetChallengeFromChallengeId(challengeId);

            float initial = ChallengeDAO.GetInitializationValue(challengeId, userId);
            UserStatistic current = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, challenge.StatisticBinding);

            if (initial >= 0 && current != null)
            {
                return Math.Min((int)(100 * (current.Value - initial) / challenge.Requirement), 100);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the formatted text progress report for the Challenge (e.g., "34.5 / 40.0").
        /// </summary>
        /// <param name="challengeId">Challenge to report progress for.</param>
        /// <param name="userId">User to evaluate.</param>
        /// <returns>Formatted text progress report for the Challenge.</returns>
        public static string GetFormattedProgress(int challengeId, int userId)
        {
            Challenge challenge = ChallengeDAO.GetChallengeFromChallengeId(challengeId);
            UserStatistic statistic = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, challenge.StatisticBinding);

            string numerator = (statistic != null ? statistic.Value : 0).ToString(challenge.FormatString);
            string denominator = challenge.Requirement.ToString(challenge.FormatString);

            return String.Format("{0} / {1}", numerator, denominator);
        }

        /// <summary>
        /// Returns true if the user has met the requirements to complete the challenge,
        /// false otherwise.
        /// </summary>
        /// <param name="challengeId">Challenge to test for completion.</param>
        /// <param name="userId">User to evaluate.</param>
        /// <returns>Whether or not the user has completed the challenge.</returns>
        public static bool IsComplete(int challengeId, int userId)
        {
            return (ChallengeManager.GetProgress(challengeId, userId) == 100);
        }

        /// <summary>
        /// Creates new challenges such that the correct number of each type of challenge (daily, weekly, monthly)
        /// are active.
        /// </summary>
        public static void GenerateNewChallenges()
        {
            int numberOfDailyChallenges = 3;
            int numberOfWeeklyChallenges = 3;
            int numberOfMonthlyChallenges = 3;

            List<Challenge> dailyChallenges = ChallengeDAO.GetActiveDailyChallenges();
            List<Challenge> weeklyChallenges = ChallengeDAO.GetActiveWeeklyChallenges();
            List<Challenge> monthlyChallenges = ChallengeDAO.GetActiveMonthlyChallenges();

            if (dailyChallenges.Count < numberOfDailyChallenges)
            {
                List<Challenge> newDailyChallenges =
                    ChallengeDAO.GenerateRandomChallenges(ChallengeType.Daily, 
                    numberOfDailyChallenges - dailyChallenges.Count);

                foreach (Challenge newDailyChallenge in newDailyChallenges)
                {
                    newDailyChallenge.Duration = new TimeSpan(1, 0, 0, 0);
                    newDailyChallenge.EndTime = DateTime.Today.Add(newDailyChallenge.Duration);
                    newDailyChallenge.IsActive = true;

                    ChallengeDAO.CreateNewChallenge(newDailyChallenge);
                }
            }

            if (weeklyChallenges.Count < numberOfWeeklyChallenges)
            {
                List<Challenge> newWeeklyChallenges = 
                    ChallengeDAO.GenerateRandomChallenges(ChallengeType.Weekly, 
                    numberOfWeeklyChallenges - weeklyChallenges.Count);

                foreach (Challenge newWeeklyChallenge in newWeeklyChallenges)
                {
                    newWeeklyChallenge.Duration = new TimeSpan(7, 0, 0, 0);
                    newWeeklyChallenge.EndTime = DateTime.Today.Add(newWeeklyChallenge.Duration).AddDays(-(int)DateTime.Today.DayOfWeek);
                    newWeeklyChallenge.IsActive = true;

                    ChallengeDAO.CreateNewChallenge(newWeeklyChallenge);
                }
            }

            if (monthlyChallenges.Count < numberOfMonthlyChallenges)
            {
                List<Challenge> newMonthlyChallenges = 
                    ChallengeDAO.GenerateRandomChallenges(ChallengeType.Monthly, 
                    numberOfMonthlyChallenges - monthlyChallenges.Count);

                foreach (Challenge newMonthlyChallenge in newMonthlyChallenges)
                {
                    newMonthlyChallenge.Duration = new TimeSpan(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 0, 0, 0);
                    newMonthlyChallenge.EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).Add(newMonthlyChallenge.Duration);
                    newMonthlyChallenge.IsActive = true;

                    ChallengeDAO.CreateNewChallenge(newMonthlyChallenge);
                }
            }
        }

        #endregion ---------- Public Methods ----------
    }
}