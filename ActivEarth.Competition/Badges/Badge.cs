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
            this._levelValues = levelValues;
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

            int newLevel = BadgeLevels.None;

            while ((newLevel < BadgeLevels.Diamond) && 
                (stat > this._levelValues[(int)newLevel + 1]))
            {
                newLevel++;
            }

            for (int i = oldLevel + 1; i <= newLevel; i++)
            {
                pointsEarned += this._levelPoints[i];
            }

            return pointsEarned;
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
        private float[] _levelValues;

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