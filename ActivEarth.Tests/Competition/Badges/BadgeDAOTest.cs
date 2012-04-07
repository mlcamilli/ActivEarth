using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.DAO;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Server.Service;
using ActivEarth.Server.Service.Competition;

namespace ActivEarth.Tests.Competition.Badges
{
    /// <summary>
    /// Tests the functionality of the Badge data layer.
    /// </summary>
    [TestClass]
    public class BadgeDAOTest
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
        /// Verifies that the badge image path changes appropriately as the badge increases in level.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) Loop through each badge level by increasing the statistics to the required level.
        /// 3) VERIFY: For each level reached, the correct image path is reported.
        /// </remarks>
        [TestMethod]
        public void TestBadgeImagePath()
        {
            int user1ID = UserDAO.GetUserIdFromUserName("badgetest1");
            User user1 = UserDAO.GetUserFromUserId(user1ID);

            Log("Creating Step Badge");
            Badge stepBadge = BadgeManager.CreateBadge(user1, Statistic.Steps);

            for (int level = BadgeLevels.None; level <= BadgeLevels.Max; level++)
            {
                Log(String.Format("Increasing badge to level {0}", level));
                stepBadge.Level = level;
                BadgeDAO.UpdateBadge(stepBadge);

                Log("Reloading badge");
                stepBadge = BadgeDAO.GetBadgeFromBadgeId(stepBadge.ID);

                Log("Verifying badge image path");
                Assert.AreEqual(BadgeConstants.Steps.IMAGES[level], stepBadge.ImagePath);
            }
        }

        /// <summary>
        /// Verifies that the badge collection for an individual user retrieves badges correctly.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize two users with a different number of badges.
        /// 2) VERIFY: Each user returns the correct number of badges.
        /// </remarks>
        [TestMethod]
        public void TestGetBadgesFromUserId()
        {
            Log("Initializing users");
            int user1ID = UserDAO.GetUserIdFromUserName("badgetest1");
            User user1 = UserDAO.GetUserFromUserId(user1ID);

            int user2ID = UserDAO.GetUserIdFromUserName("badgetest2");
            User user2 = UserDAO.GetUserFromUserId(user2ID);

            Log("Creating 6 badges for user1");
            BadgeManager.CreateBadge(user1, Statistic.Steps);
            BadgeManager.CreateBadge(user1, Statistic.BikeDistance);
            BadgeManager.CreateBadge(user1, Statistic.RunDistance);
            BadgeManager.CreateBadge(user1, Statistic.WalkDistance);
            BadgeManager.CreateBadge(user1, Statistic.GasSavings);
            BadgeManager.CreateBadge(user1, Statistic.ChallengesCompleted);

            Log("Creating 4 badges for user2");
            BadgeManager.CreateBadge(user2, Statistic.Steps);
            BadgeManager.CreateBadge(user2, Statistic.BikeDistance);
            BadgeManager.CreateBadge(user2, Statistic.RunDistance);
            BadgeManager.CreateBadge(user2, Statistic.WalkDistance);

            Log("Verifying user1's badge count");
            Assert.AreEqual(6, BadgeDAO.GetBadgesFromUserId(user1ID).Count);

            Log("Verifying user2's badge count");
            Assert.AreEqual(4, BadgeDAO.GetBadgesFromUserId(user2ID).Count);
        }

        /// <summary>
        /// Verifies that a single badge can be retrieved from the owning user and statistic.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize two users with a their badges.
        /// 2) Retrieve user1's Steps badge.
        /// 3) VERIFY: Retrieved badge is linked to the Steps statistic.
        /// 4) VERIFY: Retrieved badge is linked to user1.
        /// </remarks>
        [TestMethod]
        public void TestGetBadgeFromUserIdAndStatistic()
        {
            Log("Initializing user");
            int user1ID = UserDAO.GetUserIdFromUserName("badgetest1");
            User user1 = UserDAO.GetUserFromUserId(user1ID);
            int user2ID = UserDAO.GetUserIdFromUserName("badgetest2");
            User user2 = UserDAO.GetUserFromUserId(user2ID);

            Log("Creating badges for user1");
            BadgeManager.CreateBadge(user1, Statistic.Steps);
            BadgeManager.CreateBadge(user1, Statistic.BikeDistance);
            BadgeManager.CreateBadge(user1, Statistic.RunDistance);
            BadgeManager.CreateBadge(user1, Statistic.WalkDistance);
            BadgeManager.CreateBadge(user1, Statistic.GasSavings);
            BadgeManager.CreateBadge(user1, Statistic.ChallengesCompleted);

            Log("Retrieving badge");
            Badge badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(user1ID, Statistic.Steps);

            Log("Verifying returned badge statistic");
            Assert.AreEqual(Statistic.Steps, badge.StatisticBinding);

            Log("Verifying returned badge User link");
            Assert.AreEqual(user1ID, badge.UserID);
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
