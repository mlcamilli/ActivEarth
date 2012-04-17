using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.DAO;
using ActivEarth.Server.Service.Competition;
using ActivEarth.Server.Service.Statistics;

namespace ActivEarth.Tests.Competition.Badges
{
    /// <summary>
    /// Test the functionality of the Badge system
    /// </summary>
    [TestClass]
    public class BadgeTest
    {
        private User _user;
        private int _id;

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
        /// Creates the transaction scope for the test suite.
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

        /// <summary>
        /// Initializes a new user with fresh (level 0) badges.
        /// </summary>
        private void InitializeBadges()
        {
            _id = UserDAO.GetUserIdFromUserName("badgetest1");
            _user = UserDAO.GetUserFromUserId(_id);

            StatisticManager.SetUserStatistic(_id, Statistic.BikeDistance, 0);
            StatisticManager.SetUserStatistic(_id, Statistic.WalkDistance, 0);
            StatisticManager.SetUserStatistic(_id, Statistic.RunDistance, 0);
            StatisticManager.SetUserStatistic(_id, Statistic.Steps, 0);
            StatisticManager.SetUserStatistic(_id, Statistic.ChallengesCompleted, 0);
            StatisticManager.SetUserStatistic(_id, Statistic.GasSavings, 0);

            BadgeManager.CreateBadge(_id, Statistic.BikeDistance);
            BadgeManager.CreateBadge(_id, Statistic.WalkDistance);
            BadgeManager.CreateBadge(_id, Statistic.RunDistance);
            BadgeManager.CreateBadge(_id, Statistic.Steps);
            BadgeManager.CreateBadge(_id, Statistic.GasSavings);
            BadgeManager.CreateBadge(_id, Statistic.ChallengesCompleted);

            BadgeManager.UpdateBadge(_id, Statistic.BikeDistance);
            BadgeManager.UpdateBadge(_id, Statistic.WalkDistance);
            BadgeManager.UpdateBadge(_id, Statistic.RunDistance);
            BadgeManager.UpdateBadge(_id, Statistic.Steps);
            BadgeManager.UpdateBadge(_id, Statistic.GasSavings);
            BadgeManager.UpdateBadge(_id, Statistic.ChallengesCompleted);
        }

        #region ---------- Test Cases ----------

        /// <summary>
        /// Verifies that upon its creation, a fresh badge reports no earned activity points.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) VERIFY: No activity points are reported by Update().
        /// </remarks>
        [TestMethod]
        public void TestBadgeUpdateInitialState()
        {
            using (_trans)
            {
                InitializeBadges();
                Log("Verifying no activity points are reported by the badge");
                Assert.AreEqual(0, BadgeManager.UpdateBadge(_id, Statistic.Steps));
            }
        }


        /// <summary>
        /// Verifies that a badge does not report earned activity points more than once.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) Increase statistic such that a new badge level is earned.
        /// 3) VERIFY: Update() reports newly earned activity points on first call.
        /// 4) VERIFY; Update() reports no newly earned activity points on subsequent call.
        /// </remarks>
        [TestMethod]
        public void TestBadgeUpdateNoChange()
        {
            using (_trans)
            {
                InitializeBadges();
                Log("Updating user's step statistic to the bronze badge level");
                StatisticManager.SetUserStatistic(_id, Statistic.Steps,
                    BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]);

                Log("Verifying that first update reports bronze badge reward");
                Assert.AreEqual(BadgeConstants.Steps.REWARDS[BadgeLevels.Bronze],
                    BadgeManager.UpdateBadge(_id, Statistic.Steps));

                Log("Verifying that second update reports no new activity points");
                Assert.AreEqual(0, BadgeManager.UpdateBadge(_id, Statistic.Steps));
            }
        }

        /// <summary>
        /// Verifies that activity points are reported correctly when a badge increases by a single level.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) Loop through each badge level by increasing the statistics to the required level.
        /// 3) VERIFY: For each level, the correct number of activity points are rewarded.
        /// </remarks>
        [TestMethod]
        public void TestBadgeUpdateUpOneLevel()
        {
            using (_trans)
            {
                InitializeBadges();
                for (int level = BadgeLevels.Bronze; level <= BadgeLevels.Max; level++)
                {
                    Log(String.Format("Increasing badge to level {0}", level));
                    StatisticManager.SetUserStatistic(_id, Statistic.Steps,
                        BadgeConstants.Steps.REQUIREMENTS[level]);

                    Log("Verifying badge reward on update");
                    Assert.AreEqual(BadgeConstants.Steps.REWARDS[level],
                        BadgeManager.UpdateBadge(_id, Statistic.Steps));
                }
            }
        }

        /// <summary>
        /// Verifies that activity points are reported correctly when a badge increases by multiple levels.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) Loop through each badge level (skipping every other level) by increasing the 
        ///     statistics to the required level.
        /// 3) VERIFY: For each level reached, both the earned level and the skipped level's points are reported.
        /// </remarks>
        [TestMethod]
        public void TestBadgeUpdateUpMultipleLevels()
        {
            using (_trans)
            {
                InitializeBadges();
                for (int level = BadgeLevels.Bronze; level <= BadgeLevels.Max; level += 2)
                {
                    Log(String.Format("Increasing badge to level {0}", level));
                    StatisticManager.SetUserStatistic(_id, Statistic.Steps,
                        BadgeConstants.Steps.REQUIREMENTS[level]);

                    Log("Verifying badge reward on update");
                    Assert.AreEqual(BadgeConstants.Steps.REWARDS[level] +
                        BadgeConstants.Steps.REWARDS[level - 1], BadgeManager.UpdateBadge(_id, Statistic.Steps));
                }
            }
        }

        /// <summary>
        /// Verifies that level progress is calculated correctly for the progress bar.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) VERIFY: Progress reports 0%.
        /// 3) Set the statistic to halfway to the Bronze badge.
        /// 4) VERIFY: Progress reports 50%.
        /// 5) Set the statistic to halfway through the Bronze badge.
        /// 6) VERIFY: Progress reports 50%.
        /// 7) Set the statistic to exactly the level required for the Silver badge.
        /// 8) VERIFY: Progress reports 0%.
        /// </remarks>
        [TestMethod]
        public void TestBadgeProgress()
        {
            using (_trans)
            {
                InitializeBadges();
                Log("Fetching Steps badge");
                BadgeManager.UpdateBadge(_id, Statistic.Steps);
                Badge badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                Log("Verifying progress.");
                Assert.AreEqual(0, badge.Progress);

                Log("Updating statistic to halfway to the Bronze badge.");
                StatisticManager.SetUserStatistic(_id, Statistic.Steps,
                    BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze] / 2);
                BadgeManager.UpdateBadge(_id, Statistic.Steps);
                badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                Log("Verifying progress.");
                Assert.AreEqual(50, badge.Progress);

                float delta = BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Silver] -
                    BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze];

                Log("Updating statistic to halfway to the Silver badge.");
                StatisticManager.SetUserStatistic(_id, Statistic.Steps,
                    BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze] + (delta / 2));
                BadgeManager.UpdateBadge(_id, Statistic.Steps);
                badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                Log("Verifying progress.");
                Assert.AreEqual(50, badge.Progress);

                Log("Updating statistic to exactly get the Silver badge.");
                StatisticManager.SetUserStatistic(_id, Statistic.Steps,
                    BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Silver]);
                BadgeManager.UpdateBadge(_id, Statistic.Steps);
                badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                Log("Verifying progress.");
                Assert.AreEqual(0, badge.Progress);
            }
        }

        /// <summary>
        /// Verifies that text progress report is formatted correctly.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) VERIFY: Progress reports 0%.
        /// 3) Set the statistic to halfway to the Bronze badge.
        /// 4) VERIFY: Progress reports 50%.
        /// 5) Set the statistic to halfway through the Bronze badge.
        /// 6) VERIFY: Progress reports 50%.
        /// 7) Set the statistic to exactly the level required for the Silver badge.
        /// 8) VERIFY: Progress reports 0%.
        /// </remarks>
        [TestMethod]
        public void TestBadgeFormattedProgress()
        {
            using (_trans)
            {
                InitializeBadges();
                Log("Fetching Steps badge");
                BadgeManager.UpdateBadge(_id, Statistic.Steps);
                Badge badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                Log("Verifying progress.");
                Assert.AreEqual(0, badge.Progress);

                Log("Updating statistic to halfway to the Bronze badge.");
                StatisticManager.SetUserStatistic(_id, Statistic.Steps, 50);
                BadgeManager.UpdateBadge(_id, Statistic.Steps);

                Log("Verifying progress.");
                Assert.AreEqual(String.Format("50 / {0}", BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]),
                    BadgeManager.GetFormattedProgress(badge.ID));

                Log("Updating statistic to the Max badge.");
                StatisticManager.SetUserStatistic(_id, Statistic.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Max]);
                BadgeManager.UpdateBadge(_id, Statistic.Steps);

                Log("Verifying progress.");
                Assert.AreEqual(String.Format("{0}", BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Max]),
                    BadgeManager.GetFormattedProgress(badge.ID));

                BadgeManager.UpdateBadge(_id, Statistic.GasSavings);
                Badge gasBadge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.GasSavings);

                Assert.AreEqual(String.Format("$0.00 / ${0:0.00}", BadgeConstants.GasSavings.REQUIREMENTS[BadgeLevels.Bronze]),
                    BadgeManager.GetFormattedProgress(gasBadge.ID));
            }
            
        }

        /// <summary>
        /// Verifies that badges linked to different statistics operate independently of one another.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badges with statistic at 0 for two different statistics.
        /// 2) Increase both relevant statistics to the bronze level.
        /// 3) VERIFY: Both badges report bronze-level activity points on Update.
        /// 4) Increase one of the relevant statistics to the silver level.
        /// 5) VERIFY: The unchanged badge reports no new activity points on Update.
        /// 6) VERIFY: The changed badge reports silver-level activity points on Update.
        /// </remarks>
        [TestMethod]
        public void TestBadgeStatisticIndependence()
        {
            using (_trans)
            {
                InitializeBadges();
                Log("Fetching Steps badge");
                Badge stepBadge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                Log("Fetching Walking badge");
                Badge walkBadge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.WalkDistance);

                Log("Setting user's initial statistics to bronze levels");
                StatisticManager.SetUserStatistic(_id, Statistic.Steps, 
                    BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]);
                StatisticManager.SetUserStatistic(_id, Statistic.WalkDistance, 
                    BadgeConstants.WalkDistance.REQUIREMENTS[BadgeLevels.Bronze]);

                Log("Verifying step badge reward");
                Assert.AreEqual(BadgeConstants.Steps.REWARDS[BadgeLevels.Bronze], 
                    BadgeManager.UpdateBadge(_id, Statistic.Steps));

                Log("Verifying walking badge reward");
                Assert.AreEqual(BadgeConstants.WalkDistance.REWARDS[BadgeLevels.Bronze], 
                    BadgeManager.UpdateBadge(_id, Statistic.WalkDistance));

                Log("Increasing step statistic to silver level");
                StatisticManager.SetUserStatistic(_id, Statistic.Steps, 
                    BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Silver]);

                Log("Verifying no new walking badge points awarded");
                Assert.AreEqual(0, BadgeManager.UpdateBadge(_id, Statistic.WalkDistance));

                Log("Verifying silver level step badge points awarded");
                Assert.AreEqual(BadgeConstants.Steps.REWARDS[BadgeLevels.Silver], 
                    BadgeManager.UpdateBadge(_id, Statistic.Steps));
            }
        }

        /// <summary>
        /// Verifies that the requirement for the next level is reported correctly for each 
        /// badge level.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) Loop through each badge level by increasing the statistics to the required level.
        /// 3) VERIFY: For each level reached, the correct requirement is reported.
        /// 
        /// NOTE: When the badge level is maximized, the next level requirement should be reported
        ///     as infinity.
        /// </remarks>
        [TestMethod]
        public void TestBadgeGetNextLevelRequirement()
        {
            using (_trans)
            {
                InitializeBadges();
                Log("Fetching Steps badge");
                Badge badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                for (int level = BadgeLevels.None; level <= BadgeLevels.Max; level++)
                {
                    Log(String.Format("Increasing badge to level {0}", level));
                    StatisticManager.SetUserStatistic(_id, Statistic.Steps, BadgeConstants.Steps.REQUIREMENTS[level]);

                    Log("Updating badge");
                    BadgeManager.UpdateBadge(_id, Statistic.Steps);
                    badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                    Log(String.Format("Verifying next level requirement. Expected: {0}",
                        BadgeConstants.Steps.REQUIREMENTS[level + 1]));

                    Assert.AreEqual(BadgeConstants.Steps.REQUIREMENTS[level + 1], badge.GetNextLevelRequirement());
                }
            }
        }

        /// <summary>
        /// Verifies that the activity score reward for the next level is reported correctly 
        /// for each badge level.
        /// </summary>
        /// <remarks>
        /// Steps:
        /// 1) Initialize new badge with statistic at 0.
        /// 2) Loop through each badge level by increasing the statistics to the required level.
        /// 3) VERIFY: For each level reached, the correct reward is reported.
        /// 
        /// NOTE: When the badge level is maximized, the next level requirement should be reported
        ///     as 0.
        /// </remarks>
        [TestMethod]
        public void TestBadgeGetNextLevelPoints()
        {
            using (_trans)
            {
                InitializeBadges();
                Log("Fetching Steps badge");
                Badge badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                for (int level = BadgeLevels.None; level <= BadgeLevels.Max; level++)
                {
                    Log(String.Format("Increasing badge to level {0}", level));
                    StatisticManager.SetUserStatistic(_id, Statistic.Steps, BadgeConstants.Steps.REQUIREMENTS[level]);

                    Log("Updating badge");
                    BadgeManager.UpdateBadge(_id, Statistic.Steps);
                    badge = BadgeDAO.GetBadgeFromUserIdAndStatistic(_id, Statistic.Steps);

                    Log(String.Format("Verifying next level reward. Expected: {0}",
                        BadgeConstants.Steps.REWARDS[level + 1]));

                    Assert.AreEqual(BadgeConstants.Steps.REWARDS[level + 1], badge.GetNextLevelReward());
                }
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
