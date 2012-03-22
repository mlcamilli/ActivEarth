using System;

namespace ActivEarth.Objects.Competition.Contests
{
    /// <summary>
    /// Represents a compound of the two end condition types usable
    /// in an ActivEarth Contest.
    /// </summary>
    public class EndCondition
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// The time at which the contest shall end.
        /// </summary>
        public DateTime EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// The goal value, where a competitor reaching the goal triggers
        /// the completion of the Contest.
        /// </summary>
        public float EndValue
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a time-based EndCondition.
        /// </summary>
        /// <param name="endTime">The time at which the contest shall end.</param>
        public EndCondition(DateTime endTime)
        {
            this.EndTime = endTime;
            this.EndValue = float.PositiveInfinity;
        }

        /// <summary>
        /// Creates a value-based EndCondition.
        /// </summary>
        /// <param name="endValue">The value at which the contest shall end.</param>
        public EndCondition(float endValue)
        {
            this.EndTime = DateTime.MaxValue;
            this.EndValue = endValue;
        }

        #endregion ---------- Constructor ----------
    }
}