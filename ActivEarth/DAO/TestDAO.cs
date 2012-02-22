using System.Data.SqlClient;
using System.Linq;
using ActivEarth.Objects;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class TestDAO
    {
        public static UserDataProvider GetUserFromUserName(string userName)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return data.UserDataProviders.FirstOrDefault(u => u.user_name == userName);
            }
        }

        public static User GetUserFromUserNameAndPassword(string userName, string password)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return
                    (from u in data.UserDataProviders
                     join p in data.ProfileDataProviders on u.id equals p.user_id
                     where u.user_name == userName && u.password == password
                     select
                         new User
                             {
                                 UserName = u.user_name,
                                 UserID = u.id,
                                 Email = p.email,
                                 FirstName = p.first_name,
                                 LastName = p.last_name,
                                 City = p.city,
                                 State = p.state,
                                 Gender = p.gender,
                                 ProfileID = p.id
                             }).FirstOrDefault();
            }
        }
    }
}