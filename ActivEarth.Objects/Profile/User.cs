using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivEarth.Groups;
using ActivEarth.Competition;
using ActivEarth.Competition.Badges;
using ActivEarth.Competition.Challenges;
using ActivEarth.Competition.Contests;

namespace ActivEarth.Profile
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

        public enum Statistic
        {
            Steps,
            WalkDistance,
            BikeDistance,
            RunDistance,
            GasSavings,
            ChallengesCompleted
        };

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

        public Dictionary<uint, float> ChallengeInitialValues
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

        public Dictionary<uint, Contest> Contests
        {
            get;
            set;
        }

        public Dictionary<uint, Challenge> Challenges
        {
            get;
            set;
        }

        private Dictionary<Statistic, float> _stats;

        #endregion ---------- Private Members ----------

        #region ---------- Constructor ----------

        public User(string firstname, string lastname)
        {
            this.FirstName = firstname;
            this.LastName = lastname;

            this.ChallengeInitialValues = new Dictionary<uint, float>();

            _stats = new Dictionary<Statistic, float>();

            _stats.Add(Statistic.BikeDistance, 0);
            _stats.Add(Statistic.ChallengesCompleted, 0);
            _stats.Add(Statistic.GasSavings, 0);
            _stats.Add(Statistic.RunDistance, 0);
            _stats.Add(Statistic.Steps, 0);
            _stats.Add(Statistic.WalkDistance, 0);

            this.Badges = new Dictionary<Statistic, Badge>();
            this.Challenges = new Dictionary<uint, Challenge>();
            this.Contests = new Dictionary<uint, Contest>();
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        public float GetStatistic(Statistic statToGet)
        {
            return _stats[statToGet];
        }

        public void SetStatistic(Statistic statToSet, float val)
        {
            _stats[statToSet] = val;
        }

        #endregion ---------- Public Methods ----------
    }
}