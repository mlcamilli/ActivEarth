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

namespace ActivEarth.Tests.Competition.Contests
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
                    true, Statistic.Steps);

                Log("Adding groups to the contest");
                ContestManager.AddGroup(id, _group1);
                ContestManager.AddGroup(id, _group2);

                Contest contest = ContestManager.GetContest(id);

                Log("Verifying team count");
                Assert.AreEqual(2, contest.Teams.Count);

                Log("Verifying number of members per team");
                Assert.AreEqual(2, contest.Teams[0].Members.Count);
                Assert.AreEqual(2, contest.Teams[1].Members.Count);

                Log("Locking team initialization");
                ContestManager.LockContest(id);
                contest = ContestManager.GetContest(id);

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
                Log("Creating time-based group contest");
                int timeId = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    true, Statistic.Steps);
                Contest timedContest = ContestManager.GetContest(timeId);

                Log("Creating goal-based individual contest");
                int goalId = ContestManager.CreateContest(ContestType.Individual, "Test Contest 2",
                    "This is a test goal-based contest.", 50, DateTime.Now, 50000,
                    true, Statistic.Steps);
                Contest goalContest = ContestManager.GetContest(goalId);

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
                    true, Statistic.Steps);

                ContestManager.AddGroup(id, _group1);
                ContestManager.AddGroup(id, _group2);

                Log("Locking initial values");
                ContestManager.LockContest(id);

                Log("Setting individual statistics such that group1 is winning");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 100);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 100);
                StatisticManager.SetUserStatistic(_user3.UserID, Statistic.Steps, 50);
                StatisticManager.SetUserStatistic(_user4.UserID, Statistic.Steps, 50);

                Contest contest = ContestManager.GetContest(id);

                Log("Verifying first team is group1");
                Assert.AreEqual("Group 1", contest.Teams[0].Name);

                Log("Verifying second team is group2");
                Assert.AreEqual("Group 2", contest.Teams[1].Name);

                Log("Setting individual statistics such that group2 is winning");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 200);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 200);
                StatisticManager.SetUserStatistic(_user3.UserID, Statistic.Steps, 300);
                StatisticManager.SetUserStatistic(_user4.UserID, Statistic.Steps, 300);

                contest = ContestManager.GetContest(id);

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
                    true, Statistic.Steps);

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

                Contest contest = ContestManager.GetContest(id);

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
                    true, Statistic.Steps);

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
                Contest contest = ContestManager.GetContest(id);

                Log("Verifying team aggregate score");
                Assert.AreEqual(200, contest.Teams[0].Score);
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
