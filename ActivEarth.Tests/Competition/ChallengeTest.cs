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
using ActivEarth.Server.Service.Statistics;
using ActivEarth.DAO;

namespace ActivEarth.Tests.Competition
{
    /// <summary>
    /// Tests the functionality of the Challenge system.
    /// </summary>
    [TestClass]
    public class ChallengeTest
    {
        private User _user1;
        private User _user2;

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
            _user1 = new User
            {
                UserName = "testSubject1",
                FirstName = "Test",
                LastName = "Subject1",
                City = "Montreal",
                State = "QC",
                Gender = "M",
                Email = "email1@test.com"
            };

            _user2 = new User
            {
                UserName = "testSubject2",
                FirstName = "Test",
                LastName = "Subject2",
                City = "Vancouver",
                State = "BC",
                Gender = "M",
                Email = "email1@test.net"
            };

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
            using (_trans)
            {
                Log("Creating test users in DB");
                _user1.UserID = UserDAO.CreateNewUser(_user1, "pass1");
                _user2.UserID = UserDAO.CreateNewUser(_user2, "pass2");

                Log("Setting users' initial Step statistic");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 0);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 50);

                Log("Creating Step-Based Challenge");
                int challengeId = ChallengeManager.CreateChallenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);

                ChallengeManager.InitializeUser(challengeId, _user1.UserID);
                ChallengeManager.InitializeUser(challengeId, _user2.UserID);

                Log("Verifying User1's initialization value for the new Challenge ID");
                Assert.IsTrue(ChallengeDAO.GetInitializationValue(challengeId, _user1.UserID) == 0);

                Log("Verifying User2's initialization value for the new Challenge ID");
                Assert.IsTrue(ChallengeDAO.GetInitializationValue(challengeId, _user2.UserID) == 50);
            }
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
            using (_trans)
            {
                Log("Creating test users in DB");
                _user1.UserID = UserDAO.CreateNewUser(_user1, "pass1");
                _user2.UserID = UserDAO.CreateNewUser(_user2, "pass1");

                Log("Setting users' initial Step statistic");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 0);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 50);

                Log("Creating Step-Based Challenge");
                int challengeId = ChallengeManager.CreateChallenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);

                ChallengeManager.InitializeUser(challengeId, _user1.UserID);
                ChallengeManager.InitializeUser(challengeId, _user2.UserID);

                Challenge challenge = ChallengeManager.GetChallenge(challengeId);

                Log("Increasing User1's Step Statistic");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 200);

                Log("Increasing User2's Step Statistic");
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 200);

                Log("Verifying User1's Challenge Progress");
                Assert.AreEqual(40, ChallengeManager.GetProgress(challengeId, _user1.UserID));

                Log("Verifying User2's Challenge Progress");
                Assert.AreEqual(30, ChallengeManager.GetProgress(challengeId, _user2.UserID));

                Log("Verifying User1 has not completed the Challenge");
                Assert.IsFalse(ChallengeManager.IsComplete(challengeId, _user1.UserID));

                Log("Verifying User2 has not completed the Challenge");
                Assert.IsFalse(ChallengeManager.IsComplete(challengeId, _user2.UserID));
            }
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
            using (_trans)
            {
                Log("Creating test users in DB");
                _user1.UserID = UserDAO.CreateNewUser(_user1, "pass1");
                _user2.UserID = UserDAO.CreateNewUser(_user2, "pass1");

                Log("Setting User2's initial Step statistic");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 0);
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 50);

                Log("Creating Step-Based Challenge");
                int challengeId = ChallengeManager.CreateChallenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);

                ChallengeManager.InitializeUser(challengeId, _user1.UserID);
                ChallengeManager.InitializeUser(challengeId, _user2.UserID);

                Challenge challenge = ChallengeManager.GetChallenge(challengeId);

                Log("Increasing User1's Step Statistic");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 525);

                Log("Increasing User2's Step Statistic");
                StatisticManager.SetUserStatistic(_user2.UserID, Statistic.Steps, 550);

                Log("Verifying User1's Challenge Progress");
                Assert.AreEqual(100, ChallengeManager.GetProgress(challengeId, _user1.UserID));

                Log("Verifying User2's Challenge Progress");
                Assert.AreEqual(100, ChallengeManager.GetProgress(challengeId, _user2.UserID));

                Log("Verifying User1 has completed the Challenge");
                Assert.IsTrue(ChallengeManager.IsComplete(challengeId, _user1.UserID));

                Log("Verifying User2 has completed the Challenge");
                Assert.IsTrue(ChallengeManager.IsComplete(challengeId, _user2.UserID));

                Log("Verifying User2 has completed the Challenge - Loading challenge with userId");
                Challenge challengeLoaded = ChallengeManager.GetChallenge(challengeId, _user1.UserID);
                Assert.AreEqual(100, challengeLoaded.Progress);
            }
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
            using (_trans)
            {
                Log("Creating test users in DB");
                _user1.UserID = UserDAO.CreateNewUser(_user1, "pass1");
                _user2.UserID = UserDAO.CreateNewUser(_user2, "pass1");

                Log("Setting User1's initial Step and Bike statistics");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 50);
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.BikeDistance, 100);

                Log("Creating Step-based Challenge");
                int challengeId1 = ChallengeManager.CreateChallenge("Test Step Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);

                Log("Creating Biking-based Challenge");
                int challengeId2 = ChallengeManager.CreateChallenge("Test Bike Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.BikeDistance, 500);

                Log("Locking initial values");
                ChallengeManager.InitializeUser(challengeId1, _user1.UserID);
                ChallengeManager.InitializeUser(challengeId2, _user1.UserID);

                Log("Increasing User1's Step statistic");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 150);

                Log("Creating another Step-based Challenge");
                int challengeId3 = ChallengeManager.CreateChallenge("Test Step Challenge 2", "This is another test challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);

                ChallengeManager.InitializeUser(challengeId3, _user1.UserID);

                Log("Verifying initialization of first step-based challenge");
                Assert.AreEqual(50, ChallengeDAO.GetInitializationValue(challengeId1, _user1.UserID));

                Log("Verifying initialization of biking-based challenge");
                Assert.AreEqual(100, ChallengeDAO.GetInitializationValue(challengeId2, _user1.UserID));

                Log("Verifying initialization of second step-based challenge");
                Assert.AreEqual(150, ChallengeDAO.GetInitializationValue(challengeId3, _user1.UserID));
            }
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
            {
                Log("Creating test users in DB");
                _user1.UserID = UserDAO.CreateNewUser(_user1, "pass1");
                _user2.UserID = UserDAO.CreateNewUser(_user2, "pass1");

                Log("Creating an expired transient challenge");
                int challengeId1 = ChallengeManager.CreateChallenge("Test Step Challenge", "This is an expired transient challenge",
                    30, false, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);

                Log("Creating an ongoing transient challenge");
                int challengeId2 = ChallengeManager.CreateChallenge("Test Bike Challenge", "This is an active transient challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);

                Log("Creating an expired persistent challenge");
                int challengeId3 = ChallengeManager.CreateChallenge("Test Step Challenge 2", "This is a persistent challenge",
                    30, true, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);

                Log("Verifying that all three challenges are in the active collection (before CleanUp)");
                Assert.IsNotNull(ChallengeManager.GetChallenge(challengeId1));
                Assert.IsNotNull(ChallengeManager.GetChallenge(challengeId2));
                Assert.IsNotNull(ChallengeManager.GetChallenge(challengeId3));

                Log("Call manager's CleanUp routine");
                ChallengeManager.CleanUp();

                Log("Verifying that the expired transient challenge is now inactive");
                Assert.IsFalse(ChallengeManager.GetChallenge(challengeId1).IsActive);

                Log("Verifying that the valid transient challenge is still active");
                Assert.IsTrue(ChallengeManager.GetChallenge(challengeId2).IsActive);

                Log("Verifying that the persistent challenge is still active");
                Assert.IsTrue(ChallengeManager.GetChallenge(challengeId3).IsActive);
            }
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
            using (_trans)
            {
                Log("Creating test users in DB");
                _user1.UserID = UserDAO.CreateNewUser(_user1, "pass1");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 0);

                Log("Creating an expired persistent challenge");
                int challengeId = ChallengeManager.CreateChallenge("Test Step Challenge", "This is a persistent challenge",
                    30, true, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);

                Log("Locking initial values");
                ChallengeManager.InitializeUser(challengeId, _user1.UserID);

                Log("Increasing User1's steps Statistic");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 250);

                Log("Verifying challenge progress");
                Assert.AreEqual(50, ChallengeManager.GetProgress(challengeId, _user1.UserID));

                Log("Calling manager's CleanUp routine");
                ChallengeManager.CleanUp();

                Log("Verifying that challenge progress reset");
                Assert.AreEqual(0, ChallengeManager.GetProgress(challengeId, _user1.UserID));

                Log("Re-locking initial value");
                ChallengeManager.InitializeUser(challengeId, _user1.UserID);

                Log("Increasing User1's steps Statistic");
                StatisticManager.SetUserStatistic(_user1.UserID, Statistic.Steps, 450);

                Log("Verifying challenge progress");
                Assert.AreEqual(40, ChallengeManager.GetProgress(challengeId, _user1.UserID));

                Log("Retrieving challenge from DB");
                Challenge challenge = ChallengeDAO.GetChallengeFromChallengeId(challengeId);

                Log("Verifying new end time");
                Assert.AreEqual(DateTime.Today.AddDays(1), challenge.EndTime);
            }
        }
        
        /// <summary>
        /// Verifies that Challenge entries are correctly written to and read from the DB.
        /// </summary>
        [TestMethod]
        public void TestCreateChallenge()
        {
            using (_trans)
            {
                int id;

                Log("Creating challenge");
                Challenge challenge = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);

                Log("Adding challenge to the database");
                Assert.IsTrue((id = ChallengeDAO.CreateNewChallenge(challenge)) > 0);

                Log("Loading challenge from the database");
                Challenge retrieved = ChallengeDAO.GetChallengeFromChallengeId(id);

                Log("Verifying that a matching challenge was found");
                Assert.IsNotNull(retrieved);

                Log("Verifying the integrity of challenge fields");
                Assert.AreEqual(challenge.Name, retrieved.Name);
                Assert.AreEqual(challenge.Description, retrieved.Description);
                Assert.AreEqual(challenge.EndTime, retrieved.EndTime);
                Assert.AreEqual(challenge.IsActive, retrieved.IsActive);
                Assert.AreEqual(challenge.IsPersistent, retrieved.IsPersistent);
                Assert.AreEqual(challenge.Reward, retrieved.Reward);
                Assert.AreEqual(challenge.StatisticBinding, retrieved.StatisticBinding);
            }
        }

        /// <summary>
        /// Verifies that Challenges can be updated correctly in the DB.
        /// </summary>
        [TestMethod]
        public void TestUpdateChallenge()
        {
            using (_trans)
            {
                int id;

                Log("Creating challenge");
                Challenge challenge = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);

                Log("Adding challenge to the database");
                Assert.IsTrue((id = ChallengeDAO.CreateNewChallenge(challenge)) > 0);

                Log("Loading challenge from the database");
                Challenge retrieved = ChallengeDAO.GetChallengeFromChallengeId(id);

                Log("Verifying that a matching challenge was found");
                Assert.IsNotNull(retrieved);

                Log("Verifying that the challenge is still active");
                Assert.IsTrue(retrieved.IsActive);

                retrieved.IsActive = false;

                Log("Updating the challenge to be expired");
                Assert.IsTrue(ChallengeDAO.UpdateChallenge(retrieved));

                Log("Reloading challenge from the database");
                Challenge retrieved2 = ChallengeDAO.GetChallengeFromChallengeId(id);

                Log("Verifying that a matching challenge was found");
                Assert.IsNotNull(retrieved2);

                Log("Verifying that the challenge is inactive");
                Assert.IsFalse(retrieved2.IsActive);

            }
        }

        /// <summary>
        /// Verifies the ability to retrieve the collection of currently active challenges.
        /// </summary>
        [TestMethod]
        public void TestGetActiveChallenges()
        {
            using (_trans)
            {
                int initial = ChallengeDAO.GetActiveChallenges().Count;

                Log("Creating expired challenge");
                Challenge challenge1 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);
                challenge1.IsActive = false;

                Log("Creating valid challenge");
                Challenge challenge2 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.BikeDistance, 500);

                Log("Creating valid challenge");
                Challenge challenge3 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.ChallengesCompleted, 500);

                Log("Adding challenges to DB");
                ChallengeDAO.CreateNewChallenge(challenge1);
                ChallengeDAO.CreateNewChallenge(challenge2);
                ChallengeDAO.CreateNewChallenge(challenge3);

                Log("Verifying that GetActiveChallenges returns two challenges");
                Assert.AreEqual(initial + 2, ChallengeDAO.GetActiveChallenges().Count);
            }
        }

        /// <summary>
        /// Verifies the ability to retrieve the collection of currently active daily challenges.
        /// </summary>
        [TestMethod]
        public void TestGetActiveChallengesByLength()
        {
            using (_trans)
            {
                int initialDaily = ChallengeDAO.GetActiveDailyChallenges().Count;
                int initialWeekly = ChallengeDAO.GetActiveWeeklyChallenges().Count;

                Log("Creating expired challenge");
                Challenge challenge1 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);
                challenge1.IsActive = false;

                Log("Creating a daily challenge");
                Challenge challenge2 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.BikeDistance, 500);

                Log("Creating another daily challenge");
                Challenge challenge3 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.BikeDistance, 500);

                Log("Creating weekly challenge 1");
                Challenge challenge4 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 7, Statistic.ChallengesCompleted, 500);

                Log("Creating weekly challenge 2");
                Challenge challenge5 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 7, Statistic.ChallengesCompleted, 500);

                Log("Creating weekly challenge 3");
                Challenge challenge6 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 7, Statistic.ChallengesCompleted, 500);

                Log("Adding challenges to DB");
                ChallengeDAO.CreateNewChallenge(challenge1);
                ChallengeDAO.CreateNewChallenge(challenge2);
                ChallengeDAO.CreateNewChallenge(challenge3);
                ChallengeDAO.CreateNewChallenge(challenge4);
                ChallengeDAO.CreateNewChallenge(challenge5);
                ChallengeDAO.CreateNewChallenge(challenge6);

                Log("Verifying that GetActiveDailyChallenges returns two challenges");
                Assert.AreEqual(initialDaily + 2, ChallengeDAO.GetActiveDailyChallenges().Count);

                Log("Verifying that GetActiveWeeklyChallenges returns three challenges");
                Assert.AreEqual(initialWeekly + 3, ChallengeDAO.GetActiveWeeklyChallenges().Count);
            }
        }

        /// <summary>
        /// Verifies the ability to retrieve the entire archive of challenges.
        /// </summary>
        [TestMethod]
        public void TestGetAllChallenges()
        {
            using (_trans)
            {
                int initial = ChallengeDAO.GetActiveChallenges().Count;

                Log("Creating expired challenge");
                Challenge challenge1 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today.AddDays(-1), 1, Statistic.Steps, 500);
                challenge1.IsActive = false;

                Log("Creating valid challenge");
                Challenge challenge2 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.BikeDistance, 500);

                Log("Creating valid challenge");
                Challenge challenge3 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.ChallengesCompleted, 500);

                Log("Adding challenges to DB");
                ChallengeDAO.CreateNewChallenge(challenge1);
                ChallengeDAO.CreateNewChallenge(challenge2);
                ChallengeDAO.CreateNewChallenge(challenge3);

                Log("Verifying that GetAllChallenges returns three challenges");
                Assert.AreEqual(initial + 3, ChallengeDAO.GetAllChallenges().Count);
            }
        }

        /// <summary>
        /// Tests the assignment of format strings when a challenge is loaded.
        /// </summary>
        [TestMethod]
        public void TestGetChallengeFormatString()
        {
            using (_trans)
            {
                Log("Creating challenges");
                Challenge challenge1 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.Steps, 500);
                int id1 = ChallengeDAO.CreateNewChallenge(challenge1);

                Challenge challenge2 = new Challenge("Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Statistic.GasSavings, 500);
                int id2 = ChallengeDAO.CreateNewChallenge(challenge2);

                Log("Retrieving challenges");
                Challenge stepChallenge = ChallengeDAO.GetChallengeFromChallengeId(id1);
                Challenge gasChallenge = ChallengeDAO.GetChallengeFromChallengeId(id2);

                Log("Verifying format strings");
                Assert.AreEqual("N0", stepChallenge.FormatString);
                Assert.AreEqual("C", gasChallenge.FormatString);

            }
        }

        /// <summary>
        /// Tests the assignment of format strings when a challenge is loaded.
        /// </summary>
        [TestMethod]
        public void TestChallengeGenerator()
        {
            using (_trans)
            {
                Log("Generating Random Challenges");
                ChallengeManager.GenerateNewChallenges();

                Log("Verifying correct numbers of each type of challenge");
                Assert.AreEqual(3, ChallengeDAO.GetActiveDailyChallenges().Count);
                Assert.AreEqual(3, ChallengeDAO.GetActiveWeeklyChallenges().Count);
                Assert.AreEqual(3, ChallengeDAO.GetActiveMonthlyChallenges().Count);

                Log("Generating Random Challenges again");
                ChallengeManager.GenerateNewChallenges();

                Log("Verifying no more challenges were created");
                Assert.AreEqual(3, ChallengeDAO.GetActiveDailyChallenges().Count);
                Assert.AreEqual(3, ChallengeDAO.GetActiveWeeklyChallenges().Count);
                Assert.AreEqual(3, ChallengeDAO.GetActiveMonthlyChallenges().Count);
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
