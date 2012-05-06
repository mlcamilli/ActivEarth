using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ActivEarth.Objects.Profile;

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
    }
}
