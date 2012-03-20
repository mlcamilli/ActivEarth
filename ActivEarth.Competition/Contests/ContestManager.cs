using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Contests
{

    public class ContestManager
    {
        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new Contest Manager
        /// </summary>
        public ContestManager()
        {
            //Both should ultimately be read in from DB
            _activeContests = new List<Contest>();
            _nextID = 1;
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
        public uint CreateContest(ContestType type, string name, string description, int points, DateTime start,
            DateTime end, Placeholder.Statistic statistic)
        {
            uint id = _nextID;
            _nextID++;

            if (type == ContestType.Group)
            {
                _activeContests.Add(new GroupContest(id, name, description, points, ContestEndMode.TimeBased,
                    start, new EndCondition(end), statistic));
            }
            else
            {
                _activeContests.Add(new IndividualContest(id, name, description, points, ContestEndMode.TimeBased,
                    start, new EndCondition(end), statistic));
            }

            return id;
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
        public uint CreateContest(ContestType type, string name, string description, int points, DateTime start,
            float end, Placeholder.Statistic statistic)
        {
            uint id = _nextID;
            _nextID++;

            if (type == ContestType.Group)
            {
                _activeContests.Add(new GroupContest(id, name, description, points, ContestEndMode.GoalBased,
                    start, new EndCondition(end), statistic));
            }
            else
            {
                _activeContests.Add(new IndividualContest(id, name, description, points, ContestEndMode.GoalBased,
                    start, new EndCondition(end), statistic));
            }

            return id;
        }

        /// <summary>
        /// Retrieves a Contest based on its ID.
        /// </summary>
        /// <param name="id">ID of the Contest to be retrieved.</param>
        /// <returns>Contest with ID matching the provided ID.</returns>
        public Contest GetContest(uint id)
        {
            var query = from Contest contest in _activeContests
                        where contest.ID == id
                        select contest;

            foreach (Contest contest in query)
            {
                return contest;
            }

            //If we haven't returned a contest by now, we didn't find any that matched.
            return null;
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// List of currently active Contests.
        /// </summary>
        private List<Contest> _activeContests;

        /// <summary>
        /// ID for the next created contest.
        /// </summary>
        private uint _nextID;

        #endregion ---------- Private Fields ----------
    }
}