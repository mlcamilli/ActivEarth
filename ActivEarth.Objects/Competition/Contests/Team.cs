using System;
using System.Collections.Generic;
using System.Linq;

namespace ActivEarth.Objects.Competition.Contests
{
    /// <summary>
    /// A team of participants, to be used in Contests.
    /// </summary>
    public class Team
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Identifier for the team.
        /// </summary>
        public uint ID
        {
            get;
            private set;
        }

        /// <summary>
        /// The name of the team.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The members who make up the team.
        /// </summary>
        public List<ContestUser> Members
        {
            get;
            set;
        }

        /// <summary>
        /// The team's current score in the contest.
        /// </summary>
        public float Score
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Constructs an empty team with the given name.
        /// </summary>
        /// <param name="name">Team Name</param>
        public Team(string name)
        {
            this.Name = name;
            this.Members = new List<ContestUser>();
            this.Score = 0;
        }

        /// <summary>
        /// Constructs a team from a pre-formed list of members, with the given name.
        /// </summary>
        /// <param name="name">Team Name</param>
        /// <param name="members">List of team members</param>
        public Team(string name, List<ContestUser> members) 
            : this(name)
        {
            this.Members = members;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Adds a user to the team.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        public void Add(Placeholder.User user)
        {
            if (user == null)
            {
                return;
            }

            this.Members.Add(new ContestUser(user));
        }

        /// <summary>
        /// Adds a list of users to the team.
        /// </summary>
        /// <param name="users">List of users to be added.</param>
        public void Add(List<Placeholder.User> users)
        {
            if (users == null)
            {
                return;
            }

            foreach (Placeholder.User user in users)
            {
                this.Add(user);
            }
        }

        /// <summary>
        /// Removes a user from the team.
        /// </summary>
        /// <param name="user"></param>
        public void Remove(Placeholder.User user)
        {
            if (user == null)
            {
                return;
            }

            this.Members.Remove(new ContestUser(user));
        }

        /// <summary>
        /// Removes a list of users from the team.
        /// </summary>
        /// <param name="users"></param>
        public void Remove(List<Placeholder.User> users)
        {
            if (users == null)
            {
                return;
            }

            foreach (Placeholder.User user in users)
            {
                this.Remove(user);
            }
        }

        /// <summary>
        /// Recalculates and updates the team's contest score.
        /// </summary>
        /// <returns>Updated contest score for the team.</returns>
        public void Update(Placeholder.Statistic statistic)
        {
            this.Score = 0;

            foreach (ContestUser user in this.Members)
            {
                this.Score += user.CalculateScore(statistic);
            }
        }

        /// <summary>
        /// Locks the initial values for each team member, required for contest
        /// score calculation.
        /// </summary>
        public void LockInitialValues(Placeholder.Statistic statistic)
        {
            foreach (ContestUser user in this.Members)
            {
                user.LockInitialValues(statistic);
            }
        }

        /// <summary>
        /// Test Hook: Searches the team for a particular member.
        /// </summary>
        /// <param name="user">User to look for.</param>
        /// <returns>True if the user is a member of the team.</returns>
        public bool ContainsMember(Placeholder.User user)
        {
            var query = from ContestUser cUser in this.Members
                        where cUser.User == user
                        select cUser;

            foreach (ContestUser cUser in query)
            {
                return true;
            }

            return false;
        }

        #endregion ---------- Public Methods ----------
    }
}