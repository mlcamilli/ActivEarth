using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Competition.Challenges
{
    public class ChallengeManager
    {
        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new Challenge Manager
        /// </summary>
        public ChallengeManager()
        {
            //Both should ultimately be read in from DB
            this._activeChallenges = new List<Challenge>();
            this._nextID = 1;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Creates a new Challenge and adds it to the collection.
        /// </summary>
        /// <param name="name">Challenge Name.</param>
        /// <param name="description">Challenge Description.</param>
        /// <param name="points">Points to be awarded upon completion of the Challenge.</param>
        /// <param name="end">Time when the Challenge shall end.</param>
        /// <param name="persistent">True if the Challenge is persistent, false otherwise.</param>
        /// <param name="statistic">Statistic on which the Challenge is based.</param>
        /// <returns></returns>
        public uint CreateChallenge(string name, string description, int points, bool persistent,
            DateTime end, Placeholder.Statistics statistic)
        {
            uint id = this._nextID;
            this._nextID++;

            this._activeChallenges.Add(new Challenge(id, name, description, points, persistent,
                    end, statistic));

            return id;
        }

        /// <summary>
        /// Retrieves a Challenge based on its ID.
        /// </summary>
        /// <param name="id">ID of the Challenge to be retrieved.</param>
        /// <returns>Challenge with ID matching the provided ID.</returns>
        public Challenge GetChallenge(uint id)
        {
            var query = from Challenge challenge in this._activeChallenges
                        where challenge.ID == id
                        select challenge;

            foreach (Challenge challenge in query)
            {
                return challenge;
            }

            //If we haven't returned a challenge by now, we didn't find any that matched.
            return null;
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// List of currently active Challenges.
        /// </summary>
        private List<Challenge> _activeChallenges;

        /// <summary>
        /// ID for the next created challenge.
        /// </summary>
        private uint _nextID;

        #endregion ---------- Private Fields ----------
    }
}