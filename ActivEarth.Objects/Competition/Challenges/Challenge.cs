using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;

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
        public int Reward
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
        /// </summary>
        public Statistic StatisticBinding
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

        /// <summary>
        /// Path of the image to be displayed with the challenge.
        /// </summary>
        public string ImagePath
        {
            get;
            set;
        }

        /// <summary>
        /// Format string for reporting the challenge information.
        /// </summary>
        public string FormatString
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
        /// </summary>
        /// <param name="id">Identifier for the challenge.</param>
        /// <param name="name">Challenge Name.</param>
        /// <param name="description">Challenge Description.</param>
        /// <param name="reward">Points to be awarded upon completion of the Challenge.</param>
        /// <param name="persistent">True if the Challenge is persistent, false otherwise.</param>
        /// <param name="startTime">Time at which the challenge begins.</param>
        /// <param name="durationInDays">Number of days for which the challenge is active.</param>
        /// <param name="statistic">Statistic to which the Challenge is bound.</param>
        /// <param name="requirement">Statistic value required to complete the challenge.</param>
        public Challenge(string name, string description, int reward, bool persistent,
            DateTime startTime, int durationInDays, Statistic statistic, float requirement)
        {
            this.Name = name;
            this.Description = description;
            this.Reward = reward;
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
        /// Gets the progress made by a user in the challenge as a percentage (0-100).
        /// </summary>
        /// <param name="user">The user to evaluate.</param>
        /// <returns>User's progress in the challenge (for use in a progress bar).</returns>
        public int GetProgress(User user)
        {
            if (user.ChallengeInitialValues.ContainsKey(this.ID))
            {
                float startingPoint = user.ChallengeInitialValues[this.ID];
                return Math.Min(
                    (int)(100 * (user.GetStatistic(this.StatisticBinding) - startingPoint) / this.Requirement), 100);
            }
            else
            {
                //TODO: Throw an exception here?
                return -1;
            }
        }

        /// <summary>
        /// Returns the formatted text progress report for the Challenge (e.g., "34.5 / 40.0").
        /// </summary>
        /// <returns>Formatted text progress report for the Challenge.</returns>
        public string GetFormattedProgress(User user)
        {
            string numerator = String.Format(this.FormatString, user.GetStatistic(this.StatisticBinding));
            string denominator = String.Format(this.FormatString, this.Requirement);

            return String.Format("{0} / {1}", numerator, denominator);
        }

        /// <summary>
        /// Returns true if the user has met the requirements to complete the challenge,
        /// false otherwise.
        /// </summary>
        /// <param name="user">The user to evaluate.</param>
        /// <returns>Whether or not the user has completed the challenge.</returns>
        public bool IsComplete(User user)
        {
            return (this.GetProgress(user) == 100);
        }

        #endregion ---------- Public Methods ----------
    }
}