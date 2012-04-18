﻿using System;
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
            Contest contest;
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                contest = (from c in data.ContestDataProviders
                        where c.id == contestId
                        select
                            new Contest
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Reward = c.points,
                                StartTime = c.start,
                                EndCondition =
                                    ((ContestEndMode)c.end_mode == ContestEndMode.GoalBased ?
                                        new EndCondition((float)c.end_goal) :
                                        new EndCondition((DateTime)c.end_time)),
                                Mode = (ContestEndMode)c.end_mode,
                                Type = (ContestType)c.type,
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active,
                                DeactivatedTime = c.deactivated
                            }).FirstOrDefault();
            }

            if (contest != null)
            {
                contest.Teams = TeamDAO.GetTeamsFromContestId(contest.ID);
                contest.Teams.Sort(delegate(Team t1, Team t2) { return t2.Score.CompareTo(t1.Score); });
                contest.FormatString = StatisticInfoDAO.GetStatisticFormatString(contest.StatisticBinding);
            }
            return contest;
        }

        /// <summary>
        /// Retrieves all active contests in the DB.
        /// </summary>
        /// <returns>All active contests in the DB.</returns>
        public static List<Contest> GetActiveContests()
        {
            List<Contest> contests;

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                contests = (from c in data.ContestDataProviders
                        where c.active
                        select
                            new Contest
                            {
                                ID = c.id,
                                Name = c.name,
                                Description = c.description,
                                Reward = c.points,
                                StartTime = c.start,
                                EndCondition = 
                                    ((ContestEndMode)c.end_mode == ContestEndMode.GoalBased ? 
                                        new EndCondition((float)c.end_goal) : 
                                        new EndCondition((DateTime)c.end_time)),
                                Mode = (ContestEndMode)c.end_mode,
                                Type = (ContestType)c.type,
                                StatisticBinding = (Statistic)c.statistic,
                                IsActive = c.active,
                                DeactivatedTime = c.deactivated
                            }).ToList();

                if (contests != null)
                {
                    foreach (Contest contest in contests)
                    {
                        contest.Teams = TeamDAO.GetTeamsFromContestId(contest.ID);
                        contest.Teams.Sort(delegate (Team t1, Team t2) { return t2.Score.CompareTo(t1.Score); });

                        contest.FormatString = StatisticInfoDAO.GetStatisticFormatString(contest.StatisticBinding);
                    }
                }

                return contests;
            }
        }

        /// <summary>
        /// Gets the list of currently joinable contests with a search query (searching contests by name).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exactMatch"></param>
        /// <returns></returns>
        public static List<Contest> GetJoinableContestsFromContestName(string name, bool exactMatch)
        {
            List<Contest> contests;

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                contests = (from c in data.ContestDataProviders
                            where (exactMatch ? 
                                c.name.ToLower().Equals(name.ToLower()) : 
                                c.name.ToLower().Contains(name.ToLower())) &&
                                c.start > DateTime.Now &&
                                c.searchable == true
                            select
                                new Contest
                                {
                                    ID = c.id,
                                    Name = c.name,
                                    Description = c.description,
                                    Reward = c.points,
                                    StartTime = c.start,
                                    EndCondition =
                                        ((ContestEndMode)c.end_mode == ContestEndMode.GoalBased ?
                                            new EndCondition((float)c.end_goal) :
                                            new EndCondition((DateTime)c.end_time)),
                                    Mode = (ContestEndMode)c.end_mode,
                                    Type = (ContestType)c.type,
                                    StatisticBinding = (Statistic)c.statistic,
                                    IsActive = c.active
                                }).ToList();

                if (contests != null)
                {
                    foreach (Contest contest in contests)
                    {
                        contest.Teams = TeamDAO.GetTeamsFromContestId(contest.ID);
                        contest.Teams.Sort(delegate(Team t1, Team t2) { return t2.Score.CompareTo(t1.Score); });
                        contest.FormatString = StatisticInfoDAO.GetStatisticFormatString(contest.StatisticBinding);
                    }
                }

                return contests;
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
                        points = contest.Reward,
                        end_mode = (byte)contest.Mode,
                        end_goal = contest.EndCondition.EndValue,
                        end_time = contest.EndCondition.EndTime,
                        start = contest.StartTime,
                        type = (byte)contest.Type,
                        statistic = (byte)contest.StatisticBinding,
                        searchable = contest.IsSearchable,
                        active = contest.IsActive,
                        deactivated = contest.DeactivatedTime
                    };
                    data.ContestDataProviders.InsertOnSubmit(contestData);
                    data.SubmitChanges();

                    id = contestData.id;
                }

                foreach (Team team in contest.Teams)
                {
                    TeamDAO.CreateNewTeam(team);
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
                        dbContest.points = contest.Reward;
                        dbContest.end_mode = (byte)contest.Mode;
                        dbContest.end_goal = contest.EndCondition.EndValue;
                        dbContest.end_time = contest.EndCondition.EndTime;
                        dbContest.start = contest.StartTime;
                        dbContest.type = (byte)contest.Type;
                        dbContest.statistic = (byte)contest.StatisticBinding;
                        dbContest.searchable = contest.IsSearchable;
                        dbContest.active = contest.IsActive;
                        dbContest.deactivated = contest.DeactivatedTime;

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
                        TeamDAO.UpdateTeam(team);
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

        /// <summary>
        /// Removes contests from the DB that have were deactivated more than two weeks ago.
        /// </summary>
        /// <returns></returns>
        public static bool RemoveOldContests()
        {
            int daysToExpire = 14;

            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    List<ContestDataProvider> dbContests =
                        (from c in data.ContestDataProviders where 
                            (c.deactivated != null && 
                                c.deactivated.Value.Subtract(DateTime.Now) > new TimeSpan(daysToExpire, 0, 0, 0))
                         select c).ToList();
                    
                    foreach (ContestDataProvider dbContest in dbContests)
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

        /// <summary>
        /// Lookup for the statistic being tracked by a specific challenge.
        /// </summary>
        /// <param name="contestId">ID of the contest to query.</param>
        /// <returns>Statistic being watched by the contest.</returns>
        public static Statistic GetStatisticFromContestId(int contestId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (Statistic)(from c in data.ContestDataProviders
                                   where c.id == contestId
                                   select c.statistic).FirstOrDefault();
            }
        }

    }
}