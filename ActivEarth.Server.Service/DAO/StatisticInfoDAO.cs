using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class StatisticInfoDAO
    
    {
        /// <summary>
        /// Retrieves the name for a particular statistic.
        /// </summary>
        /// <param name="stat">Statistic to query.</param>
        /// <returns>Name to be used for the statistic.</returns>
        public static string GetStatisticName(Statistic stat)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.StatisticConstantsDataProviders
                                where c.statistic_id == (byte)stat
                                select c.name).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves the format string for a particular statistic.
        /// </summary>
        /// <param name="stat">Statistic to query.</param>
        /// <returns>Format String to be used to display the statistic.</returns>
        public static string GetStatisticFormatString(Statistic stat)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.StatisticConstantsDataProviders
                                where c.statistic_id == (byte)stat
                                select c.format_string).FirstOrDefault();
            }
        }
    }
}