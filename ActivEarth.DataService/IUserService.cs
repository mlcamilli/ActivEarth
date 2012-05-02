using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ActivEarth.Objects.Profile;

namespace ActivEarth.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        User GetUser(string userName, string password);

        [OperationContract]
        Collection<User> GetAllUsers();
    }
}
