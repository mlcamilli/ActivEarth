using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ActivEarth.DAO;
using ActivEarth.Objects.Profile;

namespace ActivEarth.RestService
{
    public class ActivEarthRestService : IActivEarthRestService
    {
        public User GetUserById(string id)
        {
            return UserDAO.GetUserFromUserId(int.Parse(id));
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return UserDAO.GetUserFromUserNameAndPassword(username, password);
        }

        public Collection<User> GetAllUsers()
        {
            return UserDAO.GetAllUsers();
        }

        public string ChangePassword(string username, string newpassword)
        {
            var userId = UserDAO.GetUserIdFromUserName(username);
            bool success = UserDAO.UpdatePassword(newpassword, userId);
            return "Password change was " + (success ? "successful." : "unsuccessful.");
        }
    }
}
