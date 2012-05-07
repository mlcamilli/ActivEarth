﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Competition.Badges;
using ActivEarth.Objects.Competition.Challenges;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IActivEarthRestService" in both code and config file together.
    [ServiceContract]
    public interface IActivEarthRestService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "user/{id}")]
        User GetUserById(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "user/{username}/password/{password}")]
        User GetUserByUsernameAndPassword(string username, string password);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "users")]
        Collection<User> GetAllUsers();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "user/{username}/changepw/{newpassword}")]
        string ChangePassword(string username, string newpassword);
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user/{id}/contests")]
        Collection<Contest> GetContestsFromUserId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "contest/{id}")]
        Contest GetContestFromContestId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user/{id}/challenges")]
        Collection<Challenge> GetActiveChallenges(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user/{id}/badges")]
        Collection<Badge> GetBadgesFromUserId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "contest/find/{search}/exact/{exactMatch}")]
        Collection<Contest> FindContests(string search, string exactMatch);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user/{id}/groups")]
        Collection<Group> GetGroupsFromUserId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "group/find/{search}")]
        Collection<Group> FindGroups(string search);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "group/{id}")]
        Group GetGroupById(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user/{id}/routes")]
        Collection<Route> GetRoutesById(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user/{id}/routes")]
        string ProcessRoute(string id, XElement input);
    }
}
