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

                this._stats = new Dictionary<Statistics, float>();

                this._stats.Add(Statistics.BikeDistance, 0);
                this._stats.Add(Statistics.ChallengesCompleted, 0);
                this._stats.Add(Statistics.GasSavings, 0);
                this._stats.Add(Statistics.RunDistance, 0);
                this._stats.Add(Statistics.Steps, 0);
                this._stats.Add(Statistics.WalkDistance, 0);

                this.Badges = new Dictionary<Statistics, Badges.Badge>();
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

            public float GetStatistic(Statistics statToGet)
            {
                return _stats[statToGet];
            }

            public void SetStatistic(Statistics statToSet, float val)
            {
                _stats[statToSet] = val;
            }

            public ActivityScore ActivityScore
            {
                get;
                set;
            }

            public Dictionary<Statistics, Badges.Badge> Badges
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

            private Dictionary<Statistics, float> _stats;
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

        public enum Statistics
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