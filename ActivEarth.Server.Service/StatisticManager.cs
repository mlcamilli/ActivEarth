using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Server.Service.Competition;

using ActivEarth.DAO;

namespace ActivEarth.Server.Service.Statistics
{
    /// <summary>
    /// Serves as a wrapper class for the StatisticDAO class, further abstracting actions.
    /// </summary>
    public class StatisticManager
    {
        public StatisticManager(User user)
        {
            _user = user;
            _userStatisticMap = new Dictionary<Statistic, UserStatistic>();
            UpdateUserStatistics();
        }

        #region ---------- Public Methods ----------

        /// <summary>
        /// Gets a statistic for a user based on the statistic type.
        /// </summary>
        /// <param name="statToGet">Statistic to retrieve.</param>
        /// <returns>UserStatistic for type of Statistic</returns>
        public UserStatistic GetUserStatistic(Statistic statToGet)
        {
            UserStatistic stat = new UserStatistic(statToGet);

            if (_userStatisticMap.ContainsKey(statToGet))
            {
                stat = _userStatisticMap[statToGet];
            }
            else
            {
               stat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(_user.UserID, statToGet);

               if (stat == null)
               {
                   stat = new UserStatistic(statToGet);
               }
            }

            return stat;
        }

        /// <summary>
        /// Returns a User's Statistic based on its ID.
        /// </summary>
        /// <param name="id">The ID of the UserStatistic to get.</param>
        /// <returns>The UserStatistic with the given id. Null if it doesn't exist.</returns>
        public UserStatistic GetUserStatistic(int id)
        {
            return UserStatisticDAO.GetStatisticFromStatisticId(id);
        }

        public void SetUserStatistic(Statistic statToSet, float val)
        {
            if (_userStatisticMap.ContainsKey(statToSet))
            {
                _userStatisticMap[statToSet].Value = val;
                //UserStatisticDAO.UpdateUserStatistic(_userStatisticMap[statToSet]);
                StatisticManager.SetUserStatistic(_user.UserID, statToSet, val);
            }
            else
            {
                UserStatistic userStatistic = new UserStatistic(statToSet, val);
                int userStatisticId = UserStatisticDAO.CreateNewStatisticForUser(_user.UserID, statToSet, 0);
                userStatistic.UserStatisticID = userStatisticId;
                _userStatisticMap[statToSet] = userStatistic;
                StatisticManager.SetUserStatistic(_user.UserID, statToSet, val);
            }
        }

        public void UpdateUserStatistics()
        {
            List<UserStatistic> userStatistics = UserStatisticDAO.GetAllStatisticsByUserId(_user.UserID);
            foreach (UserStatistic stat in userStatistics)
            {
                _userStatisticMap[(Statistic)stat.Statistic] = stat;
            }
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Static Methods ----------

        /// <summary>
        /// Sets the value of a statistic for a specific user.
        /// </summary>
        /// <param name="userId">ID of the user to update.</param>
        /// <param name="statistic">Statistic to be updated.</param>
        /// <param name="value">New value for the statistic.</param>
        /// <returns></returns>
        public static void SetUserStatistic(int userId, Statistic statistic, float value)
        {
            UserStatistic userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, statistic);

            if (userStat == null)
            {
                UserStatisticDAO.CreateNewStatisticForUser(userId, statistic, 0);
                userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, statistic);
            }

            #region Challenges - Lock User

                float oldValue = (userStat == null ? 0 : userStat.Value);

                List<Challenge> challenges = ChallengeDAO.GetActiveChallenges().Where(c => c.StatisticBinding == statistic).ToList();
                List<Challenge> incompleteChallenges = new List<Challenge>();

                foreach (Challenge challenge in challenges)
                {
                    ChallengeManager.InitializeUser(challenge.ID, userId);

                    if (!ChallengeManager.IsComplete(challenge.ID, userId))
                    {
                        incompleteChallenges.Add(challenge);
                    }
                }

            #endregion Challenges - Lock User

            #region Update Statistic Value

                if (userStat != null)
                {
                    userStat.Value = value;
                    UserStatisticDAO.UpdateUserStatistic(userStat);
                }
                else
                {
                    UserStatisticDAO.CreateNewStatisticForUser(userId, statistic, value);
                }

            #endregion Update Statistic Value

            #region Challenges - Update Completed Challenges

                int newlyCompleted = 0;

                foreach (Challenge challenge in incompleteChallenges)
                {
                    if (ChallengeManager.IsComplete(challenge.ID, userId))
                    {
                        UserDAO.AddChallengePoints(userId, challenge.Reward);
                        newlyCompleted++;

                        // Add challenge to user's completed challenge list
                    }
                }

                if (newlyCompleted > 0)
                {
                    UserStatistic challengesCompleted = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, Statistic.ChallengesCompleted);
                    float oldVal = (challengesCompleted == null ? 0 : challengesCompleted.Value);

                    StatisticManager.SetUserStatistic(userId, Statistic.ChallengesCompleted, oldVal + newlyCompleted);
                }

            #endregion Challenges - Update Completed Challenges

            #region Contests - Update Standings

                //Update the affected contests
                List<int> teamIds = TeamDAO.GetTeamIdsFromUserId(userId);

                foreach (int id in teamIds)
                {
                    TeamDAO.UpdateTeamScore(id);
                }

            #endregion Contests - Update Standings

            #region Badges - Update Badge

                //Update the affected badge
                UserDAO.AddBadgePoints(userId, BadgeManager.UpdateBadge(userId, statistic));

            #endregion Badges - Update Badge
        }

        #endregion ---------- Static Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// User of this statistic manager.
        /// </summary>
        private User _user;

        private Dictionary<Statistic, UserStatistic> _userStatisticMap;

        #endregion ---------- Private Fields ----------
    }
}