using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ActivEarth.DAO;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Server.Service;

namespace ActivEarth.Tests.Competition.Contests
{
    /// <summary>
    /// Tests the capabilities of ContestDAO and TeamDAO for manipulation
    /// of contest-related data with the database.
    /// </summary>
    [TestClass]
    public class ContestDAOTest
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
        public void TestUpdateContestChangeName()
        {
            using (_trans)
            {
                int id;
                string newName = "New Name";

                Log("Creating contest");
                Contest contest = new Contest("Test Contest", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);

                Log("Adding contest to the database");
                Assert.IsTrue((id = ContestDAO.CreateNewContest(contest)) > 0);

                Log("Loading contest from the database");
                Contest retrieved = ContestDAO.GetContestFromContestId(id);

                Log("Verifying that a matching contest was found");
                Assert.IsNotNull(retrieved);

                Log("Renaming the Contest");
                retrieved.Name = newName;

                Log("Updating the contest in the DB");
                Assert.IsTrue(ContestDAO.UpdateContest(retrieved));

                Log("Reloading contest from the database");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id);

                Log("Verifying that a matching contest was found");
                Assert.IsNotNull(retrieved2);

                Log("Verifying that the contest has the new name");
                Assert.AreEqual(newName, retrieved2.Name);

            }
        }

        [TestMethod]
        public void TestGetAllContests()
        {
            using (_trans)
            {
                Log("Creating contests");
                Contest contest1 = new Contest("Test Contest1", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);
                Contest contest2 = new Contest("Test Contest2", "This is also a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.BikeDistance);
                Contest contest3 = new Contest("Test Contest3", "This is another test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.RunDistance);

                Log("Adding contests to DB");
                ContestDAO.CreateNewContest(contest1);
                ContestDAO.CreateNewContest(contest2);
                ContestDAO.CreateNewContest(contest3);

                Log("Verifying that GetAllContests returns three contests");
                Assert.AreEqual(3, ContestDAO.GetAllContests().Count);
            }
        }

        [TestMethod]
        public void TestGetContestFromContestIdPresentNoTeams()
        {
            using (_trans)
            {
                Log("Creating contests");
                Contest contest = new Contest("Test Contest1", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id);

                Log("Verifying the integrity of the contest fields");
                Assert.AreEqual(contest.Name, retrieved.Name);
                Assert.AreEqual(contest.Description, retrieved.Description);
                Assert.AreEqual(contest.Points, retrieved.Points);
                Assert.AreEqual(contest.Teams.Count, retrieved.Teams.Count);
                Assert.AreEqual(contest.StatisticBinding, retrieved.StatisticBinding);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestGetContestFromContestIdPresentWithTeams()
        {
            using (_trans)
            {
                Log("Creating contest");
                Contest contest = new Contest("Test Contest1", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);

                Log("Creating Members");
                Placeholder.User member1 = new Placeholder.User("Test", "Subject1");
                Placeholder.User member2 = new Placeholder.User("Test", "Subject2");
                Placeholder.User member3 = new Placeholder.User("Test", "Subject3");
                Placeholder.User member4 = new Placeholder.User("Test", "Subject4");

                Log("Creating Groups");
                Placeholder.Group group1 = new Placeholder.Group("Group 1");
                group1.Members.Add(member1);
                group1.Members.Add(member2);

                Placeholder.Group group2 = new Placeholder.Group("Group 2");
                group2.Members.Add(member3);
                group2.Members.Add(member4);

                Log("Adding groups to the contest");
                contest.AddGroup(group1);
                contest.AddGroup(group2);

                Log("Adding Members to DB");
                Assert.Fail("Must enable remainder of test code once full User database functionality exists");
                /*UserDAO.CreateNewUser(member1);
                UserDAO.CreateNewUser(member2);
                UserDAO.CreateNewUser(member3);
                UserDAO.CreateNewUser(member4);*/

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id);

                Log("Verifying the correct number of teams");
                Assert.AreEqual(contest.Teams.Count, retrieved.Teams.Count);

                Log("Verifying that each member is found");
                Assert.IsTrue(retrieved.Teams[0].ContainsMember(member1));
                Assert.IsTrue(retrieved.Teams[0].ContainsMember(member2));
                Assert.IsTrue(retrieved.Teams[1].ContainsMember(member3));
                Assert.IsTrue(retrieved.Teams[1].ContainsMember(member4));
            }
        }

        [TestMethod]
        public void TestGetContestFromContestIdNotPresent()
        {
            using (_trans)
            {
                Log("Attempting to load contest that doesn't exist");
                Contest retrieved = ContestDAO.GetContestFromContestId(-1);

                Log("Verifying that no contest was loaded and no exception was thrown");
                Assert.IsNull(retrieved);
            }
        }

        [TestMethod]
        public void TestRemoveContestFromContestId()
        {
            using (_trans)
            {
                Log("Creating contest");
                Contest contest = new Contest("Test Contest1", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id);

                Log("Verifying that the contest was retrieved");
                Assert.IsNotNull(retrieved);

                Log("Removing contest from db");
                Assert.IsTrue(ContestDAO.RemoveContestFromContestId(id));

                Log("Attempting to read contest back from DB");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id);

                Log("Verifying that the contest was not found");
                Assert.IsNull(retrieved2);
            }
        }

        [TestMethod]
        public void TestRemoveContestFromContestIdNotPresent()
        {
            using (_trans)
            {
                int id = -1;

                Log("Attempting to read contest from DB");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id);

                Log("Verifying that the contest was not found");
                Assert.IsNull(retrieved2);

                Log("Attempting to Remove nonexistent contest from db");
                Assert.IsTrue(ContestDAO.RemoveContestFromContestId(id));

            }
        }

        [TestMethod]
        [Ignore]
        public void TestUpdateContestAddTeam()
        {
            using (_trans)
            {
                Log("Creating contest");
                Contest contest = new Contest("Test Contest1", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id);

                Log("Creating Members");
                Placeholder.User member1 = new Placeholder.User("Test", "Subject1");
                Placeholder.User member2 = new Placeholder.User("Test", "Subject2");
                Placeholder.User member3 = new Placeholder.User("Test", "Subject3");
                Placeholder.User member4 = new Placeholder.User("Test", "Subject4");

                Log("Creating Groups");
                Placeholder.Group group1 = new Placeholder.Group("Group 1");
                group1.Members.Add(member1);
                group1.Members.Add(member2);
                group1.Members.Add(member3);
                group1.Members.Add(member4);

                Log("Adding group to the contest");
                contest.AddGroup(group1);

                Log("Adding Members to DB");
                Assert.Fail("Must enable remainder of test code once full User database functionality exists");
                /*UserDAO.CreateNewUser(member1);
                UserDAO.CreateNewUser(member2);
                UserDAO.CreateNewUser(member3);
                UserDAO.CreateNewUser(member4);*/

                Log("Updating Contest in DB");
                Assert.IsTrue(ContestDAO.UpdateContest(retrieved));

                Log("Re-loading contest from DB");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id);

                Log("Verifying the correct number of teams");
                Assert.AreEqual(retrieved.Teams.Count, retrieved2.Teams.Count);

                Log("Verifying that each member is found");
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(member1));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(member2));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(member3));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(member4));
            }
        }

        [TestMethod]
        [Ignore]
        public void TestUpdateContestAddTeamMembers()
        {
            using (_trans)
            {
                Log("Creating contest");
                Contest contest = new Contest("Test Contest1", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);

                Log("Creating Members");
                Placeholder.User member1 = new Placeholder.User("Test", "Subject1");
                Placeholder.User member2 = new Placeholder.User("Test", "Subject2");
                Placeholder.User member3 = new Placeholder.User("Test", "Subject3");
                Placeholder.User member4 = new Placeholder.User("Test", "Subject4");

                Log("Creating Groups");
                Placeholder.Group group1 = new Placeholder.Group("Group 1");
                group1.Members.Add(member1);
                group1.Members.Add(member2);

                Log("Adding group to the contest");
                contest.AddGroup(group1);

                Log("Adding Members to DB");
                Assert.Fail("Must enable remainder of test code once full User database functionality exists");
                /*UserDAO.CreateNewUser(member1);
                UserDAO.CreateNewUser(member2);
                UserDAO.CreateNewUser(member3);
                UserDAO.CreateNewUser(member4);*/

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id);

                Log("Adding two more members to the team");
                retrieved.Teams[0].Members.Add(new TeamMember(member3));
                retrieved.Teams[0].Members.Add(new TeamMember(member4));

                Log("Updating Contest in DB");
                Assert.IsTrue(ContestDAO.UpdateContest(retrieved));

                Log("Re-loading contest from DB");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id);

                Log("Verifying the correct number of teams");
                Assert.AreEqual(retrieved.Teams.Count, retrieved2.Teams.Count);

                Log("Verifying that each member is found");
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(member1));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(member2));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(member3));
                Assert.IsTrue(retrieved2.Teams[0].ContainsMember(member4));
            }
        }

        [TestMethod]
        [Ignore]
        public void TestUpdateContestLockInitialization()
        {
            using (_trans)
            {
                Log("Creating contest");
                Contest contest = new Contest("Test Contest1", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);

                Log("Creating Members");
                Placeholder.User member1 = new Placeholder.User("Test", "Subject1");
                Placeholder.User member2 = new Placeholder.User("Test", "Subject2");
                Placeholder.User member3 = new Placeholder.User("Test", "Subject3");
                Placeholder.User member4 = new Placeholder.User("Test", "Subject4");

                Log("Creating Groups");
                Placeholder.Group group1 = new Placeholder.Group("Group 1");
                group1.Members.Add(member1);
                group1.Members.Add(member2);

                Placeholder.Group group2 = new Placeholder.Group("Group 2");
                group2.Members.Add(member3);
                group2.Members.Add(member4);

                Log("Adding groups to the contest");
                contest.AddGroup(group1);
                contest.AddGroup(group2);

                Log("Adding Members to DB");
                Assert.Fail("Must enable remainder of test code once full User database functionality exists");
                /*UserDAO.CreateNewUser(member1);
                UserDAO.CreateNewUser(member2);
                UserDAO.CreateNewUser(member3);
                UserDAO.CreateNewUser(member4);*/

                Log("Saving to DB");
                int id = ContestDAO.CreateNewContest(contest);

                Log("Reading back from DB");
                Contest retrieved = ContestDAO.GetContestFromContestId(id);

                Log("Locking Initialization");
                retrieved.LockInitialValues();

                Log("Updating contest entry in DB");
                Assert.IsTrue(ContestDAO.UpdateContest(retrieved));

                Log("Reading back from DB");
                Contest retrieved2 = ContestDAO.GetContestFromContestId(id);

                Log("Verifying that contest participants have been locked");
                Assert.IsTrue(retrieved2.Teams[0].Members[0].Initialized);
                Assert.IsTrue(retrieved2.Teams[0].Members[1].Initialized);
                Assert.IsTrue(retrieved2.Teams[1].Members[0].Initialized);
                Assert.IsTrue(retrieved2.Teams[1].Members[1].Initialized);
            }
        }

        [TestMethod]
        public void TestGetTeamByTeamId()
        {
            using (_trans)
            {
                Log("Creating contest to put the team in");
                Contest contest = new Contest("Test Contest1", "This is a test contest",
                    30, ContestEndMode.GoalBased, ContestType.Group, DateTime.Today, new EndCondition(500),
                    Placeholder.Statistic.Steps);

                Log("Adding the contest to the DB");
                int contestId = ContestDAO.CreateNewContest(contest);

                Log("Creating team");
                Team team = new Team("Test Team");
                team.Add(new Placeholder.User("Test", "Subject1"));
                team.Add(new Placeholder.User("Test", "Subject2"));

                Log("Adding team to DB");
                int id = TeamDAO.CreateNewTeam(team, contestId);

                Log("Retrieving team from DB");
                Assert.IsNotNull(TeamDAO.GetTeamFromTeamId(id));

            }
        }

        [TestMethod]
        public void TestGetTeamByTeamIdNotPresent()
        {
            using (_trans)
            {
                Log("Attempting to retrieve non-existent team from DB");
                Assert.IsNull(TeamDAO.GetTeamFromTeamId(-1));
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
