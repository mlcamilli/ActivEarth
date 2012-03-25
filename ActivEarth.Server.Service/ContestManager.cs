using System;
using System.Collections.Generic;
using System.Linq;

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
            DateTime end, Placeholder.Statistic statistic)
        {
            return ContestDAO.CreateNewContest(new Contest(name, description, points, ContestEndMode.TimeBased,
                type, start, new EndCondition(end), statistic));
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
            float end, Placeholder.Statistic statistic)
        {
            return ContestDAO.CreateNewContest(new Contest(name, description, points, ContestEndMode.GoalBased,
                type, start, new EndCondition(end), statistic));
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

        #endregion ---------- Public Methods ----------
    }
}