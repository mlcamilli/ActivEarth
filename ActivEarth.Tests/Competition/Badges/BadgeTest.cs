using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.Competition;
using ActivEarth.Competition.Badges;

using Statistics = ActivEarth.Competition.Placeholder.Statistic;
using User = ActivEarth.Competition.Placeholder.User;
using Group = ActivEarth.Competition.Placeholder.Group;

namespace ActivEarth.Tests.Competition.Badges
{
    /// <summary>
    /// Test the functionality of the Badge system
    /// </summary>
    [TestClass]
    public class BadgeTest
    {
        private Placeholder.User _user;

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
            _user = new User("Test", "Subject");

            _user.Badges[Statistics.BikeDistance] = 
                new Badge(1, "BikeDistance", _user, Statistics.BikeDistance,
                BadgeConstants.BikeDistance.REQUIREMENTS, BadgeConstants.BikeDistance.POINTS,
                BadgeConstants.BikeDistance.IMAGES);

            _user.Badges[Statistics.WalkDistance] =
                new Badge(2, "WalkDistance", _user, Statistics.WalkDistance,
                BadgeConstants.WalkDistance.REQUIREMENTS, BadgeConstants.WalkDistance.POINTS,
                BadgeConstants.WalkDistance.IMAGES);

            _user.Badges[Statistics.RunDistance] =
                new Badge(3, "RunDistance", _user, Statistics.RunDistance,
                BadgeConstants.RunDistance.REQUIREMENTS, BadgeConstants.RunDistance.POINTS,
                BadgeConstants.RunDistance.IMAGES);

            _user.Badges[Statistics.Steps] =
                new Badge(4, "Steps", _user, Statistics.Steps,
                BadgeConstants.Steps.REQUIREMENTS, BadgeConstants.Steps.POINTS,
                BadgeConstants.Steps.IMAGES);

            _user.Badges[Statistics.ChallengesCompleted] =
                new Badge(5, "ChallengesCompleted", _user, Statistics.ChallengesCompleted,
                BadgeConstants.Challenges.REQUIREMENTS, BadgeConstants.Challenges.POINTS,
                BadgeConstants.Challenges.IMAGES);

            _user.SetStatistic(Statistics.BikeDistance, 0);
            _user.SetStatistic(Statistics.WalkDistance, 0);
            _user.SetStatistic(Statistics.RunDistance, 0);
            _user.SetStatistic(Statistics.Steps, 0);
            _user.SetStatistic(Statistics.ChallengesCompleted, 0);
        }

        #region ---------- Test Cases ----------

        [TestMethod]
        public void TestBadgeUpdateInitialState()
        {
            Log("Fetching Steps badge");
            Badge badge = _user.Badges[Statistics.Steps];

            Log("Verifying no activity points are reported by the badge");
            Assert.AreEqual(0, badge.Update());
        }

        [TestMethod]
        public void TestBadgeUpdateNoChange()
        {
            Log("Fetching Steps badge");
            Badge badge = _user.Badges[Statistics.Steps];

            Log("Updating user's step statistic to the bronze badge level");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]);

            Log("Verifying that first update reports bronze badge reward");
            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Bronze], badge.Update());

            Log("Verifying that second update reports no new activity points");
            Assert.AreEqual(0, badge.Update());
        }

        [TestMethod]
        public void TestBadgeUpdateUpOneLevel()
        {
            Log("Fetching Steps badge");
            Badge badge = _user.Badges[Statistics.Steps];

            for (int level = BadgeLevels.Bronze; level <= BadgeLevels.Diamond; level++)
            {
                Log(String.Format("Increasing badge to level {0}", level));
                _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[level]);

                Log("Verifying badge reward on update");
                Assert.AreEqual(BadgeConstants.Steps.POINTS[level], badge.Update());
            }
        }

        [TestMethod]
        public void TestBadgeUpdateUpMultipleLevels()
        {
            Log("Fetching Steps badge");
            Badge badge = _user.Badges[Statistics.Steps];

            for (int level = BadgeLevels.Bronze; level <= BadgeLevels.Diamond; level+=2)
            {
                Log(String.Format("Increasing badge to level {0}", level));
                _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[level]);

                Log("Verifying badge reward on update");
                Assert.AreEqual(BadgeConstants.Steps.POINTS[level] + BadgeConstants.Steps.POINTS[level - 1], badge.Update());
            }
        }

        [TestMethod]
        public void TestBadgeStatisticIndependence()
        {
            Log("Fetching Steps badge");
            Badge stepBadge = _user.Badges[Statistics.Steps];

            Log("Fetching Walking badge");
            Badge walkBadge = _user.Badges[Statistics.WalkDistance];

            Log("Setting user's initial statistics to bronze levels");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]);
            _user.SetStatistic(Statistics.WalkDistance, BadgeConstants.WalkDistance.REQUIREMENTS[BadgeLevels.Bronze]);

            Log("Verifying step badge reward");
            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Bronze], stepBadge.Update());

            Log("Verifying walking badge reward");
            Assert.AreEqual(BadgeConstants.WalkDistance.POINTS[BadgeLevels.Bronze], walkBadge.Update());

            Log("Increasing step statistic to silver level");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Silver]);

            Log("Verifying no new walking badge points awarded");
            Assert.AreEqual(0, walkBadge.Update());

            Log("Verifying silver level step badge points awarded");
            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Silver], stepBadge.Update());
        }

        [TestMethod]
        public void TestBadgeGetImagePath()
        {
            Log("Fetching Steps badge");
            Badge stepBadge = _user.Badges[Statistics.Steps];

            Log("Verifying initial badge image path");
            Assert.AreEqual(BadgeConstants.Steps.IMAGES[BadgeLevels.None], stepBadge.GetImagePath());

            Log("Increasing badge to Bronze level");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]);
            stepBadge.Update();

            Log("Verifying bronze badge image path");
            Assert.AreEqual(BadgeConstants.Steps.IMAGES[BadgeLevels.Bronze], stepBadge.GetImagePath());

            Log("Increasing badge to Platinum level");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Platinum]);
            stepBadge.Update();

            Log("Verifying platinum badge image path");
            Assert.AreEqual(BadgeConstants.Steps.IMAGES[BadgeLevels.Platinum], stepBadge.GetImagePath());
        }

        [TestMethod]
        public void TestBadgeGetNextLevelRequirement()
        {
            Log("Fetching Steps badge");
            Badge stepBadge = _user.Badges[Statistics.Steps];

            Log("Verifying initial 'next level' requirements");
            Assert.AreEqual(BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze], stepBadge.GetNextLevelRequirement());

            Log("Increasing badge to gold level");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Gold]);
            stepBadge.Update();

            Log("Verifying 'next level' requirements");
            Assert.AreEqual(BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Platinum], stepBadge.GetNextLevelRequirement());

            Log("Increasing badge to maximum level");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Max]);
            stepBadge.Update();

            Log("Verifying 'next level' requirements reported as infinity");
            Assert.AreEqual(float.PositiveInfinity, stepBadge.GetNextLevelRequirement());
        }

        [TestMethod]
        public void TestBadgeGetNextLevelPoints()
        {
            Log("Fetching Steps badge");
            Badge stepBadge = _user.Badges[Statistics.Steps];

            Log("Verifying initial 'next level' reward");
            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Bronze], stepBadge.GetNextLevelPoints());

            Log("Increasing badge to gold level");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Gold]);
            stepBadge.Update();

            Log("Verifying 'next level' reward");
            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Platinum], stepBadge.GetNextLevelPoints());

            Log("Increasing badge to maximum level");
            _user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Max]);
            stepBadge.Update();

            Log("Verifying 'next level' reward reported as zero");
            Assert.AreEqual(0, stepBadge.GetNextLevelPoints());
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
