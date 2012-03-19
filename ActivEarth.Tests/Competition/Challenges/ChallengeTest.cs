using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.Competition;
using ActivEarth.Competition.Challenges;

using Statistics = ActivEarth.Competition.Placeholder.Statistics;
using User = ActivEarth.Competition.Placeholder.User;
using Group = ActivEarth.Competition.Placeholder.Group;

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
            this._user1 = new User("Test", "Subject1");
            this._user2 = new User("Test", "Subject2");

            this._allUsers = new Group("All Users");
            this._allUsers.Members.Add(this._user1);
            this._allUsers.Members.Add(this._user2);

            this._manager = new ChallengeManager(this._allUsers);
        }

        [TestMethod]
        public void TestChallengeCreation()
        {
            this._user2.SetStatistic(Statistics.Steps, 50);

            uint id = this._manager.CreateChallenge("Test Challenge", "This is a test challenge", 
                30, false, DateTime.Now.AddDays(1), Statistics.Steps, 500);

            Assert.IsTrue(this._user1.ChallengeInitialValues.ContainsKey(id));
            Assert.AreEqual(0, this._user1.ChallengeInitialValues[id]);

            Assert.IsTrue(this._user2.ChallengeInitialValues.ContainsKey(id));
            Assert.AreEqual(50, this._user2.ChallengeInitialValues[id]);
        }

        [TestMethod]
        public void TestChallengeProgressIncomplete()
        {
            this._user2.SetStatistic(Statistics.Steps, 50);

            uint id = this._manager.CreateChallenge("Test Challenge", "This is a test challenge", 
                30, false, DateTime.Now.AddDays(1), Statistics.Steps, 500);

            Challenge challenge = this._manager.GetChallenge(id);

            this._user1.SetStatistic(Statistics.Steps, 200);
            this._user2.SetStatistic(Statistics.Steps, 200);

            Assert.AreEqual(200, challenge.GetProgress(this._user1));
            Assert.AreEqual(150, challenge.GetProgress(this._user2));

            Assert.IsFalse(challenge.IsComplete(this._user1));
            Assert.IsFalse(challenge.IsComplete(this._user2));
        }

        [TestMethod]
        public void TestChallengeProgressComplete()
        {            
            this._user2.SetStatistic(Statistics.Steps, 50);

            uint id = this._manager.CreateChallenge("Test Challenge", "This is a test challenge", 
                30, false, DateTime.Now.AddDays(1), Statistics.Steps, 500);

            Challenge challenge = this._manager.GetChallenge(id);

            this._user1.SetStatistic(Statistics.Steps, 525);
            this._user2.SetStatistic(Statistics.Steps, 550);

            Assert.AreEqual(500, challenge.GetProgress(this._user1));
            Assert.AreEqual(500, challenge.GetProgress(this._user2));

            Assert.IsTrue(challenge.IsComplete(this._user1));
            Assert.IsTrue(challenge.IsComplete(this._user2));
        }
        
        [TestMethod]
        public void TestChallengeMultipleInitialization()
        {
            this._user1.SetStatistic(Statistics.Steps, 50);
            this._user1.SetStatistic(Statistics.BikeDistance, 100);

            uint id1 = this._manager.CreateChallenge("Test Step Challenge", "This is a test challenge",
                30, false, DateTime.Now.AddDays(1), Statistics.Steps, 500);

            uint id2 = this._manager.CreateChallenge("Test Bike Challenge", "This is a test challenge",
                30, false, DateTime.Now.AddDays(1), Statistics.BikeDistance, 100);

            this._user1.SetStatistic(Statistics.Steps, 150);

            uint id3 = this._manager.CreateChallenge("Test Step Challenge 2", "This is another test challenge",
                30, false, DateTime.Now.AddDays(1), Statistics.Steps, 500);

            Assert.IsTrue(this._user1.ChallengeInitialValues.ContainsKey(id1));
            Assert.AreEqual(50, this._user1.ChallengeInitialValues[id1]);

            Assert.IsTrue(this._user1.ChallengeInitialValues.ContainsKey(id2));
            Assert.AreEqual(100, this._user1.ChallengeInitialValues[id2]);

            Assert.IsTrue(this._user1.ChallengeInitialValues.ContainsKey(id3));
            Assert.AreEqual(150, this._user1.ChallengeInitialValues[id3]);
        }

        [TestMethod]
        public void TestChallengeCleanup()
        {
            uint id1 = this._manager.CreateChallenge("Test Step Challenge", "This is an expired transient challenge",
                30, false, DateTime.Now.AddMinutes(-5), Statistics.Steps, 500);

            uint id2 = this._manager.CreateChallenge("Test Bike Challenge", "This is an active transient challenge",
                30, false, DateTime.Now.AddDays(1), Statistics.BikeDistance, 100);

            uint id3 = this._manager.CreateChallenge("Test Step Challenge 2", "This is a persistent challenge",
                30, true, DateTime.Now.AddMinutes(-5), Statistics.Steps, 500);

            Assert.IsNotNull(this._manager.GetChallenge(id1));
            Assert.IsNotNull(this._manager.GetChallenge(id2));
            Assert.IsNotNull(this._manager.GetChallenge(id3));

            this._manager.CleanUp();

            Assert.IsNull(this._manager.GetChallenge(id1));
            Assert.IsNotNull(this._manager.GetChallenge(id2));
            Assert.IsNotNull(this._manager.GetChallenge(id3));

            Assert.IsNotNull(this._manager.GetChallenge(id1, false));
        }

        [TestMethod]
        public void TestChallengeResetPersistentChallenge()
        {
            uint id = this._manager.CreateChallenge("Test Step Challenge", "This is a persistent challenge",
                30, true, DateTime.Now.AddMinutes(-5), Statistics.Steps, 500);

            Challenge challenge = this._manager.GetChallenge(id);

            this._user1.SetStatistic(Statistics.Steps, 250);
            Assert.AreEqual(250, challenge.GetProgress(this._user1));

            this._manager.CleanUp();

            Assert.AreEqual(0, challenge.GetProgress(this._user1));
            this._user1.SetStatistic(Statistics.Steps, 500);
            Assert.AreEqual(250, challenge.GetProgress(this._user1));
        }
    }
}
