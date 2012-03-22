using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ActivEarth.DAO;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivEarth.Tests.Competition.Challenges
{
    /// <summary>
    /// Summary description for ChallengeDAOTest
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

        [TestInitialize]
        public void Initialize()
        {
            _trans = new TransactionScope();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _trans.Dispose();
        }

        #region ---------- Test Cases ----------

        [TestMethod]
        public void TestCreateChallenge()
        {
            using (_trans)
            {
                uint id = 5;

                Log("Creating challenge");
                Challenge challenge = new Challenge(id, "Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Placeholder.Statistic.Steps, 500);

                Log("Adding challenge to the database");
                Assert.IsTrue(ChallengeDAO.CreateNewChallenge(challenge));

                Log("Loading challenge from the database");
                Challenge retrieved = ChallengeDAO.GetChallengeFromChallengeId(id);

                Log("Verifying that a matching challenge was found");
                Assert.IsNotNull(retrieved);

                Log("Verifying the integrity of challenge fields");
                Assert.AreEqual(challenge.Name, retrieved.Name);
                Assert.AreEqual(challenge.Description, retrieved.Description);
                Assert.AreEqual(challenge.EndTime, retrieved.EndTime);
                Assert.AreEqual(challenge.ID, id);
                Assert.AreEqual(challenge.IsActive, retrieved.IsActive);
                Assert.AreEqual(challenge.IsPersistent, retrieved.IsPersistent);
                Assert.AreEqual(challenge.Points, retrieved.Points);
                Assert.AreEqual(challenge.StatisticBinding, retrieved.StatisticBinding);
            }
        }

        [TestMethod]
        public void TestUpdateChallenge()
        {
            using (_trans)
            {
                uint id = 7;

                Log("Creating challenge");
                Challenge challenge = new Challenge(id, "Test Challenge", "This is a test challenge",
                    30, false, DateTime.Today, 1, Placeholder.Statistic.Steps, 500);

                Log("Adding challenge to the database");
                Assert.IsTrue(ChallengeDAO.CreateNewChallenge(challenge));

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
