using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using ActivEarth.Objects;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class CarpoolDAO
    {
        /// <summary>
        /// Retrieves the collection of all carpools submitted to the server.
        /// </summary>
        /// <returns>Carpools submitted to the server.</returns>
        public static List<Carpool> GetCarpools()
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.CarpoolDataProviders
                        select
                            new Carpool
                            {
                                ID = c.id,
                                UserId = c.user_id,
                                Comments = c.comments,
                                Start = c.start,
                                Destination = c.destination,
                                Time = c.time,
                                SeatsAvailable = c.seats_available,

                            }).ToList();
            }
        }

        /// <summary>
        /// Adds a new Carpool to the database.
        /// </summary>
        /// <param name="carpool">Carpool to add to the database.</param>
        /// <returns>Primary Key (ID) of the newly added Carpool.</returns>
        public static int AddCarpool(Carpool carpool, out string errorMessage)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    errorMessage = String.Empty;

                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var carpoolData = new CarpoolDataProvider
                    {
                        start = carpool.Start,
                        destination = carpool.Destination,
                        time = carpool.Time,
                        seats_available = carpool.SeatsAvailable,
                        comments = carpool.Comments,
                        user_id = carpool.UserId
                    };

                    data.CarpoolDataProviders.InsertOnSubmit(carpoolData);
                    data.SubmitChanges();

                    return carpoolData.id;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return 0;
            }
        }
    }
}