using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using System.Web;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Objects.Groups
{
    [DataServiceKey("ID")]
    public class Group
    {

        #region ---------- Public Properties ----------

        /// <summary>
        /// A unique identification number for the group.
        /// </summary>

        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the Group.
        /// </summary>

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The list of members in the Group.
        /// </summary>
        /// 
        public List<User> Members;

        /// <summary>
        /// The name of the User with ownership of the Group.
        /// </summary>
        public User Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Description of the Group.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// The Activity Score accumulated by the group awarded from competitions.
        /// </summary>
        public ActivityScore ActivityScore
        {
            get;
            set;
        }


        /// <summary>
        /// The GreenScore assigned to the group based on the active transportation of Users in the Group.
        /// </summary>
        public int GreenScore
        {
            get;
            set;
        }

        /// <summary>
        /// The List of HashTags to be used to allow Users to search for this Group.
        /// </summary>
        public List<string> HashTags
        {
            get;
            set;
        }

        /// <summary>
        /// The Group's Wall containing Message and Recent Activity posts.
        /// </summary>
        public Wall Wall;

        /**
        /// <summary>
        /// The Badges accumulated by Users of the Group.
        /// </summary>
        public Dictionary<Statistic, Badge> Badges
        {
            get;
            set;
        }
        */
         
        /// <summary>
        /// The Contests in which the Group is participating.
        /// </summary>
        public List<Contest> Contests
        {
            get;
            set;
        }

        #endregion ---------- Public Properties ----------

        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new Group.
        /// 
        /// </summary>
        /// <param name="name">The name of the Group.</param>
        /// <param name="owner">The User that created the Group</param>
        /// <param name="description">A text description of the Group.</param>
        /// <param name="hashtags">A list of hashtags that can be used to search for the Group</param>
        public Group(string name, User owner, string description, List<string> hashtags)
        {
            this.Name = name;
            this.Owner = owner;
            this.Description = description;

            this.Members = new List<User>();
            this.Members.Add(owner);
           

            this.HashTags = hashtags;

            this.GreenScore = 0;
            this.ActivityScore = new ActivityScore();

            this.Wall = new Wall();
            this.Contests = new List<Contest>();
        }

        /// <summary>
        /// Creates a new Group.  This constructor used when rebuilding Group object from DB.
        /// </summary>
        public Group()
        {
            this.GreenScore = 0;
            this.Wall = new Wall();
            this.ActivityScore = new ActivityScore();
            this.Contests = new List<Contest>();
            this.HashTags = new List<string>();
            this.Members = new List<User>();
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Adds a User to the Group.
        /// </summary>
        /// <param name="newUser">The User to be added to the Group.</param>
        public void Join(User newUser)
        {
            this.Members.Add(newUser);
            
        }

        /// <summary>
        /// Removes a User from the Group.
        /// </summary>
        /// <param name="quittingUser">The User to be removed from the Group.</param>
        public void Quit(User quittingUser)
        {
            this.Members.Remove(quittingUser);
            
        }

        /// <summary>
        /// Posts a Message to the Group's Wall.
        /// </summary>
        /// <param name="message">The Message to be added to the Group's Wall.</param>
        public void Post(Message message)
        {
            this.Wall.Post(message);
        }

        #endregion ---------- Public Methods ----------
    }
}

