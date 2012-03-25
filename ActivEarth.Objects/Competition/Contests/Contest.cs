using System;
using System.Collections.Generic;
using System.Linq;

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
        public int Points
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
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        public Placeholder.Statistic StatisticBinding
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
            : this(string.Empty, string.Empty, 0, ContestEndMode.GoalBased,
                ContestType.Individual, DateTime.Today, null, Placeholder.Statistic.Steps)
        {

        }

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
        /// <param name="type">Contest type (group or individual)</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">End Conditions to be observed.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        public Contest(string name, string description, int points,
            ContestEndMode mode, ContestType type, DateTime start, EndCondition end, 
            Placeholder.Statistic statistic)
        {
            this.Name = name;
            this.Description = description;
            this.Points = points;
            this.Mode = mode;
            this.Type = type;
            this.StartTime = start;
            this.EndCondition = end;
            this.StatisticBinding = statistic;

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
        /// <param name="type">Contest type (group or individual)</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">End Conditions to be observed.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <param name="teams">Teams participating in the Contest.</param>
        protected Contest(int id, string name, string description, int points,
            ContestEndMode mode, ContestType type, DateTime start, EndCondition end, 
            Placeholder.Statistic statistic, List<Team> teams)
            : this(name, description, points, mode, type, start, end, statistic)
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
                team.Update(StatisticBinding);
            }

            this.SortTeamsByScore();
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
        /// Adds a group to the Contest. In a group contest, members will be added as one team,
        /// while in an individual contest members will be added individually.
        /// 
        /// DEPENDENCY: Groups.Group
        /// </summary>
        /// <param name="group">Group to be added.</param>
        public void AddGroup(Placeholder.Group group)
        {
            if (this.Type == ContestType.Group)
            {
                string teamName = group.Name;
                //TODO: Assert that no team with this name exists already

                Team newTeam = new Team(teamName);

                foreach (Placeholder.User user in group.Members)
                {
                    newTeam.Members.Add(new TeamMember(user));
                }

                this.AddTeam(newTeam);
            }
            else
            {
                foreach (Placeholder.User user in group.Members)
                {
                    this.AddUser(user);
                }
            }
        }

        /// <summary>
        /// Adds groups to the Contest. In a group contest, members will be added as one team,
        /// while in an individual contest members will be added individually.
        /// 
        /// DEPENDENCY: Groups.Group
        /// </summary>
        /// <param name="group">Groups to be added.</param>
        public void AddGroup(List<Placeholder.Group> groups)
        {
            foreach (Placeholder.Group group in groups)
            {
                this.AddGroup(group);
            }
        }

        /// <summary>
        /// Adds a user to the Contest.
        /// 
        /// DEPENDENCY: Profile.User
        /// </summary>
        /// <param name="user">User to be added.</param>
        public void AddUser(Placeholder.User user)
        {
            string teamName = String.Format("{0} {1}", user.FirstName, user.LastName);
            //TODO: Assert that no team with this name exists already

            Team newTeam = new Team(teamName);
            newTeam.Add(user);

            this.AddTeam(newTeam);
        }

        /// <summary>
        /// Adds users to the Contest.
        /// 
        /// DEPENDENCY: Profile.User
        /// </summary>
        /// <param name="user">Users to be added.</param>
        public void AddUser(List<Placeholder.User> users)
        {
            foreach (Placeholder.User user in users)
            {
                this.AddUser(user);
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

        /// <summary>
        /// Locks competitor initial values such that the calculation of
        /// deltas can begin (to calculate team scores).
        /// </summary>
        public void LockInitialValues()
        {
            foreach (Team team in this.Teams)
            {
                team.LockInitialValues(StatisticBinding);
            }
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Methods ----------

        /// <summary>
        /// Sorts the participating teams in descending order by their score.
        /// </summary>
        private void SortTeamsByScore()
        {
            this.Teams.Sort(delegate (Team t1, Team t2) { 
                return t2.Score.CompareTo(t1.Score); 
                });
        }

        #endregion ---------- Private Methods ----------
    }
}