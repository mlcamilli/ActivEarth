using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ActivEarth.DAO;
using ActivEarth.Objects.Profile;

namespace ActivEarth.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    public class UserService : IUserService
    {
       
        public User GetUser(string userName, string password)
        {
            return UserDAO.GetUserFromUserNameAndPassword(userName, password);
        }
    }
}
