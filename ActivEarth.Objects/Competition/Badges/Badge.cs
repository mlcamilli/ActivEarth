using System;

using ActivEarth.Objects.Profile;

namespace ActivEarth.Objects.Competition.Badges
{
    public class Badge
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Indentifier for the badge.
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// Indentifier for the owner of the badge.
        /// </summary>
        public int UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the badge.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Current level of the badge.
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// Progress made toward the next level of the badge as a percentage (0-100).
        /// </summary>
        public int Progress
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
        /// Path for the badge image.
        /// </summary>
        public string ImagePath
        {
            get;
            set;
        }

        /// <summary>
        /// Array of the values required to advance to each level of the badge.
        /// </summary>
        public float[] LevelRequirements
        {
            get;
            set;
        }

        /// <summary>
        /// Array of the activity points awarded for each level of the badge.
        /// </summary>
        public int[] LevelRewards
        {
            get;
            set;
        }

        /// <summary>
        /// Format string for reporting the badge information.
        /// </summary>
        public string FormatString
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Empty constructor for reading back in from the DB.
        /// </summary>
        public Badge()
        {
            this.Level = BadgeLevels.None;
            this.Progress = 0;
        }

        /// <summary>
        /// Creates a new badge belonging to a user, based on a specific statistic.
        /// </summary>
        /// <param name="user">User to whom the Badge is bound.</param>
        /// <param name="statistic">Statistic to which the Badge is bound.</param>
        public Badge(int userId, Statistic statistic)
            : this()
        {
            this.UserID = userId;
            this.StatisticBinding = statistic;
        }

        /// <summary>
        /// Creates a new badge belonging to a user, based on a specific statistic.
        /// </summary>
        /// <param name="user">User to whom the Badge is bound.</param>
        /// <param name="statistic">Statistic to which the Badge is bound.</param>
        public Badge(int userId, Statistic statistic, 
            float[] levelValues, int[] levelPoints, string[] imagePaths)
        {
            this.Level = BadgeLevels.None;
            this.Progress = 0;

            this.UserID = userId;
            this.StatisticBinding = statistic;
            this.LevelRequirements = levelValues;
            this.LevelRewards = levelPoints;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------
        
        /// <summary>
        /// Returns the statistic value required to get to the next level
        /// of the badge.
        /// </summary>
        /// <returns>Statistic requirement for the next level of the badge.</returns>
        public float GetNextLevelRequirement()
        {
            return (LevelRequirements[this.Level + 1] >= 0 ? LevelRequirements[this.Level + 1] : float.PositiveInfinity);
        }

        /// <summary>
        /// Returns the number of Activity Points awarded for achieving the
        /// next level of the badge.
        /// </summary>
        /// <returns>Number of Activity Points awarded for achieving the next
        /// level of the badge.</returns>
        public int GetNextLevelReward()
        {
            return LevelRewards[this.Level + 1];
        }

        /// <summary>
        /// Whether or not the badge path has been completed.
        /// </summary>
        /// <returns>True if the badge level has been maxed, false otherwise.</returns>
        public bool IsComplete()
        {
            return (this.Level == BadgeLevels.Max);
        }

        #endregion ---------- Public Methods ----------
    }
}