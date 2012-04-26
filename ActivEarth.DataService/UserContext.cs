using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivEarth.DAO;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;

namespace ActivEarth.DataService
{
    public class UserContext
    {
        public IQueryable<User> Users { 
         get { return UserDAO.GetAllUsers().AsQueryable(); } }
        public IQueryable<Group> Groups
        {
            get { return GroupDAO.GetAllGroups().AsQueryable(); }
        }
        public IQueryable<Badge> Badges { get { return BadgeDAO.GetAllBadges().AsQueryable(); } } 
        
        
    }
}