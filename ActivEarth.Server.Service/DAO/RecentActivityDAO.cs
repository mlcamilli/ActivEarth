using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service;

namespace ActivEarth.DAO
{
    public class RecentActivityDAO
    {
        public void GetUserRecentActivity(User user)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    //Update messages table
                    List<Message> messages = (from m in data.MessageDataProviders
                                              where m.group_id == -1 && m.user_id == user.UserID
                                              select    
                                              new Message(m.title, m.message, user, m.date, m.time)
                                         ).ToList();

                    foreach (Message message in messages)
                    {
                        user.Post(message);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void GetGroupRecentActivity(Group group)
        {
            int groupId = group.ID;

            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    //Update messages table
                    List<Message> messages = (from m in data.MessageDataProviders
                                              where m.group_id == groupId
                                              select
                                              new Message(m.title, m.message, UserDAO.GetUserFromUserId(m.user_id), m.date, m.time)
                                         ).ToList();

                    foreach (Message message in messages)
                    {
                        group.Post(message);
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        public bool UpdateUserRecentActivity(User user)
        {
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
                    List<MessageDataProvider> messages = (from m in data.MessageDataProviders
                                                          where m.group_id == -1 && m.user_id == user.UserID
                                                          select
                                                              m
                                                ).ToList();

                    foreach (MessageDataProvider messageData in messages)
                    {
                        bool found = false;
                        foreach (Message message in user.Wall.Messages)
                        {
                            if (messageData.message == message.Text && messageData.user_id == message.Poster.UserID
                                && messageData.title == message.Title && messageData.time == message.Time && messageData.date == message.Date)
                            {
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            data.MessageDataProviders.DeleteOnSubmit(messageData);
                        }
                    }

                    foreach (Message message in user.Wall.Messages)
                    {
                        bool found = false;
                        foreach (MessageDataProvider messageData in messages)
                        {
                            if (messageData.message == message.Text && messageData.user_id == message.Poster.UserID
                                && messageData.title == message.Title && messageData.time == message.Time && messageData.date == message.Date)
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
                                user_id = message.Poster.UserID,
                                group_id = -1,
                                date = message.Date,
                                time = message.Time
                            };
                            data.MessageDataProviders.InsertOnSubmit(messageData);
                        }
                    }

                    data.SubmitChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateGroupRecentActivity(Group group)
        {
            int groupId = group.ID;
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    var data = new ActivEarthDataProvidersDataContext(connection);
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
                            if (messageData.message == message.Text && messageData.user_id == message.Poster.UserID
                                && messageData.title == message.Title && messageData.time == message.Time && messageData.date == message.Date)
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
                            if (messageData.message == message.Text && messageData.user_id == message.Poster.UserID
                                && messageData.title == message.Title && messageData.time == message.Time && messageData.date == message.Date)
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
                                user_id = message.Poster.UserID,
                                group_id = groupId,
                                date = message.Date,
                                time = message.Time
                            };
                            data.MessageDataProviders.InsertOnSubmit(messageData);
                        }
                    }

                    data.SubmitChanges();
                    return true;
                }
            }
            catch(Exception)
            {
                return false;
            }  
        }
    }
}