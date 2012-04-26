using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ActivEarth.DAO;
using ActivEarth.Objects;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActivEarth.Objects.Groups;


namespace ActivEarth.Tests
{
    
    [TestClass]
    public class UserDAOTest
    {
        private TransactionScope _trans;
   
        
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

        [TestMethod]
        public void TestConfirmPassword()
        {
            using (_trans)
            {
                var user = new User
                {
                    UserName = "testy123",
                    Age = 25,
                    City = "Bleaksburg",
                    Email = "whatisthis@idont.even",
                    FirstName = "I.C.",
                    Gender = "M",
                    Height = 60,
                    LastName = "Poorcode",
                    State = "VA",
                    Weight = 130
                };
                UserDAO.CreateNewUser(user, "test");
                Assert.IsTrue(UserDAO.ConfirmPassword("test", UserDAO.GetUserIdFromUserName("testy123")));
                
            }
        }

        [TestMethod]
        public void TestUserWall()
        {
            var user1 = new User
            {
                UserName = "User1",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = "M",
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };

            var user2 = new User
            {
                UserName = "User2",
                Age = 25,
                City = "Bleaksburg",
                Email = "whatisthis@idont.even",
                FirstName = "I.C.",
                Gender = "M",
                Height = 60,
                LastName = "Poorcode",
                State = "VA",
                Weight = 130
            };

            user1.UserID = UserDAO.CreateNewUser(user1, "password");
            user2.UserID = UserDAO.CreateNewUser(user2, "password");

            Message message1 = new Message("Recent Activity 1", "User joined ActivEarth!", user1, "March 16 2012", "12:34:25 PM");
            user1.Post(message1);

            Message message2 = new Message("Recent Activity 2", "User ran 5 miles!", user2, "March 16 2012", "12:44:25 PM");
            Message message3 = new Message("Recent Activity 3", "User saved $10 in gas!", user2, "March 16 2012", "12:54:25 PM");
            user2.Post(message2);
            user2.Post(message3);

            UserDAO.UpdateUserProfile(user1);
            UserDAO.UpdateUserProfile(user2);
            
            User dbUser1 = UserDAO.GetUserFromUserId(user1.UserID);
            User dbUser2 = UserDAO.GetUserFromUserId(user2.UserID);
            Assert.AreEqual(dbUser1.Wall.Messages.Count, 1);
            Assert.AreEqual(dbUser1.Wall.Messages.First().Title, "Recent Activity 1");
            Assert.AreEqual(dbUser1.Wall.Messages.First().Text, "User joined ActivEarth!");
            Assert.AreEqual(dbUser1.Wall.Messages.First().Poster.UserName, "User1");
            Assert.AreEqual(dbUser2.Wall.Messages.Count, 2);
            Assert.AreEqual(dbUser2.Wall.Messages.First().Title, "Recent Activity 2");
            Assert.AreEqual(dbUser2.Wall.Messages.First().Text, "User ran 5 miles!");
            Assert.AreEqual(dbUser2.Wall.Messages.First().Poster.UserName, "User2");
            Assert.AreEqual(dbUser2.Wall.Messages.Last().Title, "Recent Activity 3");
            Assert.AreEqual(dbUser2.Wall.Messages.Last().Text, "User saved $10 in gas!");
            Assert.AreEqual(dbUser2.Wall.Messages.Last().Poster.UserName, "User2");

        }

    }
}
