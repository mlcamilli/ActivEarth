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
            _user1 = new User("Test", "Subject1");
            _user2 = new User("Test", "Subject2");
            _user3 = new User("Test", "Subject3");
            _user4 = new User("Test", "Subject4");

            _group1 = new Group(1, "Group 1", _user1, string.Empty, new List<string>());
            _group1.Members.Add(_user2);

            _group2 = new Group(2, "Group 2", _user3, string.Empty, new List<string>());
            _group2.Members.Add(_user4);
            _trans = new TransactionScope();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _trans.Dispose();
        }

        #region ---------- Test Cases ----------

        [TestMethod]
        public void TestContestGroupContestCreation()
        {
            using (_trans)
            {
                Log("Creating time-based group contest");
                int id = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    Statistic.Steps);
                Contest contest = ContestManager.GetContest(id);

                Log("Adding groups to the contest");
                contest.AddGroup(_group1);
                contest.AddGroup(_group2);

                Log("Verifying team count");
                Assert.AreEqual(2, contest.Teams.Count);

                Log("Verifying number of members per team");
                Assert.AreEqual(2, contest.Teams[0].Members.Count);
                Assert.AreEqual(2, contest.Teams[1].Members.Count);

                Log("Locking team initialization");
                contest.LockInitialValues();

                Log("Verifying initial team scores");
                Assert.AreEqual(0, contest.Teams[0].Score);
                Assert.AreEqual(0, contest.Teams[1].Score);
            }
        }

        [TestMethod]
        public void TestContestEndModeDetermination()
        {
            using (_trans)
            {
                Log("Creating time-based group contest");
                int timeId = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    Statistic.Steps);
                Contest timedContest = ContestManager.GetContest(timeId);

                Log("Creating goal-based individual contest");
                int goalId = ContestManager.CreateContest(ContestType.Individual, "Test Contest 2",
                    "This is a test goal-based contest.", 50, DateTime.Now, 50000,
                    Statistic.Steps);
                Contest goalContest = ContestManager.GetContest(goalId);

                Log("Verifying time-based contest end mode");
                Assert.AreEqual(ContestEndMode.TimeBased, timedContest.Mode);

                Log("Verifying goal-based contest end mode");
                Assert.AreEqual(ContestEndMode.GoalBased, goalContest.Mode);
            }
        }

        [TestMethod]
        public void TestContestGroupTeamsRemainSorted()
        {
            using (_trans)
            {
                Log("Creating group contest");
                int id = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    Statistic.Steps);
                Contest contest = ContestManager.GetContest(id);

                contest.AddGroup(_group1);
                contest.AddGroup(_group2);

                Log("Locking initial values");
                contest.LockInitialValues();

                Log("Setting individual statistics such that group1 is winning");
                _user1.SetStatistic(Statistic.Steps, 100);
                _user2.SetStatistic(Statistic.Steps, 100);
                _user3.SetStatistic(Statistic.Steps, 50);
                _user4.SetStatistic(Statistic.Steps, 50);

                Log("Updating contest scores");
                contest.UpdateScores();

                Log("Verifying first team is group1");
                Assert.IsTrue(contest.Teams[0].ContainsMember(_user1));
                Assert.IsTrue(contest.Teams[0].ContainsMember(_user2));

                Log("Verifying second team is group2");
                Assert.IsTrue(contest.Teams[1].ContainsMember(_user3));
                Assert.IsTrue(contest.Teams[1].ContainsMember(_user4));
            }
        }

        [TestMethod]
        public void TestContestIndividualTeamsRemainSorted()
        {
            using (_trans)
            {
                Log("Creating individual contest");
                int id = ContestManager.CreateContest(ContestType.Individual, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    Statistic.Steps);
                Contest contest = ContestManager.GetContest(id);

                contest.AddUser(_user1);
                contest.AddUser(_user2);
                contest.AddUser(_user3);
                contest.AddUser(_user4);

                Log("Locking initial values");
                contest.LockInitialValues();

                Log("Setting individual statistics");
                _user1.SetStatistic(Statistic.Steps, 25);
                _user2.SetStatistic(Statistic.Steps, 75);
                _user3.SetStatistic(Statistic.Steps, 50);
                _user4.SetStatistic(Statistic.Steps, 100);

                Log("Updating contest scores");
                contest.UpdateScores();

                Log("Verifying team order");
                Assert.IsTrue(contest.Teams[0].ContainsMember(_user4));
                Assert.IsTrue(contest.Teams[1].ContainsMember(_user2));
                Assert.IsTrue(contest.Teams[2].ContainsMember(_user3));
                Assert.IsTrue(contest.Teams[3].ContainsMember(_user1));
            }
        }

        [TestMethod]
        public void TestContestTeamScoreCalculation()
        {
            using (_trans)
            {
                Log("Creating group contest");
                int id = ContestManager.CreateContest(ContestType.Group, "Test Contest 1",
                    "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                    Statistic.Steps);
                Contest contest = ContestManager.GetContest(id);

                Team team = new Team("Team1");
                team.Add(_group1.Members);
                team.Add(_group2.Members);

                Log("Adding four-member team to contest");
                contest.AddTeam(team);

                Log("Setting individual initial statistics");
                _user1.SetStatistic(Statistic.Steps, 0);
                _user2.SetStatistic(Statistic.Steps, 50);
                _user3.SetStatistic(Statistic.Steps, 100);
                _user4.SetStatistic(Statistic.Steps, 150);

                Log("Locking initial values");
                contest.LockInitialValues();

                Log("Adding 50 steps to each user");
                _user1.SetStatistic(Statistic.Steps, 50);
                _user2.SetStatistic(Statistic.Steps, 100);
                _user3.SetStatistic(Statistic.Steps, 150);
                _user4.SetStatistic(Statistic.Steps, 200);

                Log("Updating contest scores");
                contest.UpdateScores();

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

        #endregion ---------- Utility Methods ----------
    }
}
