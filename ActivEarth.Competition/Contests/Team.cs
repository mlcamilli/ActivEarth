using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Contests
{
    /// <summary>
    /// A team of participants, to be used in Contests.
    /// </summary>
    public class Team
    {
        #region ---------- Public Properties ----------

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
        public void Add(ContestUser user)
        {
            if (user == null)
            {
                return;
            }

            this.Members.Add(user);
        }

        /// <summary>
        /// Adds a list of users to the team.
        /// </summary>
        /// <param name="users">List of users to be added.</param>
        public void Add(List<ContestUser> users)
        {
            if (users == null)
            {
                return;
            }

            foreach (ContestUser user in users)
            {
                this.Add(user);
            }
        }

        /// <summary>
        /// Removes a user from the team.
        /// </summary>
        /// <param name="user"></param>
        public void Remove(ContestUser user)
        {
            if (user == null)
            {
                return;
            }

            this.Members.Remove(user);
        }

        /// <summary>
        /// Removes a list of users from the team.
        /// </summary>
        /// <param name="users"></param>
        public void Remove(List<ContestUser> users)
        {
            if (users == null)
            {
                return;
            }

            foreach (ContestUser user in users)
            {
                this.Remove(user);
            }
        }

        /// <summary>
        /// Recalculates and updates the team's contest score.
        /// </summary>
        /// <returns>Updated contest score for the team.</returns>
        public void Update()
        {
            this.Score = 0;

            foreach (ContestUser user in this.Members)
            {
                this.Score += user.CalculateScore();
            }
        }

        #endregion ---------- Public Methods ----------
    }
}