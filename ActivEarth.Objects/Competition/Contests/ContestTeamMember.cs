using System;

using ActivEarth.Objects.Profile;

namespace ActivEarth.Objects.Competition.Contests
{
    /// <summary>
    /// Wrapper class for Users for use with Contests.  Allows for the calculation of
    /// score contributions since the beginning of a contest.
    /// </summary>
    public class ContestTeamMember
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// The userId wrapped by the ContestUser object.
        /// </summary>
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// The score held by the user when the contest begins.
        /// </summary>
        public float InitialScore
        {
            get;
            set;
        }

        /// <summary>
        /// True if the user has been properly initialized, false otherwise.
        /// </summary>
        public bool Initialized
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Empty constructor for restoring from DB.
        /// </summary>
        public ContestTeamMember()
        {
            this.Initialized = false;
            this.InitialScore = 0;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        #endregion ---------- Public Methods ----------
    }
}