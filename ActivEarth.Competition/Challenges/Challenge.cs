using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Challenges
{
    public class Challenge
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Identifier for the challenge.
        /// </summary>
        public uint ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Name of the challenge.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Description of the challenge.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Activity Points to be awarded upon completion of the challenge.
        /// </summary>
        public int Points
        {
            get;
            set;
        }

        /// <summary>
        /// True if the challenge resets when its End Time occurs, false
        /// if it simply expires.
        /// </summary>
        public bool IsPersistent
        {
            get;
            set;
        }

        /// <summary>
        /// The date and time at which the Challenge ends.
        /// </summary>
        public DateTime EndTime
        {
            get;
            private set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new Challenge.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        /// <param name="id">Identifier for the challenge.</param>
        /// <param name="name">Challenge Name.</param>
        /// <param name="description">Challenge Description.</param>
        /// <param name="points">Points to be awarded upon completion of the Challenge.</param>
        /// <param name="persistent">True if the Challenge is persistent, false otherwise.</param>
        /// <param name="endTime">Time at which the Challenge ends.</param>
        /// <param name="statistic">Statistic to which the Challenge is bound.</param>
        public Challenge(uint id, string name, string description, int points, bool persistent,
            DateTime endTime, Placeholder.Statistics statistic)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Points = points;
            this.IsPersistent = persistent;
            this.EndTime = endTime;
            this._statisticBinding = statistic;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Gets the progress made by a user in the challenge.
        /// 
        /// DEPENDENCY: Profile.User
        /// </summary>
        /// <param name="user">The user to evaluate.</param>
        public float GetProgress(Placeholder.User user)
        {
            if (user.ChallengeInitialValues.ContainsKey(this.ID))
            {
                float startingPoint = user.ChallengeInitialValues[this.ID];
                return user.GetStatistic(this._statisticBinding) - startingPoint;
            }
            else
            {
                //TODO: Throw an exception here?
                return -1;
            }
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// Statistic to which the badge is bound.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        private Placeholder.Statistics _statisticBinding;

        #endregion ---------- Private Fields ----------
    }
}