using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Contests
{
    public class GroupContest : Contest
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
        public GroupContest(uint id, string name, string description, int points,
            ContestEndMode mode, DateTime start, EndCondition end, Placeholder.Statistic statistic)
            : base(id, name, description, points, mode, start, end, statistic)
        {

        }

        /// <summary>
        /// Creates a new Group Contest with predetermined groups.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// DEPENDENCY: Groups.Group
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
        public GroupContest(uint id, string name, string description, int points,
            ContestEndMode mode, DateTime start, EndCondition end, Placeholder.Statistic statistic,
            List<Placeholder.Group> groups)
            : this(id, name, description, points, mode, start, end, statistic)
        {
            this.AddGroup(groups);
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Adds a group to the Contest as a new Team.
        /// 
        /// DEPENDENCY: Groups.Group
        /// </summary>
        /// <param name="group">Group to be added.</param>
        public void AddGroup(Placeholder.Group group)
        {
            string teamName = group.Name;
            //TODO: Assert that no team with this name exists already

            Team newTeam = new Team(teamName);

            foreach (Placeholder.User user in group.Members)
            {
                newTeam.Members.Add(new ContestUser(user));
            }

            base.AddTeam(newTeam);
        }

        /// <summary>
        /// Adds groups to the Contest (each as a new Team).
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

        #endregion ---------- Public Methods ----------
    }
}