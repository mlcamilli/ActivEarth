using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects.Competition.Challenges
{
    public class ChallengeManager
    {
        #region ---------- Constructor ----------

        /// <summary>
        /// Creates a new Challenge Manager
        /// </summary>
        /// <param name="allUsers">Group containing all ActivEarth users.</param>
        public ChallengeManager(Placeholder.Group allUsers)
        {
            //Both should ultimately be read in from DB
            _allChallenges = new List<Challenge>();
            _activeChallenges = new List<Challenge>();
            _nextID = 1;

            _allUsers = allUsers;
        }

        #endregion ---------- Constructor ----------

        #region ---------- Public Methods ----------

        /// <summary>
        /// Creates a new Challenge and adds it to the collection.
        /// </summary>
        /// <param name="name">Challenge Name.</param>
        /// <param name="description">Challenge Description.</param>
        /// <param name="points">Points to be awarded upon completion of the Challenge.</param>
        /// <param name="start">Time at which the challenge should begin.</param>
        /// <param name="durationInDays">Number of days that the challenge should be active.</param>
        /// <param name="persistent">True if the Challenge is persistent, false otherwise.</param>
        /// <param name="statistic">Statistic on which the Challenge is based.</param>
        /// <param name="requirement">Statistic value required to complete the challenge.</param>
        /// <returns></returns>
        public uint CreateChallenge(string name, string description, int points, bool persistent,
            DateTime start, int durationInDays, Placeholder.Statistic statistic, float requirement)
        {
            uint id = _nextID;
            _nextID++;

            Challenge newChallenge = new Challenge(id, name, description, points, persistent,
                    start, durationInDays, statistic, requirement);

            _activeChallenges.Add(newChallenge);
            _allChallenges.Add(newChallenge);

            this.LockInitialValues(id, statistic);

            return id;
        }

        /// <summary>
        /// Retrieves an active challenge based on its ID.
        /// </summary>
        /// <param name="id">ID of the Challenge to be retrieved.</param>
        /// false to search only active challenges.</param>
        /// <returns>Challenge with ID matching the provided ID, null if no match is found.</returns>
        public Challenge GetChallenge(uint id)
        {
            return this.GetChallenge(id, true);
        }

        /// <summary>
        /// Retrieves a Challenge based on its ID.
        /// </summary>
        /// <param name="id">ID of the Challenge to be retrieved.</param>
        /// <param name="activeOnly">True if only active challenges should be searched (default),
        /// false if expired challenges should also be searched.</param>
        /// <returns>Challenge with ID matching the provided ID, null if no match is found.</returns>
        public Challenge GetChallenge(uint id, bool activeOnly)
        {
            List<Challenge> source = (activeOnly ? _activeChallenges : _allChallenges);
            
            var query = from Challenge challenge in source
                        where challenge.ID == id
                        select challenge;

            foreach (Challenge challenge in query)
            {
                return challenge;
            }

            //If we haven't returned a challenge by now, we didn't find any that matched.
            return null;
        }

        /// <summary>
        /// Cleans up the challenge list, moving expired transient challenges to the archive
        /// and refreshing persistent challenges. Should be called daily at the challenge cutoff time.
        /// </summary>
        public void CleanUp()
        {
            List<Challenge> newActiveChallenges = new List<Challenge>();

            foreach (Challenge challenge in _activeChallenges)
            {
                if (challenge.EndTime > DateTime.Now)
                {
                    newActiveChallenges.Add(challenge);
                }
                else
                {
                    if (challenge.IsPersistent)
                    {
                        newActiveChallenges.Add(challenge);

                        challenge.EndTime = challenge.EndTime.Add(challenge.Duration);

                        this.LockInitialValues(challenge.ID, challenge.StatisticBinding);
                    }
                    else
                    {
                        this.RemoveInitializationValues(challenge.ID);

                        challenge.IsActive = false;
                    }
                }
            }

            _activeChallenges = newActiveChallenges;
        }

        #endregion ---------- Public Methods ----------

        #region ---------- Private Methods ----------

        /// <summary>
        /// Sets the initial statistic value for each ActivEarth user upon creation
        /// of a new challenge, so that the progress during the duration of the challenge
        /// can be calculated.
        /// </summary>
        /// <param name="id">Identifier for the challenge being initialized.</param>
        /// <param name="statistic">Statistic being tracked by the challenge.</param>
        private void LockInitialValues(uint id, Placeholder.Statistic statistic)
        {
            foreach (Placeholder.User user in _allUsers.Members)
            {
                if (user != null)
                {
                    user.ChallengeInitialValues[id] = user.GetStatistic(statistic);
                }
            }
        }

        /// <summary>
        /// Removes the initialization values of an expired challenge from the
        /// users' records.
        /// </summary>
        /// <param name="id">Expired challenge to remove initialization information for.</param>
        private void RemoveInitializationValues(uint id)
        {
            foreach (Placeholder.User user in _allUsers.Members)
            {
                if (user != null)
                {
                    user.ChallengeInitialValues.Remove(id);
                }
            }

        }

        #endregion ---------- Private Methods ----------

        #region ---------- Private Fields ----------

        /// <summary>
        /// List of currently active Challenges.
        /// </summary>
        private List<Challenge> _activeChallenges;

        /// <summary>
        /// List of all Challenges (current and expired).
        /// </summary>
        private List<Challenge> _allChallenges;

        /// <summary>
        /// ID for the next created challenge.
        /// </summary>
        private uint _nextID;

        /// <summary>
        /// Group of all users, who must be updated with the creation of new challenges.
        /// </summary>
        private Placeholder.Group _allUsers;

        #endregion ---------- Private Fields ----------
    }
}