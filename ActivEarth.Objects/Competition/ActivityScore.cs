
namespace ActivEarth.Objects.Competition
{
    public class ActivityScore
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Aggregate score from all three categories.
        /// </summary>
        public int TotalScore
        {
            get;
            private set;
        }

        /// <summary>
        /// Activity Score contributed by completed Challenges.
        /// </summary>
        public int ChallengeScore
        {
            get;
            private set;
        }

        /// <summary>
        /// Activity Score contributed by completed Contests.
        /// </summary>
        public int ContestScore
        {
            get;
            private set;
        }

        /// <summary>
        /// Activity Score contributed by earned Badges.
        /// </summary>
        public int BadgeScore
        {
            get;
            private set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new ActivityScore, initialized to 0.
        /// </summary>
        public ActivityScore()
        {
            this.TotalScore = 0;
            this.BadgeScore = 0;
            this.ChallengeScore = 0;
            this.ContestScore = 0;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Adds newly earned Activity Points from badges.
        /// </summary>
        /// <param name="points">Number of points to add to the Activity Score.</param>
        public void AddBadgePoints(int points)
        {
            this.BadgeScore += points;
            this.TotalScore += points;
        }

        /// <summary>
        /// Adds newly earned Activity Points from Challenges.
        /// </summary>
        /// <param name="points">Number of points to add to the Activity Score.</param>
        public void AddChallengePoints(int points)
        {
            this.ChallengeScore += points;
            this.TotalScore += points;
        }

        /// <summary>
        /// Adds newly earned Activity Points from Contests.
        /// </summary>
        /// <param name="points">Number of points to add to the Activity Score.</param>
        public void AddContestPoints(int points)
        {
            this.ContestScore += points;
            this.TotalScore += points;
        }

        #endregion ---------- Public Methods ----------
    }
}