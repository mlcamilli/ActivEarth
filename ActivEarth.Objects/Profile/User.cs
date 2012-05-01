using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Objects.Groups;

namespace ActivEarth.Objects.Profile
{
    [DataContract]
    [DataServiceKey("UserID")]
    public class User
    {
        [DataMember]
        public Wall Wall;
        [DataMember]
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

        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int ProfileID { get; set; }
        [DataMember]
        public int PrivacySettingID { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public String City { get; set; }
        [DataMember]
        public String State { get; set; }
        [DataMember]
        public int? Age { get; set; }
        [DataMember]
        public int? Weight { get; set; }
        [DataMember]
        public int? Height { get; set; }
        [DataMember]
        public int GreenScore { get; set; }
        [DataMember]
        public ActivityScore ActivityScore { get; set; }
        [DataMember]
        public PrivacySetting userPrivacySettings { get; set; }
    }
}