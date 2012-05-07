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

namespace ActivEarth.Tests
{
    /// <summary>
    /// Tests the functionality of the Active Routes system.
    /// </summary>
    [TestClass]
    public class ActiveRouteDAOTest
    {
        private User _user1;

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
        /// Verifies that routes can be processed to update user statistics.
        /// </summary>
        [TestMethod]
        public void TestAddRoute()
        {
            using (_trans)
            {
                Log("Creating test user in DB");
                _user1.UserID = UserDAO.CreateNewUser(_user1, "pass1");

                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.Steps, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.WalkDistance, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.RunDistance, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.BikeDistance, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.AggregateDistance, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.WalkTime, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.RunTime, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.BikeTime, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.AggregateTime, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.ChallengesCompleted, 0);
                UserStatisticDAO.CreateNewStatisticForUser(_user1.UserID, Statistic.GasSavings, 0);

                Route route = new Route()
                {
                    GMTOffset = -14400,
                    Distance = 2627,
                    EndLatitude = 37.22998,
                    EndLongitude = -80.421654,
                    EndTime = new DateTime(2012, 4, 30, 18, 25, 49),
                    Mode = "running",
                    Points = "POINTSWOULDGOHEREIFTHISWASNTATEST",
                    StartLatitude = 37.222889,
                    StartLongitude = -80.42263,
                    StartTime = new DateTime(2012, 4, 29, 19, 16, 32),
                    Steps = 709,
                    Type = "transportation",
                    UserId = _user1.UserID
                };

                Log("Adding route");
                ActiveRouteDAO.AddNewRoute(route);

                Log("Verifying that statistics updated");
                Assert.AreEqual(709, UserStatisticDAO.GetStatisticFromUserIdAndStatType(_user1.UserID, Statistic.Steps).Value);
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
