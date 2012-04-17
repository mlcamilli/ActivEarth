using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;

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
                _userStatisticMap[statToSet].value = val;
                UserStatisticDAO.UpdateUserStatistic(_userStatisticMap[statToSet]);
            }
            else
            {
                UserStatistic userStatistic = new UserStatistic(statToSet, val);
                int userStatisticId = UserStatisticDAO.CreateNewStatisticForUser(_user.UserID, statToSet, val);
                userStatistic.UserStatisticID = userStatisticId;
                _userStatisticMap[statToSet] = userStatistic;
            }
        }

        public void UpdateUserStatistics()
        {
            List<UserStatistic> userStatistics = UserStatisticDAO.GetAllStatisticsByUserId(_user.UserID);
            foreach (UserStatistic stat in userStatistics)
            {
                _userStatisticMap[(Statistic)stat.statistic] = stat;
            }
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Methods ----------


        #endregion ---------- Private Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// User of this statistic manager.
        /// </summary>
        private User _user;

        private Dictionary<Statistic, UserStatistic> _userStatisticMap;

        #endregion ---------- Private Fields ----------
    }
}