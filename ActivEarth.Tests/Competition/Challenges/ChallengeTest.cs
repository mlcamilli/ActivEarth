using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Server.Service.Competition;

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

        /// <summary>
        /// Creates two new users and a Challenge Manager to operate on them.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _user1 = new User("Test", "Subject1");
            _user2 = new User("Test", "Subject2");

            _allUsers = new Group(0, "All Users", _user1, string.Empty, new List<string>());
            _allUsers.Members.Add(_user1);
            _allUsers.Members.Add(_user2);

            _manager = new ChallengeManager(_allUsers);
            _trans = new TransactionScope();
        }

        /// <summary>
        /// Disposes of the Transaction Scope, rolling back the DB transactions.
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            _trans.Dispose();
        }

        #region ---------- Test Cases ----------

        /// <summary>
        /// Verifies that each user's challenge initialization is executed and the initial
        /// values are correct when a challenge is created by the Challenge Manager.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Set user2's steps statistic to a nonzero value (leaving user1 at 0).
        /// 2) Create a step-based challenge
        /// 3) VERIFY: user1 contains initialization information for the challenge
        /// 4) VERIFY: user1 reports 0 steps as the initialization value for the challenge
        /// 5) VERIFY: user2 contains initialization information for the challenge
        /// 6) VERIFY: user2 reports correct number of steps as the initialization 
        ///     value for the challenge
        /// </remarks>
        [TestMethod]
        public void TestChallengeInitialization()
        {
            Log("Setting User2's initial Step statistic");
            _user2.SetStatistic(Statistic.Steps, 50);

            Log("Creating Step-Based Challenge");
            int id = _manager.CreateChallenge("Test Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistic.Steps, 500);

            Log("Verifying that User1's initialization contains the new Challenge ID");
            Assert.IsTrue(_user1.ChallengeInitialValues.ContainsKey(id));

            Log("Verifying User1's initialization value for the new Challenge ID");
            Assert.AreEqual(0, _user1.ChallengeInitialValues[id]);

            Log("Verifying that User2's initialization contains the new Challenge ID");
            Assert.IsTrue(_user2.ChallengeInitialValues.ContainsKey(id));

            Log("Verifying User2's initialization value for the new Challenge ID");
            Assert.AreEqual(50, _user2.ChallengeInitialValues[id]);
        }

        /// <summary>
        /// Verifies that each user's challenge progress is correctly reported before they have reached
        /// the statistics required to complete the challenge.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Set user2's steps statistic to 50 (leaving user1 at 0).
        /// 2) Create a step-based challenge
        /// 3) Increase both user1 and user2's step statistics to 200.
        /// 3) VERIFY: user1 reports 200 steps of progress
        /// 4) VERIFY: user2 reports 150 steps of progress
        /// 5) VERIFY: Both users have not completed the challenge.
        /// </remarks>
        [TestMethod]
        public void TestChallengeProgressIncomplete()
        {
            Log("Setting User2's initial Step statistic");
            _user2.SetStatistic(Statistic.Steps, 50);

            Log("Creating Step-Based Challenge");
            int id = _manager.CreateChallenge("Test Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistic.Steps, 500);

            Challenge challenge = _manager.GetChallenge(id);

            Log("Increasing User1's Step Statistic");
            _user1.SetStatistic(Statistic.Steps, 200);

            Log("Increasing User2's Step Statistic");
            _user2.SetStatistic(Statistic.Steps, 200);

            Log("Verifying User1's Challenge Progress");
            Assert.AreEqual(200, challenge.GetProgress(_user1));

            Log("Verifying User2's Challenge Progress");
            Assert.AreEqual(150, challenge.GetProgress(_user2));

            Log("Verifying User1 has not completed the Challenge");
            Assert.IsFalse(challenge.IsComplete(_user1));

            Log("Verifying User2 has not completed the Challenge");
            Assert.IsFalse(challenge.IsComplete(_user2));
        }

        /// <summary>
        /// Verifies that each user's challenge progress is correctly reported after they have reached
        /// the statistics required to complete the challenge (and the progress is capped at the requirement).
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Set user2's steps statistic to 50 (leaving user1 at 0).
        /// 2) Create a step-based challenge with 500-step requirement.
        /// 3) Increase user1's step statistic to 525 (increase of 525)
        /// 4) Increase user2's step statistic to 550 (increase of 500)
        /// 5) VERIFY: Both users report progress of 500.
        /// 6) VERIFY: Both users have not completed the challenge.
        /// </remarks>
        [TestMethod]
        public void TestChallengeProgressComplete()
        {
            Log("Setting User2's initial Step statistic");
            _user2.SetStatistic(Statistic.Steps, 50);

            Log("Creating Step-Based Challenge");
            int id = _manager.CreateChallenge("Test Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistic.Steps, 500);

            Challenge challenge = _manager.GetChallenge(id);

            Log("Increasing User1's Step Statistic");
            _user1.SetStatistic(Statistic.Steps, 525);

            Log("Increasing User2's Step Statistic");
            _user2.SetStatistic(Statistic.Steps, 550);

            Log("Verifying User1's Challenge Progress");
            Assert.AreEqual(500, challenge.GetProgress(_user1));

            Log("Verifying User2's Challenge Progress");
            Assert.AreEqual(500, challenge.GetProgress(_user2));

            Log("Verifying User1 has completed the Challenge");
            Assert.IsTrue(challenge.IsComplete(_user1));

            Log("Verifying User2 has completed the Challenge");
            Assert.IsTrue(challenge.IsComplete(_user2));
        }

        /// <summary>
        /// Verifies that multiple challenges can be initialized without interfering with one another.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Set user1's steps to 50 and bike distance to 100
        /// 2) Create a step-based challenge
        /// 3) Create a bikedistance-based challenge
        /// 3) Increase user1's steps to 150
        /// 4) Create a step-based challenge
        /// 5) VERIFY: First challenge's initialization value is 50
        /// 6) VERIFY: Second challenge's initialization value is 100
        /// 7) VERIFY: Third challenge's initialization value is 150
        /// </remarks>
        [TestMethod]
        public void TestChallengeMultipleInitialization()
        {
            Log("Setting User1's initial Step and Bike statistics");
            _user1.SetStatistic(Statistic.Steps, 50);
            _user1.SetStatistic(Statistic.BikeDistance, 100);

            Log("Creating Step-based Challenge");
            int id1 = _manager.CreateChallenge("Test Step Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistic.Steps, 500);

            Log("Creating Biking-based Challenge");
            int id2 = _manager.CreateChallenge("Test Bike Challenge", "This is a test challenge",
                30, false, DateTime.Today, 1, Statistic.BikeDistance, 500);

            Log("Increasing User1's Step statistic");
            _user1.SetStatistic(Statistic.Steps, 150);

            Log("Creating another Step-based Challenge");
            int id3 = _manager.CreateChallenge("Test Step Challenge 2", "This is another test challenge",
                30, false, DateTime.Today, 1, Statistic.Steps, 500);

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

        /// <summary>
        /// Verifies that transient challenges are inactivated when they expire and persistent challenges
        /// are not.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Create an expired transient challenge
        /// 2) Create an ongoing transient challenge
        /// 3) Create an expired persistent challenge
        /// 4) VERIFY: All three challenges appear in the database
        /// 5) Call the cleanup routine
        /// 6) VERIFY: The expired transient challenge is now inactive
        /// 7) VERIFY: The ongoing transient challenge is still active
        /// 8) VERIFY: The persistent challenge is still active
        /// </remarks>
        [TestMethod]
        public void TestChallengeCleanup()
        {
            Log("Creating an expired transient challenge");
            int id1 = _manager.CreateChallenge("Test Step Challenge", "This is an expired transient challenge",
                30, false, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);

            Log("Creating an ongoing transient challenge");
            int id2 = _manager.CreateChallenge("Test Bike Challenge", "This is an active transient challenge",
                30, false, DateTime.Today, 1, Statistic.Steps, 500);

            Log("Creating an expired persistent challenge");
            int id3 = _manager.CreateChallenge("Test Step Challenge 2", "This is a persistent challenge",
                30, true, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);

            Log("Verifying that all three challenges are in the active collection (before CleanUp)");
            Assert.IsNotNull(_manager.GetChallenge(id1));
            Assert.IsNotNull(_manager.GetChallenge(id2));
            Assert.IsNotNull(_manager.GetChallenge(id3));

            Log("Call manager's CleanUp routine");
            _manager.CleanUp();

            Log("Verifying that the expired transient challenge is now inactive");
            Assert.IsFalse(_manager.GetChallenge(id1).IsActive);

            Log("Verifying that the valid transient challenge is still active");
            Assert.IsTrue(_manager.GetChallenge(id2).IsActive);

            Log("Verifying that the persistent challenge is still active");
            Assert.IsTrue(_manager.GetChallenge(id3).IsActive);
        }

        /// <summary>
        /// Verifies that transient challenges are re-initialized when they expire.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Create an expired step-based persistent challenge
        /// 2) Increase the user's steps to 250
        /// 3) VERIFY: User reports 250 steps of progress
        /// 4) Call the cleanup routine
        /// 5) Increase the user's steps by 200
        /// 6) VERIFY: User reports 200 steps of progess
        /// 7) VERIFY: Challenge reports updated end time
        /// </remarks>
        [TestMethod]
        public void TestChallengeResetPersistentChallenge()
        {
            Log("Creating an expired persistent challenge");
            int id = _manager.CreateChallenge("Test Step Challenge", "This is a persistent challenge",
                30, true, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);

            Log("Fetching challenge from DB");
            Challenge challenge = _manager.GetChallenge(id);

            Log("Increasing User1's steps Statistic");
            _user1.SetStatistic(Statistic.Steps, _user1.GetStatistic(Statistic.Steps) + 250);

            Log("Verifying challenge progress");
            Assert.AreEqual(250, challenge.GetProgress(_user1));

            Log("Calling manager's CleanUp routine");
            _manager.CleanUp();

            Log("Refreshing challenge from DB");
            challenge = _manager.GetChallenge(id);

            Log("Verifying that challenge progress reset");
            Assert.AreEqual(0, challenge.GetProgress(_user1));

            Log("Increasing User1's steps Statistic");
            _user1.SetStatistic(Statistic.Steps, _user1.GetStatistic(Statistic.Steps) + 200);

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
