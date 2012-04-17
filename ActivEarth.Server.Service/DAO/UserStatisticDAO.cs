using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class UserStatisticDAO
    {
        /// <summary>
        /// Retrieves a User's Statistic from the DB based on the user's ID and statistic type.
        /// </summary>
        /// <param name="userId">ID of user to fetch statistic info for.</param>
        /// <param name="statType">Type of statistic to retrieve from user.</param>
        /// <returns>Statistic specified by the user's ID and the statistic type.</returns>
        public static UserStatistic GetStatisticFromUserIdAndStatType(int userId, Statistic statType)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);

                return (from p in data.UserStatisticDataProviders
                            where p.user_id == userId
                            && p.statistic_type.Equals((byte)statType)
                            select new UserStatistic((Statistic)p.statistic_type, (float)p.value)
                            {
                                UserStatisticID = p.id
                            }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieve's a UserStatistic from the DB based on the statistic's ID.
        /// </summary>
        /// <param name="statisticId">ID of statistic to fetch.</param>
        /// <returns>The UserStatistic with the specified ID.</returns>
        public static UserStatistic GetStatisticFromStatisticId(int statisticId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.UserStatisticDataProviders
                        where c.id == statisticId
                        select
                            new UserStatistic((Statistic)c.statistic_type, (float)c.value)
                            {
                                UserStatisticID = c.id
                            }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves all of a User's Statistics from the DB.
        /// </summary>
        /// <param name="userId">ID of user to fetch statistic info for.</param>
        /// <returns>Statistic values for the user.</returns>
        public static List<UserStatistic> GetAllStatisticsByUserId(int userId)
        {
            var returnDict = new Dictionary<Statistic, float>(); 
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);

                return (from p in data.UserStatisticDataProviders
                        where p.user_id == userId
                        select new UserStatistic((Statistic)p.statistic_type, (float)p.value)
                        {
                            UserStatisticID = p.id
                        }).ToList();
            }
        }

        /// <summary>
        /// Saves a user statistic as a new entry in the DB.
        /// </summary>
        /// <param name="userId">User to create stat for.</param>
        /// <param name="statType">Statistic type to create.</param>
        /// <param name="val">Value of the statistic.</param>
        /// <returns>ID of the created statistic on success, 0 on failure.</returns>
        public static int CreateNewStatisticForUser(int userId, Statistic statType, float val)
        {
            try
            {
                int id;

                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var statisticData = new UserStatisticDataProvider
                    {
                        user_id = userId,
                        value = val,
                        statistic_type = (byte)statType
                    };

                    data.UserStatisticDataProviders.InsertOnSubmit(statisticData);
                    data.SubmitChanges();

                    id = statisticData.id;
                }

                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Updates an existing UserStatistic in the DB.
        /// </summary>
        /// <param name="statistic">UserStatistic whose record needs updating.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdateUserStatistic(UserStatistic statistic)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    UserStatisticDataProvider dbStatistic =
                        (from p in data.UserStatisticDataProviders where p.id == statistic.UserStatisticID select p).FirstOrDefault();

                    if (dbStatistic != null)
                    {
                        dbStatistic.statistic_type = (byte)statistic.statistic;
                        dbStatistic.user_id = statistic.UserID;
                        dbStatistic.value = statistic.value;

                        data.SubmitChanges();
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}