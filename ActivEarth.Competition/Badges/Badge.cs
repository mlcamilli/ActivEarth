using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Badges
{
    public class Badge
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Indentifier for the badge.
        /// </summary>
        public uint ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Name for the badge.
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
        /// Progress made toward the next level of the badge.
        /// </summary>
        public float Progress
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new badge belonging to a user, based on a specific statistic.
        /// 
        /// DEPENDENCY: Profile.User
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        /// <param name="user">User to whom the Badge is bound.</param>
        /// <param name="statistic">Statistic to which the Badge is bound.</param>
        public Badge(uint id, string name, Placeholder.User user, Placeholder.Statistics statistic, 
            float[] levelValues, int[] levelPoints, Uri[] imagePaths)
        {
            this.ID = id;
            this.Name = name;
            this.Level = BadgeLevels.None;
            this.Progress = 0;

            this._user = user;
            this._statisticBinding = statistic;
            this._levelRequirements = levelValues;
            this._levelPoints = levelPoints;
            this._ImagePaths = imagePaths;
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

            float stat = this._user.GetStatistic(this._statisticBinding);

            int newLevel = oldLevel;

            while ((newLevel < BadgeLevels.Max) && 
                (stat >= this._levelRequirements[(int)newLevel + 1]))
            {
                newLevel++;
            }

            for (int i = oldLevel + 1; i <= newLevel; i++)
            {
                pointsEarned += this._levelPoints[i];
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
            return this._ImagePaths[this.Level];
        }

        /// <summary>
        /// Returns the statistic value required to get to the next level
        /// of the badge.
        /// </summary>
        /// <returns>Statistic requirement for the next level of the badge.</returns>
        public float GetNextLevelRequirement()
        {
            return this._levelRequirements[this.Level + 1];
        }

        /// <summary>
        /// Returns the number of Activity Points awarded for achieving the
        /// next level of the badge.
        /// </summary>
        /// <returns>Number of Activity Points awarded for achieving the next
        /// level of the badge.</returns>
        public int GetNextLevelPoints()
        {
            return this._levelPoints[this.Level + 1];
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// Statistic to which the badge is bound.
        /// 
        /// DEPENDENCY: Profile.Statistics
        /// </summary>
        private Placeholder.Statistics _statisticBinding;

        /// <summary>
        /// User to which the badge is bound.
        /// 
        /// DEPENDENCY: Profile.User
        /// </summary>
        private Placeholder.User _user;

        /// <summary>
        /// Array of the values required to advance to each level of the badge.
        /// </summary>
        private float[] _levelRequirements;

        /// <summary>
        /// Array of the points awarded for each level of the badge.
        /// </summary>
        private int[] _levelPoints;

        /// <summary>
        /// Array of the image locations for each level of the badge.
        /// </summary>
        private Uri[] _ImagePaths;

        #endregion ---------- Private Fields ----------
    }
}