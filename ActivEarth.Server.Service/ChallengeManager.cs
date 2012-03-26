using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Challenges;

using ActivEarth.DAO;

namespace ActivEarth.Server.Service.Competition
{
    /// <summary>
    /// Serves as a wrapper class for the ChallengeDAO class, further abstracting actions.
    /// </summary>
    public class ChallengeManager
    {
        public ChallengeManager(Group allUsers)
        {
            _allUsers = allUsers;
        }

        #region ---------- Static Methods ----------

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
        public int CreateChallenge(string name, string description, int points, bool persistent,
            DateTime start, int durationInDays, Statistic statistic, float requirement)
        {
            Challenge newChallenge = new Challenge(name, description, points, persistent,
                    start, durationInDays, statistic, requirement);

            int id = ChallengeDAO.CreateNewChallenge(newChallenge);

            LockInitialValues(id, statistic);

            return id;
        }

        /// <summary>
        /// Retrieves an active challenge based on its ID.
        /// </summary>
        /// <param name="id">ID of the Challenge to be retrieved.</param>
        /// false to search only active challenges.</param>
        /// <returns>Challenge with ID matching the provided ID, null if no match is found.</returns>
        public Challenge GetChallenge(int id)
        {
            return ChallengeDAO.GetChallengeFromChallengeId(id);
        }

        /// <summary>
        /// Cleans up the challenge list, moving expired transient challenges to the archive
        /// and refreshing persistent challenges. Should be called daily at the challenge cutoff time.
        /// </summary>
        public void CleanUp()
        {
            foreach (Challenge challenge in ChallengeDAO.GetActiveChallenges())
            {
                if (challenge.EndTime <= DateTime.Now)
                {
                    if (challenge.IsPersistent)
                    {
                        challenge.EndTime = challenge.EndTime.Add(challenge.Duration);
                        ChallengeDAO.UpdateChallenge(challenge);

                        this.LockInitialValues(challenge.ID, challenge.StatisticBinding);
                    }
                    else
                    {
                        this.RemoveInitializationValues(challenge.ID);

                        challenge.IsActive = false;
                        ChallengeDAO.UpdateChallenge(challenge);
                    }
                }
            }
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
        private void LockInitialValues(int id, Statistic statistic)
        {
            foreach (User user in _allUsers.Members)
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
        private void RemoveInitializationValues(int id)
        {
            foreach (User user in _allUsers.Members)
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
        /// Group of all users, who must be updated with the creation of new challenges.
        /// </summary>
        private Group _allUsers;

        #endregion ---------- Private Fields ----------
    }
}