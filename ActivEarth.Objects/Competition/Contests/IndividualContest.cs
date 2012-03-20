using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Contests
{
    public class IndividualContest : Contest
    {
        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new Group Contest.
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
        public IndividualContest(uint id, string name, string description, int points,
            ContestEndMode mode, DateTime start, EndCondition end, Placeholder.Statistic statistic)
            : base(id, name, description, points, mode, start, end, statistic)
        {

        }

        /// <summary>
        /// Creates a new Group Contest with predetermined groups.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// DEPENDENCY: Profile.User
        /// </summary>
        /// <param name="id">Numeric indentifier for the contest.</param>
        /// <param name="name">Contest Name.<param>
        /// <param name="description">Contest Description.</param>
        /// <param name="points">Points to be distributed to the winner(s).</param>
        /// <param name="mode">Contest mode for determining termination.</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">End Conditions to be observed.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <param name="groups">Groups participating in the Contest.</param>
        public IndividualContest(uint id, string name, string description, int points,
            ContestEndMode mode, DateTime start, EndCondition end, Placeholder.Statistic statistic,
            List<Placeholder.User> users)
            : this(id, name, description, points, mode, start, end, statistic)
        {
            this.AddUser(users);
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

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

            base.AddTeam(newTeam);
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

        #endregion ---------- Public Methods ----------
    }
}