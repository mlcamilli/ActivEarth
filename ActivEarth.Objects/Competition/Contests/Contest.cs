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
        /// Conditions under which the contest will end (can represent either a duration
        /// or a goal score to be attained).
        /// </summary>
        public EndCondition EndCondition
        {
            get;
            set;
        }

        /// <summary>
        /// List of competing teams in the competition, maintained in sorted order
        /// for reporting standings.
        /// </summary>
        public List<Contests.Team> Teams
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


        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Default constructor for loading contests from DB.
        /// </summary>
        public Contest()
        {
            this.Teams = new List<Team>();
        }

        /// <summary>
        /// Creates a new Contest.
        /// </summary>
        /// <param name="id">Numeric indentifier for the contest.</param>
        /// <param name="name">Contest Name.<param>
        /// <param name="description">Contest Description.</param>
        /// <param name="reward">Points to be distributed to the winner(s).</param>
        /// <param name="mode">Contest mode for determining termination.</param>
        /// <param name="type">Contest type (group or individual)</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">End Conditions to be observed.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        public Contest(string name, string description, int reward,
            ContestEndMode mode, ContestType type, DateTime start, EndCondition end, 
            Statistic statistic)
        {
            this.Name = name;
            this.Description = description;
            this.Reward = reward;
            this.Mode = mode;
            this.Type = type;
            this.StartTime = start;
            this.EndCondition = end;
            this.StatisticBinding = statistic;
            this.IsActive = true;

            this.Teams = new List<Team>();
        }

        /// <summary>
        /// Creates a new Contest with predetermined teams.
        /// </summary>
        /// <param name="id">Numeric indentifier for the contest.</param>
        /// <param name="name">Contest Name.<param>
        /// <param name="description">Contest Description.</param>
        /// <param name="reward">Points to be distributed to the winner(s).</param>
        /// <param name="mode">Contest mode for determining termination.</param>
        /// <param name="type">Contest type (group or individual)</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">End Conditions to be observed.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <param name="teams">Teams participating in the Contest.</param>
        protected Contest(int id, string name, string description, int reward,
            ContestEndMode mode, ContestType type, DateTime start, EndCondition end, 
            Statistic statistic, List<Team> teams)
            : this(name, description, reward, mode, type, start, end, statistic)
        {
            this.Teams = teams;
        }

        #endregion ---------- Constructor ----------
    }
}