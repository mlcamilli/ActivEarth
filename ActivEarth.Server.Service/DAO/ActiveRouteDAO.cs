using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service;
using ActivEarth.Server.Service.Statistics;
using ActivEarth.DAO;

namespace ActivEarth.DAO
{
    public class ActiveRouteDAO
    {
        /// <summary>
        /// Retrieves the collection of routes submitted by a given user.
        /// </summary>
        /// <param name="userId">Identifier of the user.</param>
        /// <returns>Routes belonging to the user specified by the provided ID.</returns>
        public static List<Route> GetRoutesFromUserId(int userId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.ActiveRouteDataProviders
                        where c.user_id == userId
                        select
                            new Route
                            {
                                GMTOffset = c.gmt_offset,
                                Distance = c.distance,
                                EndLatitude = c.end_latitude,
                                EndLongitude = c.end_longitude,
                                EndTime = c.end_time,
                                Mode = c.mode,
                                Points = c.points,
                                StartLatitude = c.start_latitude,
                                StartLongitude = c.start_longitude,
                                StartTime = c.start_time,
                                Steps = c.steps,
                                Type = c.type
                            }).ToList();
            }
        }

        /// <summary>
        /// Adds a new Route to the database.
        /// </summary>
        /// <param name="route">Route to add to the database.</param>
        /// <returns>Primary Key (ID) of the newly added Route.</returns>
        public static int AddNewRoute(Route route)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                var routeData = new ActiveRouteDataProvider
                {
                    gmt_offset = route.GMTOffset,
                    distance = route.Distance,
                    end_latitude = route.EndLatitude,
                    end_longitude = route.EndLongitude,
                    end_time = route.EndTime,
                    mode = route.Mode,
                    points = route.Points,
                    start_latitude = route.StartLatitude,
                    start_longitude = route.StartLongitude,
                    start_time = route.StartTime,
                    steps = route.Steps,
                    type = route.Type,
                    user_id = route.UserId
                };

                data.ActiveRouteDataProviders.InsertOnSubmit(routeData);
                data.SubmitChanges();

                if (routeData.id > 0)
                {
                    ActiveRouteDAO.ProcessRoute(route);
                }

                return routeData.id;
            }
        }

        /// <summary>
        /// Processes the contents of a route and updates the corresponding user statistics.
        /// </summary>
        /// <param name="route">Route to process.</param>
        private static void ProcessRoute(Route route)
        {
            if (route == null) return;

            double distance = MetersToMiles(route.Distance);
            double time = route.EndTime.Subtract(route.StartTime).TotalHours;
            string mode = route.Mode;

            float oldDistance, oldTotalDistance, oldTime, oldTotalTime, oldSteps;

            oldTotalDistance = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.AggregateDistance).Value;
            oldTotalTime = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.AggregateTime).Value;

            switch (mode.ToLower())
            {
                case "running":
                    // Update RunDistance, RunTime, and Steps
                    oldDistance = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.RunDistance).Value;
                    oldTime = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.RunTime).Value;
                    oldSteps = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.Steps).Value;

                    StatisticManager.SetUserStatistic(route.UserId, Statistic.RunDistance, (float)(distance + oldDistance));
                    StatisticManager.SetUserStatistic(route.UserId, Statistic.RunTime, (float)(time + oldTime));
                    StatisticManager.SetUserStatistic(route.UserId, Statistic.Steps, (float)(route.Steps + oldSteps));
                    break;

                case "biking":
                    // Update BikeDistance and BikeTime
                    oldDistance = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.BikeDistance).Value;
                    oldTime = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.BikeTime).Value;

                    StatisticManager.SetUserStatistic(route.UserId, Statistic.BikeDistance, (float)(distance + oldDistance));
                    StatisticManager.SetUserStatistic(route.UserId, Statistic.BikeTime, (float)(time + oldTime));
                    break;

                case "walking":
                    // Update WalkDistance, WalkTime, and Steps
                    oldDistance = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.WalkDistance).Value;
                    oldTime = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.WalkTime).Value;
                    oldSteps = UserStatisticDAO.GetStatisticFromUserIdAndStatType(route.UserId, Statistic.Steps).Value;

                    StatisticManager.SetUserStatistic(route.UserId, Statistic.WalkDistance, (float)(distance + oldDistance));
                    StatisticManager.SetUserStatistic(route.UserId, Statistic.WalkTime, (float)(time + oldTime));
                    StatisticManager.SetUserStatistic(route.UserId, Statistic.Steps, (float)(route.Steps + oldSteps));
                    break;

                default:

                    break;
            }

            StatisticManager.SetUserStatistic(route.UserId, Statistic.AggregateDistance, (float)(distance + oldTotalDistance));
            StatisticManager.SetUserStatistic(route.UserId, Statistic.AggregateTime, (float)(time + oldTotalTime));
        }

        /// <summary>
        /// Converts a distance in meters to a distance in miles.
        /// </summary>
        /// <param name="meters">Distance (m) to convert.</param>
        /// <returns>Provided distance in miles.</returns>
        private static double MetersToMiles(double meters)
        {
            return meters / 1609.344;
        }
    }
}