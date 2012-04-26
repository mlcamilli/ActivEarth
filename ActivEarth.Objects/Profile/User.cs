using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Objects.Groups;

namespace ActivEarth.Objects.Profile
{
    [DataServiceKey("UserID")]
    public class User
    {
        public Wall Wall;

        private Dictionary<Statistic, UserStatistic> _stats;

        #region ---------- Constructor ----------

        public User()
            : this(string.Empty, string.Empty)
        {
        }

        public User(string firstname, string lastname)
        {
            FirstName = firstname;
            LastName = lastname;

            _stats = new Dictionary<Statistic, UserStatistic>();

            Badges = new List<Badge>();
            Contests = new List<Contest>();
            Groups = new List<Group>();
            Wall = new Wall();

            userPrivacySettings = new PrivacySetting();
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        public float GetStatistic(Statistic statToGet)
        {
            if (_stats.ContainsKey(statToGet))
            {
                return _stats[statToGet].Value;
            }
            else
            {
                return 0;
            }
        }

        public void SetStatistic(Statistic statToSet, float val)
        {
            if (_stats.ContainsKey(statToSet))
            {
                _stats[statToSet].Value = val;
            }
            else
            {
                _stats[statToSet] = new UserStatistic(statToSet, val);
            }
        }

        /// <summary>
        /// Sets in memory statistic values.
        /// </summary>
        /// <param name="stats">Dictionary mapping statistics to values.</param>
        public void SetStatisticsDict(Dictionary<Statistic, UserStatistic> stats)
        {
            _stats = stats;
        }

        /// <summary>
        /// Posts a Message to the Group's Wall.
        /// </summary>
        /// <param name="message">The Message to be added to the Group's Wall.</param>
        public void Post(Message message)
        {
            Wall.Post(message);
        }

        #endregion ---------- Public Methods ----------

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int UserID { get; set; }

        public int ProfileID { get; set; }

        public int PrivacySettingID { get; set; }

        public string Gender { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public int? Age { get; set; }

        public int? Weight { get; set; }

        public int? Height { get; set; }

        public List<Group> Groups { get; set; }

        public int GreenScore { get; set; }

        public ActivityScore ActivityScore { get; set; }

        public List<Badge> Badges { get; set; }

        public List<Contest> Contests { get; set; }

        public PrivacySetting userPrivacySettings { get; set; }
    }
}