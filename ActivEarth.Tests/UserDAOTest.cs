using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ActivEarth.DAO;
using ActivEarth.Objects;
using ActivEarth.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;


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
                    Gender = 'M',
                    Height = 60,
                    LastName = "Poorcode",
                    State = "VA",
                    Weight = 130
                };
                UserDAO.CreateNewUser(user, "test");
                Assert.IsTrue(UserDAO.ConfirmPassword("test", UserDAO.GetUserIdFromUserName("testy123")));
                
            }
            
            
        }

    }
}
