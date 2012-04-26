using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Server.Service;
using ActivEarth.Server.Service.Statistics;

namespace ActivEarth.DAO
{
    public class TeamDAO
    {

        #region ---------- Team Methods ----------

        #region Team Creation

        /// <summary>
        /// Saves a Team as a new entry in the DB.
        /// </summary>
        /// <param name="team">Team object to add to the DB.</param>
        /// <returns>ID of the created team on success, 0 on failure.</returns>
        public static int CreateNewTeam(Team team)
        {
            try
            {
                int id;

                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var teamData = new TeamDataProvider
                    {
                        name = team.Name,
                        score = team.Score,
                        contest_id = team.ContestId,
                        locked = team.IsLocked,
                        group_id = team.GroupId,
                        bracket = team.Bracket
                    };
                    data.TeamDataProviders.InsertOnSubmit(teamData);
                    data.SubmitChanges();

                    id = teamData.id;
                }

                TeamDAO.UpdateTeamMembers(team);

                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion Team Creation

        #region Team Retrieval 

        /// <summary>
        /// Retrieves a Team from the DB based on its ID.
        /// </summary>
        /// <param name="teamId">Identifier of the team to retrieve.</param>
        /// <param name="loadMembers">Indicates whether or not the team member list should be populated.</param>
        /// <returns>Team specified by the provided ID.</returns>
        public static Team GetTeamFromTeamId(int teamId, bool loadMembers)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                Team team = (from c in data.TeamDataProviders
                        where c.id == teamId
                        select
                            new Team
                            {
                                ID = c.id,
                                Name = c.name,
                                Score = (float)c.score,
                                IsLocked = c.locked,
                                GroupId = c.group_id,
                                ContestId = c.contest_id,
                                Bracket = c.bracket
                            }).FirstOrDefault();

                if (loadMembers && team != null)
                {
                    team.Members = GetTeamMembersFromTeamId(team.ID);
                }

                return team;
            }
        }

        /// <summary>
        /// Retrieves all teams competing in a particular contest.
        /// </summary>
        /// <param name="contestId">ID of the contest to load teams for.</param>
        /// <param name="loadMembers">Indicates whether or not the team member lists should be populated.</param>
        /// <returns>All teams currently participating in the contest specified by the given ID.</returns>
        public static List<Team> GetTeamsFromContestId(int contestId, bool loadMembers)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                List<Team> teams =  (from c in data.TeamDataProviders
                                    where c.contest_id == contestId
                                    select
                                        new Team
                                        {
                                            ID = c.id,
                                            Name = c.name,
                                            Score = (float)c.score,
                                            IsLocked = c.locked,
                                            GroupId = c.group_id,
                                            ContestId = c.contest_id,
                                            Bracket = c.bracket
                                        }).ToList();

                if (loadMembers)
                {
                    foreach (Team team in teams)
                    {
                        team.Members = GetTeamMembersFromTeamId(team.ID);
                    }
                }

                return teams;
            }
        }

        /// <summary>
        /// Retrieves the team from a given contest which contains a given member.
        /// </summary>
        /// <param name="userId">ID of the user to match the team to.</param>
        /// <param name="contestId">ID of the contest to match the team to.</param>
        /// <param name="loadMembers">Indicates whether or not the team member lists should be populated.</param>
        /// <returns>Team containing the given member from the given contest.</returns>
        public static Team GetTeamFromUserIdAndContestId(int userId, int contestId, bool loadMembers)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                int teamId = (from c in data.TeamMemberDataProviders
                        where c.user_id == userId && c.contest_id == contestId
                        select c.team_id).FirstOrDefault();

                return TeamDAO.GetTeamFromTeamId(teamId, loadMembers);
            }
        }

        /// <summary>
        /// Retrieves all team IDs of which the provided user is a member.
        /// </summary>
        /// <returns>IDs of all teams of which the provided user is a member.</returns>
        public static List<int> GetTeamIdsFromUserId(int userId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.TeamMemberDataProviders
                        where c.user_id == userId
                        select c.team_id).ToList();
            }
        }

        /// <summary>
        /// Retrieves all team IDs which match the provided group.
        /// </summary>
        /// <returns>IDs of all teams which list the provided group as their source.</returns>
        public static List<int> GetTeamIdsFromGroupId(int groupId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.TeamDataProviders
                        where c.group_id == groupId
                        select c.id).ToList();
            }
        }

        #endregion Team Retrieval

        #region Team DB Update

        /// <summary>
        /// Updates an existing Team in the DB.
        /// </summary>
        /// <param name="team">Team whose record needs updating.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdateTeam(Team team)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    TeamDataProvider dbTeam =
                        (from c in data.TeamDataProviders where c.id == team.ID select c).FirstOrDefault();
                    if (dbTeam != null)
                    {
                        dbTeam.name = team.Name;
                        dbTeam.score = team.Score;
                        dbTeam.locked = team.IsLocked;
                        dbTeam.group_id = team.GroupId;
                        dbTeam.contest_id = team.ContestId;
                        dbTeam.bracket = team.Bracket;

                        data.SubmitChanges();
                        UpdateTeamMembers(team);
                        return true;
                    }
                    else
                    {
                        CreateNewTeam(team);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Team DB Update

        #region Team Utilities

        /// <summary>
        /// Recalculates and updates the team's contest score.
        /// </summary>
        /// <param name="team">Team to be updated.</param>
        /// <param name="statistic"></param>
        public static void UpdateTeamScore(int teamId)
        {
            Team team = TeamDAO.GetTeamFromTeamId(teamId, true);

            if (team.IsLocked)
            {
                Statistic statistic = ContestDAO.GetStatisticFromContestId(team.ContestId);
                float total = 0;

                foreach (TeamMember member in team.Members)
                {
                    total += TeamDAO.CalculateUserScore(member.UserId, member.InitialScore, statistic);
                }

                team.Score = total;
                TeamDAO.UpdateTeam(team);
            }
        }

        /// <summary>
        /// Notes each user's state at the beginning of a contest so that
        /// the delta score can be calculated.
        /// 
        /// Sets the initialized flag to true, allowing the calculation of
        /// delta scores.
        /// </summary>
        public static void LockTeam(Team team)
        {
            Statistic statistic = ContestDAO.GetStatisticFromContestId(team.ContestId);

            foreach (TeamMember user in team.Members)
            {
                UserStatistic userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(user.UserId, statistic);
                user.InitialScore = (userStat != null ? userStat.Value : 0);
                user.Initialized = true;
            }

            team.IsLocked = true;
            TeamDAO.UpdateTeam(team);
        }

        /// <summary>
        /// Retrieves the contest ID for the contest that a team is participating in.
        /// </summary>
        /// <param name="teamId">Team ID to query.</param>
        /// <returns>ID of the contest that the team is participating in.</returns>
        private static int GetContestIdFromTeamId(int teamId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.TeamDataProviders
                        where c.id == teamId
                        select c.contest_id).FirstOrDefault();
            }
        }

        #endregion Team Utilities

        #region Team Removal

        /// <summary>
        /// Removes an existing Team from the DB.
        /// </summary>
        /// <param name="team">Team to remove.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool RemoveTeam(int teamId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    TeamDataProvider dbTeam =
                        (from c in data.TeamDataProviders where c.id == teamId select c).FirstOrDefault();
                    if (dbTeam != null)
                    {
                        data.TeamDataProviders.DeleteOnSubmit(dbTeam);
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

        #endregion Team Removal

        #endregion ---------- Team Methods ----------

        #region ---------- Team Member Methods ----------

        #region Team Member Creation

        /// <summary>
        /// Creates a new DB entry for a team member.
        /// </summary>
        /// <param name="teamMember">Member to be added to the DB.</param>
        /// <param name="teamId">Team ID that the member should be added to.</param>
        /// <returns>ID of the newly added Team Member entry on success, 0 on failure.</returns>
        public static int CreateNewTeamMember(TeamMember teamMember, int teamId)
        {
            try
            {
                int contestId = TeamDAO.GetContestIdFromTeamId(teamId);

                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    if (TeamDAO.UserCompetingInContest(teamMember.UserId, contestId)) { throw new Exception("User is already competing in the contest"); }
                    
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var userData = new TeamMemberDataProvider
                    {
                        contest_id = contestId,
                        team_id = teamId,
                        user_id = teamMember.UserId,
                        initialized = teamMember.Initialized,
                        initial_score = teamMember.InitialScore
                    };
                    data.TeamMemberDataProviders.InsertOnSubmit(userData);
                    data.SubmitChanges();
                    return userData.id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Creates a new DB entry for a team member.
        /// </summary>
        /// <param name="userId">ID of the Member to be added to the DB.</param>
        /// <param name="teamId">Team ID that the member should be added to.</param>
        /// <returns>ID of the newly added Team Member entry on success, 0 on failure.</returns>
        public static int CreateNewTeamMember(int userId, int teamId)
        {
            try
            {
                TeamMember member = new TeamMember()
                {
                    UserId = userId
                };

                return TeamDAO.CreateNewTeamMember(member, teamId);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion Team Member Creation

        #region Team Member Retrieval

        /// <summary>
        /// Retrieves the list of team members for a particular team.
        /// </summary>
        /// <param name="teamId">Team ID to retrieve the members for.</param>
        /// <returns>List of team members for the provided team ID.</returns>
        public static List<TeamMember> GetTeamMembersFromTeamId(int teamId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.TeamMemberDataProviders
                        where c.team_id == teamId
                        select
                            new TeamMember
                            {
                                Initialized = c.initialized,
                                InitialScore = (float)c.initial_score,
                                UserId = c.user_id
                            }).ToList();
            }
        }

        #endregion Team Member Retrieval

        #region Team Member DB Update

        /// <summary>
        /// Updates the entry of any members already existing on the team, and creates
        /// new entries for new members of the team.
        /// </summary>
        /// <param name="team">Team whose members must be updated.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdateTeamMembers(Team team)
        {
            try
            {
                foreach (TeamMember user in team.Members)
                {

                    using (SqlConnection connection = ConnectionManager.GetConnection())
                    {
                        var data = new ActivEarthDataProvidersDataContext(connection);

                        TeamMemberDataProvider dbUser =
                            (from u in data.TeamMemberDataProviders
                             where u.team_id == team.ID && u.user_id == user.UserId
                             select u).FirstOrDefault();
                        if (dbUser != null)
                        {
                            dbUser.initial_score = user.InitialScore;
                            dbUser.initialized = user.Initialized;

                            data.SubmitChanges();
                        }
                        else
                        {
                            if (CreateNewTeamMember(user, team.ID) == 0)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Team Member DB Update

        #region Team Member Utilities

        /// <summary>
        /// Queries the DB to see if a user is registered for a particular contest.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="contestId">ID of the contest to query.</param>
        /// <returns>True if the user is competing in the contest, false otherwise.</returns>
        public static bool UserCompetingInContest(int userId, int contestId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    return ((from c in data.TeamMemberDataProviders
                                              where c.user_id == userId && c.contest_id == contestId
                                              select c.team_id).FirstOrDefault()) > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates the user's change in the relevant statistic
        /// since the beginning of the contest; their 'score' for
        /// the contest.
        /// </summary>
        /// <returns></returns>
        private static float CalculateUserScore(int userId, float initial, Statistic statistic)
        {
            UserStatistic userStat = UserStatisticDAO.GetStatisticFromUserIdAndStatType(userId, statistic);
            float current = (userStat != null ? userStat.Value : 0);

            return current - initial;
        }

        #endregion Team Member Utilities

        #endregion ---------- Team Member Methods ----------
    }
}