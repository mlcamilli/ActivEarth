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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ActivEarthRestService" in code, svc and config file together.
    public class ActivEarthRestService : IActivEarthRestService
    {
        public User GetUserById(string id)
        {
            return UserDAO.GetUserFromUserId(int.Parse(id));
        }

        public Collection<User> GetAllUsers()
        {
            return UserDAO.GetAllUsers();
        }
    }
}
