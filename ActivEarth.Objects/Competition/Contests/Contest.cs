using System;
using System.Collections.Generic;
using System.Linq;

using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;

namespace ActivEarth.Objects.Competition.Contests
{
    public class Contest
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Numeric identifier for the Contest.
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The Contest's name, as a string.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The Contest's description, as a string.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// The number of Activity Points to be awarded to the victor(s).
        /// </summary>
        public int Reward
        {
            get;
            set;
        }

        /// <summary>
        /// The Contest mode, indicating whether the contest is time-based or goal-based.
        /// </summary>
        public ContestEndMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// The Contest type, indicating whether the contest is group-based or individual.
        /// </summary>
        public ContestType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Date and time when the Contest shall begin.
        /// </summary>
        public DateTime StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Goal target for a goal-based contest.
        /// </summary>
        public float? EndValue
        {
            get;
            set;
        }

        /// <summary>
        /// End time for a time-based contest.
        /// </summary>
        public DateTime? EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// List of competing teams in the competition, maintained in sorted order
        /// for reporting standings.
        /// </summary>
        public List<ContestTeam> Teams
        {
            get;
            set;
        }

        /// <summary>
        /// Statistic to which the contest is bound.
        /// </summary>
        public Statistic StatisticBinding
        {
            get;
            set;
        }

        /// <summary>
        /// Format string for reporting the contest statistic information.
        /// </summary>
        public string FormatString
        {
            get;
            set;
        }

        /// <summary>
        /// True if the contest is public and can be found by searching, false if private.
        /// </summary>
        public bool IsSearchable
        {
            get;
            set;
        }

        /// <summary>
        /// Whether or not the contest is currently running.
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// The time at which the contest was closed.
        /// </summary>
        public DateTime? DeactivatedTime
        {
            get;
            set;
        }

        /// <summary>
        /// UserId of the creator of the contest.
        /// </summary>
        public int CreatorId
        {
            get;
            set;
        }


        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Default constructor for loading contests from DB.
        /// </summary>
        public Contest()
        {
            this.Teams = new List<ContestTeam>();
        }

        #endregion ---------- Constructor ----------

        public string GetContestState()
        {
            if (StartTime > DateTime.Now)
            {
                return "SIGN-UP";
            }

            if (Mode == ContestEndMode.GoalBased && Teams.Count != 0 && this.EndValue > Teams[0].Score)
            {
                return "STARTED";
            }

            if (Mode == ContestEndMode.TimeBased && this.EndTime > DateTime.Now)
            {
                return "STARTED";
            }

            return "FINISHED";
        }
    }
}