using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects.Competition.Challenges
{
    public class Challenge
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Identifier for the challenge.
        /// </summary>
        public int ID
        {
            get;
            set;
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
        /// Statistic value required to complete the challenge.
        /// </summary>
        public float Requirement
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
            set;
        }

        /// <summary>
        /// The Duration of the challenge.
        /// </summary>
        public TimeSpan Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Statistic to which the badge is bound.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        public Placeholder.Statistic StatisticBinding
        {
            get;
            set;
        }

        /// <summary>
        /// Whether or not the challenge is currently running.
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Parameterless constructor for reconstituting challenges from the DB.
        /// </summary>
        public Challenge()
        {

        }

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
        /// <param name="startTime">Time at which the challenge begins.</param>
        /// <param name="durationInDays">Number of days for which the challenge is active.</param>
        /// <param name="statistic">Statistic to which the Challenge is bound.</param>
        /// <param name="requirement">Statistic value required to complete the challenge.</param>
        public Challenge(string name, string description, int points, bool persistent,
            DateTime startTime, int durationInDays, Placeholder.Statistic statistic, float requirement)
        {
            this.Name = name;
            this.Description = description;
            this.Points = points;
            this.IsPersistent = persistent;
            this.Duration = new TimeSpan(durationInDays, 0, 0, 0);
            this.EndTime = startTime.AddDays(durationInDays);
            this.StatisticBinding = statistic;
            this.Requirement = requirement;
            this.IsActive = true;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Gets the progress made by a user in the challenge.
        /// 
        /// DEPENDENCY: Profile.User
        /// </summary>
        /// <param name="user">The user to evaluate.</param>
        /// <returns>Minimum of the user's progress and the challenge's requirement 
        /// (for use in a progress bar).</returns>
        public float GetProgress(Placeholder.User user)
        {
            if (user.ChallengeInitialValues.ContainsKey(this.ID))
            {
                float startingPoint = user.ChallengeInitialValues[this.ID];
                return Math.Min(user.GetStatistic(this.StatisticBinding) - startingPoint,
                    this.Requirement);
            }
            else
            {
                //TODO: Throw an exception here?
                return -1;
            }
        }

        /// <summary>
        /// Returns true if the user has met the requirements to complete the challenge,
        /// false otherwise.
        /// </summary>
        /// <param name="user">The user to evaluate.</param>
        /// <returns>Whether or not the user has completed the challenge.</returns>
        public bool IsComplete(Placeholder.User user)
        {
            return (this.GetProgress(user) == this.Requirement);
        }

        #endregion ---------- Public Methods ----------
    }
}