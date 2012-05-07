using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using ActivEarth.DAO;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;

namespace ActivEarth.RestService
{
    public class ActivEarthRestService : IActivEarthRestService
    {
        public User GetUserById(string id)
        {
            return UserDAO.GetUserFromUserId(int.Parse(id));
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return UserDAO.GetUserFromUserNameAndPassword(username, password);
        }

        public Collection<User> GetAllUsers()
        {
            return UserDAO.GetAllUsers();
        }

        public string ChangePassword(string username, string newpassword)
        {
            var userId = UserDAO.GetUserIdFromUserName(username);
            string errorMessage;
            bool success = UserDAO.UpdatePassword(newpassword, userId, out errorMessage);
            return "Password change was " + (success ? "successful." : ("unsuccessful. " + errorMessage));
        }

        public Collection<Contest> GetContestsFromUserId(string id)
        {
            List<int> ids = ContestDAO.GetContestIdsFromUserId(int.Parse(id));
            Collection<Contest> contests = new Collection<Contest>();

            foreach (int contestId in ids)
            {
                contests.Add(ContestDAO.GetContestFromContestId(contestId, true, true));
            }

            return contests;
        }

        public Collection<Contest> FindContests(string search, string exactMatch)
        {
            List<int> ids = ContestDAO.FindContests(search, bool.Parse(exactMatch));
            Collection<Contest> contests = new Collection<Contest>();

            foreach (int contestId in ids)
            {
                contests.Add(ContestDAO.GetContestFromContestId(contestId, true, true));
            }

            return contests;
        }

        public Contest GetContestFromContestId(string id)
        {
            return ContestDAO.GetContestFromContestId(int.Parse(id), true, false);
        }

        public Collection<Challenge> GetActiveChallenges(string id)
        {
            return new Collection<Challenge>(ChallengeDAO.GetActiveChallenges(int.Parse(id)));
        }

        public Collection<Badge> GetBadgesFromUserId(string id)
        {
            return new Collection<Badge>(BadgeDAO.GetBadgesFromUserId(int.Parse(id)));
        }

        public Collection<Group> GetGroupsFromUserId(string id)
        {
            return new Collection<Group>(GroupDAO.GetGroupsByUser(int.Parse(id)));
        }

        public Collection<Group> FindGroups(string search)
        {
            return new Collection<Group>((List<Group>)GroupDAO.GetAllGroupsByHashTag(search).Union(GroupDAO.GetAllGroupsByName(search)));
        }

        public Group GetGroupById(string id)
        {
            return GroupDAO.GetGroupFromGroupId(int.Parse(id));
        }

        public Collection<Route> GetRoutesById(string id)
        {
            return new Collection<Route>(ActiveRouteDAO.GetRoutesFromUserId(int.Parse(id)));
        }

        public string ProcessRoute(string id, XElement input)
        {
            try
            {
                Route route = new Route()
                {
                    GMTOffset = int.Parse(input.Element("GMT offset").Value),
                    Distance = double.Parse(input.Element("distance").Value),
                    EndLatitude = double.Parse(input.Element("end point latitude").Value),
                    EndLongitude = double.Parse(input.Element("end point longitude").Value),
                    EndTime = DateTime.Parse(input.Element("end time").Value),
                    Mode = input.Element("mode").Value,
                    Points = input.Element("points").Value,
                    StartLatitude = double.Parse(input.Element("start point latitude").Value),
                    StartLongitude = double.Parse(input.Element("start point longitude").Value),
                    StartTime = DateTime.Parse(input.Element("start time").Value),
                    Steps = int.Parse(input.Element("steps").Value),
                    Type = input.Element("type").Value,
                    UserId = int.Parse(id)
                };
                string errorMessage;

                int routeId = ActiveRouteDAO.AddNewRoute(route, out errorMessage);

                return "Route addition was " + (routeId > 0 ? "successful." : ("unsuccessful. Reason: " + errorMessage));
            }
            catch (Exception e)
            {
                return String.Format("Route addition was unsuccessful. Reason: {0}", e.Message);
            }
        }
    }
}
