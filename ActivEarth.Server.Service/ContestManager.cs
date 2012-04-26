using System;
using System.Collections.Generic;
using System.Linq;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.DAO;

namespace ActivEarth.Server.Service.Competition
{

    public class ContestManager
    {
        #region ---------- Constructor ----------

        /// <summary>
        /// Private constructor, can not instantiate.
        /// </summary>
        private ContestManager()
        {
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Creates a new time-based Contest
        /// </summary>
        /// <param name="type">Determines whether the contest in Individual or Group-based.</param>
        /// <param name="name">Contest Name.</param>
        /// <param name="description">Contest Description.</param>
        /// <param name="points">Points to be distributed to the winner(s).</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">Time to end the contest.</param>
        /// <param name="searchable">True if the Contest can be found by searching (public), false if private.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <param name="creatorId">UserID of the creator of the contest.</param>
        /// <returns>ID of the newly created Contest.</returns>
        public static int CreateContest(ContestType type, string name, string description, int points, DateTime start,
            DateTime end, bool searchable, Statistic statistic, int creatorId)
        {
            Contest newContest = new Contest
            {
                Name = name,
                Description = description,
                Reward = points,
                Mode = ContestEndMode.TimeBased,
                Type = type,
                StartTime = start,
                EndCondition = new EndCondition(end),
                StatisticBinding = statistic,
                IsSearchable = searchable,
                IsActive = true,
                DeactivatedTime = null,
                CreatorId = creatorId
            };

            return ContestDAO.CreateNewContest(newContest);
        }

        /// <summary>
        /// Creates a new goal-based Contest
        /// </summary>
        /// <param name="type">Determines whether the contest in Individual or Group-based.</param>
        /// <param name="name">Contest Name.</param>
        /// <param name="description">Contest Description.</param>
        /// <param name="points">Points to be distributed to the winner(s).</param>
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">Score at which to end the contest.</param>
        /// <param name="searchable">True if the Contest can be found by searching (public), false if private.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <param name="creatorId">UserID of the creator of the contest.</param>
        /// <returns>ID of the newly created Contest.</returns>
        public static int CreateContest(ContestType type, string name, string description, int points, DateTime start,
            float end, bool searchable, Statistic statistic, int creatorId)
        {
            Contest newContest = new Contest
            {
                Name = name,
                Description = description,
                Reward = points,
                Mode = ContestEndMode.GoalBased,
                Type = type,
                StartTime = start,
                EndCondition = new EndCondition(end),
                StatisticBinding = statistic,
                IsSearchable = searchable,
                DeactivatedTime = null,
                IsActive = true,
                CreatorId = creatorId
                
            };
            
            return ContestDAO.CreateNewContest(newContest);
            
        }

        /// <summary>
        /// Retrieves a Contest based on its ID.
        /// </summary>
        /// <param name="id">ID of the Contest to be retrieved.</param>
        /// <returns>Contest with ID matching the provided ID.</returns>
        public static Contest GetContest(int id, bool loadTeams, bool loadTeamMembers)
        {
            return ContestDAO.GetContestFromContestId(id, loadTeams, loadTeamMembers);
        }

        /// <summary>
        /// Calculates the reward for a time-based contest.
        /// </summary>
        /// <param name="duration">Duration of the contest.</param>
        /// <param name="statistic">Statistic on which the contest is based.</param>
        /// <returns></returns>
        public static int CalculateContestReward(TimeSpan duration, Statistic statistic)
        {
            return 0;
        }

        /// <summary>
        /// Calculates the reward for a goal-based contest.
        /// </summary>
        /// <param name="goal">Goal value for the contest.</param>
        /// <param name="statistic">Statistic on which the contest is based.</param>
        /// <returns></returns>
        public static int CalculateContestReward(float goal, Statistic statistic)
        {
            return 0;
        }

        /// <summary>
        /// Cleans up the contest list, deactivating expired time-based contests and deleting
        /// deactivated contests that have reached the end of their retainment period.
        /// </summary>
        public static void CleanUp()
        {
            foreach (Contest contest in ContestDAO.GetActiveContests(false, false))
            {
                if (contest.EndCondition.EndTime <= DateTime.Now)
                {
                    //Distribute Activity Score rewards
                    throw new NotImplementedException("Distribution of Contest rewards not yet implemented");

                    contest.IsActive = false;
                    contest.DeactivatedTime = DateTime.Now;
                    ContestDAO.UpdateContest(contest);
                }
            }

            ContestDAO.RemoveOldContests();
        }

        /// <summary>
        /// Locks competitor initial values such that the calculation of
        /// deltas can begin (to calculate team scores).
        /// </summary>
        public static void LockContest(int contestId)
        {
            List<Team> teams = TeamDAO.GetTeamsFromContestId(contestId, true);

            foreach (Team team in teams)
            {
                TeamDAO.LockTeam(team);
            }
        }

        /// <summary>
        /// Adds a team to the Contest.
        /// </summary>
        /// <param name="team">The team to be added.</param>
        public static void AddTeam(Team team)
        {
            TeamDAO.CreateNewTeam(team);
        }

        /// <summary>
        /// Adds a group to a group Contest; will fail if a group is added to an individual contest.
        /// </summary>
        /// <param name="contestId">ID for the contest the group should be added to.</param>
        /// <param name="group">Group to be added.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool AddGroup(int contestId, Group group)
        {
            Contest contest = ContestDAO.GetContestFromContestId(contestId, true, false);
            if (contest.Type == ContestType.Group)
            {
                string teamName = group.Name;

                group.Contests.Add(contest);

                Team newTeam = new Team
                {
                    ContestId = contestId,
                    Name = teamName,
                    GroupId = group.ID
                };

                int teamId = TeamDAO.CreateNewTeam(newTeam);

                foreach (User user in group.Members)
                {
                    TeamDAO.CreateNewTeamMember(user.UserID, teamId);
                }

                return (teamId > 0);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a user to the Contest.
        /// </summary>
        /// <param name="user">User to be added.</param>
        public static void AddUser(int contestId, User user)
        {
            if (!TeamDAO.UserCompetingInContest(user.UserID, contestId))
            {
                string teamName = String.Format("{0} {1}", user.FirstName, user.LastName);
                //TODO: Assert that no team with this name exists already

                Team newTeam = new Team
                {
                    ContestId = contestId,
                    Name = teamName
                };

                int teamId = TeamDAO.CreateNewTeam(newTeam);
                TeamDAO.CreateNewTeamMember(user.UserID, teamId);
            }
        }

        /// <summary>
        /// Removes a team from its associated Contest.
        /// </summary>
        /// <param name="team">The team to be removed.</param>
        public static void RemoveTeam(Team team)
        {
            if (team != null)
            {
                TeamDAO.RemoveTeam(team.ID);
            }
        }

        /// <summary>
        /// Queries the DB to see if a user is registered for a particular contest.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="contestId">ID of the contest to query.</param>
        /// <returns>True if the user is competing in the contest, false otherwise.</returns>
        public static bool UserCompetingInContest(int userId, int contestId)
        {
            return (TeamDAO.UserCompetingInContest(userId, contestId));
        }

        #endregion ---------- Public Methods ----------
    }
}