using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace ActivEarth.Server.Service
{

    /// <summary>
    /// Manages application connection strings and retrieves new connections to the database.
    /// </summary>

    public class ConnectionManager
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DevDB"].ToString();

        /// <summary>
        /// The connection string used to connect to the Ecolink database.
        /// </summary>

        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }

            set
            {
                _connectionString = value;
                Debug.WriteLine(String.Format("Using connection string: {0}.  ", _connectionString));
            }
        }


        /// <summary>
        /// Retrieve an open connection to the database.
        /// </summary>
        /// <returns>
        /// An open connection to the database.
        /// An exception is thrown if the connection is not opened successfully.
        /// </returns>

        public static SqlConnection GetConnection()
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();
                return connection;
            }
            catch (InvalidOperationException ex)
            {
                throw new DataException("Unable to open a connection to the database.", ex);
            }

        }

    }

}
