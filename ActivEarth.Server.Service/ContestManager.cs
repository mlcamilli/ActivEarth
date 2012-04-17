using System;
using System.Collections.Generic;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.DAO
{

    public class ContestManager
    {
        #region ---------- Constructor ----------

        /// <summary>
        /// Private constructor, can not instantiate.
        /// </summary>
        private ContestManager()
        {
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Creates a new time-based Contest
        /// </summary>
        /// <param name="name">Contest Name.</param>
        /// <param name="description">Contest Description.</param>
        /// <param name="points">Points to be distributed to the winner(s).</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">Time to end the contest.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <returns>ID of the newly created Contest.</returns>
        public static int CreateContest(ContestType type, string name, string description, int points, DateTime start,
            DateTime end, bool searchable, Statistic statistic)
        {
            Contest newContest = new Contest
            {
                Name = name,
                Description = description,
                Reward = points,
                Mode = ContestEndMode.TimeBased,
                Type = type,
                StartTime = start,
                EndCondition = new EndCondition(end),
                StatisticBinding = statistic,
                IsSearchable = searchable,
                IsActive = true,
                DeactivatedTime = null
            };

            return ContestDAO.CreateNewContest(newContest);
        }

        /// <summary>
        /// Creates a new goal-based Contest
        /// </summary>
        /// <param name="name">Contest Name.</param>
        /// <param name="description">Contest Description.</param>
        /// <param name="points">Points to be distributed to the winner(s).</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">Score at which to end the contest.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <returns>ID of the newly created Contest.</returns>
        public static int CreateContest(ContestType type, string name, string description, int points, DateTime start,
            float end, bool searchable, Statistic statistic)
        {
            Contest newContest = new Contest
            {
                Name = name,
                Description = description,
                Reward = points,
                Mode = ContestEndMode.GoalBased,
                Type = type,
                StartTime = start,
                EndCondition = new EndCondition(end),
                StatisticBinding = statistic,
                IsSearchable = searchable
            };

            return ContestDAO.CreateNewContest(newContest);
        }

        /// <summary>
        /// Retrieves a Contest based on its ID.
        /// </summary>
        /// <param name="id">ID of the Contest to be retrieved.</param>
        /// <returns>Contest with ID matching the provided ID.</returns>
        public static Contest GetContest(int id)
        {
            return ContestDAO.GetContestFromContestId(id);
        }

        /// <summary>
        /// Calculates the reward for a time-based contest.
        /// </summary>
        /// <param name="duration">Duration of the contest.</param>
        /// <param name="statistic">Statistic on which the contest is based.</param>
        /// <returns></returns>
        public static int CalculateContestReward(TimeSpan duration, Statistic statistic)
        {
            return 0;
        }

        /// <summary>
        /// Calculates the reward for a goal-based contest.
        /// </summary>
        /// <param name="goal">Goal value for the contest.</param>
        /// <param name="statistic">Statistic on which the contest is based.</param>
        /// <returns></returns>
        public static int CalculateContestReward(float goal, Statistic statistic)
        {
            return 0;
        }

        /// <summary>
        /// Cleans up the contest list, deactivating expired time-based contests and deleting
        /// deactivated contests that have reached the end of their retainment period.
        /// </summary>
        public void CleanUp()
        {
            foreach (Contest contest in ContestDAO.GetActiveContests())
            {
                if (contest.EndCondition.EndTime <= DateTime.Now)
                {
                    //Distribute Activity Score rewards
                    throw new NotImplementedException("Distribution of Contest rewards not yet implemented");

                    contest.IsActive = false;
                    contest.DeactivatedTime = DateTime.Now;
                    ContestDAO.UpdateContest(contest);
                }
            }

            ContestDAO.RemoveOldContests();
        }

        #endregion ---------- Public Methods ----------
    }
}