using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition
{
    /// <summary>
    /// Temporary placeholder class to be used for return types/parameters until
    /// external feature dependencies are resolved.
    /// </summary>
    public class Placeholder
    {
        public class User
        {
            //CONSTRUCTOR
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

                this.Badges = new Dictionary<Statistic, Badges.Badge>();
                this.Challenges = new Dictionary<uint, Challenges.Challenge>();
                this.Contests = new Dictionary<uint, Contests.Contest>();
            }

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

            public Dictionary<uint, float> ChallengeInitialValues
            {
                get;
                set;
            }

            public float GetStatistic(Statistic statToGet)
            {
                return _stats[statToGet];
            }

            public void SetStatistic(Statistic statToSet, float val)
            {
                _stats[statToSet] = val;
            }

            public ActivityScore ActivityScore
            {
                get;
                set;
            }

            public Dictionary<Statistic, Badges.Badge> Badges
            {
                get;
                set;
            }

            public Dictionary<uint, Contests.Contest> Contests
            {
                get;
                set;
            }

            public Dictionary<uint, Challenges.Challenge> Challenges
            {
                get;
                set;
            }

            private Dictionary<Statistic, float> _stats;
        }

        public class Group
        {
            //CONSTRUCTOR
            public Group(string name)
            {
                this.Name = name;
                this.Members = new List<User>();
            }

            public List<User> Members
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }
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
    }
}