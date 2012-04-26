using System;
using System.Collections.Generic;
using System.Linq;

using ActivEarth.Objects.Profile;

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
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier for the contest in which the team is participating.
        /// </summary>
        public int ContestId
        {
            get;
            set;
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
        public List<TeamMember> Members
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

        /// <summary>
        /// True if member changes are allowed, false otherwise.
        /// </summary>
        public bool IsLocked
        {
            get;
            set;
        }

        /// <summary>
        /// ID of the group represented by this team, if applicable (null otherwise).
        /// </summary>
        public int? GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Results bracket currently occupied by the team (None/Bronze/Silver/Gold/Platinum/Diamond).
        /// </summary>
        public byte Bracket
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Empty constructor for restoring from DB.
        /// </summary>
        public Team()
        {
            this.Members = new List<TeamMember>();
            this.Score = 0;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Test Hook: Searches the team for a particular member.
        /// </summary>
        /// <param name="user">User to look for.</param>
        /// <returns>True if the user is a member of the team.</returns>
        public bool ContainsMember(int userId)
        {
            var query = from TeamMember cUser in this.Members
                        where cUser.UserId == userId
                        select cUser;

            foreach (TeamMember cUser in query)
            {
                return true;
            }

            return false;
        }

        #endregion ---------- Public Methods ----------
    }
}