using System;

using ActivEarth.Objects.Profile;

namespace ActivEarth.Objects.Competition.Contests
{
    /// <summary>
    /// Wrapper class for Users for use with Contests.  Allows for the calculation of
    /// score contributions since the beginning of a contest.
    /// </summary>
    public class TeamMember
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// The user object wrapped by the ContestUser object.
        /// </summary>
        public User User
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
        public TeamMember()
            : this(null)
        {

        }

        /// <summary>
        /// Creates a new ContestUser for a contest tracking a particular statistic.
        /// </summary>
        /// <param name="user">The user participating in the contest.</param>
        /// <param name="statistic">The statistic being scored in the contest.</param>
        public TeamMember(User user)
        {
            this.User = user;
            this.Initialized = false;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Notes the user's state at the beginning of a contest so that
        /// the delta score can be calculated.
        /// 
        /// Sets the initialized flag to true, allowing the calculation of
        /// delta scores.
        /// </summary>
        public void LockInitialValues(Statistic statistic)
        {
            this.InitialScore = this.User.GetStatistic(statistic);
            this.Initialized = true;
        }

        /// <summary>
        /// Calculates the user's change in the relevant statistic
        /// since the beginning of the contest; their 'score' for
        /// the contest.
        /// </summary>
        /// <returns></returns>
        public float CalculateScore(Statistic statistic)
        {
            if (this.Initialized)
            {
                return (this.User.GetStatistic(statistic) - this.InitialScore);
            }
            else
            {
                throw new Exception(String.Format("User's Score ({0} {1}) was never Initialized",
                    this.User.FirstName, this.User.LastName));
            }
        }

        #endregion ---------- Public Methods ----------
    }
}