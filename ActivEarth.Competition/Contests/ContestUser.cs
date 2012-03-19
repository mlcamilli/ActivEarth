using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Contests
{
    /// <summary>
    /// Wrapper class for Users for use with Contests.  Allows for the calculation of
    /// score contributions since the beginning of a contest.
    /// </summary>
    public class ContestUser
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// The user object wrapped by the ContestUser object.
        /// 
        /// DEPENDENCY: Profile.User
        /// </summary>
        public Placeholder.User User
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new ContestUser for a contest tracking a particular statistic.
        /// 
        /// DEPENDENCY: Profile.User
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        /// <param name="user">The user participating in the contest.</param>
        /// <param name="statistic">The statistic being scored in the contest.</param>
        public ContestUser(Placeholder.User user, Placeholder.Statistics statistic)
        {
            this.User = user;
            this._statisticBinding = statistic;
            this._initialized = false;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Notes the user's state at the beginning of a contest so that
        /// the delta score can be calculated.
        /// 
        /// Sets the initialized flag to true, allowing the calculation of
        /// delta scores.
        /// 
        /// DEPENDENCY: Profile.GetStatistic()
        /// </summary>
        public void Initialize()
        {
            this._initialScore = this.User.GetStatistic(this._statisticBinding);
            this._initialized = true;
        }

        /// <summary>
        /// Calculates the user's change in the relevant statistic
        /// since the beginning of the contest; their 'score' for
        /// the contest.
        /// 
        /// DEPENDENCY: Profile.GetStatistic()
        /// </summary>
        /// <returns></returns>
        public float CalculateScore()
        {
            if (this._initialized)
            {
                return (this.User.GetStatistic(this._statisticBinding) - this._initialScore);
            }
            else
            {
                throw new Exception(String.Format("User's Score ({0} {1}) was never Initialized",
                    this.User.FirstName, this.User.LastName));
            }
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// The score held by the user when the contest begins.
        /// </summary>
        private float _initialScore;
        
        /// <summary>
        /// True if the user has been properly initialized, false otherwise.
        /// </summary>
        private bool _initialized;

        /// <summary>
        /// Statistic to which the contest is bound.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        private Placeholder.Statistics _statisticBinding; 

        #endregion ---------- Private Fields ----------
    }
}