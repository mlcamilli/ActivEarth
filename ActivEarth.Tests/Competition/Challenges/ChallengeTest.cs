using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Server.Service.Competition;

using Statistics = ActivEarth.Objects.Competition.Placeholder.Statistic;
using User = ActivEarth.Objects.Competition.Placeholder.User;
using Group = ActivEarth.Objects.Competition.Placeholder.Group;

namespace ActivEarth.Tests.Competition.Challenges
{
    /// <summary>
    /// Tests the functionality of the Challenge system.
    /// </summary>
    [TestClass]
    public class ChallengeTest
    {
        private User _user1;
        private User _user2;

        private Group _allUsers;

        private ChallengeManager _manager;

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

            _allUsers = new Group("All Users");
            _allUsers.Members.Add(_user1);
            _allUsers.Members.Add(_user2);

            _manager = new ChallengeManager(_allUsers);
            _trans = new TransactionScope();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _trans.Dispose();
        }

        #region ---------- Test Cases ----------

        [TestMethod]
        public void TestChallengeCreation()
        {
            Log("Setting User2's initial Step statistic");
            _user2.SetStatistic(Statistics.Steps, 50);

            Log("Creating Step-Based Challenge");
            uint id = _manager.CreateChallenge("Test Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistics.Steps, 500);

            Log("Verifying that User1's initialization contains the new Challenge ID");
            Assert.IsTrue(_user1.ChallengeInitialValues.ContainsKey(id));

            Log("Verifying User1's initialization value for the new Challenge ID");
            Assert.AreEqual(0, _user1.ChallengeInitialValues[id]);

            Log("Verifying that User2's initialization contains the new Challenge ID");
            Assert.IsTrue(_user2.ChallengeInitialValues.ContainsKey(id));

            Log("Verifying User2's initialization value for the new Challenge ID");
            Assert.AreEqual(50, _user2.ChallengeInitialValues[id]);
        }

        [TestMethod]
        public void TestChallengeProgressIncomplete()
        {
            Log("Setting User2's initial Step statistic");
            _user2.SetStatistic(Statistics.Steps, 50);

            Log("Creating Step-Based Challenge");
            uint id = _manager.CreateChallenge("Test Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistics.Steps, 500);

            Challenge challenge = _manager.GetChallenge(id);

            Log("Increasing User1's Step Statistic");
            _user1.SetStatistic(Statistics.Steps, 200);

            Log("Increasing User2's Step Statistic");
            _user2.SetStatistic(Statistics.Steps, 200);

            Log("Verifying User1's Challenge Progress");
            Assert.AreEqual(200, challenge.GetProgress(_user1));

            Log("Verifying User2's Challenge Progress");
            Assert.AreEqual(150, challenge.GetProgress(_user2));

            Log("Verifying User1 has not completed the Challenge");
            Assert.IsFalse(challenge.IsComplete(_user1));

            Log("Verifying User2 has not completed the Challenge");
            Assert.IsFalse(challenge.IsComplete(_user2));
        }

        [TestMethod]
        public void TestChallengeProgressComplete()
        {
            Log("Setting User2's initial Step statistic");
            _user2.SetStatistic(Statistics.Steps, 50);

            Log("Creating Step-Based Challenge");
            uint id = _manager.CreateChallenge("Test Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistics.Steps, 500);

            Challenge challenge = _manager.GetChallenge(id);

            Log("Increasing User1's Step Statistic");
            _user1.SetStatistic(Statistics.Steps, 525);

            Log("Increasing User2's Step Statistic");
            _user2.SetStatistic(Statistics.Steps, 550);

            Log("Verifying User1's Challenge Progress");
            Assert.AreEqual(500, challenge.GetProgress(_user1));

            Log("Verifying User2's Challenge Progress");
            Assert.AreEqual(500, challenge.GetProgress(_user2));

            Log("Verifying User1 has completed the Challenge");
            Assert.IsTrue(challenge.IsComplete(_user1));

            Log("Verifying User2 has completed the Challenge");
            Assert.IsTrue(challenge.IsComplete(_user2));
        }
        
        [TestMethod]
        public void TestChallengeMultipleInitialization()
        {
            Log("Setting User1's initial Step and Bike statistics");
            _user1.SetStatistic(Statistics.Steps, 50);
            _user1.SetStatistic(Statistics.BikeDistance, 100);

            Log("Creating Step-based Challenge");
            uint id1 = _manager.CreateChallenge("Test Step Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistics.Steps, 500);

            Log("Creating Biking-based Challenge");
            uint id2 = _manager.CreateChallenge("Test Bike Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistics.BikeDistance, 500);

            Log("Increasing User1's Step statistic");
            _user1.SetStatistic(Statistics.Steps, 150);

            Log("Creating another Step-based Challenge");
            uint id3 = _manager.CreateChallenge("Test Step Challenge 2", "This is another test challenge",
                30, false, DateTime.Today, 1, Statistics.Steps, 500);

            Log("Verifying initialization of first step-based challenge");
            Assert.IsTrue(_user1.ChallengeInitialValues.ContainsKey(id1));
            Assert.AreEqual(50, _user1.ChallengeInitialValues[id1]);

            Log("Verifying initialization of biking-based challenge");
            Assert.IsTrue(_user1.ChallengeInitialValues.ContainsKey(id2));
            Assert.AreEqual(100, _user1.ChallengeInitialValues[id2]);

            Log("Verifying initialization of second step-based challenge");
            Assert.IsTrue(_user1.ChallengeInitialValues.ContainsKey(id3));
            Assert.AreEqual(150, _user1.ChallengeInitialValues[id3]);
        }

        [TestMethod]
        public void TestChallengeCleanup()
        {
            Log("Creating an expired transient challenge");
            uint id1 = _manager.CreateChallenge("Test Step Challenge", "This is an expired transient challenge",
                30, false, DateTime.Today.AddDays(-1), 1, Statistics.Steps, 500);

            Log("Creating an ongoing transient challenge");
            uint id2 = _manager.CreateChallenge("Test Bike Challenge", "This is an active transient challenge",
                30, false, DateTime.Today, 1, Statistics.Steps, 500);

            Log("Creating an expired persistent challenge");
            uint id3 = _manager.CreateChallenge("Test Step Challenge 2", "This is a persistent challenge",
                30, true, DateTime.Today.AddDays(-1), 1, Statistics.Steps, 500);

            Log("Verifying that all three challenges are in the active collection (before CleanUp)");
            Assert.IsNotNull(_manager.GetChallenge(id1));
            Assert.IsNotNull(_manager.GetChallenge(id2));
            Assert.IsNotNull(_manager.GetChallenge(id3));

            Log("Call manager's CleanUp routine");
            _manager.CleanUp();

            Log("Verifying that the expired transient challenge has been removed, but the other two remain");
            Assert.IsNull(_manager.GetChallenge(id1));
            Assert.IsNotNull(_manager.GetChallenge(id2));
            Assert.IsNotNull(_manager.GetChallenge(id3));

            Log("Verify that the expired transient challenge still exists in the 'all challenges' collection");
            Assert.IsNotNull(_manager.GetChallenge(id1, false));
        }

        [TestMethod]
        public void TestChallengeResetPersistentChallenge()
        {
            Log("Creating an expired persistent challenge");
            uint id = _manager.CreateChallenge("Test Step Challenge", "This is a persistent challenge",
                30, true, DateTime.Today.AddDays(-1), 1, Statistics.Steps, 500);

            Challenge challenge = _manager.GetChallenge(id);

            Log("Increasing User1's steps Statistic");
            _user1.SetStatistic(Statistics.Steps, _user1.GetStatistic(Statistics.Steps) + 250);

            Log("Verifying challenge progress");
            Assert.AreEqual(250, challenge.GetProgress(_user1));

            Log("Calling manager's CleanUp routine");
            _manager.CleanUp();

            Log("Verifying that challenge progress reset");
            Assert.AreEqual(0, challenge.GetProgress(_user1));

            Log("Increasing User1's steps Statistic");
            _user1.SetStatistic(Statistics.Steps, _user1.GetStatistic(Statistics.Steps) + 200);

            Log("Verifying challenge progress");
            Assert.AreEqual(200, challenge.GetProgress(_user1));

            Log("Verifying new end time");
            Assert.AreEqual(DateTime.Today.AddDays(1), challenge.EndTime);
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
