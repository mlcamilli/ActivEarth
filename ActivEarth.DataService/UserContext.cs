using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivEarth.DAO;
using ActivEarth.Objects.Profile;

namespace ActivEarth.DataService
{
    public class UserContext
    {
        public IQueryable<User> Users { 
         get { return UserDAO.GetAllUsers().AsQueryable(); } }
    }
}