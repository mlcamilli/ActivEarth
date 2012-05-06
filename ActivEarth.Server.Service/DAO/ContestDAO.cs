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
                        reward = contest.Reward,
                        end_mode = (byte)contest.Mode,
                        end_goal = contest.EndValue,
                        end_time = contest.EndTime,
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

                foreach (ContestTeam team in contest.Teams)
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
                                    Reward = c.reward,
                                    StartTime = c.start,
                                    EndValue =(float?)c.end_goal,
                                    EndTime = c.end_time,
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
                                   Reward = c.reward,
                                   StartTime = c.start,
                                   EndValue = (float?)c.end_goal,
                                   EndTime = c.end_time,
                                   Mode = (ContestEndMode)c.end_mode,
                                   Type = (ContestType)c.type,
                                   StatisticBinding = (Statistic)c.statistic,
                                   IsActive = c.active,
                                   IsSearchable = c.searchable,
                                   DeactivatedTime = c.deactivated,
                                   CreatorId = c.creator_id
                               }).FirstOrDefault();
            }

            if (contest != null)
            {
                if (loadTeams)
                {
                    contest.Teams = TeamDAO.GetTeamsFromContestId(contest.ID, loadTeamMembers);
                    contest.Teams.Sort(delegate(ContestTeam t1, ContestTeam t2) { return t2.Score.CompareTo(t1.Score); });
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
                                    Reward = c.reward,
                                    StartTime = c.start,
                                    EndValue = (float?)c.end_goal,
                                    EndTime = c.end_time,
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
                            contest.Teams.Sort(delegate(ContestTeam t1, ContestTeam t2) { return t2.Score.CompareTo(t1.Score); });
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
                        dbContest.reward = contest.Reward;
                        dbContest.end_mode = (byte)contest.Mode;
                        dbContest.end_goal = contest.EndValue;
                        dbContest.end_time = contest.EndTime;
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
                    foreach (ContestTeam team in contest.Teams)
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

        /// <summary>
        /// Updates the scores and bracket assignments for all teams in the contest.
        /// </summary>
        /// <param name="contestId">Contest ID to update.</param>
        public static void UpdateContestStandings(int contestId)
        {
            Contest contest = ContestDAO.GetContestFromContestId(contestId, true, false);

            #region Bracket Size Calculation

            int teamsRemaining = contest.Teams.Count;

            List<int> bracketSizes = ContestDAO.CalculateBracketSizes(teamsRemaining);

            #endregion Bracket Size Calculation

            #region Bracket Assignment

            int currentTeamIndex = 0;
            int currentBracketCount = 0;

            while (currentBracketCount < bracketSizes[(int)ContestBracket.Diamond])
            {
                contest.Teams[currentTeamIndex].Bracket = (int)ContestBracket.Diamond;
                currentBracketCount++;
                currentTeamIndex++;
            }

            currentBracketCount = 0;

            while (currentBracketCount < bracketSizes[(int)ContestBracket.Platinum])
            {
                contest.Teams[currentTeamIndex].Bracket = (int)ContestBracket.Platinum;
                currentBracketCount++;
                currentTeamIndex++;
            }

            currentBracketCount = 0;

            while (currentBracketCount < bracketSizes[(int)ContestBracket.Gold])
            {
                contest.Teams[currentTeamIndex].Bracket = (int)ContestBracket.Gold;
                currentBracketCount++;
                currentTeamIndex++;
            }

            currentBracketCount = 0;

            while (currentBracketCount < bracketSizes[(int)ContestBracket.Silver])
            {
                contest.Teams[currentTeamIndex].Bracket = (int)ContestBracket.Silver;
                currentBracketCount++;
                currentTeamIndex++;
            }

            currentBracketCount = 0;

            while (currentBracketCount < bracketSizes[(int)ContestBracket.Bronze])
            {
                contest.Teams[currentTeamIndex].Bracket = (int)ContestBracket.Bronze;
                currentBracketCount++;
                currentTeamIndex++;
            }

            #endregion Bracket Assignment

            ContestDAO.UpdateContest(contest);
        }

        /// <summary>
        /// Calculates the ActivityScore reward that will be awarded to teams in each bracket.
        /// </summary>
        /// <param name="contest">Contest to calculate rewards for.</param>
        /// <returns>List of bracket rewards, with Bronze occupying position 0, working up to Diamond.</returns>
        public static List<int> CalculateBracketRewards(Contest contest)
        {
            const float DIAMOND_POT_ALLOTMENT = 9;
            const float PLATINUM_POT_ALLOTMENT = 6;
            const float GOLD_POT_ALLOTMENT = 4;
            const float SILVER_POT_ALLOTMENT = 2;
            const float BRONZE_POT_ALLOTMENT = 1;

            List<int> sizes = ContestDAO.CalculateBracketSizes(contest.Teams.Count);

            float totalAllotments = 0;
            totalAllotments += DIAMOND_POT_ALLOTMENT * sizes[(int)ContestBracket.Diamond];
            totalAllotments += PLATINUM_POT_ALLOTMENT * sizes[(int)ContestBracket.Platinum];
            totalAllotments += GOLD_POT_ALLOTMENT * sizes[(int)ContestBracket.Gold];
            totalAllotments += SILVER_POT_ALLOTMENT * sizes[(int)ContestBracket.Silver];
            totalAllotments += BRONZE_POT_ALLOTMENT * sizes[(int)ContestBracket.Bronze];

            int bronzeReward = (int)Math.Round(contest.Reward * BRONZE_POT_ALLOTMENT / totalAllotments);
            int silverReward = (int)Math.Round(contest.Reward * SILVER_POT_ALLOTMENT / totalAllotments);
            int goldReward = (int)Math.Round(contest.Reward * GOLD_POT_ALLOTMENT / totalAllotments);
            int platinumReward = (int)Math.Round(contest.Reward * PLATINUM_POT_ALLOTMENT / totalAllotments);
            int diamondReward = (int)Math.Round(contest.Reward * DIAMOND_POT_ALLOTMENT / totalAllotments);

            return new List<int> { bronzeReward, silverReward, goldReward, platinumReward, diamondReward };
        }

        /// <summary>
        /// Calculates the ActivityScore reward that will be awarded to teams in each bracket.
        /// </summary>
        /// <param name="contest">Contest ID to calculate rewards for.</param>
        /// <returns>List of bracket rewards, with Bronze occupying position 0, working up to Diamond.</returns>
        public static List<int> CalculateBracketRewards(int contestId)
        {
            return ContestDAO.CalculateBracketRewards(ContestDAO.GetContestFromContestId(contestId, true, false));
        }

        /// <summary>
        /// Calculates the number of teams qualifying for each reward bracket in a contest.
        /// </summary>
        /// <param name="teamCount">Number of teams in the contest.</param>
        /// <returns>List of bracket sizes, with Bronze occupying position 0, working up to Diamond.</returns>
        public static List<int> CalculateBracketSizes(int teamCount)
        {
            // Percentages of competitors falling into each bracket
            const float DIAMOND_PERCENT_USERS = 0.03f;
            const float PLATINUM_PERCENT_USERS = 0.08f;
            const float GOLD_PERCENT_USERS = 0.15f;
            const float SILVER_PERCENT_USERS = 0.25f;

            float percentAssigned = 0;

            int diamondCount = (teamCount > 0 ? 
                (int)Math.Max(Math.Round(DIAMOND_PERCENT_USERS * teamCount), 1) : 0);
            percentAssigned += DIAMOND_PERCENT_USERS;
            teamCount -= diamondCount;

            int platinumCount = (teamCount > 0 ?
                (int)Math.Max(Math.Round(PLATINUM_PERCENT_USERS / (1 - percentAssigned) * teamCount), 1) : 0);
            percentAssigned += PLATINUM_PERCENT_USERS;
            teamCount -= platinumCount;

            int goldCount = (teamCount > 0 ?
                (int)Math.Max(Math.Round(GOLD_PERCENT_USERS / (1 - percentAssigned) * teamCount), 1) : 0);
            percentAssigned += GOLD_PERCENT_USERS;
            teamCount -= goldCount;

            int silverCount = (teamCount > 0 ?
                (int)Math.Max(Math.Round(SILVER_PERCENT_USERS / (1 - percentAssigned) * teamCount), 1) : 0);
            percentAssigned += SILVER_PERCENT_USERS;
            teamCount -= silverCount;

            int bronzeCount = teamCount;

            return new List<int> { bronzeCount, silverCount, goldCount, platinumCount, diamondCount };
        }
        

        #endregion ---------- Contest Utilities ----------

    }
}