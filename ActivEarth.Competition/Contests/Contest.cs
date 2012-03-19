using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Contests
{
    public abstract class Contest
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Numeric identifier for the Contest.
        /// </summary>
        public uint ID
        {
            get;
            private set;
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
        public int Points
        {
            get;
            set;
        }

        /// <summary>
        /// The Contest mode, indicating whether the contest is time-based or goal-based.
        /// </summary>
        public ContestEndModes Mode
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
        /// List of competing teams in the competition.
        /// </summary>
        public List<Contests.Team> Teams
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new Contest.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        /// <param name="id">Numeric indentifier for the contest.</param>
        /// <param name="name">Contest Name.<param>
        /// <param name="description">Contest Description.</param>
        /// <param name="points">Points to be distributed to the winner(s).</param>
        /// <param name="mode">Contest mode for determining termination.</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">End Conditions to be observed.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        protected Contest(uint id, string name, string description, int points,
            ContestEndModes mode, DateTime start, EndCondition end, Placeholder.Statistics statistic)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Points = points;
            this.Mode = mode;
            this.StartTime = start;
            this.EndCondition = end;
            this._statisticBinding = statistic;

            this.Teams = new List<Team>();
        }

        /// <summary>
        /// Creates a new Contest with predetermined teams.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        /// <param name="id">Numeric indentifier for the contest.</param>
        /// <param name="name">Contest Name.<param>
        /// <param name="description">Contest Description.</param>
        /// <param name="points">Points to be distributed to the winner(s).</param>
        /// <param name="mode">Contest mode for determining termination.</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">End Conditions to be observed.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <param name="teams">Teams participating in the Contest.</param>
        protected Contest(uint id, string name, string description, int points,
            ContestEndModes mode, DateTime start, EndCondition end, Placeholder.Statistics statistic,
            List<Team> teams)
            : this(id, name, description, points, mode, start, end, statistic)
        {
            this.Teams = teams;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Updates the scores of the competing teams.
        /// </summary>
        public void UpdateScores()
        {
            foreach (Team team in this.Teams)
            {
                team.Update();
            }

            this.SortTeamsByScore();
        }
        
        /// <summary>
        /// Returns the team(s) that won the Contest. In the event of a tie, multiple
        /// teams will be returned, otherwise a single team will be returned in the list.
        /// </summary>
        /// <returns>The Contest's winning team(s).</returns>
        public List<Team> GetWinner()
        {
            List<Team> winners = new List<Team>();

            if (this.Teams.Count > 0)
            {
                winners.Add(this.Teams[0]);

                float winningScore = this.Teams[0].Score;

                int i = 1;
                while (i < this.Teams.Count && this.Teams[i].Score == winningScore)
                {
                    winners.Add(this.Teams[i]);
                }
            }

            return winners;
        }
        
        /// <summary>
        /// Adds a team to the Contest.
        /// </summary>
        /// <param name="team">The team to be added.</param>
        public void AddTeam(Team team)
        {
            if (team != null)
            {
                this.Teams.Add(team);
            }
        }

        /// <summary>
        /// Adds teams to the Contest.
        /// </summary>
        /// <param name="team">The teams to be added.</param>
        public void AddTeam(List<Team> teams)
        {
            foreach (Team team in teams)
            {
                this.AddTeam(team);
            }
        }

        /// <summary>
        /// Removes a team from the Contest.
        /// </summary>
        /// <param name="team">The team to be removed.</param>
        public void RemoveTeam(Team team)
        {
            if (team != null)
            {
                this.Teams.Remove(team);
            }
        }

        /// <summary>
        /// Removes teams from the Contest.
        /// </summary>
        /// <param name="team">The teams to be removed.</param>
        public void RemoveTeam(List<Team> teams)
        {
            foreach (Team team in teams)
            {
                this.RemoveTeam(team);
            }
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Methods ----------

        /// <summary>
        /// Sorts the participating teams by their score.
        /// </summary>
        private void SortTeamsByScore()
        {
            this.Teams.Sort(delegate (Team t1, Team t2) { 
                return t1.Score.CompareTo(t2.Score); 
                });
        }

        #endregion ---------- Private Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// Statistic to which the contest is bound.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        protected Placeholder.Statistics _statisticBinding; 
        
        #endregion ---------- Private Fields ----------
    }
}