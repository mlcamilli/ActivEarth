using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using ActivEarth.DAO;
using ActivEarth.Objects;
using ActivEarth.Objects.Competition;
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
                    GMTOffset = int.Parse(input.Element("GMTOffset").Value),
                    Distance = double.Parse(input.Element("Distance").Value),
                    EndLatitude = double.Parse(input.Element("EndLatitude").Value),
                    EndLongitude = double.Parse(input.Element("EndLongitude").Value),
                    EndTime = DateTime.Parse(input.Element("EndTime").Value),
                    Mode = input.Element("Mode").Value,
                    Points = input.Element("Points").Value,
                    StartLatitude = double.Parse(input.Element("StartLatitude").Value),
                    StartLongitude = double.Parse(input.Element("StartLongitude").Value),
                    StartTime = DateTime.Parse(input.Element("StartTime").Value),
                    Steps = int.Parse(input.Element("Steps").Value),
                    Type = input.Element("Type").Value,
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

        public string ProcessUser(string id, XElement input)
        {
            try
            {
                User user = new User()
                {
                    FirstName = input.Element("FirstName").ToString(),
                    LastName = input.Element("LastName").ToString(),
                    UserName = input.Element("UserName").ToString(),
                    Email = input.Element("Email").ToString(),
                    Gender = input.Element("Gender").ToString(),
                    GreenScore = int.Parse(input.Element("Gender").Value),
                    Height = int.Parse(input.Element("Height").Value),
                    PrivacySettingID = int.Parse(input.Element("PrivacySettingID").Value),
                    ProfileID = int.Parse(input.Element("ProfileID").Value),
                    State = input.Element("State").ToString(),
                    UserID = int.Parse(input.Element("UserID").Value),
                    userPrivacySettings = new PrivacySetting()
                        {
                            Age = bool.Parse(input.Element("userPrivacySetting").Element("Age").Value),
                            Email = bool.Parse(input.Element("userPrivacySetting").Element("Email").Value),
                            Gender = bool.Parse(input.Element("userPrivacySetting").Element("Gender").Value),
                            Group = bool.Parse(input.Element("userPrivacySetting").Element("Group").Value),
                            Height = bool.Parse(input.Element("userPrivacySetting").Element("Height").Value),
                            ID = int.Parse(input.Element("userPrivacySetting").Element("ID").Value),
                            Location = bool.Parse(input.Element("userPrivacySetting").Element("Location").Value),
                            ProfileVisibility = int.Parse(input.Element("userPrivacySetting").Element("ProfileVisibility").Value),
                            User = null,
                            UserID = int.Parse(input.Element("userPrivacySetting").Element("UserID").Value),
                            Weight = bool.Parse(input.Element("userPrivacySetting").Element("Weight").Value)

                        }
                };

                var badgeScore = int.Parse(input.Element("ActivityScore").Element("BadgeScore").Value);
                var challengeScore = int.Parse(input.Element("ActivityScore").Element("ChallengeScore").Value);
                var contestScore = int.Parse(input.Element("ActivityScore").Element("ContestScore").Value);

                user.ActivityScore = new ActivityScore(badgeScore, challengeScore, contestScore);

                user.Wall = new Wall
                                {
                                    
                                };

                bool userSuccessful = UserDAO.UpdateUserProfile(user);

                return "User update was " + (userSuccessful ? "successful." : ("unsuccessful."));
            }
            catch (Exception e)
            {
                return String.Format("User update was unsuccessful. Reason: {0}", e.Message);
            }
        }

        public Collection<Carpool> GetCarpools()
        {
            return new Collection<Carpool>(CarpoolDAO.GetCarpools());
        }

        public string AddCarpool(XElement input)
        {

            try
            {
                Carpool carpool = new Carpool()
                {
                    Start = input.Element("Start").Value,
                    Destination = input.Element("Destination").Value,
                    SeatsAvailable = byte.Parse(input.Element("SeatsAvailable").Value),
                    Time = input.Element("Time").Value,
                    Comments = input.Element("Comments").Value,
                    UserId = int.Parse(input.Element("UserId").Value)
                };
                string errorMessage;

                int carpoolId = CarpoolDAO.AddCarpool(carpool, out errorMessage);

                return "Carpool addition was " + (carpoolId > 0 ? "successful." : ("unsuccessful. Reason: " + errorMessage));
            }
            catch (Exception e)
            {
                return String.Format("Carpool addition was unsuccessful. Reason: {0}", e.Message);
            }
        }

        public Collection<RecycleCenter> GetRecyclingCenters()
        {
            return new Collection<RecycleCenter>(RecyclingDAO.GetRecyclingCenters());
        }

        public string AddRecyclingCenter(XElement input)
        {
            try
            {
                RecycleCenter center = new RecycleCenter()
                {
                    Location = input.Element("Location").Value,
                    Comments = input.Element("Comments").Value,
                    Automotive = bool.Parse(input.Element("Automotive").Value),
                    Electronics = bool.Parse(input.Element("Electronics").Value),
                    Construction = bool.Parse(input.Element("Construction").Value),
                    Batteries = bool.Parse(input.Element("Batteries").Value),
                    Garden = bool.Parse(input.Element("Garden").Value),
                    Glass = bool.Parse(input.Element("Glass").Value),
                    Hazardous = bool.Parse(input.Element("Hazardous").Value),
                    Household = bool.Parse(input.Element("Household").Value),
                    Metal = bool.Parse(input.Element("Metal").Value),
                    Paint = bool.Parse(input.Element("Paint").Value),
                    Paper = bool.Parse(input.Element("Paper").Value),
                    Plastic = bool.Parse(input.Element("Plastic").Value),
                    UserId = int.Parse(input.Element("UserId").Value)
                };
                string errorMessage;

                int recycleId = RecyclingDAO.AddRecycleCenter(center, out errorMessage);

                return "Recycling Center addition was " + (recycleId > 0 ? "successful." : ("unsuccessful. Reason: " + errorMessage));
            }
            catch (Exception e)
            {
                return String.Format("Recycling Center addition was unsuccessful. Reason: {0}", e.Message);
            }
        }
    }
}
