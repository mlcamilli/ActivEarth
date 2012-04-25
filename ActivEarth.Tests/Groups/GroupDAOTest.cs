using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using ActivEarth.Objects.Groups;
using ActivEarth.DAO;
using ActivEarth.Objects.Profile;

namespace ActivEarth.Tests.Groups
{
    /// <summary>
    /// Tests Group database functionality.
    /// </summary>
    [TestClass]
    public class GroupDAOTest
    {

        private TransactionScope _trans;
        private TestContext testContextInstance;

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
//            ConnectionManager.ConnectionString =
//                "data source=.;Initial Catalog=ActivEarth_Dev;Integrated Security=SSPI;";
            _trans = new TransactionScope();
            
        }
        [TestCleanup]
        public void CleanUp()
        {
            _trans.Dispose();
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region ---------------- Test Methods -------------------

        [TestMethod]
        public void TestCreateGroup()
        {
            var owner = new User
            {
                UserName = "owner",
                Age = 25, City = "Bleaksburg", Email = "whatisthis@idont.even", FirstName = "I.C.", Gender = 'M',
                Height = 60, LastName = "Poorcode", State = "VA", Weight = 130
            };
            var member = new User
            {
                UserName = "member",
                Age = 25, City = "Bleaksburg", Email = "whatisthis@idont.even", FirstName = "I.C.", Gender = 'M',
                Height = 60, LastName = "Poorcode", State = "VA", Weight = 130
            };
            var notMember = new User
            {
                UserName = "notMember",
                Age = 25, City = "Bleaksburg", Email = "whatisthis@idont.even", FirstName = "I.C.", Gender = 'M',
                Height = 60, LastName = "Poorcode", State = "VA", Weight = 130
            };

            owner.UserID = UserDAO.CreateNewUser(owner, "password");
            member.UserID = UserDAO.CreateNewUser(member, "password");
            notMember.UserID = UserDAO.CreateNewUser(notMember, "password");

            List<string> tags = new List<string>();
            tags.Add("new");
            tags.Add("searchable");
            tags.Add("hashtags");
            
            Group testGroup = new Group("Test", owner, "This is a Group", tags);
            testGroup.Join(member);

            int id = GroupDAO.CreateNewGroup(testGroup);
            Assert.AreNotEqual(id, 0);

            Group dbGroup = GroupDAO.GetGroupFromGroupId(id);
            Assert.AreEqual("Test", dbGroup.Name);
            Assert.AreEqual("owner", dbGroup.Owner.UserName);
            Assert.AreEqual("This is a Group", dbGroup.Description);

            for (int i = 0; i < tags.Count; i++)
            {
                Assert.AreEqual(tags.ElementAt(i), dbGroup.HashTags.ElementAt(i));
            }

            Assert.AreEqual(2, dbGroup.Members.Count);
            foreach (User u in dbGroup.Members) {
                Assert.IsNotNull(u);
                Assert.AreNotEqual("notMember", u.UserName);
            }
        }

        [TestMethod]
        public void TestGetGroupByHashtag()
        {
            var owner = new User
            {
                UserName = "owner",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };
            var member1 = new User
            {
                UserName = "member1",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };
            var member2 = new User
            {
                UserName = "member2",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };

            owner.UserID = UserDAO.CreateNewUser(owner, "password");
            member1.UserID = UserDAO.CreateNewUser(member1, "password");
            member2.UserID = UserDAO.CreateNewUser(member2, "password");

            List<string> tags1 = new List<string>(); List<string> tags2 = new List<string>();
            tags1.Add("new"); tags2.Add("new");
            tags1.Add("searchable"); tags2.Add("searchable");
                              tags2.Add("hashtags");

            Group testGroup1 = new Group("Test1", owner, "This is a Group", tags1);
            testGroup1.Join(member1);
            testGroup1.ID = GroupDAO.CreateNewGroup(testGroup1);

            Group testGroup2 = new Group("Test2", owner, "This is another Group", tags2);
            testGroup2.Join(member2);
            testGroup2.ID = GroupDAO.CreateNewGroup(testGroup2);

            Assert.AreNotEqual(testGroup1.ID, 0);
            Assert.AreNotEqual(testGroup2.ID, 0);

            List<Group> taggedGroups = GroupDAO.GetAllGroupsByHashTag("hashtags");
            Assert.AreEqual(taggedGroups.Count, 1);
            Assert.AreEqual(taggedGroups.First().Name, "Test2");
            Assert.AreEqual(2, taggedGroups.First().Members.Count);
            foreach (User u in taggedGroups.First().Members)
            {
                Assert.IsNotNull(u);
                Assert.AreNotEqual("member1", u.UserName);
            }

            taggedGroups = GroupDAO.GetAllGroupsByHashTag("new");
            Assert.AreEqual(taggedGroups.Count, 2);
            Assert.AreEqual(taggedGroups.First().Name, "Test1");
            Assert.AreEqual(taggedGroups.Last().Name, "Test2");

            List<Group> allGroups = GroupDAO.GetAllGroups();
            Assert.AreEqual(allGroups.Count, 2);
            Assert.AreEqual(allGroups.First().Name, "Test1");
            Assert.AreEqual(allGroups.Last().Name, "Test2");
        }

        [TestMethod]
        public void TestGetGroupByName()
        {
            var owner = new User
            {
                UserName = "owner",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };
            var member1 = new User
            {
                UserName = "member1",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };
            var member2 = new User
            {
                UserName = "member2",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };

            owner.UserID = UserDAO.CreateNewUser(owner, "password");
            member1.UserID = UserDAO.CreateNewUser(member1, "password");
            member2.UserID = UserDAO.CreateNewUser(member2, "password");

            List<string> tags1 = new List<string>(); List<string> tags2 = new List<string>();
            tags1.Add("new"); tags2.Add("new");
            tags1.Add("searchable"); tags2.Add("searchable");
            tags2.Add("hashtags");

            Group testGroup1 = new Group("Test1", owner, "This is a Group", tags1);
            testGroup1.Join(member1);
            testGroup1.ID = GroupDAO.CreateNewGroup(testGroup1);

            Group testGroup2 = new Group("Test2", owner, "This is another Group", tags2);
            testGroup2.Join(member2);
            testGroup2.ID = GroupDAO.CreateNewGroup(testGroup2);

            Assert.AreNotEqual(testGroup1.ID, 0);
            Assert.AreNotEqual(testGroup2.ID, 0);

            List<Group> taggedGroups = GroupDAO.GetAllGroupsByName("Test2");
            Assert.AreEqual(taggedGroups.Count, 1);
            Assert.AreEqual(taggedGroups.First().Name, "Test2");
            Assert.AreEqual(2, taggedGroups.First().Members.Count);
            foreach (User u in taggedGroups.First().Members)
            {
                Assert.IsNotNull(u);
                Assert.AreNotEqual("member1", u.UserName);
            }

            taggedGroups = GroupDAO.GetAllGroupsByName("Test");
            Assert.AreEqual(taggedGroups.Count, 2);
            Assert.AreEqual(taggedGroups.First().Name, "Test1");
            Assert.AreEqual(taggedGroups.Last().Name, "Test2");
        }

        [TestMethod]
        public void TestGetGroupWall()
        {
            var owner = new User
            {
                UserName = "owner",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };

            owner.UserID = UserDAO.CreateNewUser(owner, "password");

            List<string> tags = new List<string>();
            tags.Add("new");
            tags.Add("searchable");
            tags.Add("hashtags");

            Group testGroup1 = new Group("Test1", owner, "This is a Group", tags);
            Message message1 = new Message("HI GUYS", "THIS IS AN AWESOME GROUP", owner, "March 16 2012", "12:34:25 PM");
            testGroup1.Post(message1);
            testGroup1.ID = GroupDAO.CreateNewGroup(testGroup1);

            Group testGroup2 = new Group("Test2", owner, "This is another Group", tags);
            Message message2 = new Message("I HATE YOU GUYS", "THIS IS AN AWFUL GROUP", owner, "March 16 2012", "12:34:25 PM");
            Message message3 = new Message("JUST KIDDING", "I LOVE YOU GUYS", owner, "March 16 2012", "12:34:25 PM");
            testGroup2.Post(message2);
            testGroup2.Post(message3);
            testGroup2.ID = GroupDAO.CreateNewGroup(testGroup2);

            Assert.AreNotEqual(testGroup1.ID, 0);
            Assert.AreNotEqual(testGroup2.ID, 0);

            GroupDAO.UpdateGroup(testGroup1);
            GroupDAO.UpdateGroup(testGroup2);

            Group dbGroup1 = GroupDAO.GetGroupFromGroupId(testGroup1.ID);
            Group dbGroup2 = GroupDAO.GetGroupFromGroupId(testGroup2.ID);
            Assert.AreEqual(dbGroup1.Wall.Messages.Count, 1);
            Assert.AreEqual(dbGroup1.Wall.Messages.First().Title, "HI GUYS");
            Assert.AreEqual(dbGroup1.Wall.Messages.First().Text, "THIS IS AN AWESOME GROUP");
            Assert.AreEqual(dbGroup1.Wall.Messages.First().Poster.UserName, "owner");
            Assert.AreEqual(dbGroup2.Wall.Messages.Count, 2);
            Assert.AreEqual(dbGroup2.Wall.Messages.First().Title, "I HATE YOU GUYS");
            Assert.AreEqual(dbGroup2.Wall.Messages.First().Text, "THIS IS AN AWFUL GROUP");
            Assert.AreEqual(dbGroup2.Wall.Messages.First().Poster.UserName, "owner");
            Assert.AreEqual(dbGroup2.Wall.Messages.Last().Title, "JUST KIDDING");
            Assert.AreEqual(dbGroup2.Wall.Messages.Last().Text, "I LOVE YOU GUYS");
            Assert.AreEqual(dbGroup2.Wall.Messages.Last().Poster.UserName, "owner");   
        }

        [TestMethod]
        public void TestUpdateGroup()
        {
            var owner = new User
            {
                UserName = "owner",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };
            var member1 = new User
            {
                UserName = "member1",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };
            var member2 = new User
            {
                UserName = "member2",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = 'M',
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };

            owner.UserID = UserDAO.CreateNewUser(owner, "password");
            member1.UserID = UserDAO.CreateNewUser(member1, "password");
            member2.UserID = UserDAO.CreateNewUser(member2, "password");

            List<string> tags = new List<string>();
            tags.Add("new");
            tags.Add("searchable");

            Group testGroup = new Group("Test1", owner, "This is a Group", tags);
            testGroup.Join(member1);
            Message message1 = new Message("HI GUYS", "THIS IS AN AWESOME GROUP", owner, "March 16 2012", "12:34:25 PM");
            testGroup.Post(message1);
            testGroup.ID = GroupDAO.CreateNewGroup(testGroup);

            testGroup.Name = "AWESOME GROUP! YAY!";
            testGroup.Quit(member1);
            testGroup.Join(member2);
            testGroup.HashTags.Add("hashtags");
            testGroup.HashTags.Remove("searchable");
            Message message2 = new Message("JUST KIDDING", "I HATE YOU GUYS", owner, "March 16 2012", "12:34:25 PM");
            testGroup.Post(message2);
            GroupDAO.UpdateGroup(testGroup);

            Group dbGroup = GroupDAO.GetGroupFromGroupId(testGroup.ID);
            Assert.AreEqual(dbGroup.Name, "AWESOME GROUP! YAY!");
            Assert.AreEqual(dbGroup.HashTags.ElementAt(1), "hashtags");
            Assert.AreEqual(dbGroup.Members.ElementAt(1).UserName, "member2");
            Assert.AreEqual(dbGroup.Wall.Messages.Last().Text, "I HATE YOU GUYS");
        }

        #endregion ---------------- Test Methods -------------------
    }
}
