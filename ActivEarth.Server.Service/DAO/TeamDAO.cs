using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class TeamDAO
    {
        /// <summary>
        /// Retrieves a Team from the DB based on its ID.
        /// </summary>
        /// <param name="teamId">Identifier of the team to retrieve.</param>
        /// <returns>Team specified by the provided ID.</returns>
        public static Team GetTeamFromTeamId(int teamId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.TeamDataProviders
                        where c.id == teamId
                        select
                            new Team
                            {
                                ID = c.id,
                                Name = c.name,
                                Score = (float)c.score,
                                Members = GetTeamMembersFromTeamId(c.id)
                            }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves all teams competing in a particular contest.
        /// </summary>
        /// <returns>All teams currently participating in the contest specified by the given ID.</returns>
        public static List<Team> GetTeamsFromContestId(int contestId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from c in data.TeamDataProviders
                        where c.contest_id == contestId
                        select
                            new Team
                            {
                                ID = c.id,
                                Name = c.name,
                                Score = (float)c.score,
                                Members = GetTeamMembersFromTeamId(c.id)
                            }).ToList();
            }
        }

        /// <summary>
        /// Saves a Team as a new entry in the DB.
        /// </summary>
        /// <param name="team">Team object to add to the DB.</param>
        /// <returns>ID of the created team on success, 0 on failure.</returns>
        public static int CreateNewTeam(Team team, int contestId)
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
                        contest_id = contestId
                    };
                    data.TeamDataProviders.InsertOnSubmit(teamData);
                    data.SubmitChanges();

                    id = teamData.id;
                }

                UpdateTeamMembers(team);

                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Updates an existing Team in the DB.
        /// </summary>
        /// <param name="challenge">Team whose record needs updating.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdateTeam(Team team, int contestId)
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

                        data.SubmitChanges();
                        UpdateTeamMembers(team);
                        return true;
                    }
                    else
                    {
                        CreateNewTeam(team, contestId);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

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
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var userData = new TeamMemberDataProvider
                    {
                        team_id = (int)teamId,
                        user_id = (int)teamMember.User.UserID,
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
                        where c.team_id == (int)teamId
                        select
                            new TeamMember
                            {
                                Initialized = c.initialized,
                                InitialScore = (float)c.initial_score,

                                // Activate when necessary profile dependencies are resolved
                                //User = UserDAO.GetUserFromUserId(c.user_id) 
                            }).ToList();
            }
        }

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
                                 where u.team_id == team.ID && u.user_id == user.User.UserID
                                 select u).FirstOrDefault();
                            if (dbUser != null)
                            {
                                dbUser.initial_score = user.InitialScore;
                                dbUser.initialized = user.Initialized;

                                data.SubmitChanges();
                                return true;
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
    }
}