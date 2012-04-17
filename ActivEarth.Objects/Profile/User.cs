using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Objects.Profile
{
    public class User
    {
        #region ---------- Public Members ----------

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string UserName 
        {
            get; 
            set; 
        }

        public string Email
        {
            get;
            set;
        }

        public int UserID
        {
            get;
            set;
        }

        public int ProfileID
        {
            get;
            set;
        }

        public int PrivacySettingID
        {
            get;
            set;
        }

        public char Gender
        {
            get;
            set;
        }

        public String City
        {
            get;
            set;
        }

        public String State
        {
            get;
            set;
        }

        public int? Age
        {
            get;
            set;
        }

        public int? Weight
        {
            get;
            set;
        }

        public int? Height
        {
            get;
            set;
        }

        public List<Group> Groups
        {
            get;
            set;
        }

        public uint GreenScore
        {
            get;
            set;
        }

        public Dictionary<int, float> ChallengeInitialValues
        {
            get;
            set;
        }

        public ActivityScore ActivityScore
        {
            get;
            set;
        }

        public Dictionary<Statistic, Badge> Badges
        {
            get;
            set;
        }

        public List<Contest> Contests
        {
            get;
            set;
        }

        public PrivacySetting userPrivacySettings
        {
            get;
            set;
        }

        private Dictionary<Statistic, UserStatistic> _stats;

        #endregion ---------- Private Members ----------

        #region ---------- Constructor ----------

        public User()
            : this(string.Empty, string.Empty)
        {

        }

        public User(string firstname, string lastname)
        {
            this.FirstName = firstname;
            this.LastName = lastname;

            this.ChallengeInitialValues = new Dictionary<int, float>();

            _stats = new Dictionary<Statistic, UserStatistic>();

            this.Badges = new Dictionary<Statistic, Badge>();
            this.Contests = new List<Contest>();
            this.Groups = new List<Group>();

            this.userPrivacySettings = new PrivacySetting();
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        public float GetStatistic(Statistic statToGet)
        {
            if (_stats.ContainsKey(statToGet))
            {
                return _stats[statToGet].value;
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
                _stats[statToSet].value = val;
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

        #endregion ---------- Public Methods ----------
    }
}