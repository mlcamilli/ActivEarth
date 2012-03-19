using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.Competition;
using ActivEarth.Competition.Badges;

using Statistics = ActivEarth.Competition.Placeholder.Statistics;
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
            this._user = new User("Test", "Subject");

            this._user.Badges[Statistics.BikeDistance] = 
                new Badge(1, "BikeDistance", this._user, Statistics.BikeDistance,
                BadgeConstants.BikingDistance.REQUIREMENTS, BadgeConstants.BikingDistance.POINTS,
                BadgeConstants.BikingDistance.IMAGES);

            this._user.Badges[Statistics.WalkDistance] =
                new Badge(2, "WalkDistance", this._user, Statistics.WalkDistance,
                BadgeConstants.WalkingDistance.REQUIREMENTS, BadgeConstants.WalkingDistance.POINTS,
                BadgeConstants.WalkingDistance.IMAGES);

            this._user.Badges[Statistics.RunDistance] =
                new Badge(3, "RunDistance", this._user, Statistics.RunDistance,
                BadgeConstants.RunningDistance.REQUIREMENTS, BadgeConstants.RunningDistance.POINTS,
                BadgeConstants.RunningDistance.IMAGES);

            this._user.Badges[Statistics.Steps] =
                new Badge(4, "Steps", this._user, Statistics.Steps,
                BadgeConstants.Steps.REQUIREMENTS, BadgeConstants.Steps.POINTS,
                BadgeConstants.Steps.IMAGES);

            this._user.Badges[Statistics.GasSavings] =
                new Badge(5, "BikeDistance", this._user, Statistics.GasSavings,
                BadgeConstants.GasMoney.REQUIREMENTS, BadgeConstants.GasMoney.POINTS,
                BadgeConstants.GasMoney.IMAGES);

            this._user.Badges[Statistics.ChallengesCompleted] =
                new Badge(6, "BikeDistance", this._user, Statistics.ChallengesCompleted,
                BadgeConstants.Challenges.REQUIREMENTS, BadgeConstants.Challenges.POINTS,
                BadgeConstants.Challenges.IMAGES);

            this._user.SetStatistic(Statistics.BikeDistance, 0);
            this._user.SetStatistic(Statistics.WalkDistance, 0);
            this._user.SetStatistic(Statistics.RunDistance, 0);
            this._user.SetStatistic(Statistics.Steps, 0);
            this._user.SetStatistic(Statistics.GasSavings, 0);
            this._user.SetStatistic(Statistics.ChallengesCompleted, 0);
        }

        [TestMethod]
        public void TestBadgeUpdateInitialState()
        {
            Badge badge = this._user.Badges[Statistics.Steps];
            Assert.AreEqual(0, badge.Update());
        }

        [TestMethod]
        public void TestBadgeUpdateNoChange()
        {
            Badge badge = this._user.Badges[Statistics.Steps];

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]);

            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Bronze], badge.Update());
            Assert.AreEqual(0, badge.Update());
        }

        [TestMethod]
        public void TestBadgeUpdateUpOneLevel()
        {
            Badge badge = this._user.Badges[Statistics.Steps];

            for (int level = BadgeLevels.Bronze; level <= BadgeLevels.Diamond; level++)
            {
                this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[level]);
                Assert.AreEqual(BadgeConstants.Steps.POINTS[level], badge.Update());
            }
        }

        [TestMethod]
        public void TestBadgeUpdateUpMultipleLevels()
        {
            Badge badge = this._user.Badges[Statistics.Steps];

            for (int level = BadgeLevels.Bronze; level <= BadgeLevels.Diamond; level+=2)
            {
                this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[level]);
                Assert.AreEqual(BadgeConstants.Steps.POINTS[level] + BadgeConstants.Steps.POINTS[level - 1], badge.Update());
            }
        }

        [TestMethod]
        public void TestBadgeStatisticIndependence()
        {
            Badge stepBadge = this._user.Badges[Statistics.Steps];
            Badge walkBadge = this._user.Badges[Statistics.WalkDistance];

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]);
            this._user.SetStatistic(Statistics.WalkDistance, BadgeConstants.WalkingDistance.REQUIREMENTS[BadgeLevels.Bronze]);

            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Bronze], stepBadge.Update());
            Assert.AreEqual(BadgeConstants.WalkingDistance.POINTS[BadgeLevels.Bronze], walkBadge.Update());

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Silver]);

            Assert.AreEqual(0, walkBadge.Update());
            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Silver], stepBadge.Update());
        }

        [TestMethod]
        public void TestBadgeGetImagePath()
        {
            Badge stepBadge = this._user.Badges[Statistics.Steps];
            Assert.AreEqual(BadgeConstants.Steps.IMAGES[BadgeLevels.None], stepBadge.GetImagePath());

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze]);
            stepBadge.Update();
            Assert.AreEqual(BadgeConstants.Steps.IMAGES[BadgeLevels.Bronze], stepBadge.GetImagePath());

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Platinum]);
            stepBadge.Update();
            Assert.AreEqual(BadgeConstants.Steps.IMAGES[BadgeLevels.Platinum], stepBadge.GetImagePath());
        }

        [TestMethod]
        public void TestBadgeGetNextLevelRequirement()
        {
            Badge stepBadge = this._user.Badges[Statistics.Steps];
            Assert.AreEqual(BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Bronze], stepBadge.GetNextLevelRequirement());

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Gold]);
            stepBadge.Update();
            Assert.AreEqual(BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Platinum], stepBadge.GetNextLevelRequirement());

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Max]);
            stepBadge.Update();
            Assert.AreEqual(float.PositiveInfinity, stepBadge.GetNextLevelRequirement());
        }

        [TestMethod]
        public void TestBadgeGetNextLevelPoints()
        {
            Badge stepBadge = this._user.Badges[Statistics.Steps];
            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Bronze], stepBadge.GetNextLevelPoints());

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Gold]);
            stepBadge.Update();
            Assert.AreEqual(BadgeConstants.Steps.POINTS[BadgeLevels.Platinum], stepBadge.GetNextLevelPoints());

            this._user.SetStatistic(Statistics.Steps, BadgeConstants.Steps.REQUIREMENTS[BadgeLevels.Max]);
            stepBadge.Update();
            Assert.AreEqual(0, stepBadge.GetNextLevelPoints());
        }
    }
}
