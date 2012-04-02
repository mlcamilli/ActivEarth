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
        /// Progress made toward the next level of the badge.
        /// </summary>
        public float Progress
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
            float[] levelValues, int[] levelPoints, Uri[] imagePaths)
        {
            this.Level = BadgeLevels.None;
            this.Progress = 0;

            User = user;
            StatisticBinding = statistic;
            _levelRequirements = levelValues;
            _levelRewards = levelPoints;
            _ImagePaths = imagePaths;
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

            float stat = User.GetStatistic(StatisticBinding);

            int newLevel = oldLevel;

            while ((newLevel < BadgeLevels.Max) && 
                (stat >= _levelRequirements[(int)newLevel + 1]))
            {
                newLevel++;
            }

            for (int i = oldLevel + 1; i <= newLevel; i++)
            {
                pointsEarned += _levelRewards[i];
            }

            this.Level = newLevel;

            return pointsEarned;
        }

        /// <summary>
        /// Returns the image path for the current Badge level's icon.
        /// </summary>
        /// <returns>Image path for the current Badge level's icon.</returns>
        public Uri GetImagePath()
        {
            return _ImagePaths[this.Level];
        }

        /// <summary>
        /// Returns the statistic value required to get to the next level
        /// of the badge.
        /// </summary>
        /// <returns>Statistic requirement for the next level of the badge.</returns>
        public float GetNextLevelRequirement()
        {
            return _levelRequirements[this.Level + 1];
        }

        /// <summary>
        /// Returns the number of Activity Points awarded for achieving the
        /// next level of the badge.
        /// </summary>
        /// <returns>Number of Activity Points awarded for achieving the next
        /// level of the badge.</returns>
        public int GetNextLevelReward()
        {
            return _levelRewards[this.Level + 1];
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// Array of the values required to advance to each level of the badge.
        /// </summary>
        private float[] _levelRequirements;

        /// <summary>
        /// Array of the activity points awarded for each level of the badge.
        /// </summary>
        private int[] _levelRewards;

        /// <summary>
        /// Array of the image locations for each level of the badge.
        /// </summary>
        private Uri[] _ImagePaths;

        #endregion ---------- Private Fields ----------
    }
}