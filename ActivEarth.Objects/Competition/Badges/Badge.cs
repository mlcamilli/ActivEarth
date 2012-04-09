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
        /// User to which the badge is bound.
        /// </summary>
        public User User
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
        /// Array of the image locations for each level of the badge.
        /// </summary>
        public string[] ImagePaths
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

        }

        /// <summary>
        /// Creates a new badge belonging to a user, based on a specific statistic.
        /// </summary>
        /// <param name="user">User to whom the Badge is bound.</param>
        /// <param name="statistic">Statistic to which the Badge is bound.</param>
        public Badge(User user, Statistic statistic, 
            float[] levelValues, int[] levelPoints, string[] imagePaths)
        {
            this.Level = BadgeLevels.None;
            this.Progress = 0;

            User = user;
            StatisticBinding = statistic;
            LevelRequirements = levelValues;
            LevelRewards = levelPoints;
            ImagePaths = imagePaths;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Updates the badge to reflect a change in statistics.
        /// </summary>
        /// <returns>Activity points earned since the last update.</returns>
        public int Update()
        {
            int pointsEarned = 0;

            int oldLevel = this.Level;
            int newLevel = oldLevel;

            float stat = User.GetStatistic(StatisticBinding);

            while ((newLevel < BadgeLevels.Max) && 
                (stat >= LevelRequirements[(int)newLevel + 1]))
            {
                newLevel++;
            }

            for (int i = oldLevel + 1; i <= newLevel; i++)
            {
                pointsEarned += LevelRewards[i];
            }

            this.Level = newLevel;

            if (this.Level == BadgeLevels.Max)
            {
                this.Progress = 100;
            }
            else
            {
                this.Progress = (int)(100 * (stat - this.LevelRequirements[newLevel]) /
                    (this.LevelRequirements[newLevel + 1] - this.LevelRequirements[newLevel]));
            }

            return pointsEarned;
        }

        /// <summary>
        /// Returns the image path for the current Badge level's icon.
        /// </summary>
        /// <returns>Image path for the current Badge level's icon.</returns>
        public string GetImagePath()
        {
            return ImagePaths[this.Level];
        }

        /// <summary>
        /// Returns the statistic value required to get to the next level
        /// of the badge.
        /// </summary>
        /// <returns>Statistic requirement for the next level of the badge.</returns>
        public float GetNextLevelRequirement()
        {
            return LevelRequirements[this.Level + 1];
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
        /// Returns the formatted text progress report for the Badge (e.g., "34.5 / 40.0").
        /// </summary>
        /// <returns>Formatted text progress report for the Badge.</returns>
        public string GetFormattedProgress()
        {
            string numerator = String.Format(this.FormatString, this.User.GetStatistic(this.StatisticBinding));

            if (this.Level < BadgeLevels.Max)
            {
                string denominator = String.Format(this.FormatString, this.GetNextLevelRequirement());

                return String.Format("{0} / {1}", numerator, denominator);
            }
            else
            {
                return numerator;
            }
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