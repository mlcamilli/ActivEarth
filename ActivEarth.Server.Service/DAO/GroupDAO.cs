using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Competition;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class GroupDAO
    {
        /// <summary>
        /// Retrieves a Group from the DB based on its ID.
        /// </summary>
        /// <param name="groupId">Identifier of the Group to retrieve.</param>
        /// <returns>Group specified by the provided ID.</returns>
        public static Group GetGroupFromGroupId(int groupId)
        {
            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);

                List<string> hashtags = (from h in data.GroupHashtagDataProviders
                                         where h.group_id == groupId
                                         select    
                                             h.hashtag
                                         ).ToList();

                List<int> member_ids = (from u in data.GroupMemberDataProviders
                                      where u.group_id == groupId
                                      select
                                          u.user_id
                                      ).ToList();

                List<User> members = new List<User>();
                foreach (int id in member_ids) {
                    members.Add(UserDAO.GetUserFromUserId(id));
                }

                List<int> contest_ids = ContestDAO.GetContestIdsFromGroupId(groupId);

                List<Contest> contests = new List<Contest>();
                foreach (int id in contest_ids)
                {
                    contests.Add(ContestDAO.GetContestFromContestId(id, true, true));
                }

                Group toReturn = (from g in data.GroupDataProviders
                                  where g.id == groupId
                                  select
                                  new Group
                                  {
                                      ID = g.id,
                                      Name = g.name,
                                      Description = g.description,
                                      GreenScore = g.green_score,
                                      ActivityScore = new ActivityScore(g.badge_score, g.challenge_score, g.contest_score),
                                      Members = members,
                                      HashTags = hashtags,
                                      Contests = contests
                                   }).FirstOrDefault();

                toReturn.Owner = UserDAO.GetUserFromUserId(((from g in data.GroupDataProviders
                                                            where g.id == groupId
                                                            select
                                                            g.owner_id).FirstOrDefault()));

                RecentActivityDAO.GetGroupRecentActivity(toReturn);

                return toReturn;
            }
        }

        /// <summary>
        /// Retrieves all currently created Groups.
        /// </summary>
        /// <returns>All Groups in the database.</returns>
        public static List<Group> GetAllGroups()
        {
            List<Group> allGroups = new List<Group>();

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                
                List<int> groupIds = (from g in data.GroupDataProviders
                                      select g.id
                                      ).ToList();

                foreach (int groupId in groupIds) {
                    Group group = GetGroupFromGroupId(groupId);
                    allGroups.Add(group);
                }
                
                return allGroups;
            }
        }

        /// <summary>
        /// Retrieves all Groups that a given user is a part of.
        /// </summary>
        /// <param name="userID">UserID of the desired User</param>
        /// <returns>All Groups in the database the User is in.</returns>
        public static List<Group> GetGroupsByUser(int userID)
        {
            List<Group> toReturn = new List<Group>();

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);

                List<int> groupIds = (from u in data.GroupMemberDataProviders
                                      where u.user_id == userID
                                      select u.group_id
                                      ).ToList();

                foreach (int groupId in groupIds)
                {
                    toReturn.Add(GetGroupFromGroupId(groupId));
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Retrieves all currently created Groups that have a given Owner.
        /// </summary>
        /// <param name="partialName">A string desired to be contained within the Group name</param>
        /// <returns>All Groups in the database with the given Owner.</returns>
        public static List<Group> GetAllGroupsByOwner(User owner)
        {
            List<Group> ownedGroups = new List<Group>();

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);

                List<int> groupIds = (from g in data.GroupDataProviders
                                      where g.owner_id == owner.UserID
                                      select g.id
                                      ).ToList();

                foreach (int groupId in groupIds)
                {
                    Group group = GetGroupFromGroupId(groupId);
                    ownedGroups.Add(group);
                }

                return ownedGroups;
            }
        }

        /// <summary>
        /// Retrieves all currently created Groups that have a name containing the given string.
        /// </summary>
        /// <param name="partialName">A string desired to be contained within the Group name</param>
        /// <returns>All Groups in the database with names containing the given string.</returns>
        public static List<Group> GetAllGroupsByName(string partialName)
        {
            List<Group> namedGroups = new List<Group>();

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);

                List<int> groupIds = (from g in data.GroupDataProviders
                                      where g.name.ToLower().Contains(partialName)
                                      select g.id
                                      ).ToList();

                foreach (int groupId in groupIds)
                {
                    Group group = GetGroupFromGroupId(groupId);
                    namedGroups.Add(group);
                }

                return namedGroups;
            }
        }

        /// <summary>
        /// Retrieves all currently created Groups that are tagged with the given hashtag.
        /// </summary>
        /// <param name="hashtag">A desired string tag</param>
        /// <returns>All Groups in the database with the given hashtag.</returns>
        public static List<Group> GetAllGroupsByHashTag(string hashtag)
        {
            List<Group> taggedGroups = new List<Group>();

            using (SqlConnection connection = ConnectionManager.GetConnection())
            {
                var data = new ActivEarthDataProvidersDataContext(connection);
                
                List<int> groupIds = (from h in data.GroupHashtagDataProviders
                                      where h.hashtag.ToLower().Contains(hashtag)
                                      select h.group_id
                                      ).ToList();

                foreach (int groupId in groupIds) {
                    Group group = GetGroupFromGroupId(groupId);
                    taggedGroups.Add(group);
                }
                
                return taggedGroups;
            }
        }

        /// <summary>
        /// Creates a Group as a new entry in the DB.
        /// </summary>
        /// <param name="group">Group object to add to the DB.</param>
        /// <returns>ID of the created Group on success, 0 on failure.</returns>
        public static int CreateNewGroup(Group group)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    var groupData = new GroupDataProvider
                    {
                        name = group.Name,
                        description = group.Description,
                        owner_id = group.Owner.UserID,
                        green_score = group.GreenScore,
                        badge_score = group.ActivityScore.BadgeScore,
                        challenge_score = group.ActivityScore.ChallengeScore,
                        contest_score = group.ActivityScore.ContestScore,
                    };
                    data.GroupDataProviders.InsertOnSubmit(groupData);
                    data.SubmitChanges();

                    foreach (User member in group.Members) {
                        GroupMemberDataProvider memberData = new GroupMemberDataProvider
                        {
                            user_id = member.UserID,
                            group_id = groupData.id
                        };
                        data.GroupMemberDataProviders.InsertOnSubmit(memberData);
                    }

                    foreach (string tag in group.HashTags)
                    {
                        GroupHashtagDataProvider hashtagData = new GroupHashtagDataProvider
                        {
                            hashtag = tag,
                            group_id = groupData.id
                        };
                        data.GroupHashtagDataProviders.InsertOnSubmit(hashtagData);
                    }

                    data.SubmitChanges();
                    data.Connection.Close();
                    return groupData.id;
                }
            }
            catch (Exception)
            {    
                return 0;
            }
        }

        /// <summary>
        /// Updates an existing Group in the DB.
        /// </summary>
        /// <param name="group">Group whose information needs to be updated.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool UpdateGroup(Group group)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    int groupId = group.ID;
                    GroupDataProvider dbGroup = (from g in data.GroupDataProviders 
                                                 where g.id == groupId 
                                                 select g).FirstOrDefault();
                    if (dbGroup != null)
                    {
                        dbGroup.name = group.Name;
                        dbGroup.description = group.Description;
                        dbGroup.owner_id = group.Owner.UserID;
                        dbGroup.green_score = group.GreenScore;
                        dbGroup.badge_score = group.ActivityScore.BadgeScore;
                        dbGroup.challenge_score = group.ActivityScore.ChallengeScore;
                        dbGroup.contest_score = group.ActivityScore.ContestScore;

                        //Update group_hashtags table
                        List<GroupHashtagDataProvider> hashtags = (from h in data.GroupHashtagDataProviders
                                                 where h.group_id == groupId
                                                 select
                                                     h
                                         ).ToList();

                        foreach (GroupHashtagDataProvider hashtagData in hashtags)
                        {
                            if (!group.HashTags.Contains(hashtagData.hashtag)){
                                data.GroupHashtagDataProviders.DeleteOnSubmit(hashtagData);
                            }
                        }

                        foreach (string hashtag in group.HashTags)
                        {
                            bool found = false;
                            foreach (GroupHashtagDataProvider hashtagData in hashtags)
                            {
                                if (hashtagData.hashtag == hashtag)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                GroupHashtagDataProvider hashtagData = new GroupHashtagDataProvider
                                {
                                    hashtag = hashtag,
                                    group_id = groupId
                                };
                                data.GroupHashtagDataProviders.InsertOnSubmit(hashtagData);
                            }
                        }

                        //Update group_members table
                        List<GroupMemberDataProvider> members = (from u in data.GroupMemberDataProviders
                                                where u.group_id == groupId
                                                select
                                                    u
                                              ).ToList();

                        foreach (GroupMemberDataProvider memberData in members)
                        {
                            bool found = false;
                            foreach (User member in group.Members)
                            {
                                if (memberData.user_id == member.UserID)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                data.GroupMemberDataProviders.DeleteOnSubmit(memberData);
                            }
                        }

                        foreach (User member in group.Members)
                        {
                            bool found = false;
                            foreach (GroupMemberDataProvider memberData in members)
                            {
                                if (memberData.user_id == member.UserID)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                GroupMemberDataProvider memberData = new GroupMemberDataProvider
                                {
                                    user_id = member.UserID,
                                    group_id = groupId
                                };
                                data.GroupMemberDataProviders.InsertOnSubmit(memberData);
                            }
                        }

                        //Update Teams table
                        List<TeamDataProvider> contests = (from c in data.TeamDataProviders
                                                 where c.group_id == groupId
                                                 select
                                                     c
                                                 ).ToList();

                        foreach (TeamDataProvider contestData in contests)
                        {
                            bool found = false;
                            foreach (Contest contest in group.Contests)
                            {
                                if (contestData.contest_id == contest.ID)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                data.TeamDataProviders.DeleteOnSubmit(contestData);
                            }
                        }

                        RecentActivityDAO.UpdateGroupRecentActivity(group);

                        data.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an existing Group from the DB.
        /// </summary>
        /// <param name="groupId">ID of Group whose information needs to be deleted.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool DeleteGroup(int groupId)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    GroupDataProvider dbGroup = (from g in data.GroupDataProviders
                                                 where g.id == groupId
                                                 select g).FirstOrDefault();
                    if (dbGroup != null)
                    {
                        data.GroupDataProviders.DeleteOnSubmit(dbGroup);
                        
                        //Delete entries in the group_hashtags table
                        List<GroupHashtagDataProvider> hashtags = (from h in data.GroupHashtagDataProviders
                                                                   where h.group_id == groupId
                                                                   select
                                                                       h
                                         ).ToList();

                        foreach (GroupHashtagDataProvider hashtagData in hashtags)
                        {                      
                            data.GroupHashtagDataProviders.DeleteOnSubmit(hashtagData);   
                        }


                        //Delete entries in group_members table
                        List<GroupMemberDataProvider> members = (from u in data.GroupMemberDataProviders
                                                                 where u.group_id == groupId
                                                                 select
                                                                     u
                                              ).ToList();

                        foreach (GroupMemberDataProvider memberData in members)
                        {
                            data.GroupMemberDataProviders.DeleteOnSubmit(memberData);
                        }

                        //Delete entires in group_contests table
                        List<TeamDataProvider> contests = (from c in data.TeamDataProviders
                                                                   where c.group_id == groupId
                                                                   select
                                                                       c
                                                 ).ToList();

                        foreach (TeamDataProvider contestData in contests)
                        {
                            data.TeamDataProviders.DeleteOnSubmit(contestData);
                        }
                                           
                        //Remove entires in messages table
                        List<MessageDataProvider> messages = (from m in data.MessageDataProviders
                                                              where m.group_id == groupId
                                                              select
                                                                  m
                                                  ).ToList();

                        foreach (MessageDataProvider messageData in messages)
                        {
                                data.MessageDataProviders.DeleteOnSubmit(messageData);
                        }

                        data.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}