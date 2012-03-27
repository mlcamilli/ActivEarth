using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.DAO;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Server.Service;

namespace ActivEarth.Tests.Competition.Challenges
{
    /// <summary>
    /// Tests the functionality of the Challenge data layer.
    /// </summary>
    [TestClass]
    public class ChallengeDAOTest
    {
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
        /// Creates the Transaction Scope for the test case.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
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
                Assert.AreEqual(challenge.Points, retrieved.Points);
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
                Assert.AreEqual(2, ChallengeDAO.GetActiveChallenges().Count);
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
                Assert.AreEqual(3, ChallengeDAO.GetAllChallenges().Count);
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
