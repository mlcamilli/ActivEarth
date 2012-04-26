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
        #region ---------- Contest Creation ----------

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
                        deactivated = contest.DeactivatedTime,
                        creator_id = contest.CreatorId
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

        #endregion ---------- Contest Creation ----------

        #region ---------- Contest Retrieval ----------

        public static IEnumerable<Contest> GetAllContests()
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.ContestDataProviders
                        select
                            new Contest
                                {
                                    ID = c.id,
                                    Name = c.name,
                                    Description = c.description,
                                    Reward = c.points,
                                    StartTime = c.start,
                                    EndCondition =
                                        ((ContestEndMode) c.end_mode == ContestEndMode.GoalBased
                                             ? new EndCondition((float) c.end_goal)
                                             : new EndCondition((DateTime) c.end_time)),
                                    Mode = (ContestEndMode) c.end_mode,
                                    Type = (ContestType) c.type,
                                    StatisticBinding = (Statistic) c.statistic,
                                    IsActive = c.active,
                                    DeactivatedTime = c.deactivated,
                                    CreatorId = c.creator_id
                                });
            }
        }

        /// <summary>
        /// Retrieves a Contest from the DB based on its ID.
        /// </summary>
        /// <param name="contestId">Identifier of the contest to retrieve.</param>
        /// <returns>Contest specified by the provided ID.</returns>
        public static Contest GetContestFromContestId(int contestId, bool loadTeams, bool loadTeamMembers)
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
                                   DeactivatedTime = c.deactivated,
                                   CreatorId = c.creator_id
                               }).FirstOrDefault();
            }

            if (contest != null)
            {
                if (loadTeams)
                {
                    contest.Teams = TeamDAO.GetTeamsFromContestId(contest.ID, loadTeamMembers);
                    contest.Teams.Sort(delegate(Team t1, Team t2) { return t2.Score.CompareTo(t1.Score); });
                }
                contest.FormatString = StatisticInfoDAO.GetStatisticFormatString(contest.StatisticBinding);
            }
            return contest;
        }

        /// <summary>
        /// Retrieves all active contests in the DB.
        /// </summary>
        /// <returns>All active contests in the DB.</returns>
        public static List<Contest> GetActiveContests(bool loadTeams, bool loadTeamMembers)
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
                                    DeactivatedTime = c.deactivated,
                                    CreatorId = c.creator_id
                                }).ToList();

                if (contests != null)
                {
                    foreach (Contest contest in contests)
                    {
                        if (loadTeams)
                        {
                            contest.Teams = TeamDAO.GetTeamsFromContestId(contest.ID, loadTeamMembers);
                            contest.Teams.Sort(delegate(Team t1, Team t2) { return t2.Score.CompareTo(t1.Score); });
                        }

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
        public static List<int> FindContests(string name, bool exactMatch)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.ContestDataProviders
                            where (exactMatch ?
                                c.name.ToLower().Equals(name.ToLower()) :
                                c.name.ToLower().Contains(name.ToLower())) &&
                                c.searchable == true
                            select c.id).ToList();
            }
        }

        #endregion ---------- Contest Retrieval ----------

        #region ---------- Contest DB Update ----------

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
                        dbContest.creator_id = contest.CreatorId;

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

        #endregion ---------- Contest DB Update ----------

        #region ---------- Contest DB Removal ----------

        /// <summary>
        /// Deletes an existing Contest from the DB.
        /// </summary>
        /// <param name="contestId">ID for the Contest whose record needs to be removed.</param>
        /// <returns>True on success (or the contest didn't exist), false on failure.</returns>
        public static bool RemoveContestFromContestId(int contestId)
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
                        (from c in data.ContestDataProviders
                         where
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

        #endregion ---------- Contest DB Removal ----------

        #region ---------- Contest Utilities ----------

        /// <summary>
        /// Returns the list of IDs of all contests that a user is currently participating in.
        /// </summary>
        /// <param name="userId">ID of the user to search for.</param>
        /// <returns>List of IDs of all contests that the user is currently participating in.</returns>
        public static List<int> GetContestIdsFromUserId(int userId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.TeamMemberDataProviders
                        where c.user_id == userId
                        select c.contest_id).ToList();
            }
        }

        /// <summary>
        /// Returns the list of IDs of all contests that a group is currently participating in.
        /// </summary>
        /// <param name="groupId">ID of the group to search for.</param>
        /// <returns>List of IDs of all contests that the group is currently participating in.</returns>
        public static List<int> GetContestIdsFromGroupId(int groupId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.TeamDataProviders
                        where c.group_id == groupId
                        select c.contest_id).ToList();
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

        /// <summary>
        /// Returns the name of a contest based on its ID.
        /// </summary>
        /// <param name="groupId">ID of the contest.</param>
        /// <returns>Name of the contest matching the provided ID.</returns>
        public static string GetContestNameFromContestId(int contestId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.ContestDataProviders
                        where c.id == contestId
                        select c.name).FirstOrDefault();
            }
        }

        #endregion ---------- Contest Utilities ----------

    }
}