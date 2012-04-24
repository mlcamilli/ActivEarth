using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.DAO;
using ActivEarth.Server.Service.Statistics;
using ActivEarth.Server.Service.Competition;

namespace ActivEarth.Tests.Competition
{
    /// <summary>
    /// Summary description for ContestTest
    /// </summary>
    [TestClass]
    public class ContestTest
    {
        private User _user1;
        private User _user2;
        private User _user3;
        private User _user4;

        private Group _group1;
        private Group _group2;

        private TransactionScope _trans;
        
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
        }

        [TestInitialize]
        public void Initialize()
        {
            _user1 = new User
            {
                UserName = "testSubject1",
                FirstName = "Test",
                LastName = "Subject1",
                City = "St. Louis",
                State = "MO",
                Email = "email1@test.com"
            };

            _user2 = new User
            {
                UserName = "testSubject2",
                FirstName = "Test",
                LastName = "Subject2",
                City = "Missoula",
                State = "MT",
                Email = "email1@test.net"
            };
            _user3 = new User
            {
                UserName = "testSubject3",
                FirstName = "Test",
                LastName = "Subject3",
                City = "Oakland",
                State = "CA",
                Email = "email1@test.org"
            };

            _user4 = new User
            {
                UserName = "testSubject4",
                FirstName = "Test",
                LastName = "Subject4",
                City = "Albany",
                State = "NY",
                Email = "email1@test.gov"
            };

            _group1 = new Group
            {
                Name = "Group 1",
                Owner = _user1,
                Description = String.Empty
            };
            _group1.Members.Add(_user1);
            _group1.Members.Add(_user2);

            _group2 = new Group
            {
                Name = "Group 2",
                Owner = _user3,
                Description = String.Empty
            };
            _group2.Members.Add(_user3);
            _group2.Members.Add(_user4);

            _trans = new TransactionScope();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _trans.Dispose();
        }

        #region ---------- Test Cases ----------

        /// <summary>
        /// Verifies that after locking initial values, teams in a contest report a score of 0.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Create a new group contest.
        /// 2) Add two teams to the contest.
        /// 3) VERIFY: Contest contains two teams.
        /// 4) VERIFY: Each team contains the correct number of members.
        /// 5) Lock contest initialization values.
        /// 6) VERIFY: Each team reports a score of 0.
        /// </remarks>
        [TestMethod]
        public void TestContestGroupContestInitialization()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating time-based group contest");
                int id = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    true, Statistic.Steps, _user1.UserID);

                Log("Adding groups to the contest");
                ContestManager.AddGroup(id, _group1);
                ContestManager.AddGroup(id, _group2);

                Contest contest = ContestManager.GetContest(id, true, true);

                Log("Verifying team count");
                Assert.AreEqual(2, contest.Teams.Count);

                Log("Verifying number of members per team");
                Assert.AreEqual(2, contest.Teams[0].Members.Count);
                Assert.AreEqual(2, contest.Teams[1].Members.Count);

                Log("Locking team initialization");
                ContestManager.LockContest(id);
                contest = ContestManager.GetContest(id, true, false);

                Log("Verifying initial team scores");
                Assert.AreEqual(0, contest.Teams[0].Score);
                Assert.AreEqual(0, contest.Teams[1].Score);
            }
        }

        /// <summary>
        /// Verifies that contest end modes (time vs. goal) are correctly assigned.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Create a time-based contest.
        /// 2) Create a goal-based contest.
        /// 3) VERIFY: First contest is determined to be time-based.
        /// 4) VERIFY: Second contest is determined to be goal-based.
        /// </remarks>
        [TestMethod]
        public void TestContestEndMode()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating time-based group contest");
                int timeId = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    true, Statistic.Steps, _user1.UserID);
                Contest timedContest = ContestManager.GetContest(timeId, false, false);

                Log("Creating goal-based individual contest");
                int goalId = ContestManager.CreateContest(ContestType.Individual, "Test Contest 2",
                    "This is a test goal-based contest.", 50, DateTime.Now, 50000,
                    true, Statistic.Steps, _user1.UserID);
                Contest goalContest = ContestManager.GetContest(goalId, false, false);

                Log("Verifying time-based contest end mode");
                Assert.AreEqual(ContestEndMode.TimeBased, timedContest.Mode);

                Log("Verifying goal-based contest end mode");
                Assert.AreEqual(ContestEndMode.GoalBased, goalContest.Mode);
            }
        }

        /// <summary>
        /// Verifies that after calculating scores, teams are presented in sorted order (for reporting
        /// of standings) in a group competition (multiple members per team).
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1)  Create a group contest.
        /// 2)  Add two teams to the contest.
        /// 3)  Lock contest initial values.
        /// 4)  Increase the statistics of individual members such that group1 is winning.
        /// 5)  Update contest scores.
        /// 6)  VERIFY: First team in the team collection is group1.
        /// 7)  VERIFY: Second team in the team collection is group2.
        /// 8)  Increase the statistics of individual members such that group2 is winning.
        /// 9)  Update contest scores.
        /// 10) VERIFY: First team in the team collection is group2.
        /// 11) VERIFY: Second team in the team collection is group1.
        /// </remarks>
        [TestMethod]
        public void TestContestGroupTeamsSorted()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating group contest");
                int id = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    true, Statistic.Steps, _user1.UserID);

                ContestManager.AddGroup(id, _group1);
                ContestManager.AddGroup(id, _group2);

                Log("Locking initial values");
                ContestManager.LockContest(id);

                Log("Setting individual statistics such that group1 is winning");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 100);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 100);
                StatisticManager.SetUserStatistic(_user3.UserID, Statistic.Steps, 50);
                StatisticManager.SetUserStatistic(_user4.UserID, Statistic.Steps, 50);

                Contest contest = ContestManager.GetContest(id, true, true);

                Log("Verifying first team is group1");
                Assert.AreEqual("Group 1", contest.Teams[0].Name);

                Log("Verifying second team is group2");
                Assert.AreEqual("Group 2", contest.Teams[1].Name);

                Log("Setting individual statistics such that group2 is winning");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 200);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 200);
                StatisticManager.SetUserStatistic(_user3.UserID, Statistic.Steps, 300);
                StatisticManager.SetUserStatistic(_user4.UserID, Statistic.Steps, 300);

                contest = ContestManager.GetContest(id, true, false);

                Log("Verifying first team is group2");
                Assert.AreEqual("Group 2", contest.Teams[0].Name);

                Log("Verifying second team is group1");
                Assert.AreEqual("Group 1", contest.Teams[1].Name);
            }
        }

        /// <summary>
        /// Verifies that after calculating scores, teams are presented in sorted 
        /// order (for reporting of standings) in an individual competition 
        /// (one member per team).
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Create an individual contest.
        /// 2) Add four users to the contest.
        /// 3) Lock contest initial values.
        /// 4) Increase the statistics of individual members such that the standings 
        ///     are shuffled (relative to the order members were added).
        /// 5) Update contest scores.
        /// 6) VERIFY: Teams are presented in descending order by score.
        /// </remarks>
        [TestMethod]
        public void TestContestIndividualTeamsRemainSorted()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating individual contest");
                int id = ContestManager.CreateContest(ContestType.Individual, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    true, Statistic.Steps, _user1.UserID);

                ContestManager.AddUser(id, _user1);
                ContestManager.AddUser(id, _user2);
                ContestManager.AddUser(id, _user3);
                ContestManager.AddUser(id, _user4);

                Log("Locking initial values");
                ContestManager.LockContest(id);

                Log("Setting individual statistics");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 25);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 75);
                StatisticManager.SetUserStatistic(_user3.UserID, Statistic.Steps, 50);
                StatisticManager.SetUserStatistic(_user4.UserID, Statistic.Steps, 100);

                Contest contest = ContestManager.GetContest(id, true, true);

                Log("Verifying team order");
                Assert.IsTrue(contest.Teams[0].ContainsMember(_user4.UserID));
                Assert.IsTrue(contest.Teams[1].ContainsMember(_user2.UserID));
                Assert.IsTrue(contest.Teams[2].ContainsMember(_user3.UserID));
                Assert.IsTrue(contest.Teams[3].ContainsMember(_user1.UserID));
            }
        }

        /// <summary>
        /// Verifies the correct calculation of a team's contest score.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Create a group contest.
        /// 2) Add a multi-member team to the contest.
        /// 3) Lock contest initial values.
        /// 4) Increase the statistics of the individual members.
        /// 5) Update contest scores.
        /// 6) VERIFY: Team's score is equal to the sum of the increase 
        ///     in the members' values.
        /// </remarks>
        [TestMethod]
        public void TestContestTeamScoreCalculation()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating group contest");
                int id = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    true, Statistic.Steps, _user1.UserID);

                Team team = new Team()
                {
                    Name = "Team1",
                    ContestId = id
                };

                int teamId = TeamDAO.CreateNewTeam(team);

                TeamDAO.CreateNewTeamMember(_user1.UserID, teamId);
                TeamDAO.CreateNewTeamMember(_user2.UserID, teamId);
                TeamDAO.CreateNewTeamMember(_user3.UserID, teamId);
                TeamDAO.CreateNewTeamMember(_user4.UserID, teamId);

                Log("Setting individual initial statistics");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 0);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 50);
                StatisticManager.SetUserStatistic(_user3.UserID, Statistic.Steps, 100);
                StatisticManager.SetUserStatistic(_user4.UserID, Statistic.Steps, 150);

                Log("Locking initial values");
                ContestManager.LockContest(id);

                Log("Adding 50 steps to each user");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 50);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 100);
                StatisticManager.SetUserStatistic(_user3.UserID, Statistic.Steps, 150);
                StatisticManager.SetUserStatistic(_user4.UserID, Statistic.Steps, 200);

                Log("Retrieving Contest from DB");
                Contest contest = ContestManager.GetContest(id, true, false);

                Log("Verifying team aggregate score");
                Assert.AreEqual(200, contest.Teams[0].Score);
            }
        }

        /// <summary>
        /// Tests the ability to update the value of a contest's field in the DB.
        /// </summary>
        [TestMethod]
        public void TestUpdateContestChangeName()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                int id;
                string newName = "New Name";

                Log("Creating contest");
                Contest contest = new Contest()
                {
                    Name = "Test Contest1",
                    Description = "This is a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.Steps,
                    CreatorId = _user1.UserID
                }; 

                Log("Adding contest to the database");
                Assert.IsTrue((id = ContestDAO.CreateNewContest(contest)) > 0);

                Log("Loading contest from the database");
                Contest retrieved = ContestDAO.GetContestFromContestId(id, false, false);

                Log("Verifying that a matching contest was found");
                Assert.IsNotNull(retrieved);

                Log("Renaming the Contest");
                retrieved.Name = newName;

                Log("Updating the contest in the DB");
                Assert.IsTrue(ContestDAO.UpdateContest(retrieved));

                Log("Reloading contest from the database");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id, false, false);

                Log("Verifying that a matching contest was found");
                Assert.IsNotNull(retrieved2);

                Log("Verifying that the contest has the new name");
                Assert.AreEqual(newName, retrieved2.Name);

            }
        }

        /// <summary>
        /// Tests the ability to retrieve the list of all current contests.
        /// </summary>
        [TestMethod]
        public void TestGetAllContests()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Getting pre-existing Contest count");
                int contestsBefore = ContestDAO.GetActiveContests(false, false).Count;

                Log("Creating contests");
                Contest contest1 = new Contest()
                {
                    Name = "Test Contest1",
                    Description = "This is a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.Steps,
                    CreatorId = _user1.UserID
                }; 
                
                Contest contest2 = new Contest()
                {
                    Name = "Test Contest2",
                    Description = "This is also a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.BikeDistance,
                    CreatorId = _user1.UserID
                }; 
                
                Contest contest3 = new Contest()
                {
                    Name = "Test Contest3",
                    Description = "This is another test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.RunDistance,
                    CreatorId = _user1.UserID
                };

                Log("Adding contests to DB");
                int id1 = ContestDAO.CreateNewContest(contest1);
                int id2 = ContestDAO.CreateNewContest(contest2);
                int id3 = ContestDAO.CreateNewContest(contest3);

                Log("Verifying that GetAllContests returns three contests");
                Assert.AreEqual(contestsBefore + 3, ContestDAO.GetActiveContests(false, false).Count);
            }
        }

        /// <summary>
        /// Tests the retrieval and rehydration of a contest object based on its Contest ID when
        /// there are no teams associated with the contest yet.
        /// </summary>
        [TestMethod]
        public void TestGetContestFromContestIdPresentNoTeams()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contests");
                Contest contest = new Contest()
                {
                    Name = "Test Contest1",
                    Description = "This is a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.Steps,
                    CreatorId = _user1.UserID
                }; 

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id, false, false);

                Log("Verifying the integrity of the contest fields");
                Assert.AreEqual(contest.Name, retrieved.Name);
                Assert.AreEqual(contest.Description, retrieved.Description);
                Assert.AreEqual(contest.Reward, retrieved.Reward);
                Assert.AreEqual(contest.Teams.Count, retrieved.Teams.Count);
                Assert.AreEqual(contest.StatisticBinding, retrieved.StatisticBinding);
            }
        }

        /// <summary>
        /// Tests the retrieval and rehydration of a contest object based on its Contest ID when
        /// there are teams associated with the contest.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void TestGetContestFromContestIdPresentWithTeams()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contest");
                Contest contest = new Contest()
                {
                    Name = "Test Contest1",
                    Description = "This is a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.Steps,
                    CreatorId = _user1.UserID
                };

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Adding groups to the contest");
                ContestManager.AddGroup(id, _group1);
                ContestManager.AddGroup(id, _group2);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id, true, true);

                Log("Verifying the correct number of teams");
                Assert.AreEqual(contest.Teams.Count, retrieved.Teams.Count);

                Log("Verifying that each member is found");
                Assert.IsTrue(retrieved.Teams[0].ContainsMember(_user1.UserID));
                Assert.IsTrue(retrieved.Teams[0].ContainsMember(_user2.UserID));
                Assert.IsTrue(retrieved.Teams[1].ContainsMember(_user3.UserID));
                Assert.IsTrue(retrieved.Teams[1].ContainsMember(_user4.UserID));
            }
        }

        /// <summary>
        /// Tests the attempted retrieval of a contest based on its ID, where no match
        /// is found for the provided ID.
        /// </summary>
        [TestMethod]
        public void TestGetContestFromContestIdNotPresent()
        {
            using (_trans)
            {
                Log("Attempting to load contest that doesn't exist");
                Contest retrieved = ContestDAO.GetContestFromContestId(-1, false, false);

                Log("Verifying that no contest was loaded and no exception was thrown");
                Assert.IsNull(retrieved);
            }
        }

        /// <summary>
        /// Tests the ability to remove a contest from the DB from its Contest ID.
        /// </summary>
        [TestMethod]
        public void TestRemoveContestFromContestId()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contest");
                Contest contest = new Contest()
                {
                    Name = "Test Contest1",
                    Description = "This is a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.Steps,
                    CreatorId = _user1.UserID
                };

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id, false, false);

                Log("Verifying that the contest was retrieved");
                Assert.IsNotNull(retrieved);

                Log("Removing contest from db");
                Assert.IsTrue(ContestDAO.RemoveContestFromContestId(id));

                Log("Attempting to read contest back from DB");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id, false, false);

                Log("Verifying that the contest was not found");
                Assert.IsNull(retrieved2);
            }
        }

        /// <summary>
        /// Tests the attempted removal of a contest based on its ID where no matching contest
        /// is found.
        /// </summary>
        [TestMethod]
        public void TestRemoveContestFromContestIdNotPresent()
        {
            using (_trans)
            {
                int id = -1;

                Log("Attempting to read contest from DB");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id, false, false);

                Log("Verifying that the contest was not found");
                Assert.IsNull(retrieved2);

                Log("Attempting to Remove nonexistent contest from db");
                Assert.IsTrue(ContestDAO.RemoveContestFromContestId(id));

            }
        }

        /// <summary>
        /// Tests the updating of a contest where members were added to a team 
        /// participating in the contest.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void TestUpdateContestAddTeamMembers()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contest");
                Contest contest = new Contest()
                {
                    Name = "Test Contest1",
                    Description = "This is a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.Steps,
                    CreatorId = _user1.UserID
                };

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                ContestManager.AddGroup(id, _group1);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id, true, true);

                Log("Verifying that the two added members are found");
                Assert.IsTrue(retrieved.Teams[0].ContainsMember(_user1.UserID));
                Assert.IsTrue(retrieved.Teams[0].ContainsMember(_user2.UserID));
                Assert.IsFalse(retrieved.Teams[0].ContainsMember(_user3.UserID));
                Assert.IsFalse(retrieved.Teams[0].ContainsMember(_user4.UserID));

                Log("Adding two more members to the team");
                retrieved.Teams[0].Members.Add(new TeamMember() { UserId = _user3.UserID });
                retrieved.Teams[0].Members.Add(new TeamMember() { UserId = _user4.UserID });

                ContestDAO.UpdateContest(retrieved);

                Log("Re-loading contest from DB");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id, true, true);

                Log("Verifying the correct number of teams");
                Assert.AreEqual(retrieved.Teams.Count, retrieved2.Teams.Count);

                Log("Verifying that each member is found");
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(_user1.UserID));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(_user2.UserID));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(_user3.UserID));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(_user4.UserID));
            }
        }

        /// <summary>
        /// Tests the updating of a contest where the initial values for each 
        /// participant have been locked (e.g., when the contest starts).
        /// </summary>
        [TestMethod]
        [Ignore]
        public void TestContestLockContest()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contest");
                Contest contest = new Contest()
                {
                    Name = "Test Contest1",
                    Description = "This is a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.Steps,
                    CreatorId = _user1.UserID
                };

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                ContestManager.AddGroup(id, _group1);
                ContestManager.AddGroup(id, _group2);

                Log("Locking Initialization");
                ContestManager.LockContest(id);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id, true, true);

                Log("Verifying that contest participants have been locked");
                Assert.IsTrue(retrieved.Teams[0].Members[0].Initialized);
                Assert.IsTrue(retrieved.Teams[0].Members[1].Initialized);
                Assert.IsTrue(retrieved.Teams[1].Members[0].Initialized);
                Assert.IsTrue(retrieved.Teams[1].Members[1].Initialized);
            }
        }

        /// <summary>
        /// Tests the retrieval of a Team from its Team ID.
        /// </summary>
        [TestMethod]
        public void TestGetTeamByTeamId()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contest to put the team in");
                Contest contest = new Contest()
                {
                    Name = "Test Contest1",
                    Description = "This is a test contest",
                    Reward = 30,
                    Mode = ContestEndMode.GoalBased,
                    Type = ContestType.Group,
                    StartTime = DateTime.Today,
                    EndCondition = new EndCondition(500),
                    IsActive = true,
                    IsSearchable = true,
                    StatisticBinding = Statistic.Steps,
                    CreatorId = _user1.UserID
                };

                Log("Adding the contest to the DB");
                int contestId = ContestDAO.CreateNewContest(contest);

                Log("Creating team");
                Team team = new Team()
                {
                    ContestId = contestId,
                    Name = "Test Team"
                };

                Log("Adding team to DB");
                int teamId = TeamDAO.CreateNewTeam(team);

                int i = TeamDAO.CreateNewTeamMember(_user1.UserID, teamId);
                int j = TeamDAO.CreateNewTeamMember(_user2.UserID, teamId);

                Log("Retrieving team from DB");
                Team notFound = TeamDAO.GetTeamFromTeamId(-1, true);
                Team retrieved = TeamDAO.GetTeamFromTeamId(teamId, true);

                Assert.IsNull(notFound);
                Assert.IsNotNull(retrieved);
                Assert.AreEqual(2, retrieved.Members.Count);

            }
        }

        /// <summary>
        /// Tests the assignment of format strings when a contest is loaded.
        /// </summary>
        [TestMethod]
        public void TestGetContestFormatString()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contests of different types");
                int cID1 = ContestManager.CreateContest(ContestType.Group, "Test Contest1",
                    "This is a test contest", 30, DateTime.Today, 500, true, Statistic.Steps, _user1.UserID);

                int cID2 = ContestManager.CreateContest(ContestType.Group, "Test Contest2",
                    "This is a test contest", 30, DateTime.Today, 500, true, Statistic.GasSavings, _user1.UserID);

                Log("Retrieving contests from DB");
                Contest c1 = ContestManager.GetContest(cID1, false, false);
                Contest c2 = ContestManager.GetContest(cID2, false, false);

                Log("Verifying format strings");
                Assert.AreEqual("N0", c1.FormatString);
                Assert.AreEqual("C", c2.FormatString);

            }
        }

        /// <summary>
        /// Tests the attempted retrieval of a Team from its ID where no matching 
        /// team is found.
        /// </summary>
        [TestMethod]
        public void TestGetTeamByTeamIdNotPresent()
        {
            using (_trans)
            {
                Log("Attempting to retrieve non-existent team from DB");
                Assert.IsNull(TeamDAO.GetTeamFromTeamId(-1, true));
            }
        }

        /// <summary>
        /// Tests the retrieval of currently joinable contests from their name.
        /// </summary>
        [TestMethod]
        public void TestFindContests()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contests");
                //Matches
                ContestManager.CreateContest(ContestType.Group, "Testlike",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.Steps, _user1.UserID);

                //Matches
                ContestManager.CreateContest(ContestType.Group, "Testlike Competition",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                //Matches
                ContestManager.CreateContest(ContestType.Group, "testlikeing again",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                //Matches
                ContestManager.CreateContest(ContestType.Group, "My Testlike Contest",
                    "This is a test contest", 30, DateTime.Today, 500, true, Statistic.GasSavings, _user1.UserID);

                //Doesn't Match
                ContestManager.CreateContest(ContestType.Group, "ActivEarth FTW",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                //Matches, but not public
                ContestManager.CreateContest(ContestType.Group, "testlike contest2",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, false, Statistic.GasSavings, _user1.UserID);

                //Matches
                ContestManager.CreateContest(ContestType.Group, "this is another testlike contest",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                Log("Retrieving contest list from DB: Search term: 'testlike', exact match");
                List<int> listFromTestExact = ContestDAO.FindContests("testlike", true);

                Log("Retrieving contest list from DB: Search term: 'testlike', not exact match");
                List<int> listFromTestNotExact = ContestDAO.FindContests("testlike", false);

                Log("Retrieving contest list from DB: Search term: 'salamanders', not exact match");
                List<int> listFromSalamandersNotExact = ContestDAO.FindContests("salamanders", false);

                Log("Retrieving contest list from DB: Search term: 'salamanders', exact match");
                List<int> listFromSalamandersExact = ContestDAO.FindContests("salamanders", true);

                Log("Verifying returned contest counts");
                Assert.AreEqual(1, listFromTestExact.Count);
                Assert.AreEqual(5, listFromTestNotExact.Count);
                Assert.AreEqual(0, listFromSalamandersNotExact.Count);
                Assert.AreEqual(0, listFromSalamandersExact.Count);
            }
        }

        /// <summary>
        /// Tests the retrieval of contests from a group ID.
        /// </summary>
        [TestMethod]
        public void TestGetContestsFromGroupOrUserId()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                string c1Name = "Test";
                string c2Name = "Test Competition";
                string c3Name = "testing again";

                Log("Creating contests");
                int id1 = ContestManager.CreateContest(ContestType.Group, c1Name,
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.Steps, _user1.UserID);

                int id2 = ContestManager.CreateContest(ContestType.Group, c2Name,
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                int id3 = ContestManager.CreateContest(ContestType.Group, c3Name,
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                ContestManager.AddGroup(id1, _group1);
                ContestManager.AddGroup(id2, _group1);
                ContestManager.AddGroup(id2, _group2);
                ContestManager.AddGroup(id3, _group2);

                Log("Retrieving contest lists for group1, group2, user1, and user3");
                List<int> group1Contests = ContestDAO.GetContestIdsFromGroupId(_group1.ID);
                List<int> group2Contests = ContestDAO.GetContestIdsFromGroupId(_group2.ID);
                List<int> user1Contests = ContestDAO.GetContestIdsFromUserId(_user1.UserID);
                List<int> user3Contests = ContestDAO.GetContestIdsFromUserId(_user3.UserID);

                Log("Retrieving team lists for group1, group2, user1, and user3");
                List<int> group1Teams = TeamDAO.GetTeamIdsFromGroupId(_group1.ID);
                List<int> group2Teams = TeamDAO.GetTeamIdsFromGroupId(_group2.ID);
                List<int> user1Teams = TeamDAO.GetTeamIdsFromUserId(_user1.UserID);
                List<int> user3Teams = TeamDAO.GetTeamIdsFromUserId(_user3.UserID);

                Log("Verifying returned contest counts");
                Assert.AreEqual(2, group1Contests.Count);
                Assert.AreEqual(2, group2Contests.Count);
                Assert.AreEqual(2, user1Contests.Count);
                Assert.AreEqual(2, user3Contests.Count);

                Log("Verifying returned team counts");
                Assert.AreEqual(2, group1Teams.Count);
                Assert.AreEqual(2, group2Teams.Count);
                Assert.AreEqual(2, user1Teams.Count);
                Assert.AreEqual(2, user3Teams.Count);

                Log("Verifying correct contest memberships");
                Assert.IsTrue(group1Contests.Contains(id1));
                Assert.IsTrue(group1Contests.Contains(id2));
                Assert.IsTrue(group2Contests.Contains(id2));
                Assert.IsTrue(group2Contests.Contains(id3));
                Assert.IsTrue(user1Contests.Contains(id1));
                Assert.IsTrue(user1Contests.Contains(id2));
                Assert.IsTrue(user3Contests.Contains(id2));
                Assert.IsTrue(user3Contests.Contains(id3));
            }
        }

        /// <summary>
        /// Tests the retrieval of contest name from the contest ID.
        /// </summary>
        [TestMethod]
        public void TestGetContestNameFromContestId()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                string c1Name = "Test";
                string c2Name = "Test Competition";
                string c3Name = "testing again";

                Log("Creating contests");
                int id1 = ContestManager.CreateContest(ContestType.Group, c1Name,
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.Steps, _user1.UserID);

                int id2 = ContestManager.CreateContest(ContestType.Group, c2Name,
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                int id3 = ContestManager.CreateContest(ContestType.Group, c3Name,
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                Log("Verifying contest name retrieval");
                Assert.AreEqual(c1Name, ContestDAO.GetContestNameFromContestId(id1));
                Assert.AreEqual(c2Name, ContestDAO.GetContestNameFromContestId(id2));
                Assert.AreEqual(c3Name, ContestDAO.GetContestNameFromContestId(id3));
            }
        }

        /// <summary>
        /// Tests the determination of whether or not a user is already entered in a contest.
        /// </summary>
        [TestMethod]
        public void TestContestUserIsCompetingInContest()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contests");
                int id1 = ContestManager.CreateContest(ContestType.Group, "Contest",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.Steps, _user1.UserID);

                int id2 = ContestManager.CreateContest(ContestType.Group, "Contest2",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.GasSavings, _user1.UserID);

                ContestManager.AddUser(id1, _user1);

                Log("Verifying contest name retrieval");
                Assert.IsTrue(ContestManager.UserCompetingInContest(_user1.UserID, id1));
                Assert.IsFalse(ContestManager.UserCompetingInContest(_user1.UserID, id2));
            }
        }

        /// <summary>
        /// Tests the retrieval of a team from the userId and contestId.
        /// </summary>
        [TestMethod]
        public void TestContestGetTeamFromUserIdAndContestId()
        {
            using (_trans)
            {
                InitializeTestDBEntries();

                Log("Creating contests");
                int id1 = ContestManager.CreateContest(ContestType.Group, "Contest",
                    "This is a test contest", 30, DateTime.Today.AddDays(1), 500, true, Statistic.Steps, _user1.UserID);

                ContestManager.AddUser(id1, _user1);

                Team withMembers = TeamDAO.GetTeamFromUserIdAndContestId(_user1.UserID, id1, true);
                Team withoutMembers = TeamDAO.GetTeamFromUserIdAndContestId(_user1.UserID, id1, false);

                Log("Verifying retrieved team");
                Assert.IsNotNull(withMembers);
                Assert.IsNotNull(withoutMembers);

                Assert.AreEqual(1, withMembers.Members.Count);
                Assert.AreEqual(0, withoutMembers.Members.Count);

                Assert.AreEqual(_user1.UserID, withMembers.Members.First().UserId);
            }
        }

        #endregion ---------- Test Cases ----------

        #region ---------- Utility Methods ----------

        /// <summary>
        /// Logs a message to the Test Context's output (Test Results file).
        /// </summary>
        /// <param name="message">Message to add to the test log.</param>
        private void Log(string message)
        {
            TestContext.WriteLine(message);
        }

        private void InitializeTestDBEntries()
        {
            _user1.UserID = UserDAO.CreateNewUser(_user1, "pw1");
            _user2.UserID = UserDAO.CreateNewUser(_user2, "pw2");
            _user3.UserID = UserDAO.CreateNewUser(_user3, "pw3");
            _user4.UserID = UserDAO.CreateNewUser(_user4, "pw4");

            _group1.ID = GroupDAO.CreateNewGroup(_group1);
            _group2.ID = GroupDAO.CreateNewGroup(_group2);
        }

        #endregion ---------- Utility Methods ----------
    }
}