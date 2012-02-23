using System;
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
        public static User GetUserFromUserId(int userId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                return (from u in data.UserDataProviders
                        join p in data.ProfileDataProviders on u.id equals p.user_id
                        where u.id == userId
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

        public static bool UpdateUserProfile(User user)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    ProfileDataProvider profile =
                        (from p in data.ProfileDataProviders where p.user_id == user.UserID select p).FirstOrDefault();
                    if (profile != null)
                    {
                        profile.first_name = user.FirstName;
                        profile.last_name = user.LastName;
                        profile.gender = user.Gender;
                        profile.city = user.City;
                        profile.state = user.State;
                        profile.email = user.Email;
                        data.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool ConfirmPassword(string password, int userId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    string oldPassword = (from u in data.UserDataProviders
                                          where u.id == userId
                                          select u.password).FirstOrDefault();
                    return oldPassword != null && oldPassword == password;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool UpdatePassword(string password, int userId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var user = (from u in data.UserDataProviders
                                where u.id == userId
                                select u).FirstOrDefault();
                    if (user != null)
                    {
                        user.password = password;
                        data.SubmitChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}