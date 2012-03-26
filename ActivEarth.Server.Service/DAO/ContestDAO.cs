using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class ContestDAO
    {
        /// <summary>
        /// Retrieves a Contest from the DB based on its ID.
        /// </summary>
        /// <param name="contestId">Identifier of the contest to retrieve.</param>
        /// <returns>Contest specified by the provided ID.</returns>
        public static Contest GetContestFromContestId(int contestId)
        {
            Contest toReturn;
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                toReturn = (from c in data.ContestDataProviders
                        where c.id == contestId
                        select
                            new Contest
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Points = c.points,
                                StartTime = c.start,
                                EndCondition =
                                    ((ContestEndMode)c.end_mode == ContestEndMode.GoalBased ?
                                        new EndCondition((float)c.end_goal) :
                                        new EndCondition((DateTime)c.end_time)),
                                Mode = (ContestEndMode)c.end_mode,
                                Type = (ContestType)c.type,
                                StatisticBinding = (Statistic)c.statistic
                            }).FirstOrDefault();
            }

            if (toReturn != null)
            {
                toReturn.Teams = TeamDAO.GetTeamsFromContestId(toReturn.ID);
            }
            return toReturn;
        }

        /// <summary>
        /// Retrieves all contests in the DB.
        /// </summary>
        /// <returns>All contests in the DB.</returns>
        public static List<Contest> GetAllContests()
        {
            List<Contest> toReturn;

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                toReturn = (from c in data.ContestDataProviders
                        where c.id >= 0
                        select
                            new Contest
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Points = c.points,
                                StartTime = c.start,
                                EndCondition = 
                                    ((ContestEndMode)c.end_mode == ContestEndMode.GoalBased ? 
                                        new EndCondition((float)c.end_goal) : 
                                        new EndCondition((DateTime)c.end_time)),
                                Mode = (ContestEndMode)c.end_mode,
                                Type = (ContestType)c.type,
                                StatisticBinding = (Statistic)c.statistic
                            }).ToList();

                if (toReturn != null)
                {
                    foreach (Contest contest in toReturn)
                    {
                        contest.Teams = TeamDAO.GetTeamsFromContestId(contest.ID);
                    }
                }

                return toReturn;
            }
        }

        /// <summary>
        /// Saves a contest as a new entry in the DB.
        /// </summary>
        /// <param name="contest">Contest object to add to the DB.</param>
        /// <returns>ID of the created contest on success, 0 on failure.</returns>
        public static int CreateNewContest(Contest contest)
        {
            try
            {
                int id;

                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var contestData = new ContestDataProvider
                    {
                        name = contest.Name,
                        description = contest.Description,
                        points = contest.Points,
                        end_mode = (byte)contest.Mode,
                        end_goal = contest.EndCondition.EndValue,
                        end_time = contest.EndCondition.EndTime,
                        start = contest.StartTime,
                        type = (byte)contest.Type,
                        statistic = (byte)contest.StatisticBinding,
                    };
                    data.ContestDataProviders.InsertOnSubmit(contestData);
                    data.SubmitChanges();

                    id = contestData.id;
                }

                foreach (Team team in contest.Teams)
                {
                    //TeamDAO.CreateNewTeam(team, contest.ID);
                }

                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Updates an existing Contest in the DB.
        /// </summary>
        /// <param name="contest">Contest whose record needs updating.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdateContest(Contest contest)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    ContestDataProvider dbContest =
                        (from c in data.ContestDataProviders where c.id == contest.ID select c).FirstOrDefault();
                    if (dbContest != null)
                    {
                        dbContest.name = contest.Name;
                        dbContest.description = contest.Description;
                        dbContest.points = contest.Points;
                        dbContest.end_mode = (byte)contest.Mode;
                        dbContest.end_goal = contest.EndCondition.EndValue;
                        dbContest.end_time = contest.EndCondition.EndTime;
                        dbContest.start = contest.StartTime;
                        dbContest.type = (byte)contest.Type;
                        dbContest.statistic = (byte)contest.StatisticBinding;

                        data.SubmitChanges();
                    }
                    else
                    {
                        return false;
                    }
                }

                if (contest != null)
                {
                    foreach (Team team in contest.Teams)
                    {
                        TeamDAO.UpdateTeam(team, contest.ID);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an existing Contest from the DB.
        /// </summary>
        /// <param name="contestId">ID for the Contest whose record needs to be removed.</param>
        /// <returns>True on success (or the contest didn't exist), false on failure.</returns>
        public static bool RemoveContestFromContestId(int  contestId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    ContestDataProvider dbContest =
                        (from c in data.ContestDataProviders where c.id == contestId select c).FirstOrDefault();
                    if (dbContest != null)
                    {
                        data.ContestDataProviders.DeleteOnSubmit(dbContest);
                        data.SubmitChanges();
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}