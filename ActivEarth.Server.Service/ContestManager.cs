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
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">Time to end the contest.</param>
        /// <param name="searchable">True if the Contest can be found by searching (public), false if private.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <param name="creatorId">UserID of the creator of the contest.</param>
        /// <returns>ID of the newly created Contest.</returns>
        public static int CreateContest(ContestType type, string name, string description, DateTime start,
            DateTime end, bool searchable, Statistic statistic, int creatorId)
        {
            Contest newContest = new Contest
            {
                Name = name,
                Description = description,
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
        /// <param name="start">Time to start the contest.</param>
        /// <param name="end">Score at which to end the contest.</param>
        /// <param name="searchable">True if the Contest can be found by searching (public), false if private.</param>
        /// <param name="statistic">Statistic on which the Contest is based.</param>
        /// <param name="creatorId">UserID of the creator of the contest.</param>
        /// <returns>ID of the newly created Contest.</returns>
        public static int CreateContest(ContestType type, string name, string description, DateTime start,
            float end, bool searchable, Statistic statistic, int creatorId)
        {
            Contest newContest = new Contest
            {
                Name = name,
                Description = description,
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
        /// Finds the list of teams that should be displayed to the user as part of
        /// the contest standings report.  For the user's current bracket, the user's team
        /// will be shown.  For brackets better than the user's current standing, the lowest
        /// team in the bracket is shown (the team that the user needs to pass to enter that bracket).
        /// For brackets worse than the user's current standing, the top team in the bracket is shown
        /// (the team that the user needs to stay ahead of to avoid falling into that bracket).
        /// </summary>
        /// <param name="userTeam">Team that the current user is on.</param>
        /// <param name="contest">Contest to be analyzed.</param>
        /// <returns>List of teams to display in the contest standings table.</returns>
        public static List<Team> GetTeamsToDisplay(Team userTeam, Contest contest)
        {
            int userBracket;
            List<Team> toDisplay = new List<Team>();

            if (userTeam == null)
            {
                userBracket = (int)ContestBracket.Diamond + 1;
            }
            else
            {
                userBracket = userTeam.Bracket;
            }

            List<Team> diamond = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Diamond).ToList();
            List<Team> platinum = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Platinum).ToList();
            List<Team> gold = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Gold).ToList();
            List<Team> silver = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Silver).ToList();
            List<Team> bronze = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Bronze).ToList();

            if ((int)ContestBracket.Diamond > userBracket)
                { toDisplay.Add(diamond.LastOrDefault()); }
            else if ((int)ContestBracket.Diamond == userBracket) 
                { toDisplay.Add(userTeam); }
            else
                { toDisplay.Add(diamond.FirstOrDefault()); }


            if ((int)ContestBracket.Platinum > userBracket)
                { toDisplay.Add(platinum.LastOrDefault()); }
            else if ((int)ContestBracket.Platinum == userBracket)
                { toDisplay.Add(userTeam); }
            else
                { toDisplay.Add(platinum.FirstOrDefault()); }


            if ((int)ContestBracket.Gold > userBracket)
                { toDisplay.Add(gold.LastOrDefault()); }
            else if ((int)ContestBracket.Gold == userBracket)
                { toDisplay.Add(userTeam); }
            else
                { toDisplay.Add(gold.FirstOrDefault()); }

            
            if ((int)ContestBracket.Silver > userBracket)
                { toDisplay.Add(silver.LastOrDefault()); }
            else if ((int)ContestBracket.Silver == userBracket)
                { toDisplay.Add(userTeam); }
            else
                { toDisplay.Add(silver.FirstOrDefault()); }

            
            if ((int)ContestBracket.Bronze > userBracket)
                { toDisplay.Add(bronze.LastOrDefault()); }
            else if ((int)ContestBracket.Bronze == userBracket)
                { toDisplay.Add(userTeam); }
            else
                { toDisplay.Add(bronze.FirstOrDefault()); }

            return toDisplay;
        }

        /// <summary>
        /// Calculates the reward for a time-based contest.
        /// </summary>
        /// <param name="duration">Duration of the contest.</param>
        /// <param name="statistic">Statistic on which the contest is based.</param>
        /// <returns>Total reward pot for a contest with the given values.</returns>
        public static int CalculateContestReward(int days, int members)
        {
            float membersExponent = 1.03f;
            float daysCoefficient = 0.7f;
            float daysInitial = 3;

            return (int)Math.Round(Math.Pow(members, membersExponent) * 
                ((daysCoefficient * days) + daysInitial));
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
        /// Adds a group to a group Contest; will fail if a group is added to an individual contest.
        /// </summary>
        /// <param name="contestId">ID for the contest the group should be added to.</param>
        /// <param name="group">Group to be added.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool AddGroup(int contestId, Group group)
        {
            Contest contest = ContestDAO.GetContestFromContestId(contestId, false, false);

            if (contest.Type == ContestType.Group)
            {
                string teamName = group.Name;

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

                contest.Reward = ContestManager.CalculateContestReward(
                    ContestManager.CalculateEstimatedLengthInDays(contest),
                    TeamDAO.GetCompetitorCount(contestId));
                ContestDAO.UpdateContest(contest);

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

                Team newTeam = new Team
                {
                    ContestId = contestId,
                    Name = teamName
                };

                int teamId = TeamDAO.CreateNewTeam(newTeam);
                TeamDAO.CreateNewTeamMember(user.UserID, teamId);
            }

            Contest contest = ContestDAO.GetContestFromContestId(contestId, false, false);
            contest.Reward = ContestManager.CalculateContestReward(
                ContestManager.CalculateEstimatedLengthInDays(contest),
                TeamDAO.GetCompetitorCount(contestId));
            ContestDAO.UpdateContest(contest);
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

        public static void UpdateTeamScore(int teamId)
        {
            
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Methods ----------

        /// <summary>
        /// Returns the length of a contest of a time-based contest, and estimates the length of a goal-based contest. 
        /// </summary>
        /// <param name="contest">Contest to calculate the length of.</param>
        /// <returns>Estimated length of the contest.</returns>
        private static int CalculateEstimatedLengthInDays(Contest contest)
        {
            if (contest.Mode == ContestEndMode.TimeBased)
            {
                return contest.EndCondition.EndTime.Subtract(contest.StartTime).Days;
            }
            else // ContestEndMode.GoalBased
            {
                List<float> expectedPerDay = new List<float> { 7000, 2, 4, 1, 0.6f, 1, 7, 1.25f, 0.5f, 0.5f, 0.25f  };
                return (int)Math.Round(contest.EndCondition.EndValue / expectedPerDay[(int)contest.StatisticBinding]);
            }
        }

        #endregion ---------- Private Methods ----------
    }
}