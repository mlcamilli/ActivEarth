using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using ActivEarth.Objects;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class RecyclingDAO
    {
        /// <summary>
        /// Retrieves the collection of all recycling centers submitted to the server.
        /// </summary>
        /// <returns>Recycle Centers submitted to the server.</returns>
        public static List<RecycleCenter> GetRecyclingCenters()
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.RecyclingCenterDataProviders
                        select
                            new RecycleCenter
                            {
                                UserId = c.user_id,
                                Location = c.location,
                                Comments = c.comments,
                                Automotive = c.automotive,
                                Electronics = c.electronics,
                                Construction = c.construction,
                                Batteries = c.batteries,
                                Garden = c.garden,
                                Glass = c.glass,
                                Hazardous = c.hazardous,
                                Household = c.household,
                                Metal = c.metal,
                                Paint = c.paint,
                                Paper = c.paper,
                                Plastic = c.plastic
                            }).ToList();
            }
        }

        /// <summary>
        /// Adds a new Recycle Center to the database.
        /// </summary>
        /// <param name="center">Recycle Center to add to the database.</param>
        /// <returns>Primary Key (ID) of the newly added Recycle Center.</returns>
        public static int AddRecycleCenter(RecycleCenter center, out string errorMessage)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    errorMessage = String.Empty;

                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var centerData = new RecyclingCenterDataProvider
                    {
                        location = center.Location,
                        comments = center.Comments,
                        automotive = center.Automotive,
                        electronics = center.Electronics,
                        construction = center.Construction,
                        batteries = center.Batteries,
                        garden = center.Garden,
                        glass = center.Glass,
                        hazardous = center.Hazardous,
                        household = center.Household,
                        metal = center.Metal,
                        paint = center.Paint,
                        paper = center.Paper,
                        plastic = center.Plastic,
                        user_id = center.UserId
                    };

                    data.RecyclingCenterDataProviders.InsertOnSubmit(centerData);
                    data.SubmitChanges();

                    return centerData.id;
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