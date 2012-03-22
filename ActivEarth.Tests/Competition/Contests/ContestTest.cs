using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;

using Statistics = ActivEarth.Objects.Competition.Placeholder.Statistic;
using User = ActivEarth.Objects.Competition.Placeholder.User;
using Group = ActivEarth.Objects.Competition.Placeholder.Group;

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

        private ContestManager _manager;
        
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

            _group1 = new Group("Team 1");
            _group1.Members.Add(_user1);
            _group1.Members.Add(_user2);

            _group2 = new Group("Team 2");
            _group2.Members.Add(_user3);
            _group2.Members.Add(_user4);

            _manager = new ContestManager();
        }

        #region ---------- Test Cases ----------

        [TestMethod]
        public void TestContestGroupContestCreation()
        {
            Log("Creating time-based group contest");
            uint id = _manager.CreateContest(ContestType.Group, "Test Contest 1",
                "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                Statistics.Steps);
            GroupContest contest = (GroupContest)(_manager.GetContest(id));

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

        [TestMethod]
        public void TestContestEndModeDetermination()
        {
            Log("Creating time-based group contest");
            uint timeId = _manager.CreateContest(ContestType.Group, "Test Contest 1",
                "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                Statistics.Steps);
            GroupContest timedContest = (GroupContest)(_manager.GetContest(timeId));

            Log("Creating goal-based individual contest");
            uint goalId = _manager.CreateContest(ContestType.Individual, "Test Contest 2",
                "This is a test goal-based contest.", 50, DateTime.Now, 50000,
                Statistics.Steps);
            IndividualContest goalContest = (IndividualContest)(_manager.GetContest(goalId));

            Log("Verifying time-based contest end mode");
            Assert.AreEqual(ContestEndMode.TimeBased, timedContest.Mode);

            Log("Verifying goal-based contest end mode");
            Assert.AreEqual(ContestEndMode.GoalBased, goalContest.Mode);
        }

        [TestMethod]
        public void TestContestGroupTeamsRemainSorted()
        {
            Log("Creating group contest");
            uint id = _manager.CreateContest(ContestType.Group, "Test Contest 1",
                "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                Statistics.Steps);
            GroupContest contest = (GroupContest)(_manager.GetContest(id));

            contest.AddGroup(_group1);
            contest.AddGroup(_group2);

            Log("Locking initial values");
            contest.LockInitialValues();

            Log("Setting individual statistics such that group1 is winning");
            _user1.SetStatistic(Statistics.Steps, 100);
            _user2.SetStatistic(Statistics.Steps, 100);
            _user3.SetStatistic(Statistics.Steps, 50);
            _user4.SetStatistic(Statistics.Steps, 50);

            Log("Updating contest scores");
            contest.UpdateScores();

            Log("Verifying first team is group1");
            Assert.IsTrue(contest.Teams[0].ContainsMember(_user1));
            Assert.IsTrue(contest.Teams[0].ContainsMember(_user2));

            Log("Verifying second team is group2");
            Assert.IsTrue(contest.Teams[1].ContainsMember(_user3));
            Assert.IsTrue(contest.Teams[1].ContainsMember(_user4));
        }

        [TestMethod]
        public void TestContestIndividualTeamsRemainSorted()
        {
            Log("Creating individual contest");
            uint id = _manager.CreateContest(ContestType.Individual, "Test Contest 1",
                "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                Statistics.Steps);
            IndividualContest contest = (IndividualContest)(_manager.GetContest(id));

            contest.AddUser(_user1);
            contest.AddUser(_user2);
            contest.AddUser(_user3);
            contest.AddUser(_user4);

            Log("Locking initial values");
            contest.LockInitialValues();

            Log("Setting individual statistics");
            _user1.SetStatistic(Statistics.Steps, 25);
            _user2.SetStatistic(Statistics.Steps, 75);
            _user3.SetStatistic(Statistics.Steps, 50);
            _user4.SetStatistic(Statistics.Steps, 100);

            Log("Updating contest scores");
            contest.UpdateScores();

            Log("Verifying team order");
            Assert.IsTrue(contest.Teams[0].ContainsMember(_user4));
            Assert.IsTrue(contest.Teams[1].ContainsMember(_user2));
            Assert.IsTrue(contest.Teams[2].ContainsMember(_user3));
            Assert.IsTrue(contest.Teams[3].ContainsMember(_user1));
        }

        [TestMethod]
        public void TestContestTeamScoreCalculation()
        {
            Log("Creating group contest");
            uint id = _manager.CreateContest(ContestType.Group, "Test Contest 1",
                "This is a test time-based contest.", 50, DateTime.Now, DateTime.Now.AddDays(1),
                Statistics.Steps);
            GroupContest contest = (GroupContest)(_manager.GetContest(id));

            Team team = new Team("Team1");
            team.Add(_group1.Members);
            team.Add(_group2.Members);

            Log("Adding four-member team to contest");
            contest.AddTeam(team);

            Log("Setting individual initial statistics");
            _user1.SetStatistic(Statistics.Steps, 0);
            _user2.SetStatistic(Statistics.Steps, 50);
            _user3.SetStatistic(Statistics.Steps, 100);
            _user4.SetStatistic(Statistics.Steps, 150);

            Log("Locking initial values");
            contest.LockInitialValues();

            Log("Adding 50 steps to each user");
            _user1.SetStatistic(Statistics.Steps, 50);
            _user2.SetStatistic(Statistics.Steps, 100);
            _user3.SetStatistic(Statistics.Steps, 150);
            _user4.SetStatistic(Statistics.Steps, 200);

            Log("Updating contest scores");
            contest.UpdateScores();

            Log("Verifying team aggregate score");
            Assert.AreEqual(200, contest.Teams[0].Score);
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
