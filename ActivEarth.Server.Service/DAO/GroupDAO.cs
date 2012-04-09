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

                List<int> contest_ids = (from c in data.GroupContestDataProviders
                                         where c.group_id == groupId
                                         select
                                             c.contest_id
                                         ).ToList();

                List<Contest> contests = new List<Contest>();
                foreach (int id in contest_ids)
                {
                    contests.Add(ContestDAO.GetContestFromContestId(id));
                }

                List<Message> messages = (from m in data.MessageDataProviders
                                          where m.group_id == groupId
                                          select
                                              new Message(m.title, m.message, UserDAO.GetUserFromUserId(m.poster_id)) 
                                          ).ToList();

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

                foreach(Message message in messages) {
                    toReturn.Post(message);
                }

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
        /// Retrieves all currently created Groups that are tagged with the given hashtag.
        /// </summary>
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
        /// <param name="challenge">Group object to add to the DB.</param>
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

                    foreach (Message message in group.Wall.Messages)
                    {
                        MessageDataProvider messageData = new MessageDataProvider
                        {
                            title = message.Title,
                            message = message.Text,
                            poster_id = message.Poster.UserID,
                            group_id = groupData.id
                        };
                        data.MessageDataProviders.InsertOnSubmit(messageData);
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
                    
                    foreach (Contest contest in group.Contests)
                    {
                        var contestData = new GroupContestDataProvider
                        {
                            contest_id = contest.ID,
                            group_id = groupData.id
                        };
                        data.GroupContestDataProviders.InsertOnSubmit(contestData);
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
        /// <param name="challenge">Group whose information needs to be updated.</param>
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

                        //Update group_contests table
                        List<GroupContestDataProvider> contests = (from c in data.GroupContestDataProviders
                                                 where c.group_id == groupId
                                                 select
                                                     c
                                                 ).ToList();

                        foreach (GroupContestDataProvider contestData in contests)
                        {
                            bool found = false;
                            foreach (Contest contest in group.Contests)
                            {
                                if (contestData.id == contest.ID)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                data.GroupContestDataProviders.DeleteOnSubmit(contestData);
                            }
                        }

                        foreach (Contest contest in group.Contests)
                        {
                            bool found = false;
                            foreach (GroupContestDataProvider contestData in contests)
                            {
                                if (contestData.id == contest.ID)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                GroupContestDataProvider contestData = new GroupContestDataProvider
                                {
                                    contest_id = contest.ID,
                                    group_id = groupId
                                };
                                data.GroupContestDataProviders.InsertOnSubmit(contestData);
                            }
                        }

                        //Update messages table
                        List<MessageDataProvider> messages = (from m in data.MessageDataProviders
                                                  where m.group_id == groupId
                                                  select
                                                      m
                                                  ).ToList();

                        foreach (MessageDataProvider messageData in messages)
                        {
                            bool found = false;
                            foreach (Message message in group.Wall.Messages)
                            {
                                if (messageData.message == message.Text && messageData.poster_id == message.Poster.UserID
                                    && messageData.title == message.Title)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                data.MessageDataProviders.DeleteOnSubmit(messageData);
                            }
                        }

                        foreach (Message message in group.Wall.Messages)
                        {
                            bool found = false;
                            foreach (MessageDataProvider messageData in messages)
                            {
                                if (messageData.message == message.Text && messageData.poster_id == message.Poster.UserID
                                    && messageData.title == message.Title)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                MessageDataProvider messageData = new MessageDataProvider
                                {
                                    title = message.Title,
                                    message = message.Text,
                                    poster_id = message.Poster.UserID,
                                    group_id = groupId
                                };
                                data.MessageDataProviders.InsertOnSubmit(messageData);
                            }
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