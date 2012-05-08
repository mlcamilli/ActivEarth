
using System.Runtime.Serialization;

namespace ActivEarth.Objects.Competition
{
    [DataContract]
    public class ActivityScore
    {
        #region ---------- Public Properties ----------

        /// <summary>
        /// Aggregate score from all three categories.
        /// </summary>
        [DataMember]
        public int TotalScore
        {
            get;
            private set;
        }

        /// <summary>
        /// Activity Score contributed by completed Challenges.
        /// </summary>
        [DataMember]
        public int ChallengeScore
        {
            get;
            private set;
        }

        /// <summary>
        /// Activity Score contributed by completed Contests.
        /// </summary>
        [DataMember]
        public int ContestScore
        {
            get;
            private set;
        }

        /// <summary>
        /// Activity Score contributed by earned Badges.
        /// </summary>
        [DataMember]
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

        /// <summary>
        /// Creates a new ActivityScore, initialized to the given parameters.  Used to rebuild
        /// Group and User objects when retreiving data from the DB.
        /// </summary>
        /// <param name="badgeScore">The BadgeScore stored in the DB</param>
        /// <param name="challengeScore">The ChallengeScore stored in the DB</param>
        /// <param name="contestScore">The ContestScore stored in the DB</param>
        public ActivityScore(int badgeScore, int challengeScore, int contestScore)
        {
            this.BadgeScore = badgeScore;
            this.ChallengeScore = challengeScore;
            this.ContestScore = contestScore;
            this.TotalScore = badgeScore + challengeScore + contestScore;
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