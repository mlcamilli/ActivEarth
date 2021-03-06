﻿using System;
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
                EndTime = end,
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
                EndValue = end,
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
        public static List<ContestTeam> GetTeamsToDisplay(ContestTeam userTeam, Contest contest)
        {
            int userBracket;
            List<ContestTeam> toDisplay = new List<ContestTeam>();

            if (userTeam == null)
            {
                userBracket = (int)ContestBracket.Diamond + 1;
            }
            else
            {
                userBracket = userTeam.Bracket;
            }

            List<ContestTeam> diamond = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Diamond).ToList();
            List<ContestTeam> platinum = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Platinum).ToList();
            List<ContestTeam> gold = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Gold).ToList();
            List<ContestTeam> silver = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Silver).ToList();
            List<ContestTeam> bronze = contest.Teams.Where(t => t.Bracket == (int)ContestBracket.Bronze).ToList();

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
        /// <param name="days">Duration of the contest, in days.</param>
        /// <param name="teams">Number of teams competing in the contest.</param>
        /// <returns>Total reward pot for a contest with the given values.</returns>
        public static int CalculateContestReward(int days, int teams)
        {
            float membersExponent = 1.03f;
            float daysCoefficient = 0.7f;
            float daysInitial = 3;

            return (int)Math.Round(Math.Pow(teams, membersExponent) * 
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
                if (contest.EndTime <= DateTime.Now)
                {
                    ContestManager.DistributeContestReward(contest.ID);
                }
            }

            ContestDAO.RemoveOldContests();
        }

        /// <summary>
        /// Distributes the ActivityScore reward for a contest and deactivates it.
        /// </summary>
        /// <param name="contestId">ID of the contest to process.</param>
        public static void DistributeContestReward(int contestId)
        {
            Contest contest = ContestDAO.GetContestFromContestId(contestId, true, true);
            List<int> rewardsByBracket = ContestDAO.CalculateBracketRewards(contest);

            foreach (ContestTeam team in contest.Teams)
            {
                if (team.Members.Count > 0)
                {
                    float maxScore = team.Members.Max(m => m.Score);

                    foreach (ContestTeamMember member in team.Members)
                    {
                        int reward = (maxScore == 0 ? 0 :
                            (int)Math.Round(rewardsByBracket[team.Bracket] * (member.Score / maxScore)));
                        UserDAO.AddContestPoints(member.UserId, reward);
                    }
                }
            }

            contest.IsActive = false;
            contest.DeactivatedTime = DateTime.Now;
            ContestDAO.UpdateContest(contest);
        }

        /// <summary>
        /// Locks competitor initial values such that the calculation of
        /// deltas can begin (to calculate team scores).
        /// </summary>
        public static void LockContest(int contestId)
        {
            List<ContestTeam> teams = TeamDAO.GetTeamsFromContestId(contestId, true);

            foreach (ContestTeam team in teams)
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

                ContestTeam newTeam = new ContestTeam
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
                    TeamDAO.GetTeamsFromContestId(contestId, false).Count);
                ContestDAO.UpdateContest(contest);

                return (teamId > 0);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a group from a Contest.
        /// </summary>
        /// <param name="contestId">ID for the contest the group should be removed from.</param>
        /// <param name="group">Group to be removed.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool RemoveGroup(int contestId, Group group)
        {
            try
            {
                ContestTeam team = TeamDAO.GetTeamsFromContestId(contestId, false).Where(t => t.GroupId == group.ID).FirstOrDefault();

                if (team != null && TeamDAO.RemoveTeam(team.ID))
                {
                    Contest contest = ContestDAO.GetContestFromContestId(contestId, false, false);
                    contest.Reward = ContestManager.CalculateContestReward(
                        ContestManager.CalculateEstimatedLengthInDays(contest),
                    TeamDAO.GetTeamsFromContestId(contestId, false).Count);
                    ContestDAO.UpdateContest(contest);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
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

                ContestTeam newTeam = new ContestTeam
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
                    TeamDAO.GetTeamsFromContestId(contestId, false).Count);
            ContestDAO.UpdateContest(contest);
        }

        /// <summary>
        /// Adds a user to the Contest.
        /// </summary>
        /// <param name="user">User to be added.</param>
        public static bool RemoveUser(int contestId, User user)
        {
            try
            {
                ContestTeam team = TeamDAO.GetTeamFromUserIdAndContestId(user.UserID, contestId, true);
                if (team == null) return false;

                if (team.Members.Count > 1)
                {
                    return TeamDAO.RemoveTeamMemberFromUserIdAndContestId(user.UserID, contestId);
                }
                else
                {
                    TeamDAO.RemoveTeam(team.ID);

                    Contest contest = ContestDAO.GetContestFromContestId(contestId, false, false);
                    contest.Reward = ContestManager.CalculateContestReward(
                        ContestManager.CalculateEstimatedLengthInDays(contest),
                    TeamDAO.GetTeamsFromContestId(contestId, false).Count);
                    ContestDAO.UpdateContest(contest);

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
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
                return contest.EndTime.Value.Subtract(contest.StartTime).Days;
            }
            else // ContestEndMode.GoalBased
            {
                List<float> expectedPerDay = new List<float> { 7000, 2, 4, 1, 0.6f, 1, 7, 1.25f, 0.5f, 0.5f, 0.25f  };
                return (int)Math.Round(contest.EndValue.Value / expectedPerDay[(int)contest.StatisticBinding]);
            }
        }

        #endregion ---------- Private Methods ----------
    }
}